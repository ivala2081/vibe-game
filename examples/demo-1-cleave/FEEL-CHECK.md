# /feel-check — Cleave report

> Output of the `/feel-check` skill on demo-1-cleave, post-`/juice`.
> No flattery. Cite the fault. Suggest the fix.

---

## Brief context (referenced for scoring)
- Mood: tense, weighty, climactic
- Verb: cleave
- Success: "A streamer dies 5x in a row and refuses to alt-tab."

---

## Part 1 — Swink's 6 metrics

```
Feel-Check Report
─────────────────
M1 Input:       8/10  ✓ New Input System polling. No animation-locked input. R-to-restart works post-death.
M2 Response:    7/10  ⚠ Cleave fires on release, not on press → 180+ms feedback latency from button click to first visual. By design (charge mechanic), but the *charge start* has zero feedback. Fix: small camera FOV pulse or player outline on BeginCharge.
M3 Context:     7/10  ✓ Top-down camera framing reads; arena ringed by walls makes spatial bounds legible. ⚠ No look-ahead, no FOV change with velocity — arena feels static.
M4 Polish:      6/10  ⚠ Strong on shake/hit-stop/flash/telegraph. Missing: SFX (silent), particles (no burst on impact), damage numbers, music. Polish ceiling is high but unlocked features need assets.
M5 Metaphor:    8/10  ✓ Cleave radial flash reads as "wide arc swing." Tier-scaled juice differences (weak vs. crit) are perceptible. Heavy vs. Light enemies look + behave differently.
M6 Rules:       9/10  ✓ Internal consistency: damage tier, color, shake, hit-stop all scale together. Same charge time → same outcome. No randomness in core swing.

Overall: 7.5 / 10 — Solid prototype. Mood-coherent juice. Asset gap caps M4.
```

---

## Part 2 — Fun-O-Meter

```
Time-to-fun:        ~4 seconds   (target <30s) ✓✓
                    Player spawns, can move + charge in frame 1.
                    First enemy reaches damage range at ~3s. First cleave is exhilarating.

Decision density:   ~6-8 / min   (target >4/min) ✓
                    Each enemy cluster = "when to release?" decision.
                    Crit window is the core decision unit.

Feedback latency:   ~50ms        (target <80ms) ✓
                    Input → first visual response on movement is one FixedUpdate tick.
                    EXCEPT: charge start has 0 feedback until release. Real gap.

Surprise quotient:  6/10         (lower target — i.e., LESS predictable = better)
                    Wave pattern is deterministic by time bracket. After 2-3 runs,
                    players will memorize the lull windows at 20s and 40s.
                    Light vs Heavy mix has some randomness (good).
                    Add: rare "elite" enemy or unpredictable spawn position to push higher.
```

---

## Part 3 — Top 3 fixes (ranked by impact)

### 1. [Polish] Add charge-start feedback (estimated +1.5 to M2)
**Problem:** Player presses LMB but nothing happens until release. The 180+ms gap between commitment and feedback violates [Game Feel M2](../../taste/game-feel-swink.md#m2--response).

**Source:** [Game Feel ch.7](../../taste/game-feel-swink.md#m2--response), [Vlambeer P25](../../taste/vlambeer-juice.md#p25--one-frame-ui-tween-in)

**Fix:** In `CleaveAttack.BeginCharge()`:
- Spawn a small ring around the player that scales from 0 → 0.4u over 100ms (OutBack ease)
- Spawn 4 small particle dots that orbit the player while charging (procedural — no asset needed)
- When the crit window opens (0.5s mark), the ring flashes green for one frame

Apply via: `/juice` (re-run, will detect the gap).

### 2. [Polish + Audio] Asset gap — SFX wholly missing (estimated +2 to M4)
**Problem:** Silent build. Every Vlambeer principle assumes audio is present. Without it, M4 caps at ~6.

**Source:** [Vlambeer P7 — Sounds](../../taste/vlambeer-juice.md#p7--sounds-everything-has-a-sound). *"No silent actions."*

**Fix:** Run `/grab-asset sfx-melee-impact` and `/grab-asset sfx-charge-buildup`. Recommended CC0 sets:
- [Kenney Impact Sounds](https://kenney.nl/assets/impact-sounds) — body of the hit
- [Kenney Sci-fi Sounds](https://kenney.nl/assets/sci-fi-sounds) — charge whine
- Freesound: "sword swing" tag, filter CC0 — overlay layer

Add `AudioSource` on player, fire on `BeginCharge` (charge whine) and on `Execute` (impact layer).

### 3. [Pacing] Surprise quotient — wave pattern too memorizable (estimated +1.5 to surprise)
**Problem:** Spawner bands by time (0-15s heavies, 15-30s mix, etc.) → second run will feel familiar.

**Source:** [GMTK Z-curve](../../taste/gmtk-patterns.md#the-z-curve-browns-term) is sound, but execution is too rigid.

**Fix:** In `EnemySpawner.SpawnFromCurve`:
- Add 15% chance to spawn an "elite" — Heavy with double HP, telegraph 200ms longer, +50 score on kill
- Randomize the lull windows ±3 seconds per run
- Vary `spawnRadius` per spawn (8-12u instead of fixed 10) so player can't predict spawn distance

---

## Mood alignment check ✓

Verified the build supports the brief's mood:
- ✓ "Tense" — telegraph times reward patience, panic-charge gets you killed
- ✓ "Weighty" — hit-stop durations bias long; cleave radial flash slow OutExpo
- ✓ "Climactic" — crit slo-mo on multi-kill creates payoff moments

If `/feel-check` had detected mood drift (e.g., max-chaos particles applied), it would flag here.

---

## Recommendations matrix

| Fix | Effort | Impact | When |
|---|---|---|---|
| Charge-start feedback | 15 min | +1.5 M2 | Next `/juice` |
| Add SFX via `/grab-asset` | 30 min | +2 M4 | Before ship |
| Spawner unpredictability | 20 min | +1.5 surprise | Next `/juice` |
| ParticleSystem on impact | 25 min | +0.5 M4 | Optional polish |
| Replace IMGUI with TMP/Canvas | 45 min | +0.5 M3 | Only if shipping wider audience |

**Recommended next command:** `/juice` (apply fix 1 + 3) → `/grab-asset` (fix 2) → `/feel-check` again to verify scores.

---

## Decision question

Apply fixes #1 and #3 now? They cost ~35 min and lift the overall feel-check score from 7.5 → ~8.5.

Or `/ship` the demo as-is for the showcase reel — silent but otherwise complete, demonstrating mood-aware juice and project-aware code at a 7.5/10 baseline?
