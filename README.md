# Build status
- ![linux-build-x64](https://github.com/AlexanderFurmenkov/TextFilesFormatAnalyzer/workflows/linux-build-x64/badge.svg)
- ![windows-build-x64](https://github.com/AlexanderFurmenkov/TextFilesFormatAnalyzer/workflows/windows-build-x64/badge.svg)

# Download the latest release
- [linux-build-x64 v1.0](https://github.com/AlexanderFurmenkov/TextFilesFormatAnalyzer/releases/download/v1.0/TextFilesFormatAnalyzer-linux-x64.zip)
- [windows-build-x64 v1.0](https://github.com/AlexanderFurmenkov/TextFilesFormatAnalyzer/releases/download/v1.0/TextFilesFormatAnalyzer-win10-x64.zip)

# What is it?
Text files format analyzer utility.

# What can it do?
Analyze text files with specific extensions from the specified directory.

It checks:
- text file format: utf-8, utf-16, etc...
- line endings: cr, lf, crlf, mixed

Overall statistics can be additionally included.
Report will produced in JSON format.

# Usage
Template:
```TextFilesFormatAnalyzer.exe "path_to_directory_to_scan"  "*.file_extension1|*.file_extension2" "report.json" [-l] [-s]```

Example:
```TextFilesFormatAnalyzer.exe "D:\\repo\\project1"  "*.cpp|*.h" "project1_report.json" -l -s```
