build-gen:
	docker build -t my-large-file-generator -f ./App.Cmd.Generator/Dockerfile .

run-gen-docker: build-gen
	docker run -v $(shell pwd)/Data:/app/Data --env FILE_SIZE_LIMIT=$(FILE_SIZE_LIMIT) --env OUTPUT_FILE_PATH=$(OUTPUT_FILE_PATH) -it --rm my-large-file-generator

build-sort:
	docker build -t my-large-file-sorter -f ./App.Cmd.Sorter/Dockerfile .

run-gen-sort: build-gen
	docker run -v $(shell pwd)/Data:/app/Data --env INPUT_FILE_PATH=$(INPUT_FILE_PATH) --env OUTPUT_FILE_PATH=$(OUTPUT_FILE_PATH) --env MEMORY_LIMIT=$(MEMORY_LIMIT) --env TEMP_DIR=$(TEMP_DIR) -it --rm my-large-file-sorter

run-sort-docker-2gb: build-sort
	docker run -v $(shell pwd)/Data:/app/Data --env INPUT_FILE_PATH=$(INPUT_FILE_PATH) --env OUTPUT_FILE_PATH=$(OUTPUT_FILE_PATH) --env MEMORY_LIMIT=$(MEMORY_LIMIT) --env TEMP_DIR=$(TEMP_DIR) -it --memory=2g --memory-swap=4g --rm my-large-file-sorter

run-gen:
	cd App.Cmd.Generator && dotnet run

run-sort:
	cd App.Cmd.Sorter && dotnet run
