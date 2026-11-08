$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest

$projectRoot = Split-Path -Parent $PSScriptRoot
$compiler = @(
    (Join-Path $env:WINDIR 'Microsoft.NET\Framework64\v4.0.30319\csc.exe'),
    (Join-Path $env:WINDIR 'Microsoft.NET\Framework\v4.0.30319\csc.exe')
) | Where-Object { Test-Path -LiteralPath $_ } | Select-Object -First 1

if ([string]::IsNullOrWhiteSpace($compiler)) { throw 'ไม่พบ .NET Framework C# compiler' }

$outputDirectory = Join-Path $projectRoot 'obj\tests'
$testExecutable = Join-Path $outputDirectory 'UpdateCheckerVersionTest.exe'
New-Item -ItemType Directory -Force -Path $outputDirectory | Out-Null

& $compiler /nologo /target:exe /optimize+ `
    /out:"$testExecutable" `
    /reference:System.dll `
    /reference:System.Core.dll `
    /reference:System.Runtime.Serialization.dll `
    (Join-Path $projectRoot 'src\UpdateChecker.cs') `
    (Join-Path $projectRoot 'tests\UpdateCheckerVersionTest.cs')
if ($LASTEXITCODE -ne 0) { throw 'Compile UpdateChecker tests ไม่สำเร็จ' }

& $testExecutable
if ($LASTEXITCODE -ne 0) { throw 'UpdateChecker tests ไม่ผ่าน' }
