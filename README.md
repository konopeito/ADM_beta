# Analog Dream Machine

**Developer:** Julia Fritsch (konopeito)
**Project Type:** Experimental Narrative Game
**Engine:** Unity 2D (C#)
**Perspective:** First-Person Interface-Only

---

## Overview

Analog Dream Machine is a story-driven, UI-heavy first-person narrative game experienced entirely through a simulated CRT virtual machine. The player interacts with **Eidolon-9**, an experimental analog consciousness archive designed to preserve human memory. As the system destabilizes, memory becomes fragmented, poetic, and alive.

---

## Core Themes

- Memory as identity
- Analog vs digital permanence
- Consciousness preservation through imperfection
- System decay as mortality
- Human-machine emotional fusion

---

## Core Mechanics

- UI-based terminal navigation
- Memory fragment discovery (ScriptableObject-driven)
- System scans (quick scan, deep_scan, audio trace)
- Repair vs decay mechanics (memory decay percentage)
- Intentional glitch storytelling (text corruption, screen flicker, whisper fragments)
- Hidden commands & lore puzzles (deep_scan, remember, ellion, who_am_i)
- Narrative state progression (Eidolon evolves from Mechanical → Suspicious → Recognizing → Attached)

---

## MVP Development

### What the MVP Includes

The Analog Dream Machine MVP delivers the foundational experience of interacting with Eidolon-9 through a fully functional CRT-style terminal interface built in Unity. The prototype includes a cinematic boot sequence featuring an animated Ellion Systems Laboratories logo with video playback, CRT power-on flicker effects, and a BIOS-style system check that transitions into the main terminal scene. The core terminal system features a modular command parser supporting both visible commands (help, scan, logs, open, status, clear) and hidden commands (deep_scan, who_am_i, remember, ellion) that reward player curiosity. Memory fragments are built as ScriptableObjects with corruption levels and narrative flags, enabling dynamic text corruption through the glitch system. Eidolon-9's personality evolves across four narrative states — Mechanical, Suspicious, Recognizing, and Attached — tracked by the NarrativeStateManager singleton, shifting its responses from cold system outputs to emotionally vulnerable dialogue. The GlitchSystem handles text corruption using Unicode block characters, screen flicker via CanvasGroup alpha manipulation, subtle screen jitter, and whisper fragments that surface from Eidolon's subconscious. A dedicated TerminalAudioManager provides layered sound design with three separate AudioSources for SFX, ambient loops, and typing feedback, covering keypress variations, command feedback, scanning sweeps, log access sounds, glitch bursts, and ambient electrical hum that intensifies as system decay increases.

### What I Learned

Building this MVP reinforced the importance of clean system separation — the CommandParser, NarrativeStateManager, GlitchSystem, and TerminalAudioManager each operate independently but compose into emergent storytelling. ScriptableObjects for memory fragments made content authoring significantly faster than hardcoding narrative. The biggest creative insight was that glitches-as-narrative — text corruption scaling with decay, whisper fragments surfacing unpredictably, audio distortion intensifying — create atmosphere more effectively than scripted cutscenes. Eidolon-9's personality emerges from how the system *breaks*, not from traditional dialogue trees. On the technical side, managing multiple coroutines for typing effects, delayed responses, and screen glitches required careful sequencing to avoid visual conflicts. The boot sequence taught me how scene management, video playback via RenderTextures, and audio layering work together to create a cinematic first impression within Unity's UI system.

### Next Steps

- Implement full Sub Layer Δ content with dreamlike memory landscapes described through terminal poetry
- Add Dr. Ellion's complete log series (sessions 1–9) as ScriptableObject fragments
- Build the memory repair mechanic allowing players to stabilize fragments and prevent permanent loss
- Design the audio trace system to reconstruct voice imprints from fragments
- Implement timed decay events where fragments degrade if the player takes too long
- Add CRT post-processing shader effects (scanlines, chromatic aberration, screen curvature)
- Expand ambient audio with tape hiss loops and signal interference layering
- Create the "Archive Beneath" visual style for Sub Layer Δ terminal output

---

## Feedback Collection

### Sources


**Roommates and Friends (Non-Developers):**
Non-developer playtesters offered valuable perspective on the user experience and emotional impact. They tested without guidance to evaluate how intuitive the terminal felt for someone unfamiliar with command-line interfaces. Feedback focused on atmosphere, pacing, whether and if the CRT aesthetic was immersive or disorienting.

**AI Tools (ChatGPT / GitHub Copilot):**
Used AI tools throughout development for technical architecture review, system design validation, code debugging, and narrative consistency checks. AI feedback helped identify separation-of-concerns issues in early script drafts and suggested the ScriptableObject approach for memory fragments. Also used for unbiased evaluation of the GDD structure and mechanic feasibility.



### Key Suggestions and Common Themes

| Theme | Feedback Summary |
|-------|-----------------|
| **Command Discoverability** | Players wanted more organic hints toward hidden commands rather than relying on trial and error. The "deeper commands" hint after reaching Suspicious state was effective but could appear sooner. |
| **Pacing** | The boot sequence was praised for atmosphere. Some testers felt the terminal could benefit from more unprompted Eidolon responses to break up long stretches of player-initiated interaction. |
| **Visual Atmosphere** | CRT flicker and green-on-black aesthetic was consistently praised. Testers requested more visual variety when entering Sub Layer Δ to differentiate it from standard terminal interaction. |
| **Audio** | Sound design was noted as a strong atmospheric layer. Ambient hum and typing sounds added immersion. Suggestion to add more audio variation during high-decay states. |
| **Narrative Clarity** | The Eidolon state progression was subtle enough that some players missed the shift from Mechanical to Suspicious. Suggestion to make state transitions more pronounced through visual or audio cues. |
| **Emotional Impact** | The "remember" command and Ellion trigger were highlighted as the most emotionally resonant moments. Players wanted more of these intimate Eidolon interactions. |

---

## Feedback Implementation

### Analysis and Prioritization

Feedback was analyzed based on two criteria: **impact on player experience** and **feasibility within current architecture**.

| Priority | Feedback | Implementation Plan | Status |
|----------|----------|-------------------|--------|
| 🔴 High | State transitions too subtle | Add screen glitch + audio cue + brief Eidolon message when state changes. Modify `NarrativeStateManager.OnFragmentAccessed()` to trigger `GlitchSystem.ScreenGlitch()` and `TerminalAudioManager.PlayStateShift()` on state change. | Planned |
| 🔴 High | Hidden command discoverability | Add more contextual hints in scan results and log content. Earlier hint delivery (after 1 fragment instead of 2). Add faint glitch-text hints that appear randomly during idle moments. | Planned |
| 🟡 Medium | More unprompted Eidolon responses | Implement idle timer — if player doesn't type for 30+ seconds, Eidolon whispers or comments. Add ambient narrative events tied to decay thresholds. | Planned |
| 🟡 Medium | Visual differentiation for Sub Layer Δ | Change terminal text color when in Delta layer (e.g., amber #FF9900 or deeper green). Add unique glitch patterns and border styling for Delta content. | Planned |
| 🟡 Medium | More audio variation at high decay | Add additional ambient layers (tape warble, signal interference) that activate above 60% decay. Increase glitch burst frequency. | Planned |
| 🟢 Low | More Eidolon emotional interactions | Expand hidden command set with additional intimate responses. Add fragment-specific Eidolon reactions beyond the recognition trigger. | Future |

### Implementation Notes

Changes are being prioritized in this order: state transition visibility (affects core narrative comprehension), command discoverability (affects player progression), then atmosphere enhancements (polish layer). All planned implementations work within the existing system architecture — NarrativeStateManager already tracks state changes, GlitchSystem and TerminalAudioManager already support the needed effects, and the CommandParser supports dynamic command registration for contextual hints.

---

## Project Structure

```
Assets/
├── Scripts/
│   ├── TerminalController.cs
│   ├── CommandParser.cs
│   ├── NarrativeStateManager.cs
│   ├── GlitchSystem.cs
│   ├── CRTFlicker.cs
│   ├── TerminalAudioManager.cs
│   └── BootSequence.cs
├── ScriptableObjects/
│   └── MemoryFragment.cs
├── Scenes/
│   ├── Boot_Sequence.unity
│   └── Terminal_Main.unity
├── Fonts/
├── Textures/
├── Video/
└── Audio/
    ├── Typing/
    ├── Commands/
    ├── Scanning/
    ├── Memory/
    ├── Eidolon/
    ├── Glitch/
    └── Ambient/
Docs/
├── GDD_ADM.md
```

---

## License

MIT