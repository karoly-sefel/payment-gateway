param(
    $version = "latest"
)

$dir = $PSScriptRoot
$root = Split-Path $dir -Parent
$src = "$root/src"

Push-Location $root

try
{
    docker build `
        --build-arg "BUILDKIT_INLINE_CACHE=1" `
        -t bank-simulator:$version `
        -f $src/BankSimulator/Dockerfile `
        $src

    $port = 5500
    Write-Host "Bank Simulator API running on http://localhost:$port"
    docker run --rm -it --name "bank-simulator" -p ${port}:80 bank-simulator:${version}
}
finally {
    Pop-Location
}
