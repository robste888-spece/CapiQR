$ErrorActionPreference = 'Stop'
$installDirectory = Join-Path $env:LOCALAPPDATA 'CapiQR'
$startMenuLink = Join-Path ([Environment]::GetFolderPath('Programs')) 'CapiQR.lnk'
$desktopLink = Join-Path ([Environment]::GetFolderPath('Desktop')) 'CapiQR.lnk'

if (Get-Process -Name 'CapiQR' -ErrorAction SilentlyContinue) {
    throw 'กรุณา Exit CapiQR จาก System tray ก่อนถอนการติดตั้ง'
}

Remove-ItemProperty -Path 'HKCU:\Software\Microsoft\Windows\CurrentVersion\Run' -Name 'CapiQR' -ErrorAction SilentlyContinue
Remove-Item -LiteralPath $startMenuLink -Force -ErrorAction SilentlyContinue
Remove-Item -LiteralPath $desktopLink -Force -ErrorAction SilentlyContinue
if (Test-Path -LiteralPath $installDirectory) {
    Remove-Item -LiteralPath $installDirectory -Recurse -Force
}
Write-Host 'ถอนการติดตั้ง CapiQR แล้ว'
