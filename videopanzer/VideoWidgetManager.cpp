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
        if (callInfo.media[i].type == PJMEDIA_TYPE_VIDEO && (callInfo.media[i].dir & PJMEDIA_DIR_DECODING)) {
            if (callInfo.media[i].videoIncomingWindowId != PJSUA_INVALID_ID) {
                auto& videoWindow = callInfo.media[i].videoWindow;
                if (auto currentVideoWidget = m_videoWidgets.value(callInfo.media[i].videoIncomingWindowId, nullptr)) {
                    emit aboutToDeleteVideoWidget(currentVideoWidget);
                    delete currentVideoWidget;
                }
                auto createdVideoWidget = m_videoWidgets[callInfo.media[i].videoIncomingWindowId] = new VideoWidget(videoWindow.getInfo().winHandle.handle);
                emit newVideoWidget(createdVideoWidget);
            }
            else {
                qDebug() << "Invalid window id for video media!";
            }
        }
    }
}
