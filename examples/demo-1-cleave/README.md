# Cleave — Demo #1

> Showcase demo for [Vibe Game for Unity](../../README.md).
> Built by dogfooding the skill on itself.

**60 seconds. One arena. One swing.**
The first vibe-game showcase title — a tense, weighty, click-charge cleave score-attack.

---

## What's in this folder

```
demo-1-cleave/
├── vibe-brief.md              ← What /vibe-start produced
├── README.md                  ← This file
├── JUICE-LOG.md               ← What /juice produced (citations included)
├── FEEL-CHECK.md              ← What /feel-check produced (Swink 6 + Fun-O-Meter)
└── Assets/_Project/Scripts/   ← What /prototype produced
    ├── Bootstrap.cs           ← Creates the entire scene from code
    ├── PlayerController.cs    ← Movement + charge + cleave input
    ├── CleaveAttack.cs        ← The 360° radial attack
    ├── Enemy.cs               ← Walks toward player, telegraphs
    ├── EnemySpawner.cs        ← Wave-based 60s spawn pattern
    ├── Juice.cs               ← Hit-stop, screen shake, slo-mo
    ├── GameManager.cs         ← Timer, score, win/lose
    └── ScoreUI.cs             ← Score + Combo + Time HUD
```

## Run it in 5 minutes

### 1. Create a fresh Unity project

- Unity 6000.x or 2022.3 LTS
- Template: **2D (URP)** or **3D (URP)** — both work
- Project name: `Cleave-Demo`

### 2. Install required packages

Window → Package Manager → install:
- `com.unity.inputsystem` (Input System)
- `com.unity.render-pipelines.universal` (URP — included if you picked the URP template)

When prompted to switch to the new Input System: **Yes.** Unity restarts.

### 3. Drop the scripts in

Copy this folder's `Assets/_Project/Scripts/` into your Unity project's `Assets/` folder.

You should end up with:
```
Assets/_Project/Scripts/Bootstrap.cs
Assets/_Project/Scripts/PlayerController.cs
... (8 files)
```

### 4. Create the scene

- File → New Scene → **Basic (URP)** → Save as `Cleave.unity`
- In the scene, create a GameObject (right-click in Hierarchy → Create Empty), name it **`Bootstrap`**
- Drag `Bootstrap.cs` onto it (Add Component → search Bootstrap)

That's it. No prefabs, no inspector wiring — `Bootstrap.cs` builds everything programmatically.

### 5. Press Play

- WASD or arrow keys → move
- Hold LMB → charge cleave (release in the green window for crit)
- Survive 60 seconds
- R → restart

---

## Read the dogfood artifacts

These three files show what the `vibe-game` skill pack *actually produces* when used end-to-end:

| File | What it shows |
|------|---------------|
| [vibe-brief.md](vibe-brief.md) | Output of `/vibe-start` — the 1-page contract |
| Source files in `Scripts/` | Output of `/prototype` — project-aware code |
| [JUICE-LOG.md](JUICE-LOG.md) | Output of `/juice` — each polish change with cited source |
| [FEEL-CHECK.md](FEEL-CHECK.md) | Output of `/feel-check` — Swink 6 + Fun-O-Meter scoring |

If you're evaluating `vibe-game` before installing it, these are the files to read.

## Caveats

- This demo runs **without Cinemachine** to minimize required packages. In a real project, `/juice` will prefer Cinemachine Impulse Source (more powerful, see [Vlambeer P3](../../taste/vlambeer-juice.md#p3--screen-shake)).
- Visuals are **primitives + URP/Unlit color** — no sprites. Replace with `/grab-asset` for production.
- SFX is **silent** in this drop-in version (no .wav assets ship in the repo). Run `/grab-asset` for CC0 hit/swing sounds, or pull from [Kenney Impact Sounds](https://kenney.nl/assets/impact-sounds).

## Credits

- Built using [Vibe Game for Unity](../../README.md)
- Design taste: Vlambeer, Steve Swink, Jonas Tyroller, GMTK (see [taste library](../../taste/))
