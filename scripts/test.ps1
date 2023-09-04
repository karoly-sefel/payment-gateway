param(
    [ValidateSet("Build", "Run", "BuildAndRun")]
    [string]$action = "BuildAndRun"
)

$dir = $PSScriptRoot
$root = Split-Path $dir -Parent
$src = Join-Path $root "src"
$testResultsOutputDirectory = Join-Path $root "test-results"

$env:DOCKER_BUILDKIT = "1"
$env:DOCKER_SCAN_SUGGEST = "false"

function Build-Tests {
    docker build `
        --build-arg "BUILDKIT_INLINE_CACHE=1" `
        -f $src/Api/Dockerfile `
        -t payment-gateway:test-runner `
        --target test-runner `
        $src
}

function Run-Tests {
    docker run `
        --rm `
        --name "payment-gateway-test-runner" `
        -v "${testResultsOutputDirectory}:/src/tests/Api.Specs/test-results" `
        payment-gateway:test-runner

    Write-Host "testResultsOutputDirectory: $testResultsOutputDirectory"
}

Push-Location $root

try
{
    switch ($action)
    {
        "Build" { Build-Tests }
        "Run" { Run-Tests }
        "BuildAndRun" {
            Build-Tests
            Run-Tests
        }
        Default { Write-Host "Invalid -action parameter" }
    }
}
finally {
    Pop-Location
}
