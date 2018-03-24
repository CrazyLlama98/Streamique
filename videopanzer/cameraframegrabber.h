#pragma once

#include <QAbstractVideoSurface>

class CameraFrameGrabber : public QAbstractVideoSurface
{
    Q_OBJECT

public:
    explicit CameraFrameGrabber(QObject* parent = nullptr);
    virtual ~CameraFrameGrabber() override;

    virtual QList<QVideoFrame::PixelFormat> supportedPixelFormats(QAbstractVideoBuffer::HandleType handleType) const override;
    virtual bool present(const QVideoFrame& videoFrame) override;

signals:
    void frameAvailable(const QImage& image);
};