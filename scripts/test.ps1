$dir = $PSScriptRoot
$root = Split-Path $dir -Parent
$src = Join-Path $root "src"
$testResultsOutputDirectory = Join-Path $root "test-results"

$env:DOCKER_BUILDKIT = "1"
$env:DOCKER_SCAN_SUGGEST = "false"

Push-Location $root

try
{
    docker build `
        --build-arg "BUILDKIT_INLINE_CACHE=1" `
        -f $src/Api/Dockerfile `
        -t payment-gateway:test-runner `
        --target test-runner `
        $src

    docker run `
        --rm -it `
        --name "payment-gateway-test-runner" `
        -v "${testResultsOutputDirectory}:/src/tests/Api.Specs/test-results" `
        payment-gateway:test-runner

    Write-Host "testResultsOutputDirectory: $testResultsOutputDirectory"
}
finally {
    Pop-Location
}
