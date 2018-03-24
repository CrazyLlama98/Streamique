#include <QApplication>
#include "client.h"

int main(int argc, char** argv)
{
    QApplication app(argc, argv);
    ClientMainWindow client;
    client.show();
    return app.exec();
}