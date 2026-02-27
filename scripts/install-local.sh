#!/usr/bin/env bash
set -e

dotnet pack -c Release -o ./nupkg
dotnet tool uninstall -g baseddd.cli || true
dotnet tool install -g --add-source ./nupkg baseddd.cli
