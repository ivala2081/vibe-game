# GMTK Design Patterns

> Source: Mark Brown's Game Maker's Toolkit series (youtube.com/@GMTK).
> Structural design patterns — bigger than juice, smaller than genre.

These shape *what kind of decisions* the player makes.

---

## Decision-making patterns

### Interesting choices (Sid Meier)
> "A game is a series of interesting decisions."

A decision is *interesting* when:
- Multiple options are viable (not one obvious right answer)
- Trade-offs are real and legible
- The player has enough info to choose
- The outcome teaches something for next decision

**Anti-pattern:** "choices" with one optimal answer = no choice.
**Anti-pattern:** decisions where the player can't tell what's better = noise, not choice.

### Risk vs. reward
Every high-reward option must carry visible risk.
- **Default ratio:** big reward = big risk, telegraphed clearly.
- **Spelunky model:** every item has a cost. Every shortcut has a danger.

### Mastery curve
Difficulty rises with player skill — not with clock time.
- **Pattern:** introduce mechanic safely → drill it → combine with others → demand mastery.
- **Anti-pattern:** difficulty spikes that don't teach.

---

## Level design patterns

### Kishōtenketsu (4-act level design)
1. **Ki** — Introduce the mechanic (safe environment).
2. **Shō** — Develop it (slightly harder).
3. **Ten** — Twist it (unexpected use).
4. **Ketsu** — Conclude (mastery test).

Used by Nintendo (Mario, Zelda). Every level becomes a self-contained learning arc.

### The 80/20 reveal
Players see 80% of the level, only 20% is hidden.
- **Why:** hidden content rewards exploration. Too much hidden = confusion.

### Sightlines as direction
Players follow what they can see. Camera, lighting, and silhouettes do the level design.
- **Junji Ito principle:** what's framed is what matters.

### One verb, many problems
A level should test one core mechanic against multiple problem shapes.
- **Bad:** "use 5 different items in this room."
- **Good:** "use the grapple in 5 different ways across this room."

---

## Combat design patterns

### Telegraph everything
Every enemy attack has a wind-up phase (Vlambeer P15 territory).
- **Default:** 200–500ms tell.
- **Color flash, posing, particle warning** — any of the three.

### The 3-attack pattern
Enemies that use only 1 attack are boring. 3 is the sweet spot.
- **Default:** quick / medium / heavy. Different telegraphs and counters.

### Player aggression rewards
Defensive play should always be *less* rewarding than aggressive play.
- **Pattern:** parry-only enemies get boring. Punish-window enemies stay tense.
- **Sekiro/DOOM Eternal model.**

### Trash mob role
Weak enemies exist to:
1. Make the player feel powerful
2. Force movement / positioning
3. Create chaos that hides the elite enemy's wind-up

Don't waste design budget making them complex.

---

## Roguelike / replayable patterns

### The "one more run" loop
- Run length: 20–40 min (short enough to retry, long enough to invest)
- Meta-progression: visible between runs (not just in-run)
- Surprise budget: 10–20% of content new each run

### Synergy explosion
Items combine in unexpected ways.
- **Pattern (BoI, Slay the Spire):** the joy is in finding *interactions*, not just power.
- **Test:** does build B feel different from build A, not just stronger?

---

## UI / UX patterns

### Diegetic UI
UI that exists in the world (ammo on the gun, health on the suit).
- **When to use:** immersive games with consistent style.
- **When to skip:** information-dense games (RTS, deck builders).

### The 1-second rule
Critical info (health, ammo) must be readable in 1 second from peripheral vision.
- **Test:** look at the center of screen, can you still read your HP?

### Feedback layers
Three layers for every meaningful event:
1. **Visual** — particle / animation / number
2. **Auditory** — distinct SFX
3. **Haptic** — controller vibration (if applicable)

Missing one layer = event feels weak.

---

## Difficulty / accessibility patterns

### Dynamic difficulty (Resident Evil 4 model)
Game silently tracks player performance and adjusts:
- Enemy aim accuracy
- Ammo drop rates
- Health pickup spawns

**Done well** = invisible. Player feels in flow.
**Done badly** = player notices and resents.

### Difficulty as choice
Don't gate the game behind difficulty. Let players choose.
- **Default:** easy / normal / hard, with descriptions of *what changes*, not "for casuals/pros."

### Accessibility minimums
- Subtitles for all dialogue (with size/bg options)
- Remappable controls
- Color-blind palette toggle
- Toggle-able hold-to-press buttons

---

## Pacing patterns

### Tension-release rhythm
Combat → exploration → combat → puzzle → boss.
- **Anti-pattern:** all combat. Players burn out.
- **Default:** 60% engagement / 40% breathing room.

### The Z-curve (Brown's term)
Easy → spike → easier → bigger spike → easier → boss.
- Player needs the dips to recover and feel powerful.

---

## How `/feel-check` uses these patterns

When evaluating a build:

```
GMTK Pattern Audit
──────────────────
✓ Telegraphs present on enemy attacks (200ms+)
⚠ Only 1 attack type per enemy — recommend 3-attack pattern
✓ Diegetic UI consistent with style
⚠ No clear Kishōtenketsu in current level — feels like one long corridor
✓ Risk-reward visible on pickups
```

## See also

- [[vlambeer-juice]] — moment-to-moment polish
- [[game-feel-swink]] — input/response theory
- [[tyroller-mistakes]] — what kills indie projects
