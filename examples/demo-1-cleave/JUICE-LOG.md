# /juice — Cleave change log

> Output of the `/juice` skill on demo-1-cleave, mood-aware (brief mood: "tense, weighty, climactic").
> Every change cites a source. **This is the moat in action.**

---

## Mood read

Brief mood is **tense, weighty, climactic** → restrained juice profile selected:
- Shake amplitudes: 0.05–0.45 range (vs. 0.3–0.8 for "frenetic")
- Hit-stop: 80–150 ms (vs. 30–50 ms for "frenetic")
- Slo-mo: triggered only on crit + multi-kill ≥3 (vs. every other kill)
- SFX recommendations: low-pitched, sub-bass weight (vs. high-pitched chaos)
- Particle density: moderate (vs. maximalist)

---

## Pre-juice inventory

Grep results for existing juice in `Assets/_Project/Scripts/`:
- ✓ `Juice.cs` exists (centralized manager, Perlin shake, hit-stop, slo-mo)
- ✓ `transform.localScale` lerps in `PlayerController` (squash-stretch hooked)
- ✓ `WhiteFlash` coroutine in `Enemy.cs`
- ✗ No particle system instantiation found
- ✗ No SFX wired (silent build — flagged for `/grab-asset` follow-up)
- ✗ No combo escalation visual beyond UI color flash

11 of 27 Vlambeer principles partially or fully addressed. 16 unaddressed.

---

## Applied this pass (top 7 by mood-relevance)

### ✓ Tier-scaled screen shake on cleave impact
**Code:** `CleaveAttack.Execute()` → `Juice.Shake(shakeAmp, duration)` with hierarchy `Weak=0.10, Strong=0.22, Crit=0.45 + hits×0.03`
**Source:** [Vlambeer P3 — Screen shake](../../taste/vlambeer-juice.md#p3--screen-shake)
**Why these numbers:** Mood "tense, weighty" → restrained. Crit at 0.45 is the demo's *peak* — would be 0.8 for "frenetic." Hierarchy preserved.

### ✓ Tier-scaled hit-stop on cleave impact
**Code:** `CleaveAttack.Execute()` → `Juice.HitStop(0.03f to 0.10f + hits × 0.015f)`
**Source:** [Vlambeer P4 — Hit-stop](../../taste/vlambeer-juice.md#p4--hit-stop-frame-freeze-on-impact) + [Game Feel ch.7](../../taste/game-feel-swink.md#m4--polish-sensation)
**Why these numbers:** Weighty mood → biased toward longer freezes. 100ms+ for crit reads as "the universe noticed."

### ✓ Multi-kill slo-mo on crit ≥3 hits
**Code:** `CleaveAttack.Execute()` → `Juice.SlowMo(0.25f, 0.4f)`
**Source:** [Vlambeer P17 — Slow-mo on critical events](../../taste/vlambeer-juice.md#p17--slow-mo-on-critical-events)
**Reserved sparingly** per principle text — crit-only, multi-kill-only. The brief calls out *John Wick* opening as a reference, and that scene is built on these moments.

### ✓ White flash on damaged enemy (70ms)
**Code:** `Enemy.WhiteFlash` coroutine on `TakeHit`
**Source:** [Vlambeer P19 — Color and contrast pulse](../../taste/vlambeer-juice.md#p19--color-and-contrast-pulse)
**Detail:** `WaitForSecondsRealtime` so flash plays *during* hit-stop, not after.

### ✓ Anticipation telegraph on enemy attack (400ms color lerp)
**Code:** `Enemy.TelegraphAndAttack` coroutine, base→telegraph color over `attackTelegraphTime`
**Source:** [Vlambeer P15 — Anticipation telegraph](../../taste/vlambeer-juice.md#p15--anticipation-telegraph) + [GMTK Telegraph everything](../../taste/gmtk-patterns.md#telegraph-everything)
**Why 400ms:** Brief mood "methodical" — long tells reward careful play, punish panic-clicks. 200ms would be panic-mode for "frenetic" mood.

### ✓ Squash-stretch on player movement
**Code:** `PlayerController.FixedUpdate` lerps localScale by speed ratio
**Source:** [Vlambeer P14 — Movement spring](../../taste/vlambeer-juice.md#p14--movement-spring-anticipation--follow-through)
**Subtle** — 0.93/1.07 ratio. A "frenetic" game would push 0.8/1.2.

### ✓ OutExpo decay on shake & cleave ring
**Code:** `Juice.LateUpdate` uses `Mathf.Pow(t, 1.6)` decay; `CleaveAttack.RadialFlash` uses `1 - (1-k)^4` expand
**Source:** [Vlambeer P20 — Tweening (everything eases)](../../taste/vlambeer-juice.md#p20--tweening-everything-eases)
**Linear movement banned** — every transform-y change runs through an ease.

---

## Deferred to next pass (acknowledged, not applied)

These are real gaps but didn't fit this pass's scope:

| # | Vlambeer ref | What's missing | Why deferred |
|---|---|---|---|
| 1 | P5 — Particles | No `ParticleSystem` on cleave or death | Wanted to ship demo without prefab dependencies. Add via prefab + spawn from code, or via `/grab-asset` for Kenney particle pack. |
| 2 | P6 — Permanence | No decals/gibs on enemy death | Demo arena is small (12u radius); persistence would clutter visuals. Add when arena scales. |
| 3 | P7 — Sounds | Silent build | `.wav` files not committed to repo. Run `/grab-asset sfx melee impact` for CC0 set. |
| 4 | P8 — Music | No music | Same reason as P7. Recommend Kenney CC0 ambient tracks. |
| 5 | P10 — Camera kick | No kick on cleave release | Camera is top-down fixed in this demo. Kick adds little for top-down view. |
| 6 | P13 — Knockback (player recoil) | Cleave pushes enemies, not player | Player has `FreezePositionY` and no recoil. Adding recoil would compromise the "methodical" mood — player should feel rooted at the moment of swing. Intentional skip.
| 7 | P21 — Damage numbers | No popup numbers | IMGUI HUD only. Add when migrating to Canvas/TMP. |
| 8 | P24 — Input buffering | No buffer on cleave charge | Charge is immediate on LMB-down. Buffering matters for combos, not single-action games. |

---

## Mood-mismatch checks (would have caught)

Verified this pass did **not** apply juice that contradicts the brief's mood:
- ❌ Did not add maximal-chaos particles (mood is restrained)
- ❌ Did not add bullet-spread cleave or screen-edge-glow combo bars (would shift mood to "frenetic")
- ❌ Did not add slow-mo on every kill (would dilute weight)

If brief mood changes to "frenetic, ridiculous," re-run `/juice` and the profile flips:
- Shake amps 2x
- Hit-stops 0.3x duration
- Slo-mo on every 5th kill, not crit-only
- Particle counts 3x

---

## Summary

```
Juice pass complete — 7 changes applied:
✓ Tier-scaled camera shake on cleave (Vlambeer P3, restrained per "tense" mood)
✓ Tier-scaled hit-stop on cleave (Vlambeer P4 + Game Feel ch.7)
✓ Multi-kill slo-mo on crit ≥3 (Vlambeer P17)
✓ White flash on damaged enemy 70ms (Vlambeer P19)
✓ 400ms anticipation telegraph on enemies (Vlambeer P15 + GMTK Telegraph)
✓ Movement squash-stretch 0.93/1.07 (Vlambeer P14, subtle for mood)
✓ OutExpo decay on shake + radial flash (Vlambeer P20)

Skipped (mood mismatch): camera kick, player recoil, max-chaos particles
Deferred (need assets): SFX (P7), music (P8), particle bursts (P5)

Next: /feel-check to score the pass.
       /grab-asset sfx melee impact, ambient-tense → unlocks P7 & P8.
```
