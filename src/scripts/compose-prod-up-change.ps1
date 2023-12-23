$dockerRoot = Resolve-Path -Path (Join-Path -Path $PSScriptRoot -ChildPath '..')
$composePath = Resolve-Path -Path (Join-Path -Path $dockerRoot -ChildPath 'docker-compose.yml')
$composeProdPath = Resolve-Path -Path (Join-Path -Path $dockerRoot -ChildPath 'docker-compose.prod.yml')
docker compose build webapi
docker compose -f $composePath -f $composeProdPath up --no-deps --detach webapi
