#ifndef MAINWINDOW_H
#define MAINWINDOW_H

#include <QMainWindow>
#include <QSoundEffect>

#include "SIPManager.h"

namespace Ui {
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

private:
    void registerSIP();

    enum class SIP_STATE : quint8
    {
        E_NO_STATE = 0,
        E_LOCAL_RING_STATE,
        E_CALLING_STATE
    } m_SIPState;

    Ui::MainWindow* ui = nullptr;
    QSoundEffect ringtone, outgoingRing;
    class SIPManager* m_SIPManager = nullptr;
};

#endif // MAINWINDOW_H
