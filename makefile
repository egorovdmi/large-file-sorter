build-gen:
	docker build -t my-large-file-generator -f ./App.Cmd.Generator/Dockerfile .

run-gen-docker: build-gen
	docker run -v $(shell pwd)/Data:/Data --env FILE_SIZE_LIMIT=$(FILE_SIZE_LIMIT) --env OUTPUT_FILE_PATH=$(OUTPUT_FILE_PATH) -it --rm my-large-file-generator

run-gen:
	cd App.Cmd.Generator && dotnet run

run-sort:
	cd App.Cmd.Sorter && dotnet run
