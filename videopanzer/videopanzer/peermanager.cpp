#include <QtNetwork>

#include "client.h"
#include "connection.h"
#include "peermanager.h"

static const qint32 BroadcastInterval = 2000;
static const unsigned broadcastPort = 45000;

PeerManager::PeerManager(Client *client) : QObject(client) {
    this->client = client;

    QStringList envVariables;
    envVariables << "USERNAME" << "USER" << "USERDOMAIN"
                 << "HOSTNAME" << "DOMAINNAME";

    QProcessEnvironment environment = QProcessEnvironment::systemEnvironment();
    foreach (QString string, envVariables) {
        if (environment.contains(string)) {
            username = environment.value(string).toUtf8();
            break;
        }
    }

    if (username.isEmpty())
        username = "unknown";

    updateAddresses(Client::replyData);
    serverPort = 0;

    broadcastSocket.bind(QHostAddress::Any, broadcastPort, QUdpSocket::ShareAddress
                         | QUdpSocket::ReuseAddressHint);
    connect(&broadcastSocket, SIGNAL(readyRead()),
            this, SLOT(readBroadcastDatagram()));

    broadcastTimer.setInterval(BroadcastInterval);
    connect(&broadcastTimer, SIGNAL(timeout()),
            this, SLOT(sendBroadcastDatagram()));
}

void PeerManager::setServerPort(int port) { serverPort = port; }

QByteArray PeerManager::userName() const { return username; }

void PeerManager::startBroadcasting() {
    broadcastTimer.start();
}

bool PeerManager::isLocalHostAddress(const QHostAddress &address) {
    foreach (QHostAddress localAddress, ipAddresses) {
        if (address == localAddress)
            return true;
    }
    return false;
}

void PeerManager::sendBroadcastDatagram() {
    QByteArray datagram(username);
    datagram.append('@');
    datagram.append(QByteArray::number(serverPort));

    bool validBroadcastAddresses = true;
    foreach (QHostAddress address, broadcastAddresses) {
        if (broadcastSocket.writeDatagram(datagram, address,
                                          broadcastPort) == -1)
            validBroadcastAddresses = false;
    }

    if (!validBroadcastAddresses)
        updateAddresses(Client::replyData);
}

void PeerManager::readBroadcastDatagram() {
    while (broadcastSocket.hasPendingDatagrams()) {
        QHostAddress senderIp;
        quint16 senderPort;
        QByteArray datagram;
        datagram.resize(broadcastSocket.pendingDatagramSize());
        if (broadcastSocket.readDatagram(datagram.data(), datagram.size(),
                                         &senderIp, &senderPort) == -1)
            continue;

        QList <QByteArray> list = datagram.split('@');
        if (list.size() != 2)
            continue;

        int senderServerPort = list.at(1).toInt();
        if (isLocalHostAddress(senderIp) && senderServerPort == serverPort)
            continue;

        if (!client->hasConnection(senderIp)) {
            Connection *connection = new Connection(this);
            emit newConnection(connection);
            connection->connectToHost(senderIp, senderServerPort);
        }
    }
}

void PeerManager::updateAddresses(QNetworkReply *replyData) {
    broadcastAddresses.clear();
    ipAddresses.clear();

    QNetworkAccessManager NAM;

    if(peermanager::token.isNull()) {
        QJsonObject tokenObject = QJsonDocument::fromJson(reply->readAll()).object();

        peermanager::token = tokenObject["token"].toString();

        QNetworkRequest reqLobbies(
                    QUrl("http://streamique.azurewebsites.net/ /api/lobbies/{lobbyId}/LobbyJoinRequests"));
        QNetworkRequest reqSelf(QUrl("http://streamique.azurewebsites.net/api/Users/currentUser"));

        reqLobbies.setRawHeader("Authorization", QString("Bearer " + token).toUtf8());
        reqSelf.setRawHeader("Authorization", QString("Bearer " + token).toUtf8());

        NAM.get(reqLobbies);
        NAM.get(reqSelf);
    }
    else {
        QJsonDocument replyData = QJsonDocument::fromJson(reply->readAll());

        if(replyData.isArray()) {

            QJsonArray lobbyArray = replyData.array();

            QJsonArray lobbyUsersArray = lobbyArray["joinRequests"];

            for(auto it : lobbyUsersArray){
                broadcastAddresses << it.toObject()["ipAddress"].toString();
                ipAddresses << it.toObject()["ipAddress"].toString();
            }

        }

        else if(replyData.isObject()) {
            QJsonObject currentUserObject = replyData.object();
            broadcastAddresses << currentUserObject["ipAddress"].toString();
            ipAddresses << currentUserObject["ipAddress"].toString();
        }
    }

    /*
    foreach (QNetworkInterface interface, QNetworkInterface::allInterfaces()) {
        foreach (QNetworkAddressEntry entry, interface.addressEntries()) {
            QHostAddress broadcastAddress = entry.broadcast();
            if (broadcastAddress != QHostAddress::Null && entry.ip() != QHostAddress::LocalHost) {
                broadcastAddresses << broadcastAddress;
                ipAddresses << entry.ip();
            }
        }
    }
    */
}
