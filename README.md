How to start developing:

Must run:
git config core.hooksPath .githooks
chmod +x .githooks/pre-commit

Install locally for testing:
chmod +x scripts/install-local.sh
./scripts/install-local.sh

Manually install:
dotnet build
dotnet pack
dotnet tool uninstall --global baseddd.cli
dotnet tool update --global --add-source ./nupkg BaseDDD.Cli

baseddd new <ProjectName>
baseddd lint
baseddd verify
baseddd doctor

| Command | Responsibility                           |
| ------- | ---------------------------------------- |
| new     | Generate strict project structure        |
| lint    | Validate folder + layer rules            |
| verify  | Validate architecture + required tests   |
| doctor  | Diagnose missing observability or config |
