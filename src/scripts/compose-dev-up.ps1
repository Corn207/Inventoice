$dockerRoot = Resolve-Path -Path (Join-Path -Path $PSScriptRoot -ChildPath '..')
$composePath = Resolve-Path -Path (Join-Path -Path $dockerRoot -ChildPath 'docker-compose.yml')
$composeDevPath = Resolve-Path -Path (Join-Path -Path $dockerRoot -ChildPath 'docker-compose.override.yml')
docker compose build
docker compose -f $composePath -f $composeDevPath up --detach
