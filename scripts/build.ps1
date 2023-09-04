param(
    $version = "latest"
)

$dir = $PSScriptRoot
$root = Split-Path $dir -Parent
$src = Join-Path $root "src"

$env:DOCKER_BUILDKIT = "1"
$env:DOCKER_SCAN_SUGGEST = "false"

Push-Location $root

try
{
    $src

    docker build `
        --build-arg "BUILDKIT_INLINE_CACHE=1" `
        -t payment-gateway:$version `
        -f $src/Api/Dockerfile `
        $src
}
finally {
    Pop-Location
}



