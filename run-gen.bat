@echo off

set FILE_SIZE_LIMIT=10GB
set OUTPUT_FILE_PATH="../Data/testfile.txt"

cd App.Cmd.Generator && dotnet restore && dotnet run
