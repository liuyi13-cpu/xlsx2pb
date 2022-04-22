cmake_minimum_required(VERSION 3.19)
project(xls2pb_cpp)

set(CMAKE_CXX_STANDARD 20)

if (CMAKE_BUILD_TYPE MATCHES Debug)
    add_definitions(-DDEBUG)
endif()

if (WIN32)
    add_definitions(-DWIN32)
elseif(UNIX)
    add_definitions(-DUNIX)
endif (WIN32)

MESSAGE(STATUS "operation system is ${CMAKE_SYSTEM}")

if (WIN32)
    set(CMAKE_EXE_LINKER_FLAGS ${CMAKE_EXE_LINKER_FLAGS} "-static")
endif (WIN32)

#删除调试符号 开启空间优化
set(CMAKE_C_FLAGS_RELEASE "${CMAKE_C_FLAGS_RELEASE} -s -O2")
set(CMAKE_CXX_FLAGS_RELEASE "${CMAKE_CXX_FLAGS_RELEASE} -s -O2")

include_directories(dep ${CMAKE_CURRENT_LIST_DIR}/dep/include/)

if (WIN32)
    set(static_libs ${CMAKE_CURRENT_SOURCE_DIR}/dep/lib/window)
elseif(UNIX)
    set(static_libs ${CMAKE_CURRENT_SOURCE_DIR}/dep/lib/mac)
endif (WIN32)
link_directories(${static_libs})

# 添加子工程
set(UNICODE_FILENAMES_ENABLED TRUE)
add_subdirectory(OpenXLSX)

add_executable(${PROJECT_NAME} main.cpp
        util/CommonUtil.cpp util/CommonUtil.h util/md5.cpp util/md5.h util/singleton.h util/logger.h util/logger.cpp
        proto/tsv2pb.pb.cc proto/tsv2pb.pb.h proto/tools.pb.cc proto/tools.pb.h
        xlsx2tsv.cpp xlsx2tsv.h tsv2pb.cpp tsv2pb.h xlsxcheck.cpp xlsxcheck.h)

# 链接子工程
if (WIN32)
    target_link_libraries(${PROJECT_NAME} OpenXLSX-static protobuf log4cplusS ws2_32)
elseif(UNIX)
    target_link_libraries(${PROJECT_NAME} OpenXLSX-static protobuf log4cplus)
endif (WIN32)
