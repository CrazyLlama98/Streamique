#ifndef METAVOIP_H
#define METAVOIP_H

#include <pjsua2.hpp>
#include <QObject>

extern "C" {
#include <pjlib.h>
#include <pjlib-util.h>
#include <pjmedia.h>
#include <pjmedia-codec.h>
#include <pjsip.h>
#include <pjsip_simple.h>
#include <pjsip_ua.h>
#include <pjsua-lib/pjsua.h>
}

class SIPManager : public QObject
{
    Q_OBJECT

public:
    explicit SIPManager(pjsip_transport_type_e transportType, quint16 port, QObject* parent = nullptr);
    virtual ~SIPManager() override;

    void createAccount(QString idUri, QString registrarUri, QString user, QString password);
    void registerAccount();
    void unregisterAccount();

    void makeCall(QString number);
    void acceptCall(pjsua_call_id callId);
    void hangupCall(pjsua_call_id callId);
    void ring(pjsua_call_id callId);

    void emitRegStateStarted(bool status);
    void emitRegStateChanged(bool status);
    void emitCallStateChanged(int role, int callId, int state, int status, QString remoteUri);

signals:
    void regStateStarted(bool status);
    void regStateChanged(bool status);
    void callStateChanged(int role, int callId, int state, int status, QString remoteUri);

private:
    pj::Endpoint m_SIPEndpoint;
    pj::EpConfig m_SIPEndpointConfig;
    pj::TransportConfig m_transportConfig;
    pj::AccountConfig m_accountConfig;
    class SIPAccount* m_SIPAccount = nullptr;
    class SIPCall* m_SIPCall = nullptr;
};

#endif // METAVOIP_H
