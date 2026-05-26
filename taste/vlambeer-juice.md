# Vlambeer Juice Principles

> Source: Jan Willem Nijman, "The Art of Screenshake" (INDIGO Classes 2013).
> A working canon of game-feel techniques for fast-action games.
> Apply these in `/juice` passes. **Cite the principle number** when applying.

These are not suggestions. They are **defaults** that a vibe-game project should hit
unless there's a deliberate reason otherwise.

---

## P1 — Bigger bullets
The player's projectile should be visually larger than physical hitbox suggests.
- **Default:** sprite/mesh ~1.5x hit radius.
- **Why:** legibility, perceived power.
- **Anti-pattern:** pixel-perfect bullets in fast games.

## P2 — More bullets
If one bullet is good, three feels better. Spread, burst, or shotgun pattern.
- **Default:** primary weapon fires 2–5 projectiles with slight random spread.
- **Why:** chaos sells impact.

## P3 — Screen shake
Every meaningful impact shakes the camera.
- **Default magnitudes (Unity, Cinemachine Impulse):**
  - Bullet fire: 0.05 amplitude, 0.1s decay
  - Bullet hit: 0.15 amplitude, 0.15s decay
  - Enemy death: 0.4 amplitude, 0.25s decay
  - Player hit: 0.8 amplitude, 0.4s decay
- **Use Perlin noise**, not random — biological feel.
- **Anti-pattern:** uniform shake for all events. Hierarchy is everything.

## P4 — Hit-stop (frame freeze on impact)
Pause game time briefly on impact. Player feels the weight.
- **Default:** `Time.timeScale = 0` for 30–100ms.
  - Melee hit: 60–80ms
  - Bullet hit on enemy: 30–50ms
  - Player damage: 100–150ms
  - Boss death: 200–400ms
- **Use `Time.unscaledDeltaTime`** for UI and feedback effects so they keep playing.
- **Why:** weight, satisfaction, time for the brain to register the hit.

## P5 — Particles, particles, particles
Every action emits particles. Even simple ones.
- **Minimum:** muzzle flash, hit sparks, dust on landing.
- **Default count:** 5–20 per event.
- **Style:** match game palette. Use the color of the thing being hit.

## P6 — Permanence (decals, gibs, holes)
Damage leaves marks. Enemies leave gibs. Bullets leave holes.
- **Default lifetime:** 5–15 seconds visible.
- **Pool aggressively** to keep perf.
- **Why:** the world remembers what the player did.

## P7 — Sounds (everything has a sound)
No silent actions. Even UI hovers.
- **Default sound budget per game:** 30–80 distinct SFX.
- **Layer:** primary hit + secondary metallic ting + low thud.
- **Pitch variation:** ±10% random on every play to avoid ear fatigue.

## P8 — Music
Adaptive music tracks intensity. Quiet → tense → combat → climax.
- **Minimum:** 2 layers (calm + combat), crossfade on enemy proximity.

## P9 — Camera lerp
Camera doesn't snap to player. It lerps with damping.
- **Default:** Cinemachine framing transposer, X/Y damping = 0.3–0.5.
- **Look-ahead:** 0.2–0.4 in direction of movement.

## P10 — Camera kick
On significant input (shoot, dash), camera "kicks" in opposing direction.
- **Default:** 0.1–0.3 units, return over 0.15s with ease-out.

## P11 — Lerp position smoothing
Player position should never snap. Always smooth with lerp/spring.
- **Exception:** instant-feedback actions (dash, teleport) should still snap, but
  with motion lines, ghost trail, or particle compensation.

## P12 — Sleep
On big impact, ALL systems pause for one frame. Not just time — animations, particles, audio loop position.
- **Default:** 1 frame (16ms at 60fps).
- **Effect:** the universe noticed.

## P13 — Knockback
Hits push things. Both the target AND the attacker (recoil).
- **Default:** target knockback = damage × 0.5 units. Attacker recoil = damage × 0.1.

## P14 — Movement spring (anticipation + follow-through)
Animator squash before jump, stretch during, squash on land.
- **Default:** 0.85 → 1.1 → 0.9 → 1.0 on landing impact, over ~180ms.

## P15 — Anticipation telegraph
Enemy attacks have a wind-up frame. Player can react.
- **Default:** 200–500ms telegraph (color flash, position tell, animation pose).

## P16 — Random feedback variation
Every feedback should have variation. Pitch, color, particle direction, intensity.
- **Why:** repetition is the enemy of feel. Variation is alive.

## P17 — Slow-mo on critical events
Final blow on boss, player near-death, big jump → 0.3x time scale for 0.5s.
- **Use sparingly.** Reserve for moments that matter.

## P18 — Speedlines / motion blur
Fast movement gets visual speed cues.
- **Cheap version:** UI lines that flick on velocity threshold.

## P19 — Color and contrast pulse
On hit, target flashes white (or palette-inverted) for 60–80ms.
- **Why:** universal hit-confirm. Every fighting game does this.

## P20 — Tweening (everything eases)
Nothing moves linearly. Use eases.
- **Default mapping:**
  - UI in: `OutBack`
  - UI out: `InQuad`
  - Damage numbers: `OutCubic` up + `InQuad` fade
  - Camera shake decay: `OutExpo`

## P21 — Damage numbers
Pop-up numbers on hit. Critical hits bigger, different color.
- **Default:** white normal, yellow crit, red player-damage. Float up 1 unit over 0.6s.

## P22 — Combo / streak feedback
After 3+ hits without taking damage, build visual intensity.
- **Default escalation:** color shift on UI, faster music layer, screen edge glow.

## P23 — Death is a celebration
Enemy death needs to feel rewarding. Particles, sound, slow-mo for big enemies.
- **Default:** death > spawn in feedback intensity by 3x.

## P24 — Input buffering
Accept input slightly before action is available (jump while still falling).
- **Default window:** 100–150ms.
- **Coyote time:** 80–120ms after leaving ledge.

## P25 — One-frame UI tween in
Even menu transitions get juice. Scale 0 → 1 with OutBack, 200ms.

## P26 — Don't fade — pop
Things appear suddenly with scale, not slowly with alpha.
- **Why:** alpha-fade is dead. Pop with squash-stretch is alive.

## P27 — When in doubt, more juice
If it feels off, add another particle layer, another sound layer, another shake.
- **The ceiling is higher than you think.**

---

## How `/juice` uses this

When applying juice, the skill picks a subset relevant to the change and **cites the principle**:

> ✓ Added screen shake on enemy hit (Vlambeer **P3**)
> ✓ Added 50ms hit-stop on bullet impact (Vlambeer **P4**)
> ✓ Added white flash on damaged enemy, 70ms (Vlambeer **P19**)

This is the format the player sees. Every change traceable to a principle.

## Anti-patterns to flag

If `/feel-check` detects these, it flags them:

- ❌ No screen shake on any event
- ❌ No hit-stop on melee or bullet impact
- ❌ Silent actions (any action without SFX)
- ❌ Linear tweens (no easing)
- ❌ Camera that snaps to player
- ❌ No particles on death
- ❌ No anticipation on enemy attacks
- ❌ Pure alpha-fade transitions

## See also

- [[game-feel-swink]] — the academic foundation
- [[tyroller-mistakes]] — what to avoid at the project level
- [[gmtk-patterns]] — bigger structural patterns
