param(
    [switch]$DesktopShortcut
)

$ErrorActionPreference = 'Stop'
$sourceExe = Join-Path $PSScriptRoot 'dist\CapiQR.exe'
$installDirectory = Join-Path $env:LOCALAPPDATA 'CapiQR'
$targetExe = Join-Path $installDirectory 'CapiQR.exe'

if (-not (Test-Path -LiteralPath $sourceExe)) {
    throw 'ไม่พบ dist\CapiQR.exe — ให้รัน build.ps1 ก่อน'
}

if (Get-Process -Name 'CapiQR' -ErrorAction SilentlyContinue) {
    throw 'กรุณา Exit CapiQR จาก System tray ก่อนติดตั้งหรืออัปเดต'
}

New-Item -ItemType Directory -Force -Path $installDirectory | Out-Null
Copy-Item -LiteralPath $sourceExe -Destination $targetExe -Force

$shell = New-Object -ComObject WScript.Shell
$startMenu = [Environment]::GetFolderPath('Programs')
$shortcut = $shell.CreateShortcut((Join-Path $startMenu 'CapiQR.lnk'))
$shortcut.TargetPath = $targetExe
$shortcut.WorkingDirectory = $installDirectory
$shortcut.Description = 'CapiQR — Scan QR codes from your Windows screen'
$shortcut.IconLocation = "$targetExe,0"
$shortcut.Save()

if ($DesktopShortcut) {
    $desktop = [Environment]::GetFolderPath('Desktop')
    $desktopLink = $shell.CreateShortcut((Join-Path $desktop 'CapiQR.lnk'))
    $desktopLink.TargetPath = $targetExe
    $desktopLink.WorkingDirectory = $installDirectory
    $desktopLink.Description = 'CapiQR — Scan QR codes from your Windows screen'
    $desktopLink.IconLocation = "$targetExe,0"
    $desktopLink.Save()
}

Start-Process -FilePath $targetExe
Write-Host "ติดตั้ง CapiQR แล้ว: $targetExe"
