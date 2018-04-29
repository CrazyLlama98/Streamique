#include "VideoWidget.h"
#include <QEvent>
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
        XResizeWindow(QX11Info::display(), (Window)m_windowHandle.window, width(), height());
        break;

    case QEvent::ParentChange:
        XReparentWindow(QX11Info::display(), (Window)m_windowHandle.window, (Window)winId(), 0, 0);
        break;
    case QEvent::Show:
        XMapRaised(QX11Info::display(), (Window)m_windowHandle.window);
        break;
    case QEvent::Hide:
        XUnmapWindow(QX11Info::display(), (Window)m_windowHandle.window);
    default:
        break;
    }
    return QWidget::event(event);
}
