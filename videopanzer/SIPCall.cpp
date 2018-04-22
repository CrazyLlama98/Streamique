#include "SIPCall.h"

SIPCall::SIPCall(SIPManager* _SIPManager, pj::Account &_SIPAccount, pjsua_call_id callId) :
    pj::Call(_SIPAccount, callId),
    m_SIPManager(_SIPManager)

{

}

SIPCall::~SIPCall()
{
}

void SIPCall::onCallState(pj::OnCallStateParam &callStateParameter)
{
    Q_UNUSED(callStateParameter)
    pj::CallInfo callInfo = getInfo();
    m_SIPManager->onCallStateChanged(callInfo, QString::fromStdString(callInfo.remoteUri));
}

void SIPCall::onCallMediaState(pj::OnCallMediaStateParam &callMediaStateParameter)
{
    Q_UNUSED(callMediaStateParameter)
    pj::CallInfo callInfo = getInfo();
    pj::AudDevManager& audioDevicesManager = pj::Endpoint::instance().audDevManager();
    for (auto i = 0u; i < callInfo.media.size(); ++i) {
        if (callInfo.media[i].type == PJMEDIA_TYPE_AUDIO && getMedia(i)) {
            auto audioMedia = static_cast<pj::AudioMedia*>(getMedia(i));
            audioMedia->adjustRxLevel(1.0f);
            audioMedia->adjustTxLevel(1.0f);
            audioMedia->startTransmit(audioDevicesManager.getPlaybackDevMedia());
            auto captureMedia = &audioDevicesManager.getCaptureDevMedia();
            captureMedia->adjustRxLevel(1.0f);
            captureMedia->adjustTxLevel(1.0f);
            captureMedia->startTransmit(*audioMedia);
        }
        else if (callInfo.media[i].type == PJMEDIA_TYPE_VIDEO) {
            auto& videoWindow = callInfo.media[i].videoWindow;
            videoWindow.Show(true);
        }
    }
}

void SIPCall::onCallTransferRequest(pj::OnCallTransferRequestParam &prm)
{
    Q_UNUSED(prm)
}

void SIPCall::onCallTransferStatus(pj::OnCallTransferStatusParam &prm)
{
    Q_UNUSED(prm)

}

void SIPCall::onCallReplaceRequest(pj::OnCallReplaceRequestParam &prm)
{
    Q_UNUSED(prm)
}
