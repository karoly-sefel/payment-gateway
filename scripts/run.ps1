param(
    $version = "latest"
)

$dir = $PSScriptRoot
$root = Split-Path $dir -Parent
$src = "$root/src"

Push-Location $root

try
{
    $port = 8000
    Write-Host "Payment Gateway API running on http://localhost:$port"
    docker run --rm -it --name "payment-gateway-api" -p ${port}:80 payment-gateway:${version}
}
finally {
    Pop-Location
}
