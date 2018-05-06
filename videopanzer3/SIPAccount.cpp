#include "SIPAccount.h"
#include "SIPCall.h"
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
    m_SIPManager->onRegStateStarted(accountInfo.regIsActive);
}

void SIPAccount::onRegStarted(pj::OnRegStartedParam& registerStartedParameter)
{
     pj::AccountInfo accountInfo = getInfo();
     qDebug() << (accountInfo.regIsActive? "Register: " : "Unregister: ")
              << "code: " << registerStartedParameter.renew;
     m_SIPManager->onRegStateChanged(accountInfo.regIsActive);
}

void SIPAccount::onIncomingCall(pj::OnIncomingCallParam& incomingCallParamater)
{
    qDebug() << "Incoming call with id: " << incomingCallParamater.callId;
    m_SIPManager->ring(getId(), incomingCallParamater.callId);
}

