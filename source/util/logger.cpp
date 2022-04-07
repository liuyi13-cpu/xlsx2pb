/**
 * @file logger.h
 * @brief log4cplus封装
 * @author
 * @version
 */

#include "logger.h"
#include <sstream>
#include "log4cplus/consoleappender.h"
#include "log4cplus/fileappender.h"
#include "log4cplus/initializer.h"

Logger g_main_logger;

void Log::Init(const char* log_path_and_prefix, bool log_to_console) {
    log4cplus::initialize();
    // 控制台
    log4cplus::SharedAppenderPtr consoleAppender(new log4cplus::ConsoleAppender);
    consoleAppender->setName(LOG4CPLUS_TEXT("console"));
    consoleAppender->setLayout(std::unique_ptr<log4cplus::Layout>(new log4cplus::SimpleLayout));

    // 存文件
    log4cplus::SharedAppenderPtr fileAppender(new log4cplus::FileAppender(
            LOG4CPLUS_TEXT(log_path_and_prefix),
            std::ios_base::out
    ));
    fileAppender->setName(LOG4CPLUS_TEXT("file"));
    log4cplus::tstring pattern = LOG4CPLUS_TEXT("%D{%m/%d/%y %H:%M:%S,%Q} [%t] %-5p %c - %m [%l]%n");
    fileAppender->setLayout(std::unique_ptr<log4cplus::Layout>(new log4cplus::PatternLayout(pattern)));

    g_main_logger = Logger::getInstance("main");
    if (log_to_console) {
        g_main_logger.addAppender(consoleAppender);
    }
    g_main_logger.addAppender(fileAppender);
    g_main_logger.setLogLevel(ALL_LOG_LEVEL);
}
