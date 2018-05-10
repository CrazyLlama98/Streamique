#ifndef MAINWINDOW_H
#define MAINWINDOW_H

#include <pjsua2.hpp>
#include <QMainWindow>
#include <QSoundEffect>
#include <QTextTableFormat>
#include <QMainWindow>
#include <QPushButton>
#include <QLineEdit>
#include <QLabel>
#include <QBoxLayout>
#include <QGridLayout>
#include <QMessageBox>
#include <QtAlgorithms>
#include <QStackedWidget>
#include <QScrollArea>
#include <iostream>
#include <QListWidget>
#include <QListWidgetItem>
#include <QTextEdit>
#include <QTextEdit>
#include <QLineEdit>
#include <QListWidget>
#include <QTimer>
#include <QScrollBar>
#include <QTextTable>
#include <QString>
#include <QHash>
#include <QNetworkAccessManager>
#include <QNetworkReply>
#include <vector>
#include <algorithm>
#include <fstream>

#include "client.h"

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

public slots:
    void appendMessage(const QString &from, const QString &message);

private slots:
    void onHangup();
    void onCallStateChanged(const pj::CallInfo& callInfo);
    void onNewVideoWidget(QWidget* videoWidget);
    void onAboutToDeleteVideoWidget(QWidget* videoWidget);

    void returnPressed();
    void newParticipant(const QString &nick);
    void participantLeft(const QString &nick);

    void onFinished(QNetworkReply *);

    ///slot-urile lui horatiu
    void submitLogin();
    bool isValidLogin(const QUrl &, const QString& username, const QString& password);
    void handleCall();
    void handleUserSearch();

private:
    void registerSIP(int id, const QString &host);

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

    Client client;
    QString myNickName;
    QTextTableFormat tableFormat;

    ///Variabilele lui horatiu
    QStackedWidget* stackedContainers;

    QWidget* loginContainer;
    QLabel* usernameLabel;
    QLineEdit* usernameBox;
    QLabel* passwordLabel;
    QLineEdit* passwordBox;
    QPushButton* loginButton;
    QLabel* failedLoginLabel;

    QWidget* usersContainer;
    QPushButton* callButton;
    QListWidget* usersList;
    QLineEdit* userSearchBox;

    QWidget* callContainer;
    QPushButton* hangupButton;
    QLineEdit* messageInput;
    QListWidget* usersOnChatList;
    QTextEdit* messageHistoryBox;
    QLabel* messageLabel; //literally a label with message written on it

    //QString usersFilePath = "/home/matei/Desktop/QtStuff/login_test/dummy_users.txt"; //???HOW TO WORK WITH CURRENT DIR
    QHash <QString, int> users;
    QString loggedUser = "";
    QNetworkAccessManager *NAM;
    QString token = QString::null;

    bool usersViewAlreadyCreated = false;
    bool callViewAlreadyCreated = false;

    ///Metodele lui horatiu
    void createLoginForm();
    void removeLoginForm();
    void createUsersView();
    void createCallView();
    void readUsersList();
};

#endif // MAINWINDOW_H
