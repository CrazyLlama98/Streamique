cmake_minimum_required(VERSION 2.8.12)

project(videopanzer)

add_compile_options(-std=c++11)

set(CMAKE_INCLUDE_CURRENT_DIR ON)
set(CMAKE_AUTOMOC ON)

find_package(Qt5Core)
find_package(Qt5Widgets)
find_package(Qt5Multimedia)
find_package(Qt5MultimediaWidgets)

qt5_wrap_ui(
    ui_files
    client.ui
)

add_executable(
    videopanzer
    main.cpp
    client.cpp
    client.h
    cameraframegrabber.cpp
    cameraframegrabber.h
    conversationcamerahandler.cpp
    conversationcamerahandler.h
    camerahandler.cpp
    camerahandler.h
    inputmessage.cpp
    inputmessage.h
    ${ui_files}
)

target_link_libraries(
    videopanzer
    Qt5::Core
    Qt5::Widgets
    Qt5::Multimedia
    Qt5::MultimediaWidgets
)