# Game Feel — Steve Swink

> Source: Steve Swink, *Game Feel: A Game Designer's Guide to Virtual Sensation* (2008).
> The academic foundation. Where Vlambeer is *what to do*, Swink is *why it works*.

Apply via citations in `/juice` and `/feel-check`. Cite the chapter when used.

---

## The Three Building Blocks of Game Feel

### 1. Real-time control (input → response)
The player must feel they are controlling something *now*, not queuing intentions.

- **Input latency budget:** <80ms input-to-visual response. 50ms is excellent.
- **Test:** record gameplay, frame-step the input frame vs. first visible response frame.
- **Anti-pattern:** animation-locked controls (input ignored during attack animation) without buffering.

### 2. Simulated space
The player's avatar exists in a believable spatial system. Gravity, momentum, friction matter.

- **Momentum:** acceleration and deceleration curves, not instant velocity changes.
- **Friction:** ground vs. air handling differs.
- **Mass:** heavier things feel heavier. Apply different curves to different actors.

### 3. Polish (sensation layers on top)
Particles, sound, screen effects, animations — the "skin" of feel.

This is what Vlambeer P1–P27 cover. Polish is necessary but not sufficient.
**Without good control + simulated space, polish lipsticks a pig.**

---

## Six Metrics of Feel (Swink's framework)

When `/feel-check` evaluates, it scores along these axes:

### M1 — Input
- Are inputs registered every frame? Polled correctly?
- Is there input buffering? Coyote time? Repeat suppression?
- **Pass:** all inputs accepted, buffered 100-150ms, predictable behavior.

### M2 — Response
- Time from input to first visible game response.
- **Target:** <80ms (5 frames at 60fps).

### M3 — Context
- How does the world frame the action?
- Camera position, FOV, framing, occlusion.
- **Bad context** = great mechanics that don't read.

### M4 — Polish (sensation)
- Particles, screen shake, hit-stop, audio. (Vlambeer territory.)
- **The "wow."**

### M5 — Metaphor
- Does the action read as the thing it represents?
- Is the "punch" punchy? Does the "explosion" explode?
- **Test:** describe the action to someone watching with sound off. Do they see what you intended?

### M6 — Rules
- Internal consistency of the simulation.
- Same action → same outcome (with controlled variation).
- **Anti-pattern:** stochastic feedback that doesn't reward player skill.

---

## The Aesthetic of Failure

Swink's deep insight: **how the player fails matters more than how they win.**

- Wins should be earned and emphatic.
- Failures should be readable, fair, and *immediately retryable*.
- **Time-to-retry budget:** <3 seconds from death to "playing again."

Anti-pattern: long death animations, unskippable cutscenes after fail, loading screens between attempts.

---

## Animation Principles (from Swink + Disney 12)

The 12 Disney principles applied to game feel:

1. **Squash & stretch** — Volume preserved, mass expressed. (Vlambeer P14.)
2. **Anticipation** — Wind-up before action. (Vlambeer P15.)
3. **Staging** — Frame the action clearly.
4. **Straight ahead / pose to pose** — Animation flow vs. keyframes.
5. **Follow through & overlap** — Hair, cloth, weapons keep moving after body stops.
6. **Slow in / slow out** — Eases, never linear. (Vlambeer P20.)
7. **Arcs** — Natural motion follows curves, not lines.
8. **Secondary action** — Sub-motions that support the main one.
9. **Timing** — Heaviness/lightness expressed through frame counts.
10. **Exaggeration** — Reality is too subtle. Push it.
11. **Solid drawing** — Volumes feel three-dimensional.
12. **Appeal** — Charm, character, readability.

For 2D pixel art: 1, 2, 5, 6, 9, 10 matter most.
For 3D: all 12.

---

## Camera as a Character

Swink dedicates a chapter to camera. Key rules:

- **Camera has personality.** Tight & reactive = action. Wide & slow = exploration.
- **Look-ahead** in direction of motion (Vlambeer P9, 0.2–0.4 units).
- **Damping** so it never snaps (Vlambeer P11).
- **Camera shakes** scale with importance (Vlambeer P3 hierarchy).
- **FOV pulse** on dash/sprint — widens slightly to imply speed.

---

## Failure Modes (Swink's taxonomy)

When something feels "off" but you can't articulate why, it's usually one of:

| Symptom | Likely cause | Fix |
|---------|--------------|-----|
| "Floaty" jump | Bad gravity curve, no terminal velocity | Higher gravity on descent, cap fall speed |
| "Sluggish" controls | Input latency or no buffering | Reduce frames-to-response, add 100ms buffer |
| "Mushy" combat | No hit-stop, no shake, weak SFX | Apply Vlambeer P3, P4, P19 |
| "Stuck" feel | No follow-through animation | Add overlap to weapons, hair, cloth |
| "Cheap" deaths | No anticipation telegraph | Add 200-500ms enemy wind-up (Vlambeer P15) |
| "Boring" exploration | Camera too tight, no look-ahead | Widen FOV, add Vlambeer P9 |
| "Sterile" combat | All feedback identical | Add variation (Vlambeer P16) |

---

## How `/feel-check` uses this

The skill scores the project against M1–M6 and reports:

```
Feel-Check Report
─────────────────
M1 Input:      8/10  ✓ Coyote time, buffer set, all inputs polled
M2 Response:   6/10  ⚠ 110ms shoot latency (target <80ms)
M3 Context:    7/10  ✓ Camera framing reads
M4 Polish:     4/10  ⚠ Missing hit-stop, screen shake intensity flat
M5 Metaphor:   9/10  ✓ Punch reads as punch
M6 Rules:      8/10  ✓ Consistent damage feedback

Overall: 7.0/10 — Solid prototype, polish gap
Top fix: Apply /juice — Vlambeer P3, P4 to weapon impacts.
```

## See also

- [[vlambeer-juice]] — the practical implementation layer
- [[tyroller-mistakes]] — project-level pitfalls
- [[gmtk-patterns]] — structural design patterns
