#define public Root       "..\.."
#define public BuildType  "Release"
#define public Plugin     "{app}\Plugins\AirportDataThumbnails"
#ifndef VERSION
  #define public VERSION    "v2"
#endif

[Setup]
AppName=Airport Data Thumbnails VRS Plugin
AppVerName=Airport Data Thumbnails VRS Plugin {#VERSION}
DefaultDirName={autopf}\VirtualRadar
DefaultGroupName=Virtual Radar
DisableDirPage=no
InfoBeforeFile=Plugin-AirportDataThumbnails-VersionHistory.rtf
LicenseFile={#Root}\License.txt
; .NET 4.6.1 minimum version is Windows 7 SP1
MinVersion=6.1.7601
OutputBaseFileName=Plugin-AirportDataThumbnails-{#VERSION}
SetupIconFile=..\Resources\Application.ico
WizardImageFile=..\Resources\WizardImage.bmp
WizardSmallImageFile=..\Resources\WizardSmallImage.bmp

[Messages]
WizardInfoBefore=Version History
InfoBeforeLabel=What has changed?

[Files]
; License and README
Source: "{#Root}\LICENSE.txt"; DestDir: "{#Plugin}"; Flags: ignoreversion;
Source: "{#Root}\Plugin.AirportDataThumbnails-v2\README.md"; DestDir: "{#Plugin}"; Flags: ignoreversion;

; Application files
Source: "{#Root}\Plugin.AirportDataThumbnails-v2\bin\{#BuildType}\VirtualRadar.Plugin.AirportDataThumbnails.dll"; DestDir: "{#Plugin}"; Flags: ignoreversion;

; Manifest file
Source: "{#Root}\Plugin.AirportDataThumbnails-v2\bin\{#BuildType}\VirtualRadar.Plugin.AirportDataThumbnails.xml"; DestDir: "{#Plugin}"; Flags: ignoreversion;
