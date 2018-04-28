#ifndef SIP_CALL_H
#define SIP_CALL_H

#include <pjsua2.hpp>
#include "SIPManager.h"

class SIPCall : public pj::Call
{
public:
    explicit SIPCall(SIPManager* _SIPManager, pj::Account& _SIPAccount, pjsua_call_id callId = PJSUA_INVALID_ID);
    virtual ~SIPCall() override;

    virtual void onCallState(pj::OnCallStateParam &callStateParameter);
    virtual void onCallMediaState(pj::OnCallMediaStateParam &callMediaStateParameter);
private:
    SIPManager* m_SIPManager = nullptr;
};

#endif // SIP_CALL_H
