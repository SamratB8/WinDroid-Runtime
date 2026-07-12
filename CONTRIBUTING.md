# Contributing to WinDroid Runtime

Thanks for your interest in WinDroid Runtime! The project is currently in an early
planning and research stage (see the README), so most contributions right now will
be discussions, design notes, and research rather than large code changes — and
that's just as valuable as code.

## How to contribute

1. **Fork the repository** — click "Fork" in the top-right corner of this page to
   create your own copy.
2. **Clone your fork locally**

   ```bash
   git clone https://github.com/YOUR_USERNAME/WinDroid-Runtime.git
   cd WinDroid-Runtime
   ```

3. **Create a branch** for your change

   ```bash
   git checkout -b my-change
   ```

4. **Make your change**, commit it with a clear message, and push your branch to
   your fork.
5. **Open a pull request** against `main` describing what you changed and why.

## Choosing an issue

- Check the [Issues tab](https://github.com/SamratB8/WinDroid-Runtime/issues) for
  open items, especially those labeled `good first issue`.
- Comment on the issue you'd like to work on before starting, so effort isn't
  duplicated.
- If you have an idea that isn't tracked yet, open a new issue first to discuss it
  before submitting a pull request.

## Basic expectations

- Keep pull requests focused on a single change — small, reviewable PRs merge
  faster than large ones.
- Explain the reasoning behind non-obvious decisions in the PR description.
- Given the project's early stage (C# / .NET and WinUI 3 are the planned stack),
  design discussions and research write-ups are welcome contributions on their own,
  not just code.

## Never commit secrets or local build output

- Don't commit API keys, tokens, credentials, or personal device paths.
- Don't commit local build artifacts (`bin/`, `obj/`, IDE settings, etc.) —
  `.gitignore` already covers the common ones; extend it if you add new tooling
  that generates its own output.

## Questions?

Open a [Discussion](https://github.com/SamratB8/WinDroid-Runtime/discussions) or
comment on the relevant issue if you're not sure where to start.
