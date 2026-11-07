$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest

$projectRoot = Split-Path -Parent $MyInvocation.MyCommand.Path
$compiler = 'C:\Windows\Microsoft.NET\Framework64\v4.0.30319\csc.exe'
$zxing = Join-Path $projectRoot 'vendor\ZXing.Net\lib\net48\zxing.dll'
$outputDirectory = Join-Path $projectRoot 'dist'
$objectDirectory = Join-Path $projectRoot 'obj'
$icon = Join-Path $projectRoot 'assets\CapiQR.ico'

if (-not (Test-Path -LiteralPath $compiler)) { throw 'ไม่พบ .NET Framework C# compiler' }
if (-not (Test-Path -LiteralPath $zxing)) { throw 'ไม่พบ vendor\ZXing.Net\lib\net48\zxing.dll' }

New-Item -ItemType Directory -Force -Path $outputDirectory, $objectDirectory, (Split-Path -Parent $icon) | Out-Null

& $compiler /nologo /target:exe /optimize+ /out:"$objectDirectory\GenerateIcon.exe" /reference:System.Drawing.dll "$projectRoot\tools\GenerateIcon.cs"
if ($LASTEXITCODE -ne 0) { throw 'สร้าง Icon tool ไม่สำเร็จ' }
& "$objectDirectory\GenerateIcon.exe" $icon
if ($LASTEXITCODE -ne 0) { throw 'สร้าง CapiQR.ico ไม่สำเร็จ' }

$sourceFiles = Get-ChildItem -LiteralPath (Join-Path $projectRoot 'src') -Filter '*.cs' | ForEach-Object FullName
$compileArguments = @(
    '/nologo',
    '/target:winexe',
    '/platform:anycpu',
    '/optimize+',
    '/debug:pdbonly',
    "/out:$outputDirectory\CapiQR.exe",
    "/pdb:$objectDirectory\CapiQR.pdb",
    "/win32icon:$icon",
    "/win32manifest:$projectRoot\app.manifest",
    "/resource:$zxing,CapiQR.Dependencies.zxing.dll",
    "/reference:$zxing",
    '/reference:System.dll',
    '/reference:System.Core.dll',
    '/reference:System.Drawing.dll',
    '/reference:System.Windows.Forms.dll'
) + $sourceFiles

& $compiler $compileArguments
if ($LASTEXITCODE -ne 0) { throw 'Build CapiQR ไม่สำเร็จ' }

Copy-Item -LiteralPath (Join-Path $projectRoot 'README.md') -Destination (Join-Path $outputDirectory 'README.md') -Force
Copy-Item -LiteralPath (Join-Path $projectRoot 'vendor\ZXing.Net\LICENSE.txt') -Destination (Join-Path $outputDirectory 'ZXing.Net-LICENSE.txt') -Force

$portableZip = Join-Path $outputDirectory 'CapiQR-Portable-v1.0.0.zip'
if (Test-Path -LiteralPath $portableZip) { Remove-Item -LiteralPath $portableZip -Force }
Compress-Archive -LiteralPath @(
    (Join-Path $outputDirectory 'CapiQR.exe'),
    (Join-Path $outputDirectory 'README.md'),
    (Join-Path $outputDirectory 'ZXing.Net-LICENSE.txt')
) -DestinationPath $portableZip -CompressionLevel Optimal

Write-Host "Build complete: $outputDirectory\CapiQR.exe"
Write-Host "Portable package: $portableZip"
