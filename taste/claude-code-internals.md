# Claude Code Internals

> Distilled public knowledge about how Claude Code loads plugins, skills, hooks,
> and agents — the bits the official docs leave fuzzy. Sourced from
> [official plugin docs](https://code.claude.com/docs/en/plugin-marketplaces),
> [JSON schemas](https://github.com/hesreallyhim/claude-code-json-schema), and
> direct observation of Claude Code v2.1.150 behavior.

Skills cite these by ID: `CC1` (Claude Code Insight 1), `CC2`, etc.

These are not Unity patterns — they're patterns of the **plugin host** itself.
Used by `/diagnose` and the workflow skills.

---

## CC1 — Plugin counter reports `command_count`, mislabels it as "skills"
**Observed:** `Reloaded: 3 plugins · 0 skills · 10 agents · 2 hooks · …`
**Reality:** "skills" in this output is `command_count` (commands from `commands/`).
Plugin **skills** (from `skills/<name>/SKILL.md`) are loaded via a separate path
and **not included in the counter**.

**Implication:** If you have 7 skills but the counter says "0 skills", **do not panic**.
Test by invoking `/your-plugin:your-skill` directly. If it runs, your skills are loaded.

**Diagnostic:** Look at `~/.claude/plugins/cache/<marketplace>/<plugin>/<version>/skills/`
to confirm files were copied. If they're there and `/<plugin>:<skill>` works → fine.

This is a Claude Code v2.x display quirk, not a problem with your plugin.

---

## CC2 — Plugin cache is keyed by manifest version
**How updates propagate:**
1. You edit a skill in your source repo
2. `/plugin marketplace update <marketplace>` fetches new manifest
3. **If `version` in plugin.json changed**, Claude Code clones to a new version dir
4. **If `version` unchanged**, cache is reused — your edits are NOT picked up

**Implication:** During development, **bump the version on every change** that touches
plugin contents. Use `0.x.y` patches freely; users only see the displayed version
in `displayName`.

**Cache location (Windows):**
```
C:\Users\<user>\.claude\plugins\cache\<marketplace-name>\<plugin-name>\<version>\
```

**Manual cache bust:** delete the version dir, then `/reload-plugins`.

---

## CC3 — SKILL.md frontmatter `name:` field is the **display name**, not the skill ID
**The skill ID = directory name.** A SKILL.md in `skills/vibe-start/SKILL.md` is
always invoked as `/<plugin>:vibe-start`, regardless of what `name:` says inside.

**What `name:` does:** Sets the user-facing label in some UI surfaces.
**What `name:` does NOT do:** Change the slash command, change skill discovery,
disambiguate from other plugins.

**Practical:** Keep `name:` equal to the directory name to avoid confusion.
Use `description:` to set the discovery / preview text — that's the field that
actually matters for usability.

---

## CC4 — `disable-model-invocation` controls auto-invocation, not slash access
| Value | Effect |
|-------|--------|
| `false` (default) | Model can auto-invoke when relevant; user can also `/<plugin>:<skill>` |
| `true` | Only user can invoke via slash command; model never picks it up automatically |

**For vibe-game:** Most skills use `false` so Claude can offer them when the user
hits the trigger condition (e.g., "this feels flat" → Claude suggests `/juice`).
Set `true` for skills that have side effects or cost (e.g., `/ship` build).

---

## CC5 — `paths` frontmatter activates skill conditionally on file matches
Adding `paths:` to SKILL.md frontmatter makes the skill **conditional** —
it only activates when Claude touches matching files in the session.

```yaml
---
name: juice
description: "..."
paths: Assets/**/*.cs
---
```

**Why this matters:** A juice pass on a non-Unity file makes no sense. The skill
should only surface when the user is editing Unity scripts. This keeps the
skill list focused and prevents irrelevant suggestions.

**Pattern syntax:** Uses [`ignore`](https://www.npmjs.com/package/ignore) library
(same as `.gitignore`):
- `Assets/**/*.cs` — recursive
- `*.shader` — top-level only
- `**/*.unity` — scenes anywhere
- Multiple: separate with comma or use multi-line YAML

**Conditional skills are stored, not loaded:** They're held in a registry until a
matching path is touched, then activated for the rest of the session.

---

## CC6 — Hook event catalog (20+ events, not just PreToolUse/PostToolUse)
The full set as of v2.1.150:

**Tool lifecycle:**
- `PreToolUse` — before a tool runs (can block)
- `PostToolUse` — after a tool runs successfully
- `PostToolUseFailure` — after a tool fails

**Session lifecycle:**
- `SessionStart` — at session boot (perfect for project detection)
- `SessionEnd` — at session close
- `Stop` — when Claude stops responding (user can interrupt)
- `StopFailure` — when Claude errors out

**User interaction:**
- `UserPromptSubmit` — every time the user submits input
- `Notification` — system notifications
- `Elicitation` / `ElicitationResult` — when Claude asks the user something

**Subagents:**
- `SubagentStart` / `SubagentStop` — agent spawn/exit

**Tasks (background work):**
- `TaskCreated` / `TaskCompleted`

**Permissions:**
- `PermissionRequest` / `PermissionDenied`

**Context management:**
- `PreCompact` / `PostCompact` — before/after Claude's context compression

**Worktrees:**
- `WorktreeCreate` / `WorktreeRemove`

**Setup / configuration:**
- `Setup` — first-time setup completion
- `ConfigChange` — settings file changes
- `TeammateIdle` — multi-agent coordination event

**Most useful for vibe-game:** `SessionStart` (project detection),
`PostToolUse` (Edit/Write tracking), `UserPromptSubmit` (intent inference).

---

## CC7 — Four hook types: command, prompt, agent, http
| Type | Best for |
|------|----------|
| **`command`** | Deterministic shell scripts (grep, validation, file checks). Cheap. |
| **`prompt`** | LLM-evaluated checks ("does this commit message follow our style?"). Uses small/fast model. |
| **`agent`** | Agentic verifier ("verify the unit tests actually pass for these changes"). Uses Haiku by default. |
| **`http`** | POST hook input JSON to an external service (CI integration, audit log). |

### prompt hook example

```json
{
  "hooks": {
    "PostToolUse": [
      {
        "matcher": "Edit|Write",
        "hooks": [
          {
            "type": "prompt",
            "prompt": "The user just edited a SKILL.md file. Does the frontmatter include 'description' and at least one citation from the taste library (Vlambeer, Tyroller, GMTK, Swink, or Unity Pattern)? If not, respond with a one-line reminder. Otherwise respond with empty string. Tool input: $ARGUMENTS",
            "timeout": 15,
            "model": "claude-haiku-4-5-20251001"
          }
        ]
      }
    ]
  }
}
```

### agent hook example

```json
{
  "type": "agent",
  "prompt": "Verify that the latest commit's changes don't violate the vibe-brief.md's anti-features.",
  "timeout": 60,
  "model": "claude-haiku-4-5-20251001"
}
```

**Cost discipline:** Use `command` when possible. `prompt`/`agent` should be
reserved for checks that genuinely need LLM judgment.

---

## CC8 — Hook command env variables
Inside a hook command, two variables are guaranteed:

- **`${CLAUDE_PLUGIN_ROOT}`** — absolute path to your plugin's cached copy
  (e.g. `~/.claude/plugins/cache/vibe-games/vibe-game/0.5.0/`)
- **`${CLAUDE_PROJECT_DIR}`** — the user's current working directory (where they ran `claude`)

**Use `CLAUDE_PLUGIN_ROOT` when referencing your plugin's own files** (scripts,
templates, taste docs). Use `CLAUDE_PROJECT_DIR` when checking the user's project.

For skills (the SKILL.md body itself):
- **`${CLAUDE_SKILL_DIR}`** — absolute path to the skill's own dir
- **`${CLAUDE_SESSION_ID}`** — current session ID (useful for state files)

---

## CC9 — Plugin manifest field types (correct format reference)

```json5
{
  "$schema": "https://anthropic.com/claude-code/plugin.schema.json",
  "name": "kebab-case-required",
  "displayName": "Human-Readable Optional",
  "version": "0.5.0",
  "description": "Quoted string, escapable",
  "author": { "name": "Required", "email": "optional", "url": "optional" },
  "license": "SPDX-id-string",
  "homepage": "url",
  "repository": "url",
  "keywords": ["array", "of", "strings"],
  "category": "string",

  // OPTIONAL — manually override component discovery
  // Default: auto-discover from skills/, agents/, commands/, hooks/ etc.
  "skills":  "./skills"            // string OR array of strings — DIRECTORY paths
  "agents":  "./agents/foo.md"     // string OR array — FILE paths to .md (NOT dir)
  "commands": "./commands"         // string OR array OR object mapping
  "hooks":   "./hooks/hooks.json"  // string — FILE path to hooks JSON
}
```

**Critical gotcha:** `agents` field expects FILE paths to `.md` files, not a
directory. If you have `agents/foo.md` + `agents/bar.md`, either leave the field
out (auto-discovery works) or use:

```json
"agents": ["./agents/foo.md", "./agents/bar.md"]
```

If you put `"agents": "./agents"` (directory), validation fails with
*"Invalid input: expected …"* — this is the most common manifest error and
why `/doctor` reports a load failure.

**Vibe-game lesson:** Always trust auto-discovery. Don't manually list these unless
your plugin has unusual layout.

---

## CC10 — Marketplace name reservations
The following marketplace names are reserved by Anthropic and rejected at install:

- `claude-code-marketplace`
- `claude-code-plugins`
- `claude-plugins-official`
- `anthropic-marketplace`
- `anthropic-plugins`
- `agent-skills`
- `anthropic-agent-skills`
- `knowledge-work-plugins`
- `life-sciences`
- `claude-for-legal`
- `claude-for-financial-services`
- `financial-services-plugins`

Plus any name that "impersonates" official marketplaces (e.g.
`official-claude-plugins`, `anthropic-tools-v2`).

**For vibe-game:** `vibe-games` is safe — distinctive, not reserved, not impersonating.

---

## CC11 — Plugin install path (where the marketplace clones to)

When a user runs `/plugin marketplace add owner/repo`, Claude Code:

1. Clones the repo to `~/.claude/plugins/marketplaces/<marketplace-name>/`
2. Reads `.claude-plugin/marketplace.json` from there
3. For each plugin entry, resolves `source` (relative path, github, npm, etc.)
4. Clones/copies the plugin source to
   `~/.claude/plugins/cache/<marketplace-name>/<plugin-name>/<version>/`
5. **Version dir is keyed** by the manifest's `version` field

This means: changing source code without bumping version → cache stays stale.
This is the root cause of the "I edited but it doesn't update" problem.

---

## How vibe-game skills use this file

`/diagnose` reads from this file to check:
- Counter mismatch (CC1)
- Cache freshness (CC2)
- Frontmatter sanity (CC3, CC4)
- Manifest field types (CC9)
- Marketplace name conflicts (CC10)

`/juice` uses CC5 (`paths` frontmatter) to gate itself on script edits.

Hooks use CC6-CC8 to pick the right event + type for the workflow they enforce.

## See also

- [[unity-patterns]] — Unity API patterns (the engine layer)
- [[vlambeer-juice]], [[game-feel-swink]], [[tyroller-mistakes]], [[gmtk-patterns]] — design taste layers
- [Official plugin docs](https://code.claude.com/docs/en/plugin-marketplaces)
- [hesreallyhim/claude-code-json-schema](https://github.com/hesreallyhim/claude-code-json-schema) — community schema spec
