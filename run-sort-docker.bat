@echo off

set INPUT_FILE_PATH=../Data/testfile.txt
set OUTPUT_FILE_PATH=../Data/testfile_sorted.txt
set MEMORY_LIMIT=16GB
set TEMP_DIR=../Data/Temp

docker build -t my-large-file-sorter -f ./App.Cmd.Sorter/Dockerfile .
docker run -v %cd%/Data:/app/Data --env INPUT_FILE_PATH=%INPUT_FILE_PATH% --env OUTPUT_FILE_PATH=%OUTPUT_FILE_PATH% --env MEMORY_LIMIT=%MEMORY_LIMIT% --env TEMP_DIR=%TEMP_DIR% -it --rm my-large-file-sorter
