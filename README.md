# NertsPlus

NertsPlus is an open source mod for the Steam game [NERTS! Online](https://store.steampowered.com/app/1131190/NERTS_Online/). It aims to add a number of features and new gameplay mechanics.

NERTS! Online is owned by [Zachtronics](https://www.zachtronics.com/). NertsPlus is a community creation and is not associated with Zachtronics in any way. Please avoid sending support questions their way when using NertsPlus as it is likely not something that they want to be burdened with.

## Current Features

NertsPlus currently has the following features:

- Automatically skipping the long intro music before each round, and immediately starting the game when all players are ready
- Allowing the host to force a shuffle at any point using the Enter key

## Usage

1. Install NERTS! Online.
1. Install [Mono 4.6.2](https://download.mono-project.com/archive/4.6.2/) which is known to work with the latest version of NERTS! Online.
1. Install [de4dot](https://github.com/de4dot/de4dot). This needs to be built from source as it is no longer maintained.
1. Run [de4dot](https://github.com/de4dot/de4dot) on the NERTS! Online exe from your steamsapps folder (in `~/Library/Application Support/Steam/steamapps/common/Nerts Online/NERTS\! Online.app/Contents/MacOS` on macOS).
1. Download build 510 of the BepInEx .NET launcher from https://builds.bepinex.dev/projects/bepinex_be.
1. Extract all files from the zip (`BepInEx.NetLauncher.exe` and the contents of `BepInEx/core` in to the root folder containing your NERTS! Online executable).
1. Run `BepInEx.NetLauncher.exe` (using `mono64 BepInEx.NetLauncher.exe` on macOS).
1. Once it's run once to generate the config file, find your `BepInEx/config/BepInEx.cfg` file (in `/Library/Frameworks/Mono.framework` on macOS) and update the `Assembly` key under `Preloader.Entrypoint` to be your cleaned executable file.
1. Build NertsPlus (see below).
1. Copy the `NertsPlus.dll` file in to your `BepInEx/plugins` folder.
1. Run `BepInEx.NetLauncher.exe` again.

If everything is working, the mod should now be loaded.

### Building NertsPlus

NertsPlus should be built in Visual Studio.

Building requires a reference to your cleaned NERTS! Online executable. You can do this in Visual Studio by using "Project > Add Reference..." and adding your executable under the ".Net Assembly" tab.

#### macOS

On macOS, Visual Studio requires a Mono version much higher than 4.6.2. The easiest way to resolve this is installing the latest version and then using a script to change the `Current/` symlink temporarily as Visual Studio opens:

```
sudo ln -nsf /Library/Frameworks/Mono.framework/Versions/6.12.0 /Library/Frameworks/Mono.framework/Versions/Current
open "/Applications/Visual Studio.app"

read -p "Press any key to restore Mono..."
sudo ln -nsf /Library/Frameworks/Mono.framework/Versions/4.6.2 /Library/Frameworks/Mono.framework/Versions/Current
```
