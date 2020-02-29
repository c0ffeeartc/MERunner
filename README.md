# MERunner
Modular Entitas Runner

![](https://img.shields.io/apm/l/vim-mode.svg)

Runs systems from plugin dlls in order defined by settings file.

Uses [Entitas](https://github.com/sschmid/Entitas-CSharp) with [EntitasGenericAddon](https://github.com/c0ffeeartc/EntitasGenericAddon) to allow extension by separate dll.

## How it works
  - Parses settings file into `Dictionary<String,List<String>>`
  - Imports `ISystem_Factory` classes from `MERunner.*` plugin dlls
  - Runs systems in order specified by settings file

## Usage
  - Build MERunner
  - Build `MERunner.*` plugin dlls in separate solution(ensure all runtime references are specified for each project)
  - Copy MERunner files and plugin files into same folder
  - Create settings file
  - Run `mono MERunner.exe --SettingsPath=pathToSettingsFile`
  
## Examples
  - [MERunner.Hello](https://github.com/c0ffeeartc/MERunner.Hello)
  - [MERunner.GenEntitas](https://github.com/c0ffeeartc/MERunner.GenEntitas)
