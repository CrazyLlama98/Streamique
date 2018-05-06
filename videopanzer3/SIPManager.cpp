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
        m_SIPEndpoint.libDestroy();
    } catch (pj::Error &error) {
        qDebug() << "PJLIB deleting failed: " << error.info().c_str();
    }
}

pjsua_acc_id SIPManager::createAccount(const QString &idUri, const QString &registrarUri, const QString &user, const QString &password)
{
    try {
        pj::AccountConfig SIPAccountConfig;
        SIPAccountConfig.idUri = idUri.toStdString();
        SIPAccountConfig.regConfig.registrarUri = registrarUri.toStdString();
        SIPAccountConfig.regConfig.registerOnAdd = false;
        SIPAccountConfig.sipConfig.authCreds.push_back({"digest", "*", user.toStdString(), 0, password.toStdString()});
        SIPAccountConfig.callConfig.timerMinSESec = 90;
        SIPAccountConfig.callConfig.timerSessExpiresSec = 1800;
        SIPAccountConfig.videoConfig.autoTransmitOutgoing = true;
        auto newSIPAccount = new SIPAccount(this);
        newSIPAccount->create(SIPAccountConfig);
        qDebug() << "Created account with id: " << newSIPAccount->getId();
        return newSIPAccount->getId();

    } catch(pj::Error& error) {
        qDebug() << "Account creation error: " << error.info().c_str();
        return PJSUA_INVALID_ID;
    }
}

void SIPManager::registerAccount(pjsua_acc_id accountId)
{
    try {
        if (auto registeredSIPAccount = pj::Account::lookup(accountId)) {
            registeredSIPAccount->setRegistration(true);
            qDebug() << "Registered account with id: " << accountId;
        }
        else {
            qDebug() << "Cannot register account with id: " << accountId;
        }
    } catch(pj::Error& error) {
        qDebug() << "Register error: " << error.info().c_str();
    }
}

void SIPManager::unregisterAccount(pjsua_acc_id accountId)
{
    try {
        if (auto unregisteredSIPAccount = pj::Account::lookup(accountId)) {
            unregisteredSIPAccount->setRegistration(false);
            qDebug() << "Unregistered account with id: " << accountId;
        }
        else {
            qDebug() << "Cannot unregister account with id: " << accountId;
        }
    } catch(pj::Error& error) {
        qDebug() << "Unregister error: " << error.info().c_str();
    }
}

void SIPManager::makeCall(pjsua_acc_id accountId, const QString &number)
{
    try {
        if (auto callingSIPAccount = pj::Account::lookup(accountId)) {
            auto newSIPCall = new SIPCall(this, *callingSIPAccount);
            pj::CallOpParam callOperationParameter(true);
            newSIPCall->makeCall(number.toStdString(), callOperationParameter);
            qDebug() << "Making call with id: " << newSIPCall->getId();
        }
        else {
            qDebug() << "Cannot make call from account with unrecognized id: " << accountId;
        }
    } catch(pj::Error& error) {
        qDebug() << "Call could not be made: " << error.info().c_str();
    }
}

void SIPManager::acceptCall(pjsua_call_id callId)
{
    try {
        if (auto incomingSIPCall = pj::Call::lookup(callId))
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
        if (auto hangupSIPCall = pj::Call::lookup(callId)) {
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

void SIPManager::ring(pjsua_acc_id accountId, pjsua_call_id callId)
{
    try {
        if (auto incomingSIPAccount = pj::Account::lookup(accountId)) {
            auto incomingSIPCall = new SIPCall(this, *incomingSIPAccount, callId);
            pj::CallOpParam callOperationParameter;
            callOperationParameter.statusCode = PJSIP_SC_RINGING;
            incomingSIPCall->answer(callOperationParameter);
        }
        else {
            qDebug() << "Cannot ring call with unrecognized id: " << callId;
        }
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

void SIPManager::onCallStateChanged(const pj::CallInfo& callInfo)
{
    if (callInfo.state == PJSIP_INV_STATE_DISCONNECTED) {
        if (auto disconnectedSIPCall = pj::Call::lookup(callInfo.id)) {
            qDebug() << "Deleting call with id: " << callInfo.id << " as result of disconnection!";
            delete disconnectedSIPCall;
        }
        else {
            qDebug() << "Cannot delete call with unrecognized id: " << callInfo.id;
        }
    }
    emit callStateChanged(callInfo);
}

void SIPManager::onCallMediaStateChanged(const pj::CallInfo &callInfo)
{
    emit callMediaStateChanged(callInfo);
}
