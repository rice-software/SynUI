; Script generated by the Inno Setup Script Wizard.
; SEE THE DOCUMENTATION FOR DETAILS ON CREATING INNO SETUP SCRIPT FILES!

#define MyAppName "SynUI"
#define MyAppRootDirectory "..\.."
#define MyAppOutputDirectory MyAppRootDirectory + "\Output"
#define MyAppReleaseDirectory MyAppRootDirectory + "\" + MyAppName + "\bin\Release"
#define MyAppFileName MyAppName + ".exe"
#define MyAppFilePath MyAppReleaseDirectory + "\" + MyAppFileName
#define MyAppVersion GetStringFileInfo(MyAppFilePath, "ProductVersion")
#define MyAppPublisher "rice-software"
#define MyAppURL "https://rice-software.xyz"
#define MyAppExeName "SynUI.exe"

[Setup]
; NOTE: The value of AppId uniquely identifies this application. Do not use the same AppId value in installers for other applications.
; (To generate a new GUID, click Tools | Generate GUID inside the IDE.)
AppId = {{60D9E8D6-5964-4C7F-96FE-B6EF80B1B2D1}
AppName = {#MyAppName}
AppVersion = {#MyAppVersion}
;AppVerName={#MyAppName} {#MyAppVersion}
AppPublisher = {#MyAppPublisher}
AppPublisherURL = {#MyAppURL}
AppSupportURL = {#MyAppURL}
AppUpdatesURL = {#MyAppURL}
DefaultDirName = {autopf}\{#MyAppName}
DisableProgramGroupPage = yes
; Uncomment the following line to run in non administrative install mode (install for current user only.)
PrivilegesRequired = admin
OutputBaseFilename = setup
OutputDir = {#MyAppOutputDirectory}
Compression = lzma
SolidCompression = yes
WizardStyle = modern

[Languages]
Name : "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name : "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[Files]
Source : "{#MyAppFilePath}"; DestDir: "{app}"; Flags: ignoreversion
Source : "{#MyAppReleaseDirectory}\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs
; NOTE: Don't use "Flags: ignoreversion" on any shared system files

[Icons]
Name : "{autoprograms}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name : "{autodesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon

[Run]
Filename : "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent runascurrentuser

