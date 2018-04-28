#include "SIPManager.h"
#include "SIPAccount.h"
#include "SIPCall.h"

#include <QDebug>

Q_DECLARE_METATYPE(pj::CallInfo)

SIPManager::SIPManager(pjsip_transport_type_e transportType, quint16 port, QObject* parent) :
    QObject(parent)
{
    qRegisterMetaType<pj::CallInfo>();
    try {
        pj::EpConfig SIPEndPointConfig;
        SIPEndPointConfig.logConfig.consoleLevel = 1;
        pj::TransportConfig SIPTransportConfig;
        SIPTransportConfig.port = port;
        m_SIPEndpoint.libCreate();
        m_SIPEndpoint.libInit(SIPEndPointConfig);
        m_SIPEndpoint.transportCreate(transportType, SIPTransportConfig);
        m_SIPEndpoint.libStart();
        qDebug() << "PJLIB has started!";
        auto videoCodecParameter = m_SIPEndpoint.getVideoCodecParam("H264");
        videoCodecParameter.decFmt.width = 1280;
        videoCodecParameter.decFmt.height = 720;
        m_SIPEndpoint.setVideoCodecParam("H264", videoCodecParameter);
    } catch (pj::Error &error) {
        qDebug() << "PJLIB starting failed: " << error.info().c_str();
    }
}

SIPManager::~SIPManager()
{
    try {
        delete m_SIPAccount;
    } catch (pj::Error &error) {
        qDebug() << "PJLIB deleting failed: " << error.info().c_str();
    }
}

void SIPManager::createAccount(const QString &idUri, const QString &registrarUri, const QString &user, const QString &password)
{
    try {
        pj::AccountConfig SIPAccountConfig;
        SIPAccountConfig.idUri = idUri.toStdString();
        SIPAccountConfig.regConfig.registrarUri = registrarUri.toStdString();
        pj::AuthCredInfo credentials("digest", "*", user.toStdString(), 0, password.toStdString());
        SIPAccountConfig.sipConfig.authCreds.push_back(credentials);
        SIPAccountConfig.callConfig.timerMinSESec = 90;
        SIPAccountConfig.callConfig.timerSessExpiresSec = 1800;
        SIPAccountConfig.videoConfig.autoTransmitOutgoing = true;
        m_SIPAccount = new SIPAccount(this);
        m_SIPAccount->create(SIPAccountConfig);
        qDebug() << "Account creation successful!";

    } catch(pj::Error& error) {
        qDebug() << "Account creation error: " << error.info().c_str();
    }
}

void SIPManager::registerAccount()
{
    try {
        m_SIPAccount->setRegistration(true);
        qDebug() << "Registered account successfully!";

    } catch(pj::Error& error) {
        qDebug() << "Register error: " << error.info().c_str();
    }
}

void SIPManager::unregisterAccount()
{
    try {
        m_SIPAccount->setRegistration(false);
        qDebug() << "Unregistered account successfully!";

    } catch(pj::Error& error) {
        qDebug() << "Unregister error: " << error.info().c_str();
    }
}

void SIPManager::makeCall(const QString &number)
{
    try {
        auto newSIPCall = new SIPCall(this, *m_SIPAccount);
        pj::CallOpParam callOperationParameter(true);
        newSIPCall->makeCall(number.toStdString(), callOperationParameter);
        m_SIPCalls.insert(newSIPCall->getId(), newSIPCall);
        qDebug() << "Making call with id: " << newSIPCall->getId();
    } catch(pj::Error& error) {
        qDebug() << "Call could not be made: " << error.info().c_str();
    }
}

void SIPManager::acceptCall(pjsua_call_id callId)
{
    try {
        if (auto incomingSIPCall = m_SIPCalls.value(callId, nullptr))
        {
            pj::CallOpParam callOperationParameter;
            callOperationParameter.statusCode = PJSIP_SC_OK;
            incomingSIPCall->answer(callOperationParameter);
            qDebug() << "Accepted call with id: " << callId;
        }
        else {
            qDebug() << "Cannot accept call with unrecognized id: " << callId;
        }
    } catch(pj::Error& error) {
        qDebug() << "Accepting failed: " << error.info().c_str();
    }
}

void SIPManager::hangupCall(pjsua_call_id callId)
{
    try {
        if (auto hangupSIPCall = m_SIPCalls.value(callId, nullptr)) {
            pj::CallInfo callInfo = hangupSIPCall->getInfo();
            pj::CallOpParam callOperationParameter;
            if(callInfo.lastStatusCode == PJSIP_SC_RINGING) {
                callOperationParameter.statusCode = PJSIP_SC_BUSY_HERE;
            }
            else {
                callOperationParameter.statusCode = PJSIP_SC_OK;
            }
            hangupSIPCall->hangup(callOperationParameter);
            qDebug() << "Hungup call with id: " << callId;
        }
        else {
            qDebug() << "Cannot hangup call with unrecognized id: " << callId;
        }
    } catch(pj::Error& error) {
            qDebug() << "HangupCall failed" << error.info().c_str();
    }
}

void SIPManager::ring(pjsua_call_id callId)
{
    try {
        auto incomingSIPCall = new SIPCall(this, *m_SIPAccount, callId);
        pj::CallOpParam callOperationParameter;
        callOperationParameter.statusCode = PJSIP_SC_RINGING;
        incomingSIPCall->answer(callOperationParameter);
        m_SIPCalls.insert(callId, incomingSIPCall);
    } catch(pj::Error& error) {
        qDebug() << "Ringing failed: " << error.info().c_str();
    }
}

void SIPManager::onRegStateStarted(bool status)
{
    emit regStateStarted(status);
}

void SIPManager::onRegStateChanged(bool status)
{
    emit regStateChanged(status);
}

void SIPManager::onCallStateChanged(pj::CallInfo callInfo, const QString &remoteUri)
{
    if (callInfo.state == PJSIP_INV_STATE_DISCONNECTED) {
        qDebug() << "Deleting call with id: " << callInfo.id << " as result of disconnection!";
        if (auto disconnectedSIPCall = m_SIPCalls.value(callInfo.id, nullptr)) {
            delete disconnectedSIPCall;
            m_SIPCalls.remove(callInfo.id);
        }
    }
    emit callStateChanged(callInfo, remoteUri);
}
