#ifndef VIDEOWIDGET_H
#define VIDEOWIDGET_H

#include <pjsua2.hpp>
#include <QWidget>

class VideoWidget : public QWidget
{
    Q_OBJECT
public:
    explicit VideoWidget(const pj::WindowHandle& windowHandle, QWidget* parent = nullptr);
    virtual ~VideoWidget() override;

private:
    pj::WindowHandle m_windowHandle;

protected:
    virtual bool event(QEvent *event) override;
};

#endif // VIDEOWIDGET_H
