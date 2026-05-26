#!/usr/bin/env bash
# vibe-brief-check.sh
#
# Fires after Edit/Write. If the user is touching a .cs file inside a Unity project's
# Assets/ folder but no vibe-brief.md exists, surface a one-line reminder.
#
# Why: Tyroller M2 — coding without a brief is one of the top scope-creep paths.
# /vibe-start fixes this in 5 minutes.

set -e

# Hook input arrives as JSON via stdin
input=$(cat || true)

# Extract the modified file path (Claude Code provides this in tool_input.file_path)
file=$(printf '%s' "$input" | grep -oE '"file_path"[[:space:]]*:[[:space:]]*"[^"]+"' | head -1 | sed -E 's/.*"file_path"[[:space:]]*:[[:space:]]*"([^"]+)".*/\1/')

# Bail if no file path or not a Unity C# script under Assets/
if [ -z "$file" ]; then exit 0; fi
case "$file" in
  *Assets/*.cs) ;;
  *) exit 0 ;;
esac

# Walk up to find the Unity project root (folder containing Assets/ and ProjectSettings/)
dir=$(dirname "$file")
while [ "$dir" != "/" ] && [ "$dir" != "." ]; do
  if [ -d "$dir/Assets" ] && [ -d "$dir/ProjectSettings" ]; then
    project_root="$dir"
    break
  fi
  dir=$(dirname "$dir")
done

if [ -z "${project_root:-}" ]; then exit 0; fi

# Check for vibe-brief at common locations
if [ -f "$project_root/vibe-brief.md" ] \
   || [ -f "$project_root/Assets/_Project/vibe-brief.md" ] \
   || [ -f "$project_root/Assets/vibe-brief.md" ]; then
  exit 0
fi

# Surface a reminder (goes to user via stdout per Claude Code hook spec)
cat <<'EOF'
[vibe-game] No vibe-brief.md found in this Unity project.
Consider running /vibe-game:vibe-start before deep coding —
shapes mood/verb/scope and prevents Tyroller M2 (planning vacuum).
EOF
exit 0
