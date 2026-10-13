# GameSelector

GameSelector is a lightweight Windows launcher for selecting and launching locally installed games through Steam, including cracked games. Add it as a non-Steam shortcut, choose a game folder, and launch the desired executable from its automatically generated list.

## Features

- Automatically scans the selected folder and all subfolders for executable files.
- Remembers the selected folder separately for each launcher filename.
- Includes a dark interface and optional launch arguments.
- Hides unwanted executables from the selector without deleting them, with an option to restore all hidden entries.
- Launches games from their own working directories and stays open while the launched process runs.
- Requires no manually maintained game lists.

## Download and use

Download **GameSelector.zip** from [Releases](https://github.com/WalterHBlack/nobk-folder-launcher/releases/latest), extract it, and open **gameselector.exe**.

1. Select a game folder on first launch.
2. Select the game executable and click the launch button or double-click it.
3. Use the folder selection button whenever you want to change the saved folder.

The application interface is currently in Turkish. Its buttons provide folder selection, refresh, hide selected executable, restore hidden entries, and launch.

The folder and hidden entries are saved per launcher filename. Renaming the launcher to a new name creates a separate selection; returning to a previously used name restores that name's settings. Copies with the same name share settings for the current Windows user, regardless of their location. Settings are stored under `%LOCALAPPDATA%\GameSelector\Folders`.

Hiding affects only the selector list. Restoring hidden entries clears all hidden preferences for the current launcher name. Moving or renaming a hidden executable gives it a new path, so it may appear again. Folder links are not followed during scanning. The launcher itself and executables named `nobk.exe` or `gameselector.exe` are excluded from the list.

## Launch through Steam

Add GameSelector to Steam using **Add a Game → Add a Non-Steam Game**, enable Steam Overlay for the shortcut, and launch the shortcut from your Steam library. Select the game's executable inside GameSelector. Keep the selector open while playing.

GameSelector can also be used as an entry executable in an existing Steam launcher setup. Back up the original entry executable before replacing it with a renamed copy of GameSelector. It can also run independently of Steam.

Steam Overlay compatibility depends on the game, permissions, and launch chain. A game launcher that immediately hands off to another process may require selecting the actual game executable or keeping GameSelector open manually. GameSelector does not download games or modify their DRM.

## Requirements and building

Windows with .NET Framework 4.5 or later. Windows 10/11 installations with .NET Framework 4.8 are suitable.

Build from PowerShell:

```powershell
powershell -ExecutionPolicy Bypass -File .\build.ps1
```

- `Launcher.cs`: interface, scanning, saved preferences, and game launching.
- `build.ps1`: builds `dist/gameselector.exe` with the Windows .NET Framework compiler.
- `Tests.cs`: checks scanning, per-name settings, non-destructive hiding, and process launching.

## Contributors

- [WalterHBlack](https://github.com/WalterHBlack) — project owner and requirements.
- OpenAI Codex — AI-assisted development and implementation.
