# Taste Library — Index

> The moat. The reason `vibe-game` is different from every other Claude skill.

This is encoded design knowledge from people who actually shipped great indie games.
Every skill in this pack consults the library and **cites the source** when applying.

---

## Files

| File | Domain | When to consult |
|------|--------|-----------------|
| [vlambeer-juice.md](vlambeer-juice.md) | Moment-to-moment polish (WHAT to do) | `/juice`, `/feel-check` |
| [game-feel-swink.md](game-feel-swink.md) | Input/response theory, the 6 metrics (WHY it works) | `/feel-check`, `/prototype` |
| [tyroller-mistakes.md](tyroller-mistakes.md) | Project-level survival | `/cut`, `/death-watch`, `/vibe-start` |
| [gmtk-patterns.md](gmtk-patterns.md) | Structural design patterns | `/prototype`, `/feel-check` |
| [unity-patterns.md](unity-patterns.md) | Unity-specific HOW-TO (the API layer) | `/prototype`, `/juice` |
| [claude-code-internals.md](claude-code-internals.md) | Claude Code loader quirks & format reference (CC1–CC11) | `/diagnose`, all plugin authors |

---

## Citation format

Skills must cite the source when applying a rule:

> ✓ Added 60ms hit-stop on melee impact ([Vlambeer P4](vlambeer-juice.md#p4--hit-stop-frame-freeze-on-impact))
> ✓ Camera kick on dash ([Vlambeer P10](vlambeer-juice.md#p10--camera-kick))
> ✓ Cinemachine Impulse Source on player (Unity Pattern [UP5](unity-patterns.md#up5--cinemachine-30-impulse-for-camera-shake))
> ⚠ Telegraph time only 100ms — recommend 200ms+ ([GMTK Telegraph everything](gmtk-patterns.md#telegraph-everything))
> ⚠ Inventory crafting may be scope creep ([Tyroller M11](tyroller-mistakes.md#m11--perfectionism-on-side-systems))

This is non-negotiable. **Every recommendation traceable to a source.**

That's the moat. Other skills give you templates. We give you *why*.

---

## Adding new sources

When the user's project requires knowledge not in the library:

1. Identify the gap (e.g., "we need shooter-specific patterns")
2. Add a new file (e.g., `shooter-patterns.md`)
3. Source from named, credible designers (avoid generic "best practices")
4. Cross-link with `[[file-name]]` syntax
5. Add to this INDEX

Never invent rules without a named source. Taste comes from people, not consensus.

---

## What this is NOT

- ❌ A list of "best practices" with no source
- ❌ Personal preferences
- ❌ Theory disconnected from games that shipped
- ❌ Genre clichés

What it IS:

- ✅ Encoded knowledge from designers with shipped, beloved games
- ✅ Actionable rules with specific defaults (numbers, times, magnitudes)
- ✅ Connected — every rule has anti-patterns and "see also" links
- ✅ Cited — every application traces back to source

---

## The reading list (for humans using vibe-game)

If you want to internalize these rules yourself:

1. **"The Art of Screenshake"** — Jan Willem Nijman talk (37 min, free on YouTube). Watch first.
2. **Game Feel** by Steve Swink (book). Read chapters 1, 7, 14.
3. **GMTK YouTube channel** — Mark Brown. Start with "Game Maker's Toolkit: Boss Keys."
4. **Jonas Tyroller's devlog** — "Why most indie games fail" and the *Will You Snail* devlog series.
5. **Spelunky** by Derek Yu (book). The model post-mortem.

These five sources cover ~90% of the taste needed to ship a great indie game.
