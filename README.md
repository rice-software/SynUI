# SynUI
just another [Synapse X](https://x.synapse.to) ui lol.

## Table of contents {#table-of-contents}
1. [Table of contents](#table-of-contents)
2. [Installtion](#installation)
	1. [Prerequisite](#installation/prerequisite)
	2. [Installation](#installation/installation)
3. [Building from source](#building-from-source)
	1. [Prerequisite](#building-from-source/prerequisite)
	2. [Manually](#building-from-source/manually)
4. [Bug reporting](#bug-reporting)
5. [Licensing](#licensing)

## Installation {#installation}
### Prerequisite {#installation/prerequisite}
- Windows 10 and up
- [.NET Framework 4.8.1](https://dotnet.microsoft.com/en-us/download/dotnet-framework/net481)
- [Synapse X](https://x.synapse.to) (with a registered account)

### Installation {#installation/installation}
- Download SynUI from the [release](https://github.com/Rice-Software/SynUI/releases) page.
- Extract the zip file.
- Launch SynUI.exe
- profits.

## Building from source {#building-from-source}
### Prerequisite {#building-from-source/prerequisite}
- [MSBuild]()
- [.NET Framework 4.8.1](https://dotnet.microsoft.com/en-us/download/dotnet-framework/net481)
- An IDE/Code Editor (preferably [Visual Studio](https://visualstudio.microsoft.com/))

### Manually {#building-from-source/manually}
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

## Bug reporting {#bug-reporting}
If you encounter a bug, please make an [issue](https://github.com/Rice-Software/SynUI/issues).

## Licensing {#licensing}
This project is licensed under the [GNU General Public License v3.0](https://choosealicense.com/licenses/gpl-3.0/) license. Any forks of this project must be disclosed.