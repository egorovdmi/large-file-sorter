# large-file-sorter

Application that can sort large files up to 100GB.

## CONTENTS

Files and directories:

      App.Cmd.Generator/            Generator application
      App.Cmd.Sorter/               Sorter application
      Business/                     Business logic
      Foundation/                   Foundation/Kit packages
      Data/                         Default directory for writing output files

## REQUIREMENTS

- Visual Studio Code.
- .NET Core 7 SDK x64.
- Internet access for packages downloading.

## IMPORTANT!!! FOR WINDOWS

**.NET Core 7 SDK x64** must be installed and set in PATH of environment to run in x64 mode to increase performance and address more physical memory (otherwise it would fail with OutOfMemory exception).

1. Open VS Code in the source folder.
2. Run the following commands in a terminal of VS Code:

#### To run the generator with a default preset:

```
./run-gen
```

#### To run the sorter with a default preset:

```
./run-sort
```

#### To run the sorter in x86 mode (with low memory) with a default preset:

```
./run-sort-x86
```

#### To run the generator in docker:

```
./run-gen-docker
```

#### To run the sorter in docker:

```
./run-sort-docker
```

## Mac OS or Linux

.NET Core 7 SDK must be installed to run without docker.

1. Open VS Code in the source folder.
2. Run the following commands in a terminal of VS Code:

#### To run the generator with a default preset:

```
make run-gen
```

#### To run the sorter with a default preset (32GB memory settings, adjust the bat file if needed):

```
make run-sort
```

#### To run the generator in docker:

```
make run-gen-docker
```

#### To run the sorter in docker:

```
make run-sort-docker
```
