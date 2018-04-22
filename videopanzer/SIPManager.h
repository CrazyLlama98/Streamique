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

class SIPAccount;
class SIPCall;

class SIPManager : public QObject
{
    Q_OBJECT

public:
    explicit SIPManager(pjsip_transport_type_e transportType, quint16 port, QObject* parent = nullptr);
    virtual ~SIPManager() override;

    void createAccount(const QString& idUri, const QString& registrarUri, const QString& user, const QString& password);
    void registerAccount();
    void unregisterAccount();

    void makeCall(const QString& number);
    void acceptCall();
    void hangupCall();
    void ring(pjsua_call_id callId);

    void emitRegStateStarted(bool status);
    void emitRegStateChanged(bool status);
    void onCallStateChanged(pj::CallInfo callInfo, const QString& remoteUri);

signals:
    void regStateStarted(bool status);
    void regStateChanged(bool status);
    void callStateChanged(pj::CallInfo callInfo, const QString& remoteUri);

private:
    pj::Endpoint m_SIPEndpoint;
    pj::EpConfig m_SIPEndpointConfig;
    pj::TransportConfig m_transportConfig;
    pj::AccountConfig m_accountConfig;
    SIPAccount* m_SIPAccount = nullptr;
    SIPCall* m_SIPCall = nullptr;
    pjsua_call_id m_currentCallId = PJSUA_INVALID_ID;
};

#endif // METAVOIP_H
