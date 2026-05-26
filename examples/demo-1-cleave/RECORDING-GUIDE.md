# Recording the Cleave Demo GIF

> For the README hero asset. ~30 seconds of gameplay, looped, <5 MB.

The repo needs a gameplay GIF to land. Without it, the README reads as docs.
With it, it reads as a product.

## Target output

- **Format:** `.gif` (universally supported in markdown) or `.mp4` (better quality, GitHub renders both)
- **Duration:** 15-30 seconds
- **Dimensions:** 960×540 or 1280×720
- **File size:** <5 MB (GIF), <10 MB (MP4)
- **Content:** core verb on display — charge, release, crit, multi-kill, slo-mo
- **Loop point:** end gameplay should match start position so loop feels seamless

## Method A — Unity Recorder (best quality, recommended)

### Setup

1. Open Cleave-Demo Unity project.
2. **Window → Package Manager**
3. Click "+" → "Add package by name..."
4. Enter: `com.unity.recorder`
5. Wait for install (~30s)

### Record

1. Open scene with Bootstrap GameObject
2. **Window → General → Recorder → Recorder Window**
3. Click "+ Add Recorder" → **Image Sequence** for GIF source, or **Movie** for MP4
4. Set:
   - **Recording Mode:** Manual
   - **Frame Rate:** 30 fps (60 if MP4)
   - **Output Resolution:** 960×540
   - **Output Path:** `Builds/Recording/cleave-gameplay`
5. Click **START RECORDING**
6. **Press Play** in Unity Editor (Recorder will auto-start)
7. Play 30 seconds — show:
   - Movement (3s)
   - Charge a weak swing (2s)
   - Charge to crit window — kill 2-3 enemies in golden flash (10s)
   - Take a hit, white flash + screen shake (3s)
   - Final crit multi-kill with slow-mo (5s)
   - Score increasing (visible)
8. **Stop recording**

### Convert to GIF (if used Image Sequence)

Use [ezgif.com/maker](https://ezgif.com/maker) — upload the image sequence,
set frame delay to 33ms (30fps), output GIF.

Or use ffmpeg:
```bash
ffmpeg -framerate 30 -i Recording/cleave-gameplay_%04d.png \
       -vf "fps=20,scale=960:-1:flags=lanczos" \
       -loop 0 cleave-gameplay.gif
```

## Method B — ShareX (no Unity setup, fastest)

1. Install [ShareX](https://getsharex.com/) (free, open source, Windows)
2. Open Cleave-Demo in Unity, press Play
3. Hotkey **Shift+PrintScreen** in ShareX → "Screen recording (GIF)"
4. Select the Unity Game tab area
5. Record 30 seconds
6. ShareX saves to `Documents/ShareX/Screenshots/YYYY-MM/`
7. Open the GIF in ezgif.com to optimize file size if needed

## Method C — Windows Game Bar (built-in, MP4 only)

1. Open Unity, press Play
2. **Win + G** → open Xbox Game Bar
3. Click record icon (or **Win + Alt + R**)
4. Record 30 seconds
5. Saves to `Videos/Captures/`
6. Convert to GIF via ezgif.com if needed (or just use MP4 in README)

## Where to place the file

Put the final GIF/MP4 here:

```
docs/screenshots/cleave-gameplay.gif
```

(Create `docs/screenshots/` if it doesn't exist.)

## Embed in README

Add to the top of `README.md` after the badges, before "Built for Unity":

```markdown
![Cleave gameplay — charge, release, kill](docs/screenshots/cleave-gameplay.gif)
```

Optionally a clickable thumbnail to a longer video:

```markdown
[![Cleave gameplay](docs/screenshots/cleave-gameplay.gif)](https://youtu.be/YOUR_VIDEO_ID)
```

## Filming tips

- **Crop the Unity chrome out.** Just gameplay. Use the Game tab fullscreen
  (right-click tab → maximize).
- **Hide the FPS counter** if visible.
- **Mute the system** to avoid background sounds bleeding in (the demo is silent anyway).
- **Practice run first.** Get to the moment you want to capture quickly.
- **Loop seamlessly.** End on the same camera framing you started with.
- **Compress.** A 15 MB GIF dies on slow connections. Run through
  [ezgif.com/optimize](https://ezgif.com/optimize) — set "Lossy" to 30-50.

## Posting

Once embedded in README and pushed:

```bash
git add docs/screenshots/cleave-gameplay.gif README.md
git commit -m "docs: add Cleave gameplay GIF to README"
git push
```

Then mention it in your devlog post:
```
Cleave demo now lives on GitHub with a 30s gameplay GIF.
github.com/ivala2081/vibe-game

Built end-to-end using the skill pack on itself.
```

That's how the showcase reel begins.
