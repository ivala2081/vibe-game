# Tyroller's Indie Mistakes

> Source: Jonas Tyroller, "Why most indie games fail" and devlog series.
> Project-level anti-patterns that kill solo indie projects.

Where [[vlambeer-juice]] is *moment-to-moment* and [[game-feel-swink]] is *theoretical*,
this is *project survival*. The `/cut` skill and `/death-watch` reference these.

---

## M1 — Scope explosion
The single biggest indie killer.
- **Pattern:** "Let's add multiplayer too" / "What if it's an MMO?"
- **Heuristic:** if it'll take >2 weeks and isn't core, **cut it**.
- **Fix:** scope to what you can build in 1/3 of your believed timeline.

## M2 — No prototype, lots of planning
50-page GDD, no playable build after 2 months.
- **Heuristic:** if you don't have something playable in week 1, you're failing.
- **Fix:** [[vibe-brief]] (1 page) + `/prototype` (30 min playable).

## M3 — Polishing the wrong thing
6 weeks on menus while the core loop is unfun.
- **Heuristic:** every hour of polish only matters if the core loop is fun.
- **Fix:** `/feel-check` the core loop before *anything* else gets polish.

## M4 — Premature visual style
2 months on art before knowing if the game is fun.
- **Fix:** placeholder art / Kenney CC0 / shapes until the gameplay is locked.

## M5 — Solo dev with no playtest
Building in a vacuum.
- **Heuristic:** if no one outside your head has played it in 2 weeks, danger zone.
- **Fix:** `/feel-check` (AI playtest) + send to 3 humans every fortnight.

## M6 — "It'll be different when..."
"It'll be fun when the art is in" / "...when there's music" / "...when the story's there."
- **Truth:** if the gray-box prototype isn't fun, the finished game won't be either.
- **Test:** strip all art, sound, story. Is the *interaction* still engaging?

## M7 — Engine over-investment
6 months building a custom engine for a game that should ship in Unity.
- **Fix:** use Unity. Use Cinemachine. Use the Input System. Use Universal RP. Don't reinvent.

## M8 — Tutorial bloat
2 hours of tutorial for a 30-minute game.
- **Heuristic:** tutorial length ≤ 5% of game length.
- **Fix:** in-context teaching, not gated upfront tutorials.

## M9 — Saving for "later" features
Building infrastructure for features that may never ship.
- **Heuristic:** YAGNI. Build what you need now. The future is uncertain.

## M10 — Marketing as an afterthought
"I'll do Twitter when the game is done."
- **Truth:** by then you have no audience.
- **Fix:** devlog from day 1. Even 1 GIF per week.

## M11 — Perfectionism on side systems
3 weeks on inventory UI when no one will spend >2 min in inventory.
- **Heuristic:** time-spent-in-system × system-importance = time-allowed.

## M12 — Genre mash without reason
"It's a Roguelike Metroidvania Bullet-Hell Deck-Builder."
- **Truth:** mashing genres is hard. Each one needs to earn its place.
- **Test:** can you explain the appeal in 1 sentence without "meets"?

## M13 — Ignoring the first 30 seconds
Players judge within 30 seconds. If yours starts with menus and tutorials, you lose them.
- **Fix:** verb-first start. Player is *doing* the core verb within 10 seconds.

## M14 — No clear "out"
Game with no endgame. Player doesn't know when they've "won."
- **Fix:** explicit win state, even if it's "beat the final boss" or "reach floor 50."

## M15 — Difficulty tuned to the developer
You're the worst playtester of your own game — you're the best player.
- **Fix:** target playtest from someone who's never played. They are your real difficulty curve.

## M16 — Late-game content first
Building floor 50 before floor 1 is fun.
- **Fix:** fix floor 1 until it's *great*. Then 2. Then 3. Late game is the last 10%.

## M17 — Ignoring the kill switch
Not knowing when to pivot or stop.
- **Heuristic:** if it's been 6 months and no playtester says "wow," consider pivoting.
- **Fix:** `/death-watch` — schedule honesty checkpoints.

---

## Bonus: Tyroller's "Will it be fun?" filter

Before adding *anything*:
1. Will this make the first 30 seconds better? (If no, lower priority.)
2. Will this make the player make more interesting decisions? (If no, kill it.)
3. Could I cut this and the game still works? (If yes, strongly consider.)
4. Does this earn its development time? (If <2x value, cut.)

---

## How `/cut` uses this

The scope-killer agent quotes specific mistakes when recommending cuts:

> ⚠ Removing inventory crafting system.
>
> **Why:** ~~Tyroller M11~~ (time-spent × importance). Crafting is 3 weeks of work.
> Players spend <2 min/session in it. Doesn't drive your core verb (combat).
>
> **Saved:** ~80 hours of dev time. Spend it on `/juice` for combat instead.
>
> **If you keep it:** justify in 1 sentence why it earns its 3 weeks.

## How `/death-watch` uses this

When triggered (2+ weeks low activity, no playtest, scope creep):

> 🚨 Death-watch alert.
>
> Indicators:
> - Last playtest: 18 days ago (Tyroller M5)
> - Features added since: 4 (Tyroller M1, scope creep)
> - Core loop changes: 0 (Tyroller M3, polishing wrong thing)
>
> Hard questions:
> 1. Is the core loop fun? (When did someone last say "wow"?)
> 2. Should we cut the last 4 features and ship?
> 3. Are we polishing because we're scared to ship?

## See also

- [[vlambeer-juice]] — moment-to-moment feel
- [[game-feel-swink]] — theoretical foundation
- [[gmtk-patterns]] — design patterns
- [[vibe-brief]] — the 1-page scope shield
