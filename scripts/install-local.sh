#!/usr/bin/env bash
set -e

rm -Rf ./nupkg ./obj
dotnet pack -c Release -o ./nupkg
dotnet tool uninstall -g baseddd.cli || true
dotnet tool install -g --add-source ./nupkg baseddd.cli
