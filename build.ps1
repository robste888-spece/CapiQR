param(
    [string]$Version,
    [string]$OutputPath
)

$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest

$projectRoot = Split-Path -Parent $MyInvocation.MyCommand.Path
$versionFile = Join-Path $projectRoot 'VERSION'
$compilerCandidates = @(
    (Join-Path $env:WINDIR 'Microsoft.NET\Framework64\v4.0.30319\csc.exe'),
    (Join-Path $env:WINDIR 'Microsoft.NET\Framework\v4.0.30319\csc.exe')
)
$compiler = $compilerCandidates | Where-Object { Test-Path -LiteralPath $_ } | Select-Object -First 1
$zxing = Join-Path $projectRoot 'vendor\ZXing.Net\lib\net48\zxing.dll'
$outputDirectory = if ([string]::IsNullOrWhiteSpace($OutputPath)) { Join-Path $projectRoot 'dist' } else { $OutputPath }
$objectDirectory = Join-Path $projectRoot 'obj'
$icon = Join-Path $projectRoot 'assets\CapiQR.ico'

if ([string]::IsNullOrWhiteSpace($Version)) {
    if (-not (Test-Path -LiteralPath $versionFile)) { throw 'ไม่พบไฟล์ VERSION' }
    $Version = (Get-Content -LiteralPath $versionFile -Raw).Trim()
}

$Version = $Version.Trim()
if ($Version -notmatch '^(0|[1-9]\d*)\.(0|[1-9]\d*)\.(0|[1-9]\d*)$') {
    throw "Version ต้องเป็นรูปแบบ Major.Minor.Patch เช่น 1.1.0 (ได้รับ: $Version)"
}

$versionParts = @($Matches[1], $Matches[2], $Matches[3]) | ForEach-Object { [int]$_ }
if ($versionParts | Where-Object { $_ -gt 65535 }) {
    throw 'ตัวเลขแต่ละส่วนของ Version ต้องไม่เกิน 65535'
}
$fileVersion = "$Version.0"

if ([string]::IsNullOrWhiteSpace($compiler)) { throw 'ไม่พบ .NET Framework C# compiler' }
if (-not (Test-Path -LiteralPath $zxing)) { throw 'ไม่พบ vendor\ZXing.Net\lib\net48\zxing.dll' }

New-Item -ItemType Directory -Force -Path $outputDirectory, $objectDirectory, (Split-Path -Parent $icon) | Out-Null

$generatedVersionInfo = Join-Path $objectDirectory 'VersionInfo.generated.cs'
@"
using System.Reflection;
[assembly: AssemblyVersion("$fileVersion")]
[assembly: AssemblyFileVersion("$fileVersion")]
[assembly: AssemblyInformationalVersion("$Version")]
"@ | Set-Content -LiteralPath $generatedVersionInfo -Encoding UTF8

& $compiler /nologo /target:exe /optimize+ /out:"$objectDirectory\GenerateIcon.exe" /reference:System.Drawing.dll "$projectRoot\tools\GenerateIcon.cs"
if ($LASTEXITCODE -ne 0) { throw 'สร้าง Icon tool ไม่สำเร็จ' }
& "$objectDirectory\GenerateIcon.exe" $icon
if ($LASTEXITCODE -ne 0) { throw 'สร้าง CapiQR.ico ไม่สำเร็จ' }

$sourceFiles = @(
    Get-ChildItem -LiteralPath (Join-Path $projectRoot 'src') -Filter '*.cs' | ForEach-Object FullName
) + $generatedVersionInfo
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
    '/reference:System.Runtime.Serialization.dll',
    '/reference:System.Windows.Forms.dll'
) + $sourceFiles

& $compiler $compileArguments
if ($LASTEXITCODE -ne 0) { throw 'Build CapiQR ไม่สำเร็จ' }

Copy-Item -LiteralPath (Join-Path $projectRoot 'README.md') -Destination (Join-Path $outputDirectory 'README.md') -Force
Copy-Item -LiteralPath (Join-Path $projectRoot 'vendor\ZXing.Net\LICENSE.txt') -Destination (Join-Path $outputDirectory 'ZXing.Net-LICENSE.txt') -Force
Set-Content -LiteralPath (Join-Path $outputDirectory 'VERSION.txt') -Value $Version -Encoding ASCII

$portableZip = Join-Path $outputDirectory "CapiQR-Portable-v$Version.zip"
if (Test-Path -LiteralPath $portableZip) { Remove-Item -LiteralPath $portableZip -Force }
Compress-Archive -LiteralPath @(
    (Join-Path $outputDirectory 'CapiQR.exe'),
    (Join-Path $outputDirectory 'README.md'),
    (Join-Path $outputDirectory 'ZXing.Net-LICENSE.txt'),
    (Join-Path $outputDirectory 'VERSION.txt')
) -DestinationPath $portableZip -CompressionLevel Optimal

Write-Host "Build complete: $outputDirectory\CapiQR.exe (version $fileVersion)"
Write-Host "Portable package: $portableZip"
