# Test Plan — Cleave Demo

> Step-by-step test guide. If something breaks, paste the console error or screenshot back into Claude Code.

## Setup (5 min)

### 1. Create a new Unity project
- Open Unity Hub → New Project
- Editor version: **Unity 6000.x** (preferred) or **2022.3 LTS**
- Template: **Universal 3D** (URP). 2D URP also works but expect a darker look.
- Project name: `Cleave-Demo` (any name fine)
- Location: anywhere convenient
- Click **Create project**

### 2. Verify Input System is active
- Edit → Project Settings → Player → Other Settings → Active Input Handling
- Set to **Input System Package (New)** or **Both**
- If Unity prompts to restart, click **Apply**

If "Input System Package" isn't installed:
- Window → Package Manager → Unity Registry → search "Input System" → Install

### 3. Drop in the scripts
- Open your Unity project folder in File Explorer
- Copy `Assets/_Project/Scripts/` from this demo folder into your Unity project's `Assets/`
- You should end up with:
  ```
  YourProject/Assets/_Project/Scripts/
    ├── Bootstrap.cs
    ├── CleaveAttack.cs
    ├── Enemy.cs
    ├── EnemySpawner.cs
    ├── GameManager.cs
    ├── Juice.cs
    ├── PlayerController.cs
    ├── ScoreUI.cs
    └── UnityCompat.cs
  ```
- Switch back to Unity — wait for it to compile (bottom-right spinner stops)

### 4. Check the console
- Window → General → Console (or Ctrl+Shift+C)
- **Should be empty** (no red errors)
- If there are **red errors**: copy them and paste back to Claude Code

### 5. Create the scene
- File → New Scene → **Basic (Built-in)** or **Basic (URP)** → Create → Save as `Cleave`
- In the Hierarchy panel, right-click → Create Empty → name it `Bootstrap`
- Click the `Bootstrap` GameObject → in Inspector, click **Add Component** → type "Bootstrap" → select it

### 6. Press Play
- Click the ▶ Play button at the top of the editor
- Expected behavior:
  - You see a dark arena ringed by gray walls
  - A white capsule (you) at the center
  - "60" timer at the top center of the screen
  - "SCORE 0" at the top-left
  - Enemies (red/orange capsules) spawn from the arena edge and walk toward you

## Game test (60 seconds)

### Movement
- **WASD or arrow keys** → player moves
- Camera stays static (top-down fixed view)
- Player rotates to face the mouse cursor
- Expected: smooth movement with slight squash-stretch when moving

### Cleave (the core verb)
- **Hold Left Mouse Button** → start charging
- Release LMB → swing
- **Timing windows:**
  - Released < 0.18s → no swing (too short)
  - Released 0.18s - 0.5s → **WEAK** cleave (small radius 2u, small shake)
  - Released 0.5s - 1.2s → **CRIT** cleave (big radius 5u, big shake, golden flash, possible slo-mo on multi-kill)
  - Released > 1.2s → **STRONG** cleave (medium radius 3.6u)
- Expected: hits cause camera shake, brief time freeze (hit-stop), white flash on hit enemies, knockback

### Enemies
- Red capsules = **Heavy** (3 HP, slow, telegraph attack with color change)
- Orange capsules = **Light** (1 HP, fast)
- Enemy attack: red capsule pulses orange for 400ms, then if you're in range, you take a hit
- After 3 hits → you die → "DOWN" appears
- After 60 seconds → win screen with rank (SURVIVED / CLEAVER / REAPER)

### Restart
- Press **R** to restart at any time (during or after match)

## What to watch for

### Likely OK
- Movement feels responsive
- Cleave timing has clear feedback
- Camera shake on impact
- Hit-stop on impact (frame freezes briefly)
- Combo counter increases with consecutive kills (no damage between)
- Combo resets to 0 when you take damage

### Possibly broken (report if you see)
- ❌ Compile errors in console (red text)
- ❌ Materials look pink/magenta (shader missing — URP/Unlit not found)
- ❌ Mouse facing broken (player rotates wildly or not at all)
- ❌ Enemies don't spawn
- ❌ Enemies don't attack
- ❌ Cleave doesn't hit (or hits nothing)
- ❌ HUD text invisible or off-screen
- ❌ Time stays frozen after restart
- ❌ Frame rate drops below 60 on a decent PC

### Acceptable for v0.1 (will polish later)
- ⚠ No sound (silent build — needs `/grab-asset` for SFX)
- ⚠ No particles on impact (need ParticleSystem prefabs)
- ⚠ Damage numbers absent (IMGUI HUD is minimal)
- ⚠ Visuals very primitive (capsules + boxes — `/grab-asset` replaces these)

## How to report back

If something works: just say so. We move to next step.

If something breaks:
1. **Copy the console error** if any (full text)
2. **Take a screenshot** if visual
3. **Describe what you did + what you saw vs expected**

Paste into Claude Code. I'll diagnose and patch.

## What this test validates

- ✅ `/prototype` skill produces working Unity code, not boilerplate
- ✅ Code is project-aware (Unity 6 + new Input System + URP detected)
- ✅ Mood-aware juice (restrained, weighty — not max chaos)
- ✅ 30-minute-to-playable rule (you got there in ~5 min setup + 0 min code-writing)
- ✅ Verb-first start (cleave available in second 1 of pressing Play)

A successful test means `vibe-game` actually delivers on its core promises.
