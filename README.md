# Olav

[![CI](https://github.com/your-org/olav/actions/workflows/ci.yml/badge.svg)](https://github.com/your-org/olav/actions/workflows/ci.yml)
[![NuGet](https://img.shields.io/nuget/v/Olav.Cli.svg)](https://www.nuget.org/packages/Olav.Cli)
[![NuGet Downloads](https://img.shields.io/nuget/dt/Olav.Cli.svg)](https://www.nuget.org/packages/Olav.Cli)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](./LICENSE)

> **Scaffold production-grade .NET APIs with strict Domain-Driven Design — from zero to architecture in one command.**

---

## Why Olav?

**Sir Nils Olav III** is a king penguin living at Edinburgh Zoo. He holds the rank of Brigadier Sir Nils Olav III, Knight of the Order of St. Olav, in the Norwegian King's Guard — a military rank he inherited after decades of ceremonial inspection and promotion by the Norwegian Royal Guard. He did not earn it through battle. He earned it by showing up, standing straight, and being inspected and found correct every single time.

**Saint Olav II** — Olav Haraldsson — was a Viking king who unified Norway under a single set of laws and a single faith. He didn't ask for permission. He built the structure first and let history decide whether it was right. It was. He's Norway's patron saint.

Olav the tool borrows from both. It doesn't fight you. It doesn't ask questions. It shows up, stands straight, and generates a project that passes inspection before you've written a single line of business logic. Like the saint, it imposes structure not to constrain you — but because structure is what makes things last.

---

## What is Olav?

Olav is a .NET CLI tool that generates production-ready API projects following strict **Domain-Driven Design (DDD)** architecture. Think of it as the [FastAPI](https://fastapi.tiangolo.com/) of the .NET world — opinionated, fast to start, and designed to enforce good practices from day one rather than letting them drift in over time.

It is aimed at **beginner to mid-level .NET developers** who want to start building APIs the right way without spending days configuring architecture, tests, Docker, and CI/CD pipelines from scratch.

A single command gives you:

- A layered DDD solution (`Domain`, `Application`, `Infrastructure`, `Web`)
- Architecture tests that **fail the build** if your layers talk to the wrong neighbours
- Integration tests wired and ready
- Observability middleware out of the box
- Docker and Docker Compose configuration
- GitHub Actions CI/CD pipeline
- Git hooks enforcing code quality before every push
- A `olav.json` contract file tracking your project's template version

---

## Prerequisites

| Tool     | Version            |
| -------- | ------------------ |
| .NET SDK | 10.0+              |
| Docker   | Any recent version |
| Git      | Any recent version |

---

## Installation

```bash
dotnet tool install --global Olav.Cli
```

---

## Quick Start

```bash
olav new MyApi
```

That's it. You now have a fully structured, architecture-tested, Docker-ready .NET API.

### Options

```bash
olav new MyApi --owner "Acme Corp" --license "MIT"
```

| Option      | Default          | Description                                          |
| ----------- | ---------------- | ---------------------------------------------------- |
| `--owner`   | Your OS username | Sets the copyright owner in file headers and license |
| `--license` | `MIT`            | Sets the license type                                |

---

## Generated Project Structure

```
MyApi/
├── src/
│   ├── MyApi.Domain/           # Entities, value objects, domain events — no dependencies
│   ├── MyApi.Application/      # Use cases, handlers, interfaces — depends only on Domain
│   ├── MyApi.Infrastructure/   # Repositories, services, external integrations
│   └── MyApi.Web/              # API controllers, middleware, observability, entry point
├── tests/
│   ├── MyApi.ArchitectureTests/   # Enforces DDD layer rules at build time
│   └── MyApi.IntegrationTests/    # Integration tests wired and ready
├── docker/
├── Directory.Build.props
├── Directory.Packages.props
├── global.json
└── olav.json                   # Template version contract
```

---

## Architecture Enforcement

This is the core of what Olav gives you. The generated `ArchitectureTests` project runs on every build and **fails loudly** if your code breaks DDD rules. There is no silent drift.

Rules enforced out of the box:

| Rule                                   | What it prevents                                        |
| -------------------------------------- | ------------------------------------------------------- |
| `Domain` has no outward dependencies   | Domain referencing Infrastructure or Application        |
| `Application` depends only on `Domain` | Application importing Infrastructure directly           |
| All handlers live in `Application`     | Business logic leaking into Web or Infrastructure       |
| All services live in `Infrastructure`  | Infrastructure concerns bleeding into Application       |
| `Web` is the only entry point          | Controllers or middleware defined outside the Web layer |

In a future version these rules will also be enforced at development time via **Roslyn analyzers**, catching violations before the build even runs.

---

## Commands

### `olav new`

Generates a new DDD API project.

```bash
olav new MyApi
olav new MyApi --owner "Acme Corp" --license "Apache-2.0"
```

### `olav lint`

Validates your project's folder structure and layer organisation against Olav's rules.

```bash
olav lint
```

Run this in CI to catch structural regressions before they merge.

### `olav verify`

Validates architecture rules and confirms all required tests are present and passing.

```bash
olav verify
```

Stricter than lint — this is your full architectural health check.

### `olav migrate`

Shows pending template upgrades for your generated project. Use `--apply` to write the changes.

```bash
olav migrate          # dry run — shows what would change
olav migrate --apply  # applies the migration
```

Olav tracks the template version your project was generated with inside `olav.json`. When the tool is updated with new conventions, `migrate` brings your project up to date without you having to start over.

### `olav doctor` _(planned)_

Diagnoses missing observability configuration, bad environment setup, or misconfigured tooling.

---

## The `olav.json` Contract

Every generated project contains an `olav.json` at its root:

```json
{
  "toolVersion": "0.1.0",
  "templateVersion": "1.0",
  "createdAt": "2025-01-15T10:30:00Z",
  "updatedAt": "2025-01-15T10:30:00Z"
}
```

This file is the handshake between your project and the tool. It lets `verify`, `lint`, and `migrate` know exactly what version of the conventions your project was built against — and what needs updating when conventions evolve.

---

## Roadmap

Olav is at `v0.1.0`. The foundation is solid. Here's what's coming:

| Feature                        | Description                                                             |
| ------------------------------ | ----------------------------------------------------------------------- |
| **Roslyn Enforcement**         | Catch layer violations at dev time, not just build time                 |
| **Doctor Mode**                | Diagnose observability gaps, missing config, and setup issues           |
| **Plugin System**              | Add infrastructure plugins (Postgres, Redis, RabbitMQ) with one command |
| **Modular Monolith**           | Generate modular monolith structures alongside standard DDD             |
| **Multi-Environment Support**  | Environment-aware configuration scaffolding                             |
| **Upgrade Command**            | Upgrade generated projects to newer Olav conventions                    |
| **Security Enforcement**       | Enforce secure defaults in generated projects                           |
| **Observability Hardening**    | Structured logging, tracing, and metrics wired by default               |
| **Code Scaffolding**           | Generate entities, handlers, and repositories inside existing projects  |
| **API Contract Enforcement**   | Enforce API versioning and contract stability                           |
| **Performance Baseline Suite** | Generate performance benchmarks alongside architecture tests            |
| **Modes System**               | Switch between DDD modes (strict, relaxed, modular)                     |
| **Documentation Generator**    | Auto-generate API documentation from your domain model                  |
| **Benchmark Suite**            | Built-in benchmarking scaffolding with BenchmarkDotNet                  |

---

## Contributing

Olav is open source and welcomes contributions. To get started locally:

```bash
git clone https://github.com/your-org/olav
cd olav
git config core.hooksPath .githooks
chmod +x .githooks/pre-commit
chmod +x .githooks/pre-push
```

To install locally for testing:

```bash
chmod +x scripts/install-local.sh
./scripts/install-local.sh
```

Or manually:

```bash
dotnet build
dotnet pack
dotnet tool uninstall --global Olav.Cli
dotnet tool update --global --add-source ./nupkg Olav.Cli
```

---

## License

MIT © [Lucas Caovilla](https://github.com/lucascaovilla)

---

<p align="center">
  Named after <a href="https://en.wikipedia.org/wiki/Nils_Olav">Sir Nils Olav III</a>, Brigadier of the Norwegian King's Guard,<br/>
  and <a href="https://en.wikipedia.org/wiki/Olaf_II_of_Norway">Saint Olav II</a>, Viking king and patron saint of Norway.<br/>
  Both imposed order. Both were right.
</p>
