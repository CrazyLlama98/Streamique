#ifndef MYCALL_H
#define MYCALL_H

#include <pjsua2.hpp>
#include "SIPManager.h"

class SIPCall : public pj::Call
{
public:
    explicit SIPCall(SIPManager* _SIPManager, pj::Account& _SIPAccount, pjsua_call_id callId = PJSUA_INVALID_ID);
    virtual ~SIPCall() override;

    virtual void onCallState(pj::OnCallStateParam &callStateParameter);
    virtual void onCallMediaState(pj::OnCallMediaStateParam &callMediaStateParameter);
    virtual void onCallTransferRequest(pj::OnCallTransferRequestParam &prm);
    virtual void onCallTransferStatus(pj::OnCallTransferStatusParam &prm);
    virtual void onCallReplaceRequest(pj::OnCallReplaceRequestParam &prm);

private:
    SIPManager* m_SIPManager = nullptr;
};

#endif // MYCALL_H
