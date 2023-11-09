@echo off

set FILE_SIZE_LIMIT=10GB
set OUTPUT_FILE_PATH=../Data/testfile.txt

docker build -t my-large-file-generator -f ./App.Cmd.Generator/Dockerfile .
docker run -v %cd%/Data:/app/Data --env FILE_SIZE_LIMIT=%FILE_SIZE_LIMIT% --env OUTPUT_FILE_PATH=%OUTPUT_FILE_PATH% -it --rm my-large-file-generator
