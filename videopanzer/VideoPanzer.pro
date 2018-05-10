QT += core gui widgets quick multimedia network

TARGET = MetaVoIP
TEMPLATE = app

#CONFIG += console

SOURCES += main.cpp\
        SIPManager.cpp \
        SIPAccount.cpp \
        SIPCall.cpp \
    VideoWidgetManager.cpp \
    MainWindow.cpp \
    VideoWidget.cpp \
    client.cpp \
    connection.cpp \
    peermanager.cpp \
    server.cpp

HEADERS += \
        SIPManager.h \
        SIPAccount.h \
        SIPCall.h \
    VideoWidgetManager.h \
    MainWindow.h \
    VideoWidget.h \
    client.h \
    connection.h \
    peermanager.h \
    server.h

FORMS   += mainwindow.ui

#requires(qtConfig(udpsocket))
#requires(qtConfig(listwidget))

unix {
QT += x11extras
INCLUDEPATH += $$PWD/pjproject-2.7.2/pjsip/include \
               $$PWD/pjproject-2.7.2/pjlib/include \
               $$PWD/pjproject-2.7.2/pjlib-util/include \
               $$PWD/pjproject-2.7.2/pjmedia/include \
               $$PWD/pjproject-2.7.2/pjnath/include

    LIBS += -L$$PWD/pjproject-2.7.2/pjsip/lib \
            -L$$PWD/pjproject-2.7.2/pjlib/lib \
            -L$$PWD/pjproject-2.7.2/pjlib-util/lib \
            -L$$PWD/pjproject-2.7.2/pjmedia/lib \
            -L$$PWD/pjproject-2.7.2/pjnath/lib \
            -L$$PWD/pjproject-2.7.2/third_party/lib \
    -lpjsua2-x86_64-unknown-linux-gnu \
    -lpjsua-x86_64-unknown-linux-gnu \
    -lpjsip-simple-x86_64-unknown-linux-gnu \
    -lpjsdp-x86_64-unknown-linux-gnu \
    -lpjsip-x86_64-unknown-linux-gnu \
    -lpjmedia-audiodev-x86_64-unknown-linux-gnu \
    -lpjmedia-videodev-x86_64-unknown-linux-gnu \
    -lpjsip-ua-x86_64-unknown-linux-gnu \
    -lpjnath-x86_64-unknown-linux-gnu \
    -lpjmedia-codec-x86_64-unknown-linux-gnu \
    -lpjmedia-x86_64-unknown-linux-gnu \
    -lpj-x86_64-unknown-linux-gnu \
    -lilbccodec-x86_64-unknown-linux-gnu \
    -lgsmcodec-x86_64-unknown-linux-gnu \
    -lspeex-x86_64-unknown-linux-gnu \
    -lresample-x86_64-unknown-linux-gnu \
    -lsrtp-x86_64-unknown-linux-gnu \
    -lyuv-x86_64-unknown-linux-gnu \
    -lwebrtc-x86_64-unknown-linux-gnu \
    -lpjlib-util-x86_64-unknown-linux-gnu \
    -lasound \
    -luuid \
    -lssl \
    -lcrypto \
    -lavformat \
    -lavcodec \
    -lavutil \
    -lswscale \
    -lSDL2 \
    -lv4l2 \
    -lX11
}

RESOURCES += \
    images.qrc \
    sounds.qrc
