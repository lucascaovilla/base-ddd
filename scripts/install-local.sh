#!/usr/bin/env bash
set -e

VERSION=$(grep -oP '(?<=<Version>)[^<]+' src/BaseDDD.Cli/BaseDDD.Cli.csproj)

rm -Rf ./nupkg ./obj
dotnet pack src/BaseDDD.Cli/BaseDDD.Cli.csproj -c Release -o ./nupkg
dotnet tool uninstall -g baseddd.cli || true
dotnet tool install -g --add-source ./nupkg baseddd.cli --version "$VERSION"

echo "Installed baseddd version $VERSION"
