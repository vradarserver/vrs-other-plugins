# vrs-other-plugins
These are a collection of plugins that I've written on an ad-hoc basis to make small changes
to Virtual Radar Server's behaviour.

## How to Build
1. Install Visual Studio 2017 (the free version will be fine).

2. Visual Studio will download NuGet dependencies by default. If you have this switched off
then install the latest version of the **VirtualRadar.ProjectLibraries** NuGet package (if you want
to build the v3 betas then install the latest version 3 beta of the package).

3. Build the plugin from Visual Studio.

## How to Install a Plugin
1. Go to the installation folder of the Virtual Radar Server instance where you want to install the
plugin.

2. If there isn't one already then create a folder called **Plugins** within the VRS install folder.

3. In the Plugins folder create a folder for the plugin you want to install. The name doesn't matter.

Then copy these two files (and *just* these two files) into the plugin installation folder from the
plugin's build folder:

a. VirtualRadar.**PLUGIN NAME HERE**.dll

b. VirtualRadar.**PLUGIN NAME HERE**.xml

Note that the build folder will have other files and DLLs in addition to the plugin DLL and XML file.
You can ignore those files, they were only required to build and are not required at runtime.

Restart VRS if it's already running and you should see the plugin in the **Tools | Plugins** menu.

## Version 2 vs. Version 3
The plugin projects have a -v2 or -v3 suffix. These refer to the version of Virtual Radar Server that
they will work with. Version 2 is the current release version of VRS, version 3 is in beta.

The plugin specification is different between v2 and v3 so you need to make sure that you build and
install the correct version for the instance of Virtual Radar Server that you want to use with the
plugin.
