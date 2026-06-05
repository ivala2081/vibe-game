# Cinemachine 3.0 Impulse — Concrete Setup

> Reference: [[unity-patterns#up5]] + [[vlambeer-juice#p3]]
> 5-minute setup. Then every shake in your project flows through one system.

---

## Prerequisites

- Unity 6 (Cinemachine 3.0+) or Unity 2022.3 LTS (Cinemachine 2.x — slightly different API,
  uses `CinemachineVirtualCamera` instead of `CinemachineCamera`)
- URP or Built-in pipeline — both work

## Install

Window → Package Manager → Unity Registry → search "Cinemachine" → Install

Or add to `Packages/manifest.json`:
```json
"com.unity.cinemachine": "3.1.0"
```

---

## Scene setup (one-time)

### 1. Camera + listener

If you have an existing `Main Camera`:
1. Add Component → **Cinemachine Brain** (this routes Cinemachine cameras through it)
2. Add Component → **Cinemachine Impulse Listener**
   - Channel Mask: leave at default (or use specific channels for filtering)
   - Gain: 1.0
   - 2D / 3D Distance: leave default

### 2. Cinemachine Camera (replaces virtual camera)

1. Hierarchy → right-click → **Cinemachine → CinemachineCamera**
2. On the new `CinemachineCamera` GameObject:
   - **Follow:** drag the Player Transform
   - **Look At:** drag the Player Transform (or leave empty for top-down)
   - Body component: **Position Composer** (top-down) or **Third Person Follow**
   - Damping (Position Composer): X=0.4, Y=0.4, Z=0.2 — [[vlambeer-juice#p9]]
   - Look-Ahead: 0.3 in movement direction — [[vlambeer-juice#p9]]

---

## Impulse Sources (per-hit)

Each thing that *causes* a shake needs an Impulse Source.

### Player damage source

On the Player GameObject:
1. Add Component → **Cinemachine Impulse Source**
2. **Impulse Definition** section:
   - **Impulse Shape:** `Bump` (sharp, decays cleanly) — for most hits
   - **Use 6D mode:** off (2D/3D positional + rotational, but rare to need)
   - **Custom Impulse Shape:** leave default
3. **Default Velocity:** `(0, 1, 0)` — direction of the kick. Y=1 means upward.
4. **Time Envelope:**
   - **Attack:** 0 (instant kick)
   - **Sustain:** 0
   - **Decay:** 0.4 (vibe-game default; tune per game feel)

### Code call

```csharp
using UnityEngine;
using Unity.Cinemachine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] CinemachineImpulseSource impulse;

    public void TakeDamage()
    {
        // Force argument scales the magnitude. See table below.
        impulse.GenerateImpulseWithForce(1.0f);
    }
}
```

---

## Magnitude reference (from [[vlambeer-juice#p3]])

Single source of truth — match these in your project for consistent hierarchy:

| Event | Force | When |
|-------|-------|------|
| UI click / soft tap | 0.05 | menu interactions |
| Bullet fire (muzzle kick) | 0.1 | weapon firing |
| Bullet hit on enemy | 0.3 | every projectile impact |
| Melee swing whiff | 0.05 | swing without hit |
| Melee swing landed | 0.4 | swing that connected |
| Enemy death | 0.6 | enemy expires |
| Boss minor hit | 0.8 | medium boss damage |
| Player damage | 1.0 | player took a hit |
| Boss major phase change | 1.2 | screen-wide reaction |
| Boss death | 1.5 | climactic moment |

**Multiplier convention:** if a swing hits 3 enemies, fire at `0.4 + 0.05 * extraHits`. Up to a cap.

---

## Multiple sources, same scene

If you have many things shaking at once:
- Use **Channel Mask** on the Impulse Source + Listener.
- Channels are bitmask integers. Default channel 1 = all sources affect main camera.
- For split-screen or boss-only cameras, isolate to channel 2, 4, 8, etc.

Bigger gotcha: **multiple impulses stack additively**. If 5 enemies die at once at force 0.6,
the camera gets a 3.0 magnitude impulse — too much. Mitigate with:

```csharp
// Cap concurrent impulse magnitude
public static class ImpulseGuard
{
    static float _lastFireTime;
    public static void Fire(CinemachineImpulseSource src, float force, float minGap = 0.02f)
    {
        if (Time.time - _lastFireTime < minGap) return;
        src.GenerateImpulseWithForce(force);
        _lastFireTime = Time.time;
    }
}
```

---

## Mood profile (read from Vibe Brief)

`/juice` picks a magnitude profile by reading the brief's mood:

| Mood | Multiplier |
|------|-----------|
| Tense, methodical, cold | 0.6× (restrained, weighty rather than chaotic) |
| Frantic, chaotic, sweaty | 1.5× (max chaos, screen always alive) |
| Melancholic, quiet, weighty | 0.4× (subtle, almost imperceptible) |
| Playful, springy | 1.0× (default baseline) |

Apply the multiplier to the table above when generating code:

```csharp
const float MOOD_MULT = 0.6f;  // "tense, methodical, cold"
impulse.GenerateImpulseWithForce(0.4f * MOOD_MULT);  // restrained melee landed
```

---

## Anti-patterns

- ❌ Manually tweaking `Camera.transform.position` for shake (use Impulse instead)
- ❌ Unscaled shake durations during hit-stop (use `Time.unscaledDeltaTime` for the listener)
- ❌ Same force for all events (kills the hierarchy — see Vlambeer P3)
- ❌ No Listener on Brain (impulses fire into the void)

---

## Verification

After setup, run the game and watch the Game view:
- Hit an enemy → camera should kick briefly, return smoothly
- Multiple kills → camera should *intensify*, not jitter
- Standing still → camera should not micro-shake (no idle noise)

If micro-shake on idle: check Decay isn't too long, or that no source is firing on Update.

## See also

- [[unity-patterns#up5]] — code pattern
- [[vlambeer-juice#p3]] — design rationale
- [[game-feel-swink#m4--polish-sensation]] — theory layer
