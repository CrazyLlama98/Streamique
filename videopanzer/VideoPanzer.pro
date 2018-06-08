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
    VideoWidget.cpp

HEADERS += \
        SIPManager.h \
        SIPAccount.h \
        SIPCall.h \
    VideoWidgetManager.h \
    MainWindow.h \
    VideoWidget.h

FORMS   += mainwindow.ui

unix {
QT += x11extras
INCLUDEPATH += $$PWD/pjproject/pjsip/include \
               $$PWD/pjproject/pjlib/include \
               $$PWD/pjproject/pjlib-util/include \
               $$PWD/pjproject/pjmedia/include \
               $$PWD/pjproject/pjnath/include

    LIBS += -L$$PWD/pjproject/pjsip/lib \
            -L$$PWD/pjproject/pjlib/lib \
            -L$$PWD/pjproject/pjlib-util/lib \
            -L$$PWD/pjproject/pjmedia/lib \
            -L$$PWD/pjproject/pjnath/lib \
            -L$$PWD/pjproject/third_party/lib \
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
