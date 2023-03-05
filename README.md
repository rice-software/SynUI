# SynUI
the predecessor to [BetterSynapse](https://github.com/rice-cracker-2234/BetterSynapse).

<a name=""></a>
## Table of contents 
1. [Table of contents](#table-of-contents)
2. [Installtion](#installation)
	1. [Prerequisite](#prerequisite)
	2. [Installation](#installation-1)
3. [Building from source](#building-from-source)
	1. [Prerequisite](#prerequisite-1)
	2. [Manually](#manually)
4. [Bug reporting](#bug-reporting)
5. [Known issues](#known-issues)
6. [Licensing](#licensing)

## Installation 
### Prerequisite 
- Windows 10 and up
- [.NET Framework 4.8.1](https://dotnet.microsoft.com/en-us/download/dotnet-framework/net481)
- [Synapse X](https://x.synapse.to) (with a registered account)

### Installation 
- Download SynUI from the [release](https://github.com/Rice-Software/SynUI/releases) page.
  - Alternatively, you can download the development builds [here](https://github.com/Rice-Software/SynUI/actions).
- Extract the zip file.
- Launch SynUI.exe
- profits.

## Building from source 
### Prerequisite 
- [MSBuild]()
- [.NET Framework 4.8.1](https://dotnet.microsoft.com/en-us/download/dotnet-framework/net481)
- An IDE/Code Editor (preferably [Visual Studio](https://visualstudio.microsoft.com/))

### Manually 
- Clone this project
```
git clone https://github.com/Rice-Software/SynUI.git
cd ./SynUI
```

- Restore Nuget packages
```
msbuild /t:Restore
```

- Build the project
```
msbuild /t:Build
```

## Bug reporting 
If you encounter a bug, please make an [issue](https://github.com/Rice-Software/SynUI/issues).

## Known issues 
### SynUI still runs after closing
- This is an issue from SxLib.

## Licensing 
This project is licensed under the [GNU General Public License v3.0](https://choosealicense.com/licenses/gpl-3.0/) license. Any forks of this project must be disclosed.
