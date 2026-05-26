#!/usr/bin/env bash
# juice-citation-check.sh
#
# Fires after Edit/Write. If the change touches juice-relevant code (shake, hit-stop,
# easing, particles, etc.) and the diff body has no taste-library citation
# (Vlambeer P#, Game Feel ch.#, GMTK, Tyroller M#), nudge the model to cite.
#
# Why: the moat of vibe-game is *cited* taste. Uncited polish is just bolted-on juice.

set -e

input=$(cat || true)
file=$(printf '%s' "$input" | grep -oE '"file_path"[[:space:]]*:[[:space:]]*"[^"]+"' | head -1 | sed -E 's/.*"file_path"[[:space:]]*:[[:space:]]*"([^"]+)".*/\1/')

if [ -z "$file" ]; then exit 0; fi
case "$file" in
  *.cs|*.md) ;;
  *) exit 0 ;;
esac

# Only nag if the file actually mentions juice topics
if [ ! -f "$file" ]; then exit 0; fi

if ! grep -qiE 'Shake|HitStop|Impulse|timeScale|SlowMo|Particle|squash|stretch|easing|telegraph' "$file"; then
  exit 0
fi

# If file already cites a taste source anywhere, we're fine
if grep -qiE 'Vlambeer P[0-9]+|Game Feel|Tyroller M[0-9]+|GMTK|Swink' "$file"; then
  exit 0
fi

cat <<'EOF'
[vibe-game] Juice-adjacent code change detected, but no taste-library citation found.
Add a comment referencing the source (e.g. // [Vlambeer P4] hit-stop on impact).
The moat of vibe-game is cited taste — uncited polish is just bolted-on juice.
See taste/INDEX.md for the citation format.
EOF
exit 0
