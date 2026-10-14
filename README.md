# ExeGameSelector

ExeGameSelector is a lightweight launcher for people who want to launch games through Steam and track their playtime.

## How to Use

First, download the ZIP file from the Releases page and extract it into the folder of the Steam game whose playtime you want to track. <img width="1692" height="1003" alt="GameSelector folder setup" src="https://github.com/user-attachments/assets/5c6f67af-e339-4b09-837d-db6bde7ee832" /> Then rename `gameselector.exe` to the name of the game's executable. This is required for Steam to launch the selector correctly. <img width="1689" height="998" alt="GameSelector executable setup" src="https://github.com/user-attachments/assets/3b2efa88-e618-4316-bba1-f64afe32169f" />
Then open the renamed selector and choose the game's folder.(You have to extract the files of the games you want to play to your game's folder.)<img width="779" height="503" alt="image" src="https://github.com/user-attachments/assets/ba65ae9d-d8b2-4ba8-840e-a558505d362f" />
After selecting folder, you can select the game you want to play and that's it.

## Requirements and Building

Windows with .NET Framework 4.5 or later. Windows 10/11 installations with .NET Framework 4.8 are suitable.

Build from PowerShell:

```powershell
powershell -ExecutionPolicy Bypass -File .\build.ps1
```

- `Launcher.cs`: interface, scanning, saved preferences, and game launching.
- `build.ps1`: builds `dist/gameselector.exe` with the Windows .NET Framework compiler.
- `Tests.cs`: checks scanning, per-name settings, non-destructive hiding, and process launching.
