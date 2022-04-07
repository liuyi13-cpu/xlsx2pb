#=======================================================================================================================
# EXTERNAL LIBRARIES
#   Define external libraries used by OpenXLSX. The libraries (Zippy and PugiXML) are header-only, so
#   INTERFACE libraries should be defined.
#=======================================================================================================================
add_library(Zippy INTERFACE IMPORTED)
target_include_directories(Zippy SYSTEM INTERFACE $<BUILD_INTERFACE:${CMAKE_CURRENT_LIST_DIR}/external/zippy/>)

add_library(PugiXML INTERFACE IMPORTED)
target_include_directories(PugiXML SYSTEM INTERFACE $<BUILD_INTERFACE:${CMAKE_CURRENT_LIST_DIR}/external/pugixml/>)

if (${ENABLE_COMPACT_MODE})
    target_compile_definitions(PugiXML INTERFACE PUGIXML_COMPACT)
endif ()

if (${ENABLE_UNICODE_FILENAMES})
    add_compile_definitions(UNICODE_FILENAMES_ENABLED)
endif ()


#=======================================================================================================================
# COMPILER FEATURES
#   Some older C++17 compilers don't support the char_conv features. If the compiler doesn't support it,
#   a less optimal work-around will be used.
#=======================================================================================================================
include(CheckCXXSourceCompiles)
check_cxx_source_compiles("
                          #include <array>
                          #include <string>
                          #include <charconv>
                          
                          int main() {
                                  std::array<char, 7> str {};
                                  auto p = std::to_chars(str.data(), str.data() + str.size(), 12345).ptr;
                                  auto strResult = std::string(str.data(), p - str.data());
                                  
                                  unsigned long value = 0;
                                  std::from_chars(strResult.data(), strResult.data() + strResult.size(), value);

                                  return 0;
                          }" CHARCONV_RESULT)

if (CHARCONV_RESULT)
    add_compile_definitions(CHARCONV_ENABLED)
endif ()

#=======================================================================================================================
# PROJECT FILES
#   List of project source files
#=======================================================================================================================
set(OPENXLSX_SOURCES
        ${CMAKE_CURRENT_LIST_DIR}/sources/XLCell.cpp
        ${CMAKE_CURRENT_LIST_DIR}/sources/XLCellIterator.cpp
        ${CMAKE_CURRENT_LIST_DIR}/sources/XLCellRange.cpp
        ${CMAKE_CURRENT_LIST_DIR}/sources/XLCellReference.cpp
        ${CMAKE_CURRENT_LIST_DIR}/sources/XLCellValue.cpp
        ${CMAKE_CURRENT_LIST_DIR}/sources/XLColor.cpp
        ${CMAKE_CURRENT_LIST_DIR}/sources/XLColumn.cpp
        ${CMAKE_CURRENT_LIST_DIR}/sources/XLContentTypes.cpp
        ${CMAKE_CURRENT_LIST_DIR}/sources/XLDocument.cpp
        ${CMAKE_CURRENT_LIST_DIR}/sources/XLProperties.cpp
        ${CMAKE_CURRENT_LIST_DIR}/sources/XLRelationships.cpp
        ${CMAKE_CURRENT_LIST_DIR}/sources/XLRow.cpp
        ${CMAKE_CURRENT_LIST_DIR}/sources/XLRowData.cpp
        ${CMAKE_CURRENT_LIST_DIR}/sources/XLSharedStrings.cpp
        ${CMAKE_CURRENT_LIST_DIR}/sources/XLSheet.cpp
        ${CMAKE_CURRENT_LIST_DIR}/sources/XLWorkbook.cpp
        ${CMAKE_CURRENT_LIST_DIR}/sources/XLXmlData.cpp
        ${CMAKE_CURRENT_LIST_DIR}/sources/XLXmlFile.cpp
        ${CMAKE_CURRENT_LIST_DIR}/sources/XLZipArchive.cpp
    )


#=======================================================================================================================
# STATIC LIBRARY
#   Define the static library
#=======================================================================================================================
add_library(OpenXLSX-static STATIC "")
target_sources(OpenXLSX-static PRIVATE ${OPENXLSX_SOURCES})
target_include_directories(OpenXLSX-static
                           PUBLIC
                           $<BUILD_INTERFACE:${CMAKE_CURRENT_LIST_DIR}>
                           $<BUILD_INTERFACE:${CMAKE_CURRENT_LIST_DIR}/headers>
                           $<BUILD_INTERFACE:${CMAKE_CURRENT_BINARY_DIR}>)     # For export header
target_link_libraries(OpenXLSX-static PRIVATE Zippy PugiXML)
target_compile_definitions(OpenXLSX-static PRIVATE OPENXLSX_STATIC_DEFINE)

#=======================================================================================================================

#=======================================================================================================================
# GENERATE EXPORT HEADER
#   Generate preprocessor definitions for setting symbol visibility.
#=======================================================================================================================
include(GenerateExportHeader)
generate_export_header(OpenXLSX-static
                       BASE_NAME openxlsx
                       EXPORT_FILE_NAME OpenXLSX-Exports.hpp
                       EXPORT_MACRO_NAME OPENXLSX_EXPORT
                       NO_EXPORT_MACRO_NAME OPENXLSX_HIDDEN)



