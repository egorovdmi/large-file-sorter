@echo off

set INPUT_FILE_PATH=../Data/testfile.txt
set OUTPUT_FILE_PATH=../Data/testfile_sorted.txt
set MEMORY_LIMIT=32GB
set TEMP_DIR=../Data/Temp

cd App.Cmd.Sorter && dotnet restore && dotnet run
