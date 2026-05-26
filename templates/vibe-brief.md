# Vibe Brief — `{{GAME_NAME}}`

> 1 page replaces 50 pages of GDD.
> If you can't fit it on one page, you don't understand your game yet.

---

## Mood
**3 adjectives.** What does the player *feel*?

> Example: "tense, lonely, hopeful"
> Example: "frenetic, vibrant, ridiculous"

`{{MOOD}}`

---

## Verb
**1 word.** What does the player physically *do*?

> Example: "drift" / "build" / "shoot" / "deduce" / "weave"

`{{VERB}}`

---

## References
**3 games + 1 thing from outside games** (film, book, song, painting, weather, anything).

> Example:
> - *Hotline Miami* — the violent rhythm
> - *Devil Daggers* — the score-attack tension
> - *Katana ZERO* — the slow-mo planning beats
> - + The cold opening of *Drive (2011)*

`{{REFERENCES}}`

---

## Hook
**1 sentence.** What would you tell a stranger on the bus?

> Example: "It's a roguelike where every enemy plays the same song you do, slightly out of tune."
> Example: "You're a courier on a dead planet, and you can't run out of fuel."

`{{HOOK}}`

---

## Anti-features
What this game is **NOT**. Be specific.

> Example:
> - Not a Souls-like (no stamina, no parry)
> - No tutorial (in-context only)
> - No story cutscenes
> - No microtransactions
> - No procgen (handcrafted)

`{{ANTI_FEATURES}}`

---

## Win / Lose
**2 lines each.**

**Win state:** how does the player finish?
> Example: "Reach floor 50. Or place top 10 on the daily seed."

**Lose state:** how does the player fail?
> Example: "Die. Restart from floor 1. Permadeath. <5 sec retry."

`{{WIN}}`

`{{LOSE}}`

---

## Core loop (60 seconds)
What is the player doing in any given 60 seconds of gameplay?

> Example: "Enter room → assess enemies → execute attack rhythm → collect drops → next room. Loop, with rising difficulty."

`{{CORE_LOOP}}`

---

## Scope budget
- **Total dev time target:** `{{TIMELINE}}` (e.g., "48h jam" / "3 months" / "1 year")
- **Levels / content:** `{{CONTENT_BUDGET}}` (e.g., "10 rooms" / "5 biomes" / "1 long level")
- **Enemy types:** `{{ENEMY_COUNT}}`
- **Weapons / abilities:** `{{ABILITY_COUNT}}`

If your scope is bigger than [Tyroller M1](../taste/tyroller-mistakes.md#m1--scope-explosion) allows,
the `/cut` skill will flag it.

---

## What success looks like
**1 sentence.** Not "good reviews" — concrete.

> Example: "A streamer plays it and replays the final boss 3 times because it felt that good."
> Example: "I send it to 10 friends and 6 finish it."
> Example: "It places top 30 in the jam."

`{{SUCCESS}}`

---

## Decision log
*(Auto-populated as the project evolves. Every major design choice and what was chosen instead.)*

| Date | Decision | Alternatives considered | Why |
|------|----------|------------------------|-----|
| | | | |

---

## Notes for vibe-game skills

- `/cut` reads this brief. If a proposed feature violates **Anti-features**, it auto-cuts.
- `/prototype` builds the scene to support the **Verb** + **Core loop**.
- `/juice` matches the **Mood** when picking polish (a "tense" game gets restrained shake, a "frenetic" game gets max chaos).
- `/feel-check` scores against the **Success** definition.

**The brief is the contract.** Update it deliberately. Reference it constantly.
