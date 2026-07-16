# WinDroid Runtime

![Status](https://img.shields.io/badge/status-early%20development-blue)
![Platform](https://img.shields.io/badge/platform-Windows%2011-blue)
![Language](https://img.shields.io/badge/language-C%23-purple)
![License](https://img.shields.io/badge/license-Apache%202.0-green)

[Website](https://novasystemslab.org) ·
[Organization](https://github.com/Nova-Systems-Lab) ·
[Discussions](https://github.com/Nova-Systems-Lab/WinDroid-Runtime/discussions) ·
[Issues](https://github.com/Nova-Systems-Lab/WinDroid-Runtime/issues)

**WinDroid Runtime** is an independent Android-compatible runtime and toolkit for Windows.

The long-term goal is to build a modular platform that allows Android applications to run, integrate, and feel natural on Windows. Development begins with WinDroid Studio, ADB integration, APK management, diagnostics, and developer tooling before moving into runtime-engine research, Windows integration, consumer experiences, and later gaming-focused work.

> [!IMPORTANT]
> **Current project status**
>
> WinDroid Runtime is in early development. The repository currently focuses on the native Windows management application and ADB-based tooling.
>
> It is **not yet a usable Android runtime**, and the Android runtime engine, consumer edition, gaming edition, anti-cheat compatibility, and most advanced features described below are long-term plans rather than current capabilities.

> [!NOTE]
> **Roadmap sequencing**
>
> The current roadmap remains developer-first through Issue #36. After Issue #36 is complete, the project will open a dedicated architecture/RFC discussion to evaluate the future Developer, Desktop, and Gaming experiences.
>
> This does **not** mean those editions will be implemented immediately after Issue #36. That checkpoint begins architecture planning, feasibility work, validation, and prioritisation. Shared platform components will remain unified unless later evidence justifies a different design.

---

## Vision

Windows previously offered official Android application support through Windows Subsystem for Android. Following its discontinuation, there is renewed interest in clean, secure, and well-integrated Android-on-Windows experiences.

WinDroid Runtime aims to explore that space through an original, open-source Android-compatible runtime platform built independently from the ground up.

The long-term product vision is:

> **One shared runtime platform with specialised experiences for developers, ordinary Windows users, and gamers.**

```text
WinDroid Platform
├── Shared Runtime Foundation
│   ├── Runtime Engine
│   ├── Android Virtual Device Layer
│   ├── Hardware Acceleration
│   ├── Graphics and Audio
│   ├── App Lifecycle Manager
│   ├── ADB and Debug Bridge
│   ├── Windows Integration Layer
│   ├── Security and Permissions
│   └── Update and Package System
│
├── WinDroid Developer
│   └── Developers, testers, QA teams, and advanced users
│
├── WinDroid Desktop
│   └── General users, productivity applications, and managed deployments
│
└── WinDroid Gaming
    └── Gaming-focused performance, input, and compatibility features
```

The project will not attempt to satisfy all three markets in its first release.

---

## Product Direction

### WinDroid Developer

The first product direction.

Planned areas include:

- ADB and debugging tools
- APK installation and app management
- Device and environment information
- Logs and crash diagnostics
- Screenshots and file transfer
- Shell access
- Multiple Android environments
- Device and API-level profiles
- Snapshots
- Network and sensor simulation
- Automated testing
- IDE and CI integration
- Team and enterprise deployment tools

The first practical releases will focus on the smaller and more achievable subset represented by the current GitHub issues.

### WinDroid Desktop

A future consumer and productivity experience built on the same shared runtime foundation.

Potential features include:

- Simple installation and onboarding
- Reliable Android application execution
- Per-app windows
- Start Menu shortcuts
- Windows notifications
- Clipboard and file sharing
- Camera, microphone, and location permission controls
- Privacy-oriented defaults
- Low idle resource usage
- Offline usability
- No advertisements or mandatory behavioural profiling
- Optional ADB access for advanced users
- Managed deployment for businesses, schools, and kiosks

This experience will not begin until the runtime foundation reaches suitable compatibility, reliability, security, and usability gates.

### WinDroid Gaming

A later gaming-focused experience.

Potential research and development areas include:

- High and stable frame rates
- Keyboard and mouse mapping
- Controller support
- Multi-instance execution
- Macros and game profiles
- High-refresh rendering
- Low input latency
- Streaming and recording tools
- Graphics compatibility
- Performance tuning
- Game-specific optimisation
- Anti-cheat compatibility research

> [!WARNING]
> **Anti-cheat compatibility is a long-term research goal, not a current guarantee.**
>
> WinDroid intends to pursue at least basic compatibility where technically and legally possible. Some anti-cheat systems may reject virtualised, translated, rooted, modified, or otherwise unsupported Android environments by design. Kernel-level or vendor-controlled anti-cheat systems may remain incompatible regardless of WinDroid improvements.
>
> Compatibility will be documented honestly on a per-game and per-version basis.

Gaming work is planned after developer and desktop foundations because it requires exceptional performance, broad graphics compatibility, low latency, and extensive testing.

---

## Core Goals

- Build an independent Android-compatible runtime for Windows.
- Provide a native Windows control application for managing the platform.
- Support APK installation, uninstallation, launching, and debugging.
- Integrate ADB-based developer tools.
- Research Android image booting and runtime backends.
- Explore native-feeling Windows integration for Android applications.
- Maintain shared runtime, ADB, security, update, and lifecycle components across future editions.
- Design for privacy, offline usability, signed updates, and explicit permission controls.
- Build a transparent and sustainable open-source project structure.
- Avoid dependency on proprietary WSA binaries or misleading branding.
- Communicate unsupported applications, services, games, and anti-cheat limitations honestly.

---

## Current Development Scope

The current repository work is focused on **WinDroid Studio and the ADB tooling foundation**.

The present development sequence includes:

- ADB path detection
- Safe process execution
- Device discovery and parsing
- Connected-device dashboard
- Logging and output
- APK selection and installation
- Installed package listing
- Package launching and uninstalling
- Device details
- ADB server controls
- Basic logcat viewing
- Tests, CI, and contributor infrastructure

Completing this work demonstrates the management and developer-tooling layer. It does **not** by itself prove the final Android runtime engine.

---

## Planned Components

### WinDroid Studio

The native Windows control application for managing WinDroid Runtime and connected Android targets.

Current and near-term planned features:

- Runtime and device dashboard
- ADB detection and configuration
- Connected device/runtime discovery
- APK installation
- Installed package list
- App launch and uninstall controls
- Device details
- Log and output panel
- Logcat viewer
- Runtime settings
- Developer tools

WinDroid Studio is currently the developer-first interface. After Issue #36, the project will evaluate whether future user experiences should remain modes within one application or become separate front-end projects.

### WinDroid Core

Shared application logic and contracts.

Planned responsibilities:

- Configuration management
- Runtime state tracking
- App metadata handling
- Logging
- Error handling
- Shared models and services
- Security and permission abstractions
- Update and compatibility metadata

### WinDroid ADB Layer

A dedicated service layer for Android Debug Bridge operations.

Planned features:

- Detect ADB installation
- Start and stop the ADB server
- List connected devices
- Install APK files
- List installed packages
- Launch applications
- Uninstall packages
- Capture logs
- Run shell commands
- Transfer files
- Capture screenshots
- Return structured command results

### WinDroid Engine

The long-term experimental runtime backend.

Research areas:

- Android x86/x86_64 images
- AOSP and Generic System Images
- Virtualisation on Windows
- QEMU
- Hyper-V
- Windows Hypervisor Platform
- Android boot process
- Hardware acceleration
- Graphics, input, audio, networking, and file bridges
- App lifecycle management
- Isolation and permissions
- Updates and rollback
- Runtime compatibility measurement

---

## Architecture Principles

1. One shared runtime foundation should serve all future product experiences.
2. The first usable product should target developers and technically advanced users.
3. Consumer and gaming experiences should begin only after technical and market validation.
4. WinDroid Studio and the WinDroid runtime engine are architecturally distinct.
5. `WinDroid.Core`, `WinDroid.Adb`, and `WinDroid.Engine` should remain shared.
6. Future editions should not duplicate the runtime engine or low-level service layers.
7. Each market expansion requires its own readiness gate, security review, success metrics, and user research.
8. Proprietary Microsoft, Google, Amazon, store, service, or anti-cheat components will not be redistributed without appropriate rights.
9. Privacy, offline usability, signed updates, and explicit permission controls are foundational requirements.
10. Scope may be reduced, delayed, or redirected when feasibility or user evidence does not support the existing plan.

---

## Development Roadmap

> [!CAUTION]
> Roadmap phases describe intended direction, not fixed release dates or guaranteed features. Later phases depend on feasibility, licensing, contributor capacity, security review, performance, and real user demand.

### Phase 0 — Planning and Project Foundation

- [x] Choose project name
- [x] Define initial project scope
- [x] Add license
- [x] Create initial README
- [x] Create contribution guidelines
- [x] Document legal and trademark boundaries
- [ ] Complete initial architecture documentation
- [ ] Establish compatibility and security research notes

### Phase 1 — Native Windows and ADB Foundation

- [x] Create WinUI 3 / .NET solution structure
- [ ] Complete safe ADB process execution
- [ ] Detect and configure ADB
- [ ] Run and parse device commands
- [ ] Display connected devices
- [ ] Add settings persistence
- [ ] Add logging and output
- [ ] Add tests and CI validation

### Phase 2 — APK and Application Management

- [ ] Select APK files
- [ ] Install APKs to selected targets
- [ ] List installed packages
- [ ] Launch installed applications
- [ ] Uninstall applications with confirmation
- [ ] Display command results and failures clearly

### Phase 3 — Initial Developer Tools

- [ ] Add connected-device details
- [ ] Add ADB server controls
- [ ] Add basic logcat viewer
- [ ] Add screenshot capture
- [ ] Add file push and pull
- [ ] Add basic shell command interface
- [ ] Add exportable diagnostic reports

### Architecture Checkpoint — After Issue #36

After Issue #36 is complete:

- [ ] Open a dedicated multi-experience architecture/RFC issue
- [ ] Review lessons from the current WinDroid Studio implementation
- [ ] Define what remains shared across all future editions
- [ ] Evaluate one application with modes versus separate front-end applications
- [ ] Define developer, desktop, and gaming readiness gates
- [ ] Define measurable performance and compatibility targets
- [ ] Review licensing, security, privacy, telemetry, and update requirements
- [ ] Conduct targeted user research
- [ ] Decide whether and when additional front-end projects should be created

This checkpoint starts planning only. It does not begin immediate consumer or gaming implementation.

### Phase 4 — Runtime Feasibility and Architecture Research

- [ ] Select and evaluate possible Android bases
- [ ] Compare virtual machine, container, compatibility-layer, and hybrid approaches
- [ ] Research supported Windows virtualisation technologies
- [ ] Define graphics, audio, input, storage, and networking strategies
- [ ] Define supported Windows and hardware baselines
- [ ] Complete component-level licensing research
- [ ] Create a security and threat model
- [ ] Define update, rollback, and recovery architecture
- [ ] Boot a minimal Android-compatible image
- [ ] Connect to the experimental runtime through ADB
- [ ] Install and launch a test APK

### Phase 5 — Developer Runtime Prototype

- [ ] Create repeatable Android environments
- [ ] Add environment and device profiles
- [ ] Add snapshots and recovery
- [ ] Add advanced diagnostics
- [ ] Add network and sensor simulation research
- [ ] Add automated testing hooks
- [ ] Research IDE and CI integration
- [ ] Run external developer testing
- [ ] Measure startup, stability, compatibility, and resource usage

### Phase 6 — Windows Desktop Integration

- [ ] Explore per-app window forwarding
- [ ] Research Start Menu and shortcut integration
- [ ] Research notification bridging
- [ ] Research clipboard sharing
- [ ] Research file sharing
- [ ] Add host permission controls
- [ ] Define privacy-preserving defaults
- [ ] Build installer, updater, rollback, and recovery flows
- [ ] Conduct consumer and managed-deployment pilots

### Phase 7 — Gaming Feasibility Research

- [ ] Measure graphics performance and frame pacing
- [ ] Research low-latency input
- [ ] Add keyboard and mouse mapping prototypes
- [ ] Research controller support
- [ ] Evaluate multi-instance execution
- [ ] Research streaming and recording integration
- [ ] Build a game compatibility test programme
- [ ] Research basic anti-cheat compatibility
- [ ] Document unsupported anti-cheat systems and external restrictions
- [ ] Proceed only if technically and commercially justified

### Phase 8 — Long-Term Platform Goals

- [ ] Stable independent runtime backend
- [ ] Shared runtime foundation across product experiences
- [ ] Per-app windows
- [ ] Audio, networking, and GPU acceleration
- [ ] Compatibility database
- [ ] Modular installer
- [ ] Stable, Beta, Preview, and Canary release channels
- [ ] Developer preview
- [ ] Desktop pilot
- [ ] Gaming preview, if justified
- [ ] Public beta

---

## Future Product Structure

No immediate repository division is planned.

The current structure remains:

```text
WinDroid-Runtime/
├── src/
│   ├── WinDroid.Studio/      # Current developer-first WinUI application
│   ├── WinDroid.Core/        # Shared logic, models, and abstractions
│   ├── WinDroid.Adb/         # Shared ADB integration layer
│   └── WinDroid.Engine/      # Shared experimental runtime backend
```

After Issue #36 and the architecture/RFC review, a future structure may be considered:

```text
WinDroid-Runtime/
├── src/
│   ├── WinDroid.Core/        # Shared
│   ├── WinDroid.Adb/         # Shared
│   ├── WinDroid.Engine/      # Shared
│   ├── WinDroid.Studio/      # Developer experience
│   ├── WinDroid.Desktop/     # Possible future consumer experience
│   └── WinDroid.Gaming/      # Possible future gaming experience
```

This structure is illustrative and has not yet been approved.

---

## Readiness Gates

### Developer Readiness

Before broader expansion:

- Reliable device discovery
- APK installation and uninstallation
- Stable logcat and diagnostics
- Repeatable environment creation
- Crash recovery
- Automated testing
- Measured startup and command reliability
- External technical testers
- Evidence of a specific workflow improvement

### Desktop Readiness

Before a consumer-facing release:

- Simple installation and onboarding
- Stable app lifecycle handling
- Permission controls
- Signed updates and rollback
- No-data-loss recovery
- Acceptable idle resource usage
- Security review
- Compatibility testing
- Clear unsupported-app messaging

### Gaming Readiness

Before a gaming-focused release:

- Proven graphics performance
- Stable frame pacing
- Low-latency input
- Keyboard, mouse, and controller testing
- Multi-instance resource controls
- Game-specific compatibility testing
- A documented anti-cheat compatibility position

---

## Compatibility Programme

Planned compatibility states:

```text
Verified
Works with limitations
Launches but unstable
Unsupported
Blocked by external dependency
```

Compatibility records may include:

- WinDroid version
- Android version
- Application or game version
- CPU architecture
- Graphics backend
- Required proprietary services
- Input method
- Known issues
- Test source
- Anti-cheat status where applicable

Compatibility claims will be version-specific and evidence-based.

---

## Security, Privacy, and Trust

Planned security principles include:

- Per-app and per-environment isolation
- Host filesystem boundaries
- Explicit clipboard, file, camera, microphone, and location controls
- ADB disabled by default for consumer users
- Signed update verification
- Rollback and recovery
- Clear handling of administrator privileges
- Unsafe APK warnings
- Log redaction
- Consent before uploading diagnostics
- No automatic upload of sensitive logs
- Offline operation where practical
- Enterprise telemetry controls
- No mandatory advertisements or behavioural profiling

A complete threat model will be developed during runtime feasibility work.

---

## Licensing and Proprietary Components

WinDroid Runtime is an independent project and will not redistribute proprietary components without appropriate legal rights.

This includes, but is not limited to:

- Microsoft WSA components
- Google Play Store
- Google Mobile Services
- Amazon Appstore components
- Proprietary Android system images
- Proprietary drivers or firmware
- Third-party anti-cheat components
- Store or service credentials

The project may support APK sideloading and open-source application distribution methods where legally permitted.

Official Google Play Store or Google Mobile Services integration is not included and will not be bundled unless all applicable permission, licensing, and certification requirements are satisfied.

---

## Current Solution Structure

The initial multi-project solution has been scaffolded.

```text
WinDroid-Runtime/
├── src/
│   ├── WinDroid.Studio/     # WinUI 3 desktop application
│   ├── WinDroid.Core/       # Shared class library
│   ├── WinDroid.Adb/        # ADB class library, references Core
│   └── WinDroid.Engine/     # Isolated runtime-engine boundary
│
├── tests/
├── docs/
├── Directory.Build.props
├── WinDroid.Runtime.slnx
├── README.md
├── LICENSE
├── CONTRIBUTING.md
├── SECURITY.md
└── .gitignore
```

Project dependencies:

```text
WinDroid.Studio  ->  WinDroid.Core, WinDroid.Adb
WinDroid.Adb     ->  WinDroid.Core
WinDroid.Core    ->  (no project references)
WinDroid.Engine  ->  (isolated)
```

---

## Building

Prerequisites:

- Windows 11
- .NET SDK 10.0.x
- Windows App SDK 2.2.0, restored through NuGet
- Visual Studio 2026 or 2022 with:
  - Windows App SDK C#
  - .NET Desktop Development
  - A Windows 10 or Windows 11 SDK

The class libraries currently target `net8.0`. The application targets `net8.0-windows10.0.19041.0`.

Build from the repository root:

```powershell
dotnet restore .\WinDroid.Runtime.slnx
dotnet build .\WinDroid.Runtime.slnx --configuration Debug --no-restore
dotnet build .\WinDroid.Runtime.slnx --configuration Release --no-restore
```

The solution can also be opened and built directly in Visual Studio.

---

## Technology Stack

Current stack:

- **Language:** C#
- **Framework:** .NET
- **Desktop UI:** WinUI 3 / Windows App SDK
- **Platform:** Windows 11
- **Tooling:** Android Debug Bridge
- **Version Control:** Git and GitHub

Long-term research may involve:

- C++
- Rust
- AOSP
- QEMU
- Hyper-V
- Windows Hypervisor Platform
- VirtIO
- Graphics translation
- Input and audio bridges
- Virtualisation and container technologies

No specific long-term backend is considered final until feasibility research is complete.

---

## What This Project Is

WinDroid Runtime is:

- An independent open-source project
- A Windows-focused Android compatibility and runtime research project
- A developer toolkit for Android-on-Windows workflows
- A long-term platform vision with specialised developer, desktop, and gaming experiences
- A project that intends to validate feasibility and user value incrementally

## What This Project Is Not

WinDroid Runtime is not:

- A fork of Microsoft WSA
- An official or unofficial continuation of Microsoft WSA
- A Microsoft, Google, or Amazon product
- A redistribution of proprietary WSA binaries
- A promise that all Android applications or games will work
- A promise of universal anti-cheat compatibility
- A project that currently provides a production-ready Android runtime
- A project that bundles Google Play Store or Google Play Services without permission

---

## Legal and Trademark Notice

WinDroid Runtime is not affiliated with, endorsed by, sponsored by, or connected to Microsoft, Google, Amazon, or any related organisation.

Windows, Android, Google Play, Amazon Appstore, Microsoft, Google, Amazon, and related names, logos, and trademarks are the property of their respective owners.

This project does not use Microsoft WSA binaries, Microsoft branding, Google Play binaries, or proprietary application-store components.

For the complete project policy, see [Legal, Licensing, and Trademark Boundaries](docs/legal-notes.md).

---

## Contributing

WinDroid Runtime is an early-stage open-source project.

Contributors, mentors, researchers, and developers interested in the following areas are welcome:

- C# and .NET
- WinUI 3
- Android Debug Bridge tooling
- Automated testing and CI
- Android internals
- AOSP
- Virtualisation
- Hyper-V and Windows Hypervisor Platform
- QEMU
- Graphics and input systems
- Security and sandboxing
- Performance measurement
- Compatibility testing
- Technical documentation

Before starting work:

1. Read the complete issue and its dependencies.
2. Comment that you would like to work on it.
3. Wait for a maintainer to assign it.
4. Keep the pull request focused.
5. Link the pull request to the issue.

See [CONTRIBUTING.md](CONTRIBUTING.md) for the complete contribution process.

---

## Community and Support

For quick questions, community discussion, testing feedback, and contributor coordination, join the Nova Systems Lab Discord server:

[Join Nova Systems Lab on Discord](https://discord.gg/sfFyVyTfX8)

For official project work:

- Use GitHub Discussions for detailed proposals and long-term discussions.
- Use GitHub Issues for confirmed bugs and actionable tasks.
- Use Pull Requests for code and documentation changes.

For private security reports, follow the [security policy](SECURITY.md) or contact [security@novasystemslab.org](mailto:security@novasystemslab.org).

---

## Current First Milestone

The first practical milestone is to build the developer-tooling foundation in WinDroid Studio.

Near-term capabilities include:

- Detect and configure ADB
- List connected Android devices and emulators
- Display device information
- Install APK files
- List, launch, and uninstall installed applications
- View command output and basic logs
- Use initial diagnostic tools

This milestone creates the management and developer-tooling foundation for deeper runtime work later.

---

## Part of Nova Systems Lab

<p align="center">
  <img
    src="https://raw.githubusercontent.com/Nova-Systems-Lab/.github/main/profile/assets/nova-systems-lab-horizontal.png"
    alt="Nova Systems Lab"
    width="420"
  >
</p>

WinDroid Runtime is developed under **Nova Systems Lab**, an independent open-source organisation focused on systems software, developer tools, platform integration, and experimental runtime technologies.

---

## License

This project is licensed under the Apache License 2.0.

See the [LICENSE](LICENSE) file for details.