#include "SIPAccount.h"
#include "SIPCall.h"

#include <QString>
#include <QDebug>

SIPAccount::SIPAccount(SIPManager* _SIPManager) :
    m_SIPManager(_SIPManager)
{
}

SIPAccount::~SIPAccount()
{
}

void SIPAccount::onRegState(pj::OnRegStateParam& registerStateParamater)
{
    pj::AccountInfo accountInfo = getInfo();
    qDebug() << (accountInfo.regIsActive ? "Register: " : "Unregister: ")
             << "code: " << registerStateParamater.code;
    m_SIPManager->emitRegStateStarted(accountInfo.regIsActive);
}

void SIPAccount::onRegStarted(pj::OnRegStartedParam& registerStartedParameter)
{
     pj::AccountInfo accountInfo = getInfo();
     qDebug() << (accountInfo.regIsActive? "Register: " : "Unregister: ")
              << "code: " << registerStartedParameter.renew;
     m_SIPManager->emitRegStateChanged(accountInfo.regIsActive);
}

void SIPAccount::onIncomingCall(pj::OnIncomingCallParam& incomingCallParamater)
{
    qDebug() << "Incoming call with callId: " << incomingCallParamater.callId;
    m_SIPManager->ring(incomingCallParamater.callId);
}

