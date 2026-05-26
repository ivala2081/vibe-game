# Vibe Game — for Unity

> **Ship indie Unity games solo, fast.**
> The taste your code is missing — Vlambeer, Steve Swink, Jonas Tyroller, and GMTK encoded into every skill.

[![Unity](https://img.shields.io/badge/Unity-6000.x_%7C_2022.3_LTS-black?logo=unity)](https://unity.com)
[![Claude Code](https://img.shields.io/badge/Claude_Code-Plugin-orange)](https://claude.com/claude-code)
[![Version](https://img.shields.io/badge/Version-0.2.0-blue.svg)](CHANGELOG.md)
[![License: MIT](https://img.shields.io/badge/License-MIT-green.svg)](LICENSE)

*Package: `vibe-game`. **Unity-only.** Not affiliated with or endorsed by Unity Technologies.*

---

## 🎮 Built for Unity, only for Unity

This is **not** a generic game-dev skill. Every skill, every agent, every line of generated code is tuned for the Unity engine:

- 🎯 **Engine:** Unity **6000.x** (preferred) or **2022.3 LTS**
- 🎨 **Render pipeline:** Universal RP (URP) default — Built-in supported
- 🎮 **Input:** Unity's **new Input System** (legacy Input Manager supported)
- 📷 **Camera:** **Cinemachine 3.0** for damping, shake, and impulse
- 🎬 **Recording:** **Unity Recorder** for devlog GIFs
- 📦 **Build target:** WebGL → itch.io as the default ship path

No Godot. No Unreal. No Löve2D. **Unity, deeply.**

---

## What this is

Every other Claude skill for game dev gives you **templates**. None give you **taste**.

`vibe-game` is a Unity-focused Claude Code skill pack that doesn't just write Unity code — it has *opinions*. About game feel. About scope. About when to ship. About what's fun.

It's what you'd get if Vlambeer, Steve Swink, and Jonas Tyroller sat at the next desk while you built your Unity game.

## The 7 Unity skills

| Skill | What it does in your Unity project |
|-------|------------------------------------|
| `/vibe-start` | 1-page Vibe Brief replaces 50-page GDD. Saved to `Assets/_Project/vibe-brief.md`. |
| `/prototype` | **Project-aware** Unity scene, playable in 30 minutes. Reads your `Packages/manifest.json`, render pipeline, Input System version, existing MonoBehaviours — generates code that *fits your project*, not a generic template. |
| `/juice` | Game feel pass. Cinemachine Impulse for screen shake, `Time.timeScale` hit-stop, ParticleSystem bursts, animation easing, anticipation. Every change cites its source (Vlambeer P3, Game Feel ch.7). |
| `/feel-check` | Critical playtester. Scores against Swink's 6 metrics. "This jump feels dead. Here's why. Here's the Unity fix." |
| `/cut` | Scope killer. Ruthless. Cites solo-dev failure data for every cut. Quantifies hours saved. |
| `/grab-asset` | Unity asset pipeline — Kenney, OpenGameArt, Itch CC0 → palette-matched → imported with correct PPU/compression settings → added to `CREDITS.md`. |
| `/ship` | Unity WebGL build via batch mode → itch.io page draft → launch devlog. One command. |

## What makes it different

### 1. Encoded design taste (the moat)
A real `taste/` library — Vlambeer's 27 juice principles, Steve Swink's *Game Feel* metrics, Tyroller's 17 indie mistakes, GMTK design patterns — turned into rules the skills *cite* when they change your Unity code.

### 2. Project-aware Unity code
Before generating, skills read your Unity project: Input System version (`com.unity.inputsystem`), render pipeline (URP / HDRP / Built-in), Cinemachine version, existing MonoBehaviour namespaces. The output fits *your* project — not a YouTube tutorial template.

### 3. Fun-O-Meter
Heuristic playtest score after every meaningful change:
- **Time-to-fun** — how many seconds from Play to first interesting moment?
- **Decision density** — choices per minute
- **Feedback latency** — input → visual response (ms)
- **Surprise quotient** — how predictable is the next 30s?

### 4. No "I have no assets" excuse
Unity-ready procedural placeholder pipeline: jsfxr SFX (drop-in .wav), shader textures (Quad-ready), Kenney CC0 search with import settings auto-applied.

### 5. Scope killer agent
A dedicated agent whose only job is to say *"cut this"* — citing post-mortems and quantifying hours saved.

### 6. Death-watch (optional, v2)
After 2 weeks of low Unity Editor activity / no playtest, the skill asks the hard questions most devs avoid.

### 7. Hooks that nag at the right time
Two starter hooks in `hooks/`:
- **vibe-brief-check** — if you edit code in a Unity project without a `vibe-brief.md`, reminds you to run `/vibe-start` first.
- **juice-citation-check** — if you touch juice-relevant code without citing a taste source (Vlambeer P#, Game Feel ch.#), nudges you to add the reference.

The moat is *cited* taste. The hooks enforce it gently.

---

## Quickstart (Unity project)

In Claude Code, from your Unity project root:

```
/plugin marketplace add Donchitos/vibe-game
/plugin install vibe-game@vibe-games
```

Then run:

```
/vibe-game:vibe-start
```

**Requirements:**
- Unity 6000.x (preferred) or 2022.3 LTS
- Claude Code v2.1.143+ (for `displayName` support)
- A Unity project (new or existing — both work)

The plugin auto-discovers your Unity setup (Input System version, render pipeline, Cinemachine,
existing controllers) and adapts its output accordingly.

### Alternative: clone for local dev

If you want to hack on `vibe-game` itself:

```bash
git clone https://github.com/Donchitos/vibe-game
cd vibe-game
# In Claude Code:
/plugin marketplace add ./
/plugin install vibe-game@vibe-games
```

### Working demo

Pre-built: see [`examples/demo-1-cleave/`](examples/demo-1-cleave/) — a 60-second score-attack
cleave game built end-to-end with `vibe-game`, including the actual `/juice` change log and
`/feel-check` report. Drop the scripts into a fresh Unity project, attach `Bootstrap.cs` to an
empty GameObject, press Play.

---

## Unity stack assumptions

The skills assume — and will configure if missing — this Unity stack:

| Package | Why | Required? |
|---|---|---|
| `com.unity.inputsystem` | New Input System for clean, polling-free input | Recommended (legacy supported) |
| `com.unity.cinemachine` | Camera damping, look-ahead, Impulse for shake | Required for `/juice` camera work |
| `com.unity.render-pipelines.universal` | URP for performant lit/unlit shaders | Optional (Built-in works) |
| `com.unity.recorder` | Devlog GIFs and screenshots | Optional (recommended) |

If missing, `/prototype` adds them to `Packages/manifest.json` and refreshes.

## Where things live in your Unity project

```
Assets/_Project/
├── vibe-brief.md           ← The 1-page design contract
├── Prototype/
│   ├── Scenes/             ← The playable scene
│   ├── Scripts/            ← Project-aware code
│   ├── Prefabs/            ← Player, dummy targets, etc.
│   └── Input/              ← .inputactions
├── Art/Sourced/            ← /grab-asset downloads with CREDITS.md
├── Audio/Sourced/          ← SFX + music attributions
└── CREDITS.md              ← Attribution log for shipped builds

Builds/
├── WebGL/                  ← /ship output
├── webgl-build.zip         ← itch.io upload
├── itch-page.md            ← Page draft
└── devlog-launch.md        ← Launch post
```

## Showcase

- **[Cleave](examples/demo-1-cleave/)** — 60-second score-attack cleave. Charge-and-release timing,
  crit windows, mood-aware juice. Built end-to-end with `vibe-game` (dogfood test). Read
  [JUICE-LOG.md](examples/demo-1-cleave/JUICE-LOG.md) and [FEEL-CHECK.md](examples/demo-1-cleave/FEEL-CHECK.md)
  to see what the skill pack actually outputs.

> _Demo reel coming with v1.0 — 5 indie Unity games built with vibe-game, each in under 2 hours, full timelapse._

## Philosophy

- **Vibe before system.** Mood and verb come before mechanics.
- **30-minute loop.** Every Unity iteration must produce something playable in <30 min.
- **Cite the source.** Every game feel change references *why*.
- **Cut, don't add.** When stuck, the answer is usually "remove."
- **Ship the prototype.** Indie Unity projects die in polish hell. Get it out.

## License

MIT.

## Credits

Encoded design knowledge sourced from public talks, books, and devlogs by:
Vlambeer (Jan Willem Nijman, Rami Ismail), Steve Swink (*Game Feel*),
Jonas Tyroller, Mark Brown (Game Maker's Toolkit), Derek Yu (*Spelunky*),
and the broader Unity indie dev community.

This skill stands on their shoulders. Buy the book. Watch the talks.

---

*"Unity" is a trademark of Unity Technologies. This project is independent fan tooling, not affiliated with or endorsed by Unity Technologies.*
