run-gen:
	cd App.Cmd.Generator && FILE_SIZE_LIMIT=$(FILE_SIZE_LIMIT) OUTPUT_FILE_PATH=$(OUTPUT_FILE_PATH) dotnet run

ifeq ($(origin FILE_SIZE_LIMIT), undefined)
    FILE_SIZE_LIMIT := 10MB
endif
export FILE_SIZE_LIMIT

ifeq ($(origin OUTPUT_FILE_PATH), undefined)
    OUTPUT_FILE_PATH := ../Data/testfile.txt
endif
export OUTPUT_FILE_PATH
