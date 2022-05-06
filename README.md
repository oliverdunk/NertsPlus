# NertsPlus

NertsPlus is an open source mod for the Steam game [NERTS! Online](https://store.steampowered.com/app/1131190/NERTS_Online/). It aims to add a number of features and new gameplay mechanics.

<img src="images/screenshot.png" width="500px">

Read more on my blog: https://oliverdunk.com/2022/05/06/nerts-plus

NERTS! Online is owned by [Zachtronics](https://www.zachtronics.com/). NertsPlus is a community creation and is not associated with Zachtronics in any way. Please avoid sending support questions their way when using NertsPlus as it is likely not something that they want to be burdened with.

## Current Features

NertsPlus currently has the following features:

- Changing/skipping the intro music before each round
- Allowing the host to force a shuffle at any point (press ';')
- Exporting scores as CSV or JSON
- Adds a number of fun loading messages in place of the normal "Shuffling cards..."

## Usage

1. Install the latest version of the .NET framework.
1. Install NERTS! Online.
1. If running on macOS, install [Mono 4.6.2](https://download.mono-project.com/archive/4.6.2/) which is known to work with the latest version of NERTS! Online.
1. Run `python3 patch.py` to patch the Nerts executable and build NertsPlus.
1. Run `python3 run.py` to start the game.

If everything is working, the mod should now be loaded.

Note: NertsPlus is automatically disabled in public lobbies.

### Development

#### Opening NertsPlus in Visual Studio on macOS

On macOS, Visual Studio requires a Mono version much higher than 4.6.2. The easiest way to resolve this is installing the latest version and then using a script to change the `Current/` symlink temporarily as Visual Studio opens:

```
sudo ln -nsf /Library/Frameworks/Mono.framework/Versions/6.12.0 /Library/Frameworks/Mono.framework/Versions/Current
open "/Applications/Visual Studio.app"

read -p "Press any key to restore Mono..."
sudo ln -nsf /Library/Frameworks/Mono.framework/Versions/4.6.2 /Library/Frameworks/Mono.framework/Versions/Current
```
