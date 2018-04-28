#include "VideoWidget.h"
#include <QEvent>
#include <QDebug>

#include <X11/Xlib.h>
#include <X11/Xutil.h>
#include <QX11Info>

VideoWidget::VideoWidget(const pj::WindowHandle& windowHandle, QWidget *parent) :
    QWidget(parent),
    m_windowHandle(windowHandle)
{
}

VideoWidget::~VideoWidget()
{

}

bool VideoWidget::event(QEvent* event)
{
    switch(event->type()) {

    case QEvent::Resize:
    {
        XResizeWindow(QX11Info::display(), (Window)m_windowHandle.window, width(), height());
    } break;

    case QEvent::ParentChange:
    {
        Window parent = (Window)winId();
        XReparentWindow(QX11Info::display(), (Window)m_windowHandle.window, parent, 0, 0);
    } break;
    default:
    {
    } break;
    }
    return QWidget::event(event);
}
