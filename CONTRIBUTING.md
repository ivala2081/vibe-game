# Contributing to Vibe Game for Unity

Thanks for considering a contribution. This is a young project with strong opinions —
the bar for additions is "does it reinforce the moat?"

The **moat** is **cited design taste**. Every skill must trace back to a named source.
Generic suggestions are out of scope.

---

## What we want

### High value (please PR)

- **New taste sources** in `taste/` — designers with shipped, beloved games.
  Examples we don't have yet: Derek Yu (Spelunky-specific patterns), Tom Francis,
  Daniel Cook (Lost Garden), Ben Esposito (Donut County). Cite specific talks/posts.
- **New demos** in `examples/` — different mood, different genre, different scope.
  Especially **cozy**, **puzzle**, **rhythm**, **racing** demos. The mood-aware
  juice claim needs broader sample size.
- **Bug fixes** in existing SKILL.md procedures or demo code.
- **Real dogfood reports** — *"I built X with vibe-game and here's what went right/wrong."*
  These become the next round of spec rules.
- **Tests / linting** — JSON schema validation for plugin.json/marketplace.json,
  SKILL.md frontmatter linting.

### Low value (please don't PR)

- Generic "best practices" without a named source. We have a strong "cite or skip" rule.
- "AI-generated improvements" to existing taste docs. They're curated.
- Engine ports (Godot, Unreal, etc.) — Unity-only is a deliberate scope decision.
  Fork it if you want a Godot version, and we'll cross-link.
- Adding more skills "just because" — every skill should solve a real workflow gap.

---

## Workflow

1. **Fork** this repo.
2. **Branch** off `main`. Name: `feature/short-description` or `fix/short-description`.
3. **Develop locally**. Test your changes with the actual Claude Code plugin loader:
   ```bash
   # From your fork's root
   /plugin marketplace add ./
   /plugin install vibe-game@vibe-games
   /reload-plugins
   ```
4. **Bump `version`** in `.claude-plugin/plugin.json` and `marketplace.json`
   if you touched skill/agent/hook content. Patch bumps (`0.2.4`) are fine for fixes;
   minor (`0.3.0`) for new skills/agents; major (`1.0.0`) for breaking changes.
5. **Update `CHANGELOG.md`** with a `[Unreleased]` entry that describes the change.
6. **PR** against `main`. Include in the description:
   - What the change does
   - Why it reinforces (not dilutes) the moat
   - How you tested it (Claude Code version, Unity version if relevant)

---

## Taste-library citations

If you add a taste source, the file must:

1. Live in `taste/` (e.g., `taste/yu-spelunky.md`)
2. Have a `> Source:` line at the top with the original author + medium (book, talk, devlog)
3. Use the `[ID]` format for citations within the file (e.g., `Y1 — Yu's risk-reward`)
4. Be added to `taste/INDEX.md` with a row in the table
5. Be referenced by **at least one** existing skill (otherwise it's orphaned)

Citations matter. This isn't a wiki; it's a curated body of working knowledge.

---

## SKILL.md format

Every skill must:

1. Live at `skills/<kebab-case-name>/SKILL.md`
2. Have YAML frontmatter:
   ```yaml
   ---
   name: <skill-name>
   description: "What this skill does, in 1-3 sentences. Quoted string."
   disable-model-invocation: false
   ---
   ```
3. Start with a `# heading` matching the skill name
4. Include sections in order: *When to invoke*, *Hard rules*, *Procedure*,
   *Anti-patterns*, *Cross-references*
5. Cite at least one taste source via `[[file-name#anchor]]` link

---

## Demo additions

A new demo in `examples/demo-N-<name>/` should include:

1. `README.md` — install + run instructions
2. `vibe-brief.md` — the brief that drove it
3. `Assets/_Project/Scripts/` — drop-in Unity scripts
4. `JUICE-LOG.md` — what `/juice` applied and why (with citations)
5. `FEEL-CHECK.md` — Swink 6 + Fun-O-Meter scoring
6. `BUGS-FIXED.md` — bugs surfaced during dogfood, lessons promoted to spec

The demo must actually run in Unity. No mockup PRs.

---

## Code of conduct

Be honest, direct, and specific. No hedging. No flattery. This is a project
that values *taste* and *clarity*. Communicate the same way.

Personal attacks, harassment, or hostility get you removed without warning.

---

## License

By contributing, you agree your changes are released under MIT (matching the project license).

If you cite a taste source, you must respect their copyright — short summaries
of public talks/books/posts (fair use) only. **No verbatim chapter reproductions.**

---

## Questions?

Open an issue with the `question` label. Or DM @ivala2081 on GitHub.
