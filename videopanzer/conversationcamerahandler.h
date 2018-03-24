#pragma once

#include <QWidget>

class ConversationCameraHandler : public QWidget
{
    Q_OBJECT

public:
    explicit ConversationCameraHandler(QWidget* parent = nullptr);
    virtual ~ConversationCameraHandler() override;

private:
    class QHBoxLayout* m_hLayout = nullptr;
};