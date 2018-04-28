#ifndef MAINWINDOW_H
#define MAINWINDOW_H

#include <pjsua2.hpp>
#include <QMainWindow>
#include <QSoundEffect>

namespace Ui
{
    class MainWindow;
}

class MainWindow : public QMainWindow
{
    Q_OBJECT
public:
    explicit MainWindow(QWidget* parent = nullptr);
    virtual ~MainWindow() override;

private slots:
    void onCall();
    void onHangup();
    void onCallStateChanged(const pj::CallInfo& callInfo);
    void onNewVideoWidget(QWidget* videoWidget);
    void onAboutToDeleteVideoWidget(QWidget* videoWidget);

private:
    void registerSIP();

    enum class SIP_STATE : quint8
    {
        E_NO_STATE = 0,
        E_LOCAL_RING_STATE,
        E_REMOTE_RING_STATE,
        E_CALLING_STATE
    } m_SIPState;

    pjsua_acc_id m_currentAccountId = PJSUA_INVALID_ID;
    pjsua_call_id m_currentCallId = PJSUA_INVALID_ID;

    Ui::MainWindow* ui = nullptr;
    class SIPManager* m_SIPManager = nullptr;
    class VideoWidgetManager* m_videoWidgetManager = nullptr;

    QSoundEffect ringtone;
    QSoundEffect outgoingRing;
};

#endif // MAINWINDOW_H
