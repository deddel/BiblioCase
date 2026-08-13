# Contributing to BiblioCase

Thank you for contributing to BiblioCase. This document explains repository-level conventions and how to handle tooling files that may be created by editor extensions (for example, GitHub Copilot workspace instructions).

## Guidelines

- Keep repository metadata under the `.github/` directory minimal and intentional. Files in `.github/` are intended for repository-wide configuration and automation.
- If a file under `.github/` is added to support a team-wide tool or workflow (for example GitHub Actions, issue templates, or Copilot workspace instructions used by the team), commit it so all contributors benefit.
- If a file was created only by a local extension and is not part of a shared workflow, do not commit it. Remove it locally or add it to `.gitignore`.

## Handling `.github/copilot-instructions.md`

- Purpose: This file contains guidance for GitHub Copilot or related repository tooling. It does not affect build or runtime behavior.
- Decision checklist before committing:
  - Is your team using GitHub Copilot and do you want consistent Copilot guidance for all contributors? If yes, commit the file.
  - Does the file reference tools or commands your repository uses (for example `azmcp_bestpractices_get`) and are those documented or available to contributors? If not, update the file to reflect the actual tooling or do not commit it.
  - Does the file contain secrets or local-only configuration? If yes, remove those details before committing and prefer a team-maintained version.
- If you choose not to commit it, add the path to `.gitignore` to prevent accidental commits:

```gitignore
.github/copilot-instructions.md
```

## Azure-related guidance

- If repository Copilot rules reference Azure-specific tools (for example `azmcp_bestpractices_get`), ensure the repository documentation explains whether Azure is in use and how contributors can enable or opt-in to those tools.
- If the project does not use Azure services, do not commit Azure-specific guidance that will confuse contributors. Replace it with accurate instructions or omit the file.

## Code style and checks

- Follow the project's `.editorconfig` (if present) for formatting and naming conventions.
- Add or update CI checks and documentation when introducing repository-level tooling files so contributors understand how to opt in and how the tools are used.

## PRs and commits

- Make small, focused commits. Add a clear commit message explaining why repository metadata was added.
- When adding or changing `.github/` configuration, include a short note in the PR description explaining the purpose and expected impact.

## Contact

If you're unsure whether to commit a workspace instruction file, open an issue or ask the maintainers for guidance before committing.