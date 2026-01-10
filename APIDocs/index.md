---
_layout: landing
---

# IGCLWrapper API Docs

Welcome to the generated API reference for **IGCLWrapper**, a C# facade over the Intel AMD IGCL SDK. Use this site to explore the public classes, helpers, and DTOs that make up the wrapper.

The goal of IGCLWrapper project is to provide a lightweight, simpler way to access the Intel IGCL API, to read settings and make changes to the Intel GPU settings on a PC. The IGCLWrapper project provides Helper objects that provide a pointer-free, simple ergonomic API surface to make it easy to use, and it still exposes the native handles if you need to do something advanced.

## How to use

- Start at the API landing page: [IGCLWrapper API Reference](/api/IGCLWrapper.html).
- Navigate by feature: helpers like `IGCLDisplayHelper`, `IGCLOverclockHelper`, and `IGCLPowerHelper` list the available operations and event hooks.
- Facade helpers return DTOs with `bool` properties; use `*Native()` helpers to access raw structs when needed.
- Get/Set operations are split into `Get*()` and `Set*()` helpers; `GetSet*Native()` remains for direct IGCL calls.

## Where to Learn More

- **Project overview & usage**: See `README.md` at the repository root or the repo at https://github.com/terrymacdonald/IGCLWrapper for quick-start examples and patterns.
- **IGCL SDK reference**: The upstream IGCL SDK official docs are at https://intel.github.io/drivers.gpu.control-library/Control/INTRO.html.
- **IGCL SDK repository**: The upstream IGCL SDK repository is at https://github.com/intel/drivers.gpu.control-library. Samples provided by Intel are available at `..\drivers.gpu.control-library\Samples`.
- **IGCLWrapper Samples**: Runnable samples demonstrating display, desktop, GPU, and event-listener flows are under `Samples/`.
- **Wrapper internals**: Core helpers and facades are in `IGCLWrapper/` (e.g., `IGCLDisplayHelper`, `IGCLOverclockHelper`, `IGCLPowerHelper`).

## Regenerating Docs

Use `./refresh_IGCL_api_docs.ps1` from the repo root to rebuild the DocFX site and serve it locally on port 8000.
