build-gen:
	docker build -t my-large-file-generator -f ./App.Cmd.Generator/Dockerfile .

run-gen-docker: build-gen
	docker run -v $(shell pwd)/Data:/Data --env FILE_SIZE_LIMIT=$(FILE_SIZE_LIMIT) --env OUTPUT_FILE_PATH=$(OUTPUT_FILE_PATH) -it --rm my-large-file-generator

run-gen-docker-interactive: build-gen
	docker run -v $(shell pwd)/Data:/Data --env FILE_SIZE_LIMIT=$(FILE_SIZE_LIMIT) --env OUTPUT_FILE_PATH=$(OUTPUT_FILE_PATH) -it --rm --entrypoint /bin/bash my-large-file-generator

run-gen:
	cd App.Cmd.Generator && FILE_SIZE_LIMIT=$(FILE_SIZE_LIMIT) OUTPUT_FILE_PATH=$(OUTPUT_FILE_PATH) dotnet run

run-sort:
	cd App.Cmd.Sorter && dotnet run

ifeq ($(origin FILE_SIZE_LIMIT), undefined)
    FILE_SIZE_LIMIT := 10GB
endif
export FILE_SIZE_LIMIT

ifeq ($(origin OUTPUT_FILE_PATH), undefined)
    OUTPUT_FILE_PATH := ../Data/testfile.txt
endif
export OUTPUT_FILE_PATH
