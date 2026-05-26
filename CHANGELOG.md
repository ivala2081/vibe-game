# Changelog

All notable changes to **Vibe Game for Unity** are documented here.

The format follows [Keep a Changelog](https://keepachangelog.com/en/1.1.0/), and this
project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

---

## [0.3.0] — 2026-05-26

### Added
- **3 new skills** filling the v2 promise gap from v0.2:
  - `/jam-mode` — Game jam workflow with locked scope, auto-checkpoints, CC0-only assets,
    auto-devlog cadence, and a hard ship gate at T-2h.
  - `/devlog` — Generate GMTK-style devlog posts in three lengths (Twitter, itch.io, blog)
    with GIF placement guidance. Mood-aware tone calibration.
  - `/death-watch` — Honest project health diagnostic. Tracks activity, scope drift,
    playtest cadence, and "it'll be fun when..." excuse count. Outputs four paths:
    PIVOT / SHIP / CUT-AND-CONTINUE / ABANDON.
- **`CONTRIBUTING.md`** — moat-aware contribution guidelines. High-value PRs (taste sources,
  demos, dogfood reports) vs low-value (generic tips, engine ports).
- **GitHub Actions CI** at `.github/workflows/validate.yml`:
  - JSON validity for plugin.json, marketplace.json, hooks/hooks.json
  - Required field check for plugin.json
  - Version consistency check between plugin.json and marketplace.json
  - SKILL.md frontmatter linting (required `name`, `description`)
  - Shellcheck on hook scripts
- **`RECORDING-GUIDE.md`** in `examples/demo-1-cleave/` — step-by-step Unity Recorder
  / ShareX / Game Bar instructions for capturing the README hero GIF.

### Changed
- Bumped version to 0.3.0 — feature add (10 skills now, up from 7).

### Notes
- Initial public release on GitHub at https://github.com/ivala2081/vibe-game
- Plugin install path validated:
  ```
  /plugin marketplace add ivala2081/vibe-game
  /plugin install vibe-game@vibe-games
  /vibe-game:vibe-start
  ```

---

## [0.2.0] — 2026-05-26

### Added
- **Cleave demo** ([`examples/demo-1-cleave/`](examples/demo-1-cleave/)) — first dogfood/showcase
  game, built using the skill pack on itself.
  - Full Vibe Brief
  - 9 Unity C# scripts (Unity 6 + 2022.3 LTS compatible via `UnityCompat` extension)
  - `JUICE-LOG.md` — example `/juice` output with cited sources
  - `FEEL-CHECK.md` — example `/feel-check` output (Swink 6 metrics + Fun-O-Meter)
  - `BUGS-FIXED.md` — log of bugs surfaced during real Unity test
- **`marketplace.json`** at `.claude-plugin/marketplace.json` — makes the plugin installable via
  `/plugin marketplace add ivala2081/vibe-game` then `/plugin install vibe-game@vibe-games`.
- **`LICENSE`** — MIT, with Unity trademark disclaimer and fair-use note for taste sources.
- **`hooks/`** directory with two starter hooks:
  - `pre-edit-vibe-brief.sh` — warn if a session edits code without a Vibe Brief present
  - `post-edit-juice-citation.sh` — remind the model to cite a taste source when juice-adjacent
    code is changed

### Changed
- **`plugin.json`** rewritten to match the official Claude Code plugin manifest spec.
  - Removed non-spec fields (`engine`, `engineVersions`, `skills`, `agents` lists). Skills and
    agents are discovered automatically from their directories.
  - Added `$schema`, `repository`, `category` fields.
  - `author` upgraded from string to object (per spec).
- **`/prototype` SKILL.md** — added hard rule #6: *visual asymmetry on the player is mandatory*.
  Surfaced by the Cleave demo: a real tester reported "couldn't move with mouse" when in fact
  rotation was working — the symmetric placeholder capsule hid the facing change.
- **`/juice` SKILL.md** — added hard rule #6: *press-time feedback is mandatory for charged
  inputs*, not only release-time. The Cleave demo's `FEEL-CHECK.md` predicted this gap before
  testing; the real test confirmed it.
- **`README.md`** — version bumped, Cleave demo linked, install instructions point to the new
  `/plugin marketplace add` flow instead of raw `git clone`.

### Fixed (in Cleave demo)
- Player `OnPlayerDamaged` was never called from `TakeHit` → combo never reset on damage.
- `Camera.main` acquired in `Awake` had timing dependency; moved to lazy lookup.
- `Time.timeScale = 0` from active hit-stop persisted across scene reloads → game stayed paused
  after `SceneManager.LoadScene`. Reset on `Juice.Awake`.
- URP `_BaseColor` vs legacy `_Color` mismatch — materials silently failed to colorize. Now uses
  `HasProperty` checks and sets both where present.
- `EnemySpawner.SpawnOne` had a `col` variable collision (CapsuleCollider + Color same name).
- Match timer kept counting after match end. Now frozen at end state.
- HUD overlap on the end screen (DOWN text overlapping SCORE).
- Opening pace too aggressive for "tense, methodical" mood — first spawn delay 1.5s → 2.5s,
  base interval 1.4s → 2.4s.
- Added a forward-facing nose primitive on the player so mouse-rotation reads visually.
- Added a press-time charge ring with crit-window color shift (the FEEL-CHECK-predicted fix).

---

## [0.1.0] — 2026-05-26 — Initial Release

### Added
- **7 skills** (each with detailed `SKILL.md` procedure):
  `vibe-start`, `prototype`, `juice`, `feel-check`, `cut`, `grab-asset`, `ship`
- **4 agents** (`game-feel-engineer`, `critical-playtester`, `scope-killer`, `vibe-director`)
- **Taste library** — the moat:
  - `taste/vlambeer-juice.md` — 27 principles with numeric defaults
  - `taste/game-feel-swink.md` — 6 metrics + Disney 12 + camera principles
  - `taste/tyroller-mistakes.md` — 17 indie killers with "Will it be fun?" filter
  - `taste/gmtk-patterns.md` — structural design patterns
  - `taste/INDEX.md` — citation format mandate
- **Vibe Brief template** at `templates/vibe-brief.md` (1 page replaces 50)
- Initial `README.md`, `plugin.json`
