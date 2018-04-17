#include "mainwindow.h"
#include "ui_mainwindow.h"
#include "SIPManager.h"

#include <QFile>
#include <QMessageBox>

MainWindow::MainWindow(QWidget *parent) :
    QMainWindow(parent),
    ui(new Ui::MainWindow)
{
    ui->setupUi(this);
    ui->number->setFont(QFont("Helvetica", 12, QFont::Bold));
    connect(ui->callButton, &QPushButton::clicked, this, &MainWindow::onCall);

    outgoingRing.setSource(QUrl::fromLocalFile(":/sounds/ring.wav"));
    outgoingRing.setLoopCount(QSoundEffect::Infinite);
    ringtone.setSource(QUrl::fromLocalFile(":/sounds/ring.wav"));
    ringtone.setLoopCount(QSoundEffect::Infinite);

    registerSIP();
}

void MainWindow::registerSIP()
{
    m_SIPManager = new SIPManager(PJSIP_TRANSPORT_UDP, 5061);
    connect(m_SIPManager, SIGNAL(callStateChanged(int, int, int, int, QString)), this, SLOT(on_callState_changed(int, int, int, int, QString)));
    m_SIPManager->createAccount("sip:1234@192.168.1.20", "sip:192.168.1.20", "1234", "1234");
}

//void MainWindow::on_hangButton_clicked()
//{
//    ui->number->clear();
//    metaVoIP->hangupCall(callId);
//    sipState = "";
//    ui->info->clear();
//}
//

void MainWindow::onCall()
{
    if(m_SIPState == SIP_STATE::E_LOCAL_RING_STATE) {
        m_SIPManager->acceptCall(0);
        m_SIPState = SIP_STATE::E_CALLING_STATE;
        ringtone.stop();
    }
    else if(m_SIPState != SIP_STATE::E_LOCAL_RING_STATE && !ui->number->text().isEmpty()) {
        m_SIPManager->makeCall("sip:"+ ui->number->text() + "@" + "192.168.1.20");
        ui->number->clear();
    }
}
//
//void MainWindow::on_callState_changed(int role, int callId, int state, int status, QString remoteUri)
//{
//    if(state == PJSIP_INV_STATE_EARLY && status == 180)
//    {
//        if(role == 1)
//        {
//            ui->info->setText(remoteUri + " is calling...");
//            sipState = "localRing";
//            ringtone.play();
//        }
//        else
//        {
//            sipState = "remoteRing";
//            outgoingRing.play();
//        }
//
//        this->callId = callId;
//    }
//    else if(state == PJSIP_INV_STATE_CONFIRMED && status == 200)
//    {
//        sipState = "calling";
//        ringtone.stop();
//        outgoingRing.stop();
//    }
//    else if(state == PJSIP_INV_STATE_DISCONNECTED)
//    {
//        ui->info->clear();
//        sipState = "";
//        ringtone.stop();
//        outgoingRing.stop();
//    }
//}

MainWindow::~MainWindow()
{
    delete ui;
}
