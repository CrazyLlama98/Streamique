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
    connect(ui->hangupButton, &QPushButton::clicked, this, &MainWindow::onHangup);

    outgoingRing.setSource(QUrl::fromLocalFile(":/sounds/ring.wav"));
    outgoingRing.setLoopCount(QSoundEffect::Infinite);
    ringtone.setSource(QUrl::fromLocalFile(":/sounds/ring.wav"));
    ringtone.setLoopCount(QSoundEffect::Infinite);

    registerSIP();
}

void MainWindow::registerSIP()
{
    m_SIPManager = new SIPManager(PJSIP_TRANSPORT_UDP, 5061);
    m_SIPManager->connect(m_SIPManager, &SIPManager::callStateChanged, this, &MainWindow::onCallStateChanged);
    m_SIPManager->createAccount("sip:100@192.168.1.20", "sip:192.168.1.20", "100", "1234");
}

void MainWindow::onCall()
{
    if(m_SIPState == SIP_STATE::E_LOCAL_RING_STATE) {
        m_SIPManager->acceptCall(m_currentCallId);
        m_SIPState = SIP_STATE::E_CALLING_STATE;
        ringtone.stop();
    }
    else if(m_SIPState != SIP_STATE::E_LOCAL_RING_STATE && !ui->number->text().isEmpty()) {
        m_SIPManager->makeCall("sip:"+ ui->number->text() + "@" + "192.168.1.20");
        ui->number->clear();
    }
}

void MainWindow::onHangup()
{
    ui->number->clear();
    m_SIPManager->hangupCall(m_currentCallId);
    m_SIPState = SIP_STATE::E_NO_STATE;
    //ui->info->clear();
}

void MainWindow::onCallStateChanged(pj::CallInfo callInfo, const QString& remoteUri)
{
    m_currentCallId = callInfo.id;
    if(callInfo.state == PJSIP_INV_STATE_EARLY && callInfo.lastStatusCode == PJSIP_SC_RINGING)
    {
        if(callInfo.role == PJSIP_ROLE_UAS)
        {
            //ui->info->setText(remoteUri + " is calling...");
            m_SIPState = SIP_STATE::E_LOCAL_RING_STATE;
            ringtone.play();
        }
        else if (callInfo.role == PJSIP_ROLE_UAC)
        {
            m_SIPState = SIP_STATE::E_REMOTE_RING_STATE;
            outgoingRing.play();
        }
    }
    else if(callInfo.state == PJSIP_INV_STATE_CONFIRMED && callInfo.lastStatusCode == PJSIP_SC_OK)
    {
        m_SIPState = SIP_STATE::E_CALLING_STATE;
        ringtone.stop();
        outgoingRing.stop();
    }
    else if(callInfo.state == PJSIP_INV_STATE_DISCONNECTED)
    {
        //ui->info->clear();
        m_SIPState = SIP_STATE::E_NO_STATE;
        ringtone.stop();
        outgoingRing.stop();
    }
}

MainWindow::~MainWindow()
{
    delete ui;
}
