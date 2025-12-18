# GradeBook (C# / .NET Core)

A console-based grade book application that lets an instructor create and manage gradebooks, enroll students, record grades, calculate GPAs, and persist data to disk.

This repo is portfolio-ready: it demonstrates object-oriented design (inheritance + polymorphism), input validation, a command-driven UI, algorithmic ranking logic, and automated tests.

## Technical accomplishments

- Built an extensible domain model with an abstract `BaseGradeBook` and concrete implementations:
  - `StandardGradeBook`: traditional letter-grade thresholds.
  - `RankedGradeBook`: assigns letter grades by class percentile (top 20% = A, next 20% = B, etc.) with minimum cohort rules.
- Implemented ranked grading algorithm using sorting and percentile thresholds, including guard rails:
  - Throws an `InvalidOperationException` when ranked grading is attempted with fewer than 5 students.
  - Short-circuits statistics calculation with an explanatory message when requirements are not met.
- Added weighted GPA support:
  - `IsWeighted` toggles whether Honors / Dual Enrolled students get +1 GPA point.
  - Student classification via `StudentType` enum (Standard, Honors, DualEnrolled).
- Created a command router + interactive console UI:
  - `StartingUserInterface` handles app-level commands (create/load/help/quit).
  - `GradeBookUserInterface` handles gradebook-level commands (add/remove/list/addgrade/removegrade/statistics/save/close).
- Implemented file persistence with JSON:
  - Saves to `<GradeBookName>.gdbk` using Newtonsoft.Json.
  - Loads gradebooks and resolves the correct runtime type via reflection-based deserialization.
- Validated behavior with automated tests (xUnit): ranked grading rules, constructor contracts, and weighted GPA behavior.

## Tech stack

- Language: C#
- Runtime: .NET Core (netcoreapp2.0)
- Testing: xUnit
- Serialization: Newtonsoft.Json

## How to run

From the repo root:

- Run the app:
  - `cd GradeBook`
  - `dotnet run`

- Run tests:
  - `cd GradeBookTests`
  - `dotnet test`

## Example usage (interactive)

When the app starts, you can create or load a gradebook:

- `create MyBook standard true`
- `create MyBook ranked false`
- `load MyBook`
- `help`
- `quit`

When a gradebook is open:

- Add a student:
  - `add Alice Honors Campus`
- Add grades (0–100):
  - `addgrade Alice 95`
  - `addgrade Alice 88`
- View statistics:
  - `statistics all`
  - `statistics Alice`
- Persist and exit:
  - `save`
  - `close`

## Project structure

- `GradeBook/`
  - `GradeBooks/`: gradebook types (`BaseGradeBook`, `StandardGradeBook`, `RankedGradeBook`)
  - `UserInterfaces/`: command routing and CLI UI
  - `Enums/`: `GradeBookType`, `StudentType`, `EnrollmentType`
  - `Student.cs`: student entity and grade tracking
- `GradeBookTests/`: xUnit tests validating behavior and contracts

## Notes

- Ranked grading requires at least 5 students (by design), so add at least five students with grades before using ranked statistics.