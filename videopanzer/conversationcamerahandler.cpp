#include "conversationcamerahandler.h"

#include <QBoxLayout>

ConversationCameraHandler::ConversationCameraHandler(QWidget* parent) :
    QWidget(parent), 
    m_hLayout(new QHBoxLayout(this))
{
}

ConversationCameraHandler::~ConversationCameraHandler()
{
    
}