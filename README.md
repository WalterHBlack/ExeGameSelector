# ExeGameSelector

ExeGameSelector is an executable selector for people who want to launch games through Steam and have their playtime tracked.

## How to use

First, download the executable or ZIP from the Releases page. Then, extract it into the folder of the Steam game whose playtime you want to track.  <img width="1692" height="1003" alt="Ekran görüntüsü 2026-10-14 031200" src="https://github.com/user-attachments/assets/5c6f67af-e339-4b09-837d-db6bde7ee832" /> (I'm using Sonic Mania for instance). Then you need to rename gameselector.exe to your games exe name (you have to do this for opening selector on steam). <img width="1689" height="998" alt="Ekran görüntüsü 2026-10-14 031236" src="https://github.com/user-attachments/assets/3b2efa88-e618-4316-bba1-f64afe32169f" />
Then you have to select your games folder from common file.  


## Requirements and building

Windows with .NET Framework 4.5 or later. Windows 10/11 installations with .NET Framework 4.8 are suitable.

Build from PowerShell:

```powershell
powershell -ExecutionPolicy Bypass -File .\build.ps1
```

- `Launcher.cs`: interface, scanning, saved preferences, and game launching.
- `build.ps1`: builds `dist/gameselector.exe` with the Windows .NET Framework compiler.
- `Tests.cs`: checks scanning, per-name settings, non-destructive hiding, and process launching.
