# BUGS-FIXED — Cleave Demo Dogfood Log

> Log of every bug surfaced while actually running this demo in Unity 6.4.
> Kept as evidence of the dogfood process and the skill pack's iteration loop.

---

## Test environment
- Unity 6.4 (6000.4.8f1) — Universal 3D template
- Windows 11
- Real human tester (project author)
- New Input System, URP, Cinemachine absent (by design)

## Bug ledger

### B1 — `OnPlayerDamaged` never called → combo never resets on hit
- **Surface:** code review before first run
- **Root cause:** `PlayerController.TakeHit()` didn't notify `GameManager`
- **Fix:** added `GameManager.Instance?.OnPlayerDamaged()` to `TakeHit`
- **Spec impact:** none — implementation oversight

### B2 — `Camera.main` acquired in `Awake` had timing dependency
- **Surface:** code review
- **Root cause:** `Bootstrap.BuildJuiceCamera` runs after `BuildPlayer`, so `Camera.main` may be a default-scene camera or null when `PlayerController.Awake` fires
- **Fix:** lazy `Camera.main` lookup in `FaceMouse`
- **Spec impact:** none — implementation pattern

### B3 — `Time.timeScale = 0` from hit-stop persisted across scene reload
- **Surface:** anticipated; would have shown up first time tester died during hit-stop
- **Root cause:** `SceneManager.LoadScene` does not reset `Time.timeScale`
- **Fix:** `Juice.Awake` sets `Time.timeScale = 1f`
- **Spec impact:** add to `/prototype` skill — any time-warping system must reset on init

### B4 — URP `_BaseColor` vs legacy `_Color` mismatch
- **Surface:** anticipated; URP/Unlit shader uses `_BaseColor`, `Material.color` setter expects `_Color`
- **Root cause:** Unity 6 URP shader property naming
- **Fix:** `HasProperty` checks, set both where present
- **Spec impact:** add to `/prototype` skill — when generating materials at runtime, support both naming conventions

### B5 — `EnemySpawner.SpawnOne` had `col` variable name collision
- **Surface:** first compile error in the test (Unity console)
- **Root cause:** I introduced a second `var col = ...` (Color) in the same scope where `var col = AddComponent<CapsuleCollider>()` already existed
- **Fix:** renamed second to `tint`
- **Spec impact:** none — my coding error

### B6 — Match timer kept counting after match end (showed "0.0" forever)
- **Surface:** first run — death screen showed timer at 0.0 still ticking
- **Root cause:** `TimeRemaining` computed from `Time.time - _matchStart` always; not frozen on end
- **Fix:** snapshot `_frozenTimeRemaining` in `EndMatch`, use it after `_ended`
- **Spec impact:** add to `/prototype` skill — UI bound to live state must explicitly freeze on terminal state

### B7 — HUD overlap on end screen ("DOWN" text on top of "SCORE 0")
- **Surface:** first run screenshot
- **Root cause:** `DrawCentered` rects at `h/2 - 30` and `h/2 + 20` with big font (56px) → overlap
- **Fix:** spacing increased to `h/2 - 80` and `h/2 + 10`; added dim full-screen overlay for legibility
- **Spec impact:** add to `/prototype` skill — end screens need both spacing AND a backdrop layer

### B8 — Opening pace too aggressive for "tense, methodical" mood
- **Surface:** first run — player swarmed immediately, never got a clean swing
- **Root cause:** first spawn at 1.5s, base interval 1.4s — fine for "frenetic" mood, wrong for this brief
- **Fix:** first spawn 2.5s, base interval 2.4s
- **Spec impact:** **add to `/prototype` skill — spawner pacing must align with brief mood; mood mismatch is a real bug, not preference**

### B9 — Player capsule symmetric → couldn't see mouse-rotation working ⭐
- **Surface:** real tester reported "couldn't move with mouse"; rotation was actually working
- **Root cause:** primitive capsule has no readable facing direction; even with `transform.rotation` updating every frame, the user sees nothing
- **Fix:** added a small yellow cube as forward-facing "nose" child
- **Spec impact:** **HARD RULE added to `/prototype` SKILL.md (rule #6) — visual asymmetry on the player is mandatory**

### B10 — No press-time feedback on charge ⭐⭐
- **Surface:** real tester confirmed; FEEL-CHECK.md predicted this fix before testing
- **Root cause:** `CleaveAttack.BeginCharge` set a bool but spawned no visual; user pressed LMB and saw nothing for 180+ms until release
- **Fix:** spawn a `LineRenderer` charge ring on `BeginCharge`, scale + color it with crit-window awareness, destroy on release/execute
- **Spec impact:** **HARD RULE added to `/juice` SKILL.md (rule #6) — press-time feedback mandatory for any charged input**

---

## Score

| Stage | Bugs surfaced | Bugs spec-relevant |
|-------|---|---|
| Code review (pre-run) | 4 | 2 |
| First test run | 4 | 2 |
| User feedback iteration | 2 | 2 |
| **Total** | **10** | **6** |

Six bugs were generic enough to become **spec rules** in the skill pack. They've been folded into
[`/prototype`](../../skills/prototype/SKILL.md) and [`/juice`](../../skills/juice/SKILL.md)
SKILL.md files. The next person running this skill pack on a new game won't hit them.

That's the dogfood loop working as intended.
