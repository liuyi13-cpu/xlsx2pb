/**
 * @file logger.h
 * @brief log4cplus封装
 * @author
 * @version
 */

#pragma once

#include <unordered_set>
#include "log4cplus/configurator.h"
#include "log4cplus/loggingmacros.h"

using namespace log4cplus;

extern Logger g_main_logger;

class Log
{
public:
    static void Init(const char* log_path_and_prefix, bool log_to_console);
};

#define LOG_TRACE(p)                                                              \
    do {                                                                          \
        LOG4CPLUS_TRACE(g_main_logger, p);                                        \
    } while (0)

#define LOG_DEBUG(p)                                                              \
    do {                                                                          \
        LOG4CPLUS_DEBUG(g_main_logger, p);                                        \
    } while (0)

#define LOG_INFO(p)                                                              \
    do {                                                                         \
        LOG4CPLUS_INFO(g_main_logger, p);                                        \
    } while (0)

#define LOG_WARN(p)                                                              \
    do {                                                                         \
        LOG4CPLUS_WARN(g_main_logger, p);                                        \
    } while (0)

#define LOG_ERROR(p)                                                             \
    do {                                                                         \
        LOG4CPLUS_ERROR(g_main_logger, p);                                       \
    } while (0)

#define LOG_FATAL(p)                                                             \
    do {                                                                         \
        LOG4CPLUS_FATAL(g_main_logger, p);                                       \
    } while (0)


#define TO_STR(x) #x
#define LOG_VAR(x) LOG_DEBUG("LOG_VAR: " << TO_STR(x) << " = " << x)
#define LOG_ERROR_VAR(x) LOG_ERROR("LOG_ERROR_VAR: " << TO_STR(x) << " = " << x)

#define LOG_ASSERT(condition, p)                                  \
    do {                                                          \
        if (!(condition)) {                                       \
            LOG_FATAL("CHECK failed: " << TO_STR(x) << " " << p); \
        }                                                         \
    } while (0)

#define ASSERT(condition) LOG_ASSERT(condition, "")
