#ifndef VIDEOWIDGETMANAGER_H
#define VIDEOWIDGETMANAGER_H

#include <pjsua2.hpp>
#include <QWidget>
#include <QHash>

class VideoWidget;

class VideoWidgetManager : public QObject
{
    Q_OBJECT
public:
    explicit VideoWidgetManager(QObject* parent = nullptr);
    virtual ~VideoWidgetManager() override;

signals:
    void newVideoWidget(QWidget* videoWidget);
    void aboutToDeleteVideoWidget(QWidget* videoWidget);

public slots:
    void onCallMediaStateChanged(const pj::CallInfo& callInfo);

private:
    QHash<pjsua_vid_win_id, VideoWidget*> m_videoWidgets;
};

#endif // VIDEOWIDGETMANAGER_H
