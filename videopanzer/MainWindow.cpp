#include "MainWindow.h"
#include "ui_mainwindow.h"
#include "SIPManager.h"
#include "VideoWidgetManager.h"

#include <QFile>
#include <QMessageBox>

#include <QJsonObject>
#include <QJsonArray>
#include <QJsonValue>
#include <QJsonDocument>

static const QString HOST = "10.11.81.223";

MainWindow::MainWindow(QWidget *parent) :
    QMainWindow(parent),
    ui(new Ui::MainWindow),
    m_videoWidgetManager(new VideoWidgetManager(this)),
    NAM(new QNetworkAccessManager(this))
{


    ui->setupUi(this);
    //nothing shall be done before the ui setup statement

    stackedContainers = new QStackedWidget(this);
    loginContainer = new QWidget();
    usersContainer = new QWidget();
    callContainer = new QWidget();
    stackedContainers->addWidget(loginContainer);
    stackedContainers->addWidget(usersContainer);
    stackedContainers->addWidget(callContainer);

    connect(NAM, &QNetworkAccessManager::finished, this, &MainWindow::onFinished);
    createLoginForm();

    outgoingRing.setSource(QUrl::fromLocalFile(":/sounds/ring.wav"));
    outgoingRing.setLoopCount(QSoundEffect::Infinite);
    ringtone.setSource(QUrl::fromLocalFile(":/sounds/ring.wav"));
    ringtone.setLoopCount(QSoundEffect::Infinite);
}

void MainWindow::registerSIP(int id, const QString &host)
{
    m_SIPManager = new SIPManager(PJSIP_TRANSPORT_UDP, 5060 + id);
    m_SIPManager->connect(m_SIPManager, &SIPManager::callStateChanged, this, &MainWindow::onCallStateChanged);
    m_SIPManager->connect(m_SIPManager, &SIPManager::callMediaStateChanged, m_videoWidgetManager, &VideoWidgetManager::onCallMediaStateChanged);
    m_currentAccountId = m_SIPManager->createAccount(
                QString("sip:%1@%2").arg(id).arg(host),
                QString("sip:%1").arg(host),
                QString::number(id),
                "1234");
    //sip:id_acc@host, host, user, pwd
    m_SIPManager->registerAccount(m_currentAccountId);
}

void MainWindow::onHangup()
{
    m_SIPManager->hangupCall(m_currentCallId);
    m_SIPState = SIP_STATE::E_NO_STATE;
    createUsersView();
}

void MainWindow::onCallStateChanged(const pj::CallInfo &callInfo)
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
        createUsersView();
    }
}

void MainWindow::onNewVideoWidget(QWidget* videoWidget)
{
    videoWidget->setParent(videoContainer);
    videoWidget->show();
}

void MainWindow::onAboutToDeleteVideoWidget(QWidget *videoWidget)
{
    //ui->holderLayout->removeWidget(videoWidget);
    videoWidget->setParent(nullptr);
}

MainWindow::~MainWindow()
{
    delete ui;
}

void MainWindow::onFinished(QNetworkReply *reply) {
    qDebug() << reply->attribute(QNetworkRequest::HttpStatusCodeAttribute);

    if(token.isNull()) {
        QJsonObject tokenObject = QJsonDocument::fromJson(reply->readAll()).object();

        token = tokenObject["token"].toString();

        QNetworkRequest reqUsers(QUrl("http://streamique.azurewebsites.net/api/Users"));
        QNetworkRequest reqSelf(QUrl("http://streamique.azurewebsites.net/api/Users/currentUser"));

        reqUsers.setRawHeader("Authorization", QString("Bearer " + token).toUtf8());
        reqSelf.setRawHeader("Authorization", QString("Bearer " + token).toUtf8());

        NAM->get(reqUsers);
        NAM->get(reqSelf);
    }
    else {
        QJsonDocument replyData = QJsonDocument::fromJson(reply->readAll());

        if(replyData.isArray()) {

            QJsonArray usersArray = replyData.array();

            for(auto it : usersArray){
                auto userNickname = it.toObject()["nickname"].toString();
                auto userId = it.toObject()["id"].toInt();
                users.insert(userNickname, userId);
            }

            createUsersView();
        }

        else if(replyData.isObject()) {
            QJsonObject currentUserObject = replyData.object();
            auto selfId = currentUserObject["id"].toInt() * 100;
            registerSIP(selfId, HOST);
        }
    }


}

void MainWindow::createLoginForm(){
    stackedContainers->setCurrentIndex(0);

    setWindowTitle("Login to Videopanzer");
    QGridLayout* layout = new QGridLayout;

    usernameLabel = new QLabel("Username: ");
    usernameBox = new QLineEdit();
    passwordLabel = new QLabel("Password: ");
    passwordBox = new QLineEdit();
    loginButton = new QPushButton("Log In");
    failedLoginLabel = new QLabel();

    passwordBox->setEchoMode(QLineEdit::Password);
    failedLoginLabel->setStyleSheet("QLabel{color:red;}");

    connect(loginButton, SIGNAL(clicked(bool)), this, SLOT(submitLogin()));

    loginButton->setMaximumSize(this->width() / 5, 25);
    layout->setHorizontalSpacing(this->width() / 4);
    layout->addWidget(usernameLabel, 1, 1);
    layout->addWidget(usernameBox, 1, 2);
    layout->addWidget(passwordLabel, 2, 1);
    layout->addWidget(passwordBox, 2, 2);
    layout->addWidget(loginButton, 3, 2);
    layout->addWidget(failedLoginLabel, 4, 1, 1, 2);

    loginContainer->setLayout(layout);
    setCentralWidget(stackedContainers);
}

void MainWindow::submitLogin(){

    QUrl serviceUrl("http://streamique.azurewebsites.net/api/Accounts/login");

    if(isValidLogin(serviceUrl, usernameBox->text(), passwordBox->text())){
        //login is good, do something
        loggedUser = usernameBox->text();
        setWindowTitle(this->windowTitle() + " --- " + loggedUser);
    }else{ //login is not good, do not at any cost let the user proceed
        usernameBox->setText("");
        passwordBox->setText("");
        failedLoginLabel->setText("Wrong username or password");
    }

}

void MainWindow::createUsersView(){
    stackedContainers->setCurrentIndex(1);

    if(usersViewAlreadyCreated){
        return;
    }
    usersViewAlreadyCreated = true;

    setWindowTitle("My contacts");
    QGridLayout* layout = new QGridLayout();

    callButton = new QPushButton("Call");
    usersList = new QListWidget();
    userSearchBox = new QLineEdit();

    for(const auto& user : users.keys()){
        usersList->addItem(new QListWidgetItem(user));
    }

    layout->addWidget(usersList, 1, 1);
    layout->addWidget(callButton, 2, 1);
    layout->addWidget(userSearchBox, 3, 1);

    usersContainer->setLayout(layout);

    connect(callButton, SIGNAL(clicked(bool)), this, SLOT(handleCall()));
    connect(userSearchBox, SIGNAL(textEdited(QString)), this, SLOT(handleUserSearch()));
}

void MainWindow::createCallView(){
    stackedContainers->setCurrentIndex(2);

    if(callViewAlreadyCreated){
        return;
    }

    callViewAlreadyCreated = true;

    QGridLayout* layout = new QGridLayout();

    hangupButton = new QPushButton("Hangup");
    messageInput = new QLineEdit();
    //usersOnChatList = new QListWidget();
    messageHistoryBox = new QTextEdit();
    messageLabel = new QLabel("Message: ");

    messageHistoryBox->setReadOnly(true);

    videoContainer = new QWidget(); //doar temporar
    videoContainer->setSizePolicy(QSizePolicy::Expanding, QSizePolicy::Expanding);

    layout->addWidget(videoContainer, 1, 1, 1, 9);
    //layout->addWidget(messageHistoryBox, 2, 1, 1, 6);
    //layout->addWidget(usersOnChatList, 2, 7, 1, 3);
    layout->addWidget(messageLabel, 3, 1, 1, 1);
    layout->addWidget(messageInput, 3, 2, 1, 7);
    layout->addWidget(hangupButton, 3, 9, 1, 1);

    callContainer->setLayout(layout);


    connect(hangupButton, &QPushButton::clicked, this, &MainWindow::onHangup);
    connect(m_videoWidgetManager, &VideoWidgetManager::newVideoWidget, this, &MainWindow::onNewVideoWidget);
    connect(m_videoWidgetManager, &VideoWidgetManager::aboutToDeleteVideoWidget, this, &MainWindow::onAboutToDeleteVideoWidget);

    messageInput->setFocusPolicy(Qt::StrongFocus);
    //messageHistoryBox->setFocusPolicy(Qt::NoFocus);
    //messageHistoryBox->setReadOnly(true);
    //usersOnChatList->setFocusPolicy(Qt::NoFocus);
}

void MainWindow::handleCall(){
    createCallView();
    if(m_SIPState == SIP_STATE::E_LOCAL_RING_STATE) {
        m_SIPManager->acceptCall(m_currentCallId);
        m_SIPState = SIP_STATE::E_CALLING_STATE;
        ringtone.stop();
    }
    else if(m_SIPState != SIP_STATE::E_LOCAL_RING_STATE) {
        QString userToCall = usersList->currentItem()->text();
        m_SIPManager->makeCall(m_currentAccountId, "sip:"+ QString::number(users[userToCall] * 100) + "@" + HOST);
    }
}

void MainWindow::handleUserSearch(){
    QString typedText = userSearchBox->text();
}

bool MainWindow::isValidLogin(const QUrl &serviceUrl, const QString& user, const QString& pass){
    QJsonObject loginData;
    loginData["email"] =  user;
    loginData["password"] = pass;

    QNetworkRequest req(serviceUrl);

    req.setHeader(QNetworkRequest::ContentTypeHeader, "application/json");

    QJsonDocument loginDoc(loginData);

    NAM->post(req, loginDoc.toJson());

    return true;
}

