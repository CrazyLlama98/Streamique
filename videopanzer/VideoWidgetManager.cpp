#include "VideoWidgetManager.h"
#include "VideoWidget.h"
#include <QDebug>

VideoWidgetManager::VideoWidgetManager(QObject* parent) :
    QObject(parent)
{
}

VideoWidgetManager::~VideoWidgetManager()
{

}

void VideoWidgetManager::onCallMediaStateChanged(const pj::CallInfo &callInfo)
{
    for (auto i = 0u; i < callInfo.media.size(); ++i) {
        if (callInfo.state != PJSIP_INV_STATE_CONFIRMED) {
            continue;
        }
        auto& currentMediaInfo = callInfo.media[i];
        if (currentMediaInfo.type == PJMEDIA_TYPE_VIDEO && (currentMediaInfo.dir & PJMEDIA_DIR_DECODING)) {
            auto videoIncomingWindowId = currentMediaInfo.videoIncomingWindowId;
            if (videoIncomingWindowId != PJSUA_INVALID_ID) {
                if (auto currentVideoWidget = m_videoWidgets.value(videoIncomingWindowId, nullptr)) {
                    emit aboutToDeleteVideoWidget(currentVideoWidget);
                    delete currentVideoWidget;
                    m_videoWidgets.remove(videoIncomingWindowId);
                }
                try {
                    auto videoWindowInfo = currentMediaInfo.videoWindow.getInfo();
                    auto createdVideoWidget = *m_videoWidgets.insert(videoIncomingWindowId, new VideoWidget(videoWindowInfo.winHandle.handle));
                    emit newVideoWidget(createdVideoWidget);
                } catch(const pj::Error& error) {
                    qDebug() << "Video Widget creation error: " << error.info().c_str();
                }
            }
            else {
                qDebug() << "Invalid window id for video media!";
            }
        }
    }
}
