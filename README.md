# GradeBook — a console app with opinions (and tests)

A C#/.NET console grade book that lets an instructor create gradebooks, enroll students, record grades, calculate letter grades + GPAs, and persist everything to disk.

It’s small on purpose: the goal is to demonstrate clean OO design, an interactive command-driven UI, algorithmic grading logic, and automated tests—without hiding behind a framework. (In other words: no UI fluff, just the stuff employers actually hire for.)

## Quick pitch (for employers & recruiters)

This repository demonstrates:

- Object-oriented modeling with inheritance + polymorphism (`BaseGradeBook` → `StandardGradeBook` / `RankedGradeBook`).
- Practical input validation + guard-rail behavior (clear errors and user-facing messages).
- A simple command router and interactive CLI workflow (two-level UI: app-level + gradebook-level).
- JSON persistence with type-safe rehydration for multiple gradebook implementations.
- Unit tests (xUnit) validating contracts and algorithmic edge cases.

If you’re looking for evidence of “can ship working code, can test it, can design for change,” this is that—just in a smaller container.

## Features

- **Two gradebook types**
  - **Standard**: classic 90/80/70/60 thresholds.
  - **Ranked**: assigns letter grades by percentile (top 20% = A, next 20% = B, etc.).
- **Weighted GPA option**
  - Toggle per gradebook via `IsWeighted` (set at creation time).
  - Adds +1 GPA point for `Honors` and `DualEnrolled` students.
- **Student modeling**
  - `StudentType`: `Standard`, `Honors`, `DualEnrolled`.
  - `EnrollmentType`: `Campus`, `State`, `National`, `International`.
- **Statistics**
  - Per-student grades and GPA.
  - Aggregate averages broken down by enrollment and student type.
- **Persistence**
  - Saves to `<Name>.gdbk` (JSON) in the current working directory.
  - Loads by deserializing into the correct gradebook implementation.

## Design & development highlights

### Architecture (intentionally simple)

- **Domain layer**: `BaseGradeBook` holds student management + core calculation behavior.
- **Extensibility via overrides**: `RankedGradeBook` overrides grading/stats behavior while reusing shared logic.
- **Console UI**: two command routers
  - `StartingUserInterface`: create/load/help/quit.
  - `GradeBookUserInterface`: add/list/grades/statistics/save/close.

### Ranked grading (algorithm + guard rails)

- Uses sorting + percentile thresholds to place a score into A/B/C/D/F “buckets.”
- **Minimum cohort rule**: ranked grading requires at least **5 students**.
  - `GetLetterGrade` throws `InvalidOperationException` when violated.
  - Stats commands short-circuit with a friendly message instead of half-working.

### Persistence (JSON)

- Save uses `Newtonsoft.Json` serialization.
- Load converts JSON into the appropriate runtime type.
  - This is implemented with reflection-based type discovery.
  - It’s a pragmatic approach for a small project; in larger systems you’d likely use an explicit discriminator + custom `JsonConverter`.

## Tech stack

- Language: C#
- Runtime: .NET `net8.0`
- Serialization: Newtonsoft.Json
- Testing: xUnit

## Getting started

### Prerequisites

- .NET SDK 8.x (`dotnet --version` should show `8.*`)

### Build / run / test

From the repo root:

- Build: `dotnet build .\GradeBook.sln -c Release`
- Run: `dotnet run --project .\GradeBook\GradeBook.csproj -c Release`
- Test: `dotnet test .\GradeBookTests\GradeBookTests.csproj -c Release`

### Docker (optional)

The Docker image builds and runs the console app on .NET 8.

- Build the image:
  - `docker build -t gradebook .`

- Run it interactively (recommended for console apps):
  - PowerShell: `docker run -it --rm gradebook`
  - cmd.exe: `docker run -it --rm gradebook`

- Persist `.gdbk` save files by mounting the current folder into the container:
  - PowerShell: `docker run -it --rm -v ${PWD}:/work -w /work gradebook`
  - cmd.exe: `docker run -it --rm -v %cd%:/work -w /work gradebook`

## 2026 updates (portfolio refresh)

As of January 2026, this repo got a practical refresh focused on “easy to evaluate, easy to run”:

- **Modern Docker build**: updated to a .NET 8 multi-stage image that starts the app by default.
- **Employer-friendly docs**: expanded the README with architecture notes, command reference, and Docker usage.
- **Sample data stays committed**: `mybook.gdbk` is intentionally tracked so reviewers can `load mybook` immediately.

## Using the app (command reference)

Commands are case-insensitive; arguments are space-separated.

### App-level commands

- `create <Name> <Type> <Weighted>`
  - `<Type>`: `standard` | `ranked`
  - `<Weighted>`: `true` | `false`
  - Example: `create MyBook ranked true`
- `load <Name>`
  - Loads `<Name>.gdbk` from the working directory.
  - Example: `load mybook`
- `help`
- `quit`

### Gradebook-level commands

- `add <StudentName> <StudentType> <EnrollmentType>`
  - Example: `add Alice Honors Campus`
- `remove <StudentName>`
- `list`
- `addgrade <StudentName> <Score>`
  - Score is 0–100 (out of range throws an exception).
  - Example: `addgrade Alice 95`
- `removegrade <StudentName> <Score>`
- `statistics all`
- `statistics <StudentName>`
- `save`
- `close`
- `help`

## Sample data

There’s a tiny example gradebook included: [mybook.gdbk](mybook.gdbk)

- Try: `load mybook`
- Then: `statistics all`

## Project structure

- [GradeBook/](GradeBook/)
  - [GradeBook/GradeBooks/](GradeBook/GradeBooks/): domain implementations (`BaseGradeBook`, `StandardGradeBook`, `RankedGradeBook`)
  - [GradeBook/UserInterfaces/](GradeBook/UserInterfaces/): CLI command routing
  - [GradeBook/Enums/](GradeBook/Enums/): enums for types and classifications
  - [GradeBook/Student.cs](GradeBook/Student.cs): student entity + grade tracking
- [GradeBookTests/](GradeBookTests/): xUnit tests validating behavior and contracts

## Notes (the “gotchas” you’d want in production docs)

- Ranked gradebooks require at least **5 students** before ranked grading/statistics make sense.
- `.gdbk` files are just JSON; the app writes them next to where you run the process.
- [Dockerfile](Dockerfile) uses a .NET 8 multi-stage build; use `docker run -it` for an interactive console session.