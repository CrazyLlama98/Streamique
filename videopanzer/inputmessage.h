#pragma once

#include <QPlainTextEdit>

class InputMessage : public QPlainTextEdit
{
    Q_OBJECT

public:
    explicit InputMessage(QWidget* parent = nullptr);
    virtual ~InputMessage() override;
};