# Changelog

All notable changes to **Vibe Game for Unity** are documented here.

The format follows [Keep a Changelog](https://keepachangelog.com/en/1.1.0/), and this
project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

---

## [0.5.0] — 2026-06-05 — "Loader Insight"

### Added

- **`taste/claude-code-internals.md`** — eleven documented Claude Code quirks
  (`CC1`–`CC11`) covering the loader behavior, cache versioning, frontmatter
  spec, hook event catalog, manifest field types, and marketplace name
  reservations. Sourced from official docs + community schema + direct
  observation of v2.1.150.
- **`/diagnose` skill** — self-diagnostic for vibe-game install health.
  Verifies cache freshness, lints SKILL.md frontmatter, validates plugin.json
  field types, checks hook config, and explains the misleading "0 skills"
  counter so users don't file false bugs.
- **README "Known Quirks of Claude Code" section** — surfaces the 0-skills
  counter mislabel, stale-cache-after-edit, and `agents` field gotcha. Links
  to the new `claude-code-internals.md` for the full reference.
- **`hooks/session-start.sh`** — SessionStart hook. Detects Unity projects
  from cwd, surfaces a one-line welcome with the next recommended skill
  (`/vibe-start` if no brief, `/prototype` or `/juice` if brief exists).
- **Prompt-type hook in `hooks.json`** — LLM evaluation on SKILL.md edits.
  Checks frontmatter for required fields and YAML quoting issues using a fast
  model. Demonstrates the `prompt` hook type beyond `command`.

### Changed
- **Skill count: 10 → 11** (`/diagnose` added).
- **`hooks/hooks.json`** — now includes 3 hook entries across 2 events
  (SessionStart + PostToolUse), with a mix of `command` and `prompt` types.
- **`taste/INDEX.md`** — registers `claude-code-internals.md` as the
  plugin-host knowledge layer (companion to `unity-patterns.md`'s
  engine layer and the four design-taste layers).
- **README** — version badge bumped to 0.5.0; `/diagnose` listed in the
  Workflow & accountability table.

### Notes

The "Loader Insight" theme: vibe-game v0.5.0 stops being a black box for
debugging. When something looks broken, run `/vibe-game:diagnose` and get a
cited explanation — not just "try restarting Claude Code." That self-knowledge
is now a moat feature, not a developer-only debugging tool.

---

## [0.4.0] — 2026-05-26

### Added — Unity API layer

A new taste-library file plus four drop-in templates close the "how do I actually do this in Unity?" gap that the design-taste files (Vlambeer, Swink, Tyroller, GMTK) leave open.

- **`taste/unity-patterns.md`** — 15 numbered Unity-specific patterns (`UP1`–`UP15`):
  - UP1 ScriptableObject configs
  - UP2 ScriptableObject event channels (Ryan Hipple)
  - UP3 Object pooling via `ObjectPool<T>`
  - UP4 Tweening (DOTween or coroutine + curve)
  - UP5 Cinemachine 3.0 Impulse (with magnitude table)
  - UP6 `Time.timeScale` hit-stop with `unscaledDeltaTime` discipline
  - UP7 MaterialPropertyBlock for hit flash (no draw-call cost)
  - UP8 Animator parameter hashing
  - UP9 InputAction in code (prototype scope)
  - UP10 Audio mixer with sidechain ducking
  - UP11 JsonUtility save/load
  - UP12 Coroutine vs async/UniTask
  - UP13 URP HitFlash shader
  - UP14 Build profiles + scene management
  - UP15 OnValidate editor-time guards
- **`templates/scriptable-objects/`** — 3 ready-to-use `.cs` templates:
  - `WeaponConfig.cs`
  - `EnemyConfig.cs`
  - `IntEventChannel.cs`
- **`templates/cinemachine-impulse-setup.md`** — 5-minute Cinemachine 3.0 walkthrough with magnitude table (force values per event type, mood multipliers).
- **`templates/shaders/HitFlash.shader`** — URP Unlit + `_FlashAmount` shader driven via MaterialPropertyBlock.

### Changed
- **`/prototype` SKILL.md** — added Unity Pattern decision tree, citing UP1–UP15 by ID. Lists templates available for drop-in use.
- **`/juice` SKILL.md** — added concrete implementations for camera shake (Cinemachine Impulse), hit-stop (static helper), hit flash (MPB or shader), tweening (DOTween or coroutine), audio ducking. Added mood-multiplier constants.
- **`taste/INDEX.md`** — added `unity-patterns.md` to the file table with a new "Unity-specific HOW-TO" domain.

### Notes
- The Unity API layer is **knowledge**, not bundled Unity packages. Templates are reference `.cs` and `.shader` files Claude reads and adapts to the user's project, not files that ship into `Packages/`.
- Citations in generated code now look like: `// [UP3] pooled bullets to avoid Instantiate/Destroy hitches`.

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
