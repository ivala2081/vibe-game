# Vibe Brief — Cleave

> *Showcase demo #1 for `vibe-game`. Built by dogfooding the skill itself.*

---

## Mood
**tense, weighty, climactic**

## Verb
**cleave**

## References
- *Hotline Miami* — the rhythm of one perfect strike per beat
- *Devil Daggers* — the 60-second score-attack purity
- *Sekiro* — the **weight** of each swing, the silence between
- + The opening hallway scene from *John Wick (2014)* — calm, deliberate, devastating

## Hook
**60 seconds. One arena. Each swing of your blade matters more than the last.**

## Anti-features
- ❌ No menus, no title screen, no settings
- ❌ No tutorial — verb-first, learn by doing
- ❌ No upgrades, no loadouts, no meta-progression
- ❌ No music selection (one track, set to mood)
- ❌ No multiple weapons — one scythe, mastered
- ❌ No score boards beyond local — keep scope clean

## Win / Lose
**Win:** Survive 60 seconds. Score >100 = "Cleaver." Score >250 = "Reaper."

**Lose:** 3 hits and you die. Instant restart with one keypress.

## Core loop (60 seconds)
Move with WASD → identify the densest enemy cluster → hold left click to charge cleave (timing window opens 0.5-1.2s into charge) → release at peak for a 360° radial cleave → kills cascade with multi-hit combo → score pulses up → wave intensifies → repeat. After 60s, score screen + restart.

## Scope budget
- **Total dev time target:** 4 hours (showcase demo, not a full game)
- **Levels:** 1 (a single fixed arena)
- **Enemy types:** 2 (slow heavy + fast light)
- **Weapons / abilities:** 1 (the cleave)
- **UI:** Score + Timer + Combo only. No menus.

## What success looks like
A streamer dies 5 times trying to beat their last score and **refuses to alt-tab.** They re-share the demo organically because each death felt fair and the next attempt felt achievable.

## Why this game showcases vibe-game
| `vibe-game` feature | How Cleave demonstrates it |
|---|---|
| Encoded design taste | Every juice element cites Vlambeer P# / Game Feel ch.# in JUICE-LOG |
| Mood-aware juice | "Tense, weighty" → restrained shake (0.05-0.15), longer hit-stop (80-150ms), low-pitched SFX — *not* frenetic chaos |
| Project-aware code | Code generated against Unity 6 + URP + new Input System (no Cinemachine dependency for demo simplicity) |
| 30-minute prototype | The playable charge-and-cleave loop ships in ~30 minutes; juice + balance ships at hour 2 |
| Scope discipline | 1 verb, 1 arena, 1 weapon, 2 enemy types, 60-second loop. `/cut` would approve. |
| Verb-first start | Player swings within 3 seconds of pressing Play |

## Decision log

| Date | Decision | Alternatives considered | Why |
|------|----------|------------------------|-----|
| 2026-05-26 | Single verb (cleave), no dash/dodge | Add dash for evasion | Tyroller M1 + M9 — one verb mastered beats two underdeveloped |
| 2026-05-26 | No Cinemachine dependency | Use Cinemachine Impulse | Demo needs to drop into the simplest possible Unity project. Manual camera shake works for 60s scope. |
| 2026-05-26 | Programmatic scene (Bootstrap.cs) | YAML scene file | Drop-in install path: blank scene + 1 GameObject. No version-fragile .unity file. |
| 2026-05-26 | Two enemy types only | One type (simpler) / four types (richer) | Two gives behavior contrast (telegraph timing differs) without scope creep. GMTK "one verb, many problems" pattern. |

---

*Created by `/vibe-start` (dogfood) on 2026-05-26.*
*Next: `/prototype` produced the Unity scripts. See [README.md](README.md).*
