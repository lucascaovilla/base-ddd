#!/usr/bin/env bash
set -e

VERSION=$(grep -oP '(?<=<Version>)[^<]+' src/Olav.Cli/Olav.Cli.csproj)

rm -Rf ./nupkg ./obj
dotnet pack src/Olav.Cli/Olav.Cli.csproj -c Release -o ./nupkg
dotnet tool uninstall -g olav.cli || true
dotnet tool install -g --add-source ./nupkg olav.cli --version "$VERSION"

echo "Installed olav version $VERSION"
