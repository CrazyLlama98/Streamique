#include "inputmessage.h"

InputMessage::InputMessage(QWidget* parent) : 
    QPlainTextEdit(parent)
{
    setFixedHeight(50);
    auto currentFont = font();
    currentFont.setPixelSize(28);
    setFont(currentFont);
}

InputMessage::~InputMessage()
{

}