#!/usr/bin/env bash
# session-start.sh
#
# Fires once at the start of every Claude Code session in a project.
# If the cwd looks like a Unity project, surface a one-line vibe-game welcome
# with the next recommended action (depending on whether a vibe-brief exists).
#
# Why a SessionStart hook: zero-friction onboarding. The user doesn't need to
# remember /vibe-game:vibe-start — we tell them what's next based on state.

set -e

# Hook input arrives on stdin (we don't strictly need it for SessionStart but
# read it to keep the stream clean and avoid stalling)
input=$(cat || true)

# Locate the cwd from the hook payload, fall back to PWD
cwd=$(printf '%s' "$input" | grep -oE '"cwd"[[:space:]]*:[[:space:]]*"[^"]+"' | head -1 | sed -E 's/.*"cwd"[[:space:]]*:[[:space:]]*"([^"]+)".*/\1/')
cwd="${cwd:-$PWD}"

# Detect Unity project: Assets/ + ProjectSettings/ both exist
if [ ! -d "$cwd/Assets" ] || [ ! -d "$cwd/ProjectSettings" ]; then
  exit 0
fi

# Identify Unity version if we can
unity_version="unknown"
if [ -f "$cwd/ProjectSettings/ProjectVersion.txt" ]; then
  unity_version=$(grep -E '^m_EditorVersion:' "$cwd/ProjectSettings/ProjectVersion.txt" 2>/dev/null | awk '{print $2}' || echo "unknown")
fi

# Check for an existing vibe-brief in the standard locations
has_brief="no"
for candidate in \
    "$cwd/vibe-brief.md" \
    "$cwd/Assets/_Project/vibe-brief.md" \
    "$cwd/Assets/vibe-brief.md"; do
  if [ -f "$candidate" ]; then
    has_brief="yes"
    break
  fi
done

# Surface a single line — keep it tight so it doesn't pollute the session start
if [ "$has_brief" = "yes" ]; then
  printf '[vibe-game] Unity %s project + vibe-brief detected. Run /vibe-game:prototype or /vibe-game:juice to keep moving.\n' "$unity_version"
else
  printf '[vibe-game] Unity %s project detected, no vibe-brief.md yet. Start with /vibe-game:vibe-start (5 min to lock the soul).\n' "$unity_version"
fi

exit 0
