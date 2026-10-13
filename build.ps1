$ErrorActionPreference = 'Stop'
$compiler = Join-Path $env:WINDIR 'Microsoft.NET\Framework64\v4.0.30319\csc.exe'
New-Item -ItemType Directory -Force (Join-Path $PSScriptRoot 'dist') | Out-Null
& $compiler /nologo /target:winexe /optimize+ /out:"$PSScriptRoot\dist\gameselector.exe" /reference:System.Windows.Forms.dll /reference:System.Drawing.dll "$PSScriptRoot\Launcher.cs"
if ($LASTEXITCODE -ne 0) { throw 'Build failed' }
Copy-Item -LiteralPath "$PSScriptRoot\README.md" -Destination "$PSScriptRoot\dist\README.md"
