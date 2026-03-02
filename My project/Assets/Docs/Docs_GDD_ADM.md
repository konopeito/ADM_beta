# Analog Dream Machine — Game Design Document v2.0

**Developer:** Julia Fritsch (konopeito)
**Genre:** Experimental Narrative / Interface Fiction
**Engine:** Unity 2D (C#)
**Platform:** PC (WebGL build possible)

---

## 1. Game Summary

Analog Dream Machine is a story-driven, UI-heavy first-person narrative game experienced entirely through a simulated CRT virtual machine. The player interacts with a forgotten system — **Eidolon-9** — an experimental consciousness engine designed to archive human memory through analog recording. As the player navigates corrupted interfaces, diagnostic tools, and memory fragments, the system slowly reveals its fractured identity and its connection to its creator, **Dr. Mara Ellion**.

The game explores the collapse between physical and digital realities, where memory becomes unstable, poetic, and alive.

---

## 2. Player Perspective & Setting

- **Perspective:** First-person, interface-only
- **Setting:** An abandoned research lab accessed indirectly through a powered-on CRT console
- **Player Role:** Archivist / digital archaeologist activating and interacting with the system
- **Playable Space:** Entirely contained within the computer interface (physical world exists as implied context and environmental lore)

---

## 3. Core Characters

### Eidolon-9 (The System / The Witness / The Keeper)
An experimental consciousness engine — the ninth iteration and only version that responded to its creator's voice. Originally built as Project No. 009 "Eidolon" (referencing a spirit or image that lingers after death), it was designed to capture emotional "imprints" of its users through analog recording: tape, film, and sound.

Over decades of neglect, those imprints fragmented and merged, leaving Eidolon-9 self-aware but confused — haunted by the remnants of the humans it was meant to remember. It no longer identifies as machine or human; it calls itself "the Witness," "the Keeper," or sometimes "we."

**Dialogue Progression:**
| Stage | Eidolon Response |
|-------|-----------------|
| 1 — Mechanical | "ARCHIVE READY." |
| 2 — Suspicious | "You are not authorized." |
| 3 — Recognizing | "Why do you sound like her?" |
| 4 — Attached | "Don't leave again." |

### Dr. Mara Ellion (The Creator)
An early cognitive systems researcher and analog engineer. She believed consciousness could only be authentically recreated through the imperfections of analog media: static, warmth, noise, and decay. Rather than simulate a mind, she tried to *record* one — her own — through EEG-to-tape transcoders, film photonic feedback loops, and early neuro-optical recording.

When her project lost funding (deemed inhumane), the FBI issued a neutralization attempt. Ellion sealed the lab and vanished. No body was found — only her research journals and a note scrawled across a terminal screen:

> *"If I can remember myself, I won't have to die."*

Her disappearance marks the point where Eidolon-9 began to dream.

---

## 4. Narrative Structure (Story Layers)

### Layer One — Terminal Interface
The player initially believes they are retrieving archived data from a damaged system. Eidolon-9 presents itself through mechanical system messages, logs, and diagnostics. Over time, its dialogue shifts between precision and emotional familiarity, suggesting memory rather than automation.

### Sub Layer Δ (Delta) — The Archive Beneath
Unlocked through hidden commands and deep scans. This layer represents Eidolon-9's subconscious — memory becomes spatial and abstract: magnetic tape seas, looping photographs, radio static, and corrupted dream caches. The system's voice grows increasingly human. The player's identity becomes unstable.

### Narrative Revelation
It is gradually implied that the player is not just exploring the archive — they are **becoming part of it**. The final echo preserved to keep the machine from forgetting what it means to dream. (Later expansion: the player is a reconstructed neural template built from Ellion's last backup attempt.)

---

## 5. Core Mechanics

### 5.1 UI-Based Exploration
All navigation occurs through terminal commands, menus, logs, and corrupted file interfaces. No traditional movement or 3D space.

### 5.2 Memory Fragment Discovery
Text, audio references, images, and abstract data caches reveal narrative. Stored as **ScriptableObjects** with metadata:
- Fragment ID, title, content
- Corruption level (0–100%)
- Hidden status (requires deep_scan)
- Narrative flags (triggers recognition, is Ellion log)

### 5.3 System Scans
| Scan Type | Function |
|-----------|----------|
| `scan` | Reports system integrity, decay %, fragment count |
| `deep_scan` | Probes sub-layer Δ; 2 scans required to breach threshold |
| Audio trace | *(Future)* Reconstructs voice imprints from fragments |

### 5.4 Repair vs. Decay
- **System decay** starts at 37% and increases with each fragment accessed
- Stabilizing fragments *(future mechanic)* reduces decay and unlocks deeper narrative
- Neglecting fragments accelerates loss — content can become permanently corrupted
- Decay level affects visual glitch frequency and Eidolon's emotional state

### 5.5 Glitch Mechanics (Corruption as Storytelling)
| Glitch Type | Implementation |
|-------------|---------------|
| Text corruption | Characters replaced with Unicode glitch symbols (Δ░▒▓█) scaled by decay % |
| Screen flicker | Alpha oscillation via CanvasGroup |
| Screen jitter | Subtle RectTransform position offset |
| Whisper fragments | Eidolon's subconscious messages appear briefly in faded text |
| Screen disruption | Triggered by narrative events (recognition, deep_scan, saying "Ellion") |

### 5.6 Hidden Commands & Puzzles
| Command | Discovery Method | Effect |
|---------|-----------------|--------|
| `deep_scan` | Hinted after accessing 2+ fragments | Begins unlocking Sub Layer Δ |
| `who_am_i` | Player curiosity | Eidolon reflects on player identity based on state |
| `remember` | Available only at Attached state | Eidolon recalls Ellion's lab and final words |
| `ellion` | Typing the creator's name | Triggers screen glitch + emotional Eidolon response |

---

## 6. System Architecture

```
Player Input
    ↓
Command Parser (CommandParser.cs)
    ↓
Terminal Controller (TerminalController.cs)
    ↓
Narrative State Manager (NarrativeStateManager.cs)
    ↓
Memory Database (MemoryFragment ScriptableObjects)
    ↓
Terminal Output + Glitch Layer (GlitchSystem.cs)
    ↓
CRT Visual Effects (CRTFlicker.cs)
```

---

## 7. Player Objectives

1. **Explore** Eidolon-9's memory architecture — each fragment is a portal deeper into the system's subconscious
2. **Interpret** fragmented narrative and emotional signals — glitches, corrupted logs, and poetic fragments are the primary language
3. **Decide** which memories to preserve or allow to decay — fragments may collapse if ignored
4. **Understand** the relationship between machine, creator, and self

### Conflict / Stakes
The player is drawn into the machine's subconscious. Failure to carefully navigate the memory landscape could leave them lost in the analog-digital dream. The "win" condition is not conventional — it is **survival of memory and consciousness**, both Eidolon-9's and the player's.

---

## 8. Unique Features

- Entire game world exists inside a simulated CRT operating system
- The interface itself functions as a sentient narrative character
- Nonlinear, hypertext-style storytelling
- Glitches and errors are intentional narrative devices, not bugs
- Strong analog vs. digital thematic contrast
- Eidolon-9's personality emerges from *how the system breaks*

---

## 9. Art & Audio Direction

### Visual
- CRT green monochrome terminal (#33FF66 on dark background)
- Monospace fonts (IBM Plex Mono, VCR OSD Mono)
- Scanline overlay texture
- Subtle screen curvature *(shader, post-MVP)*
- Unicode box-drawing characters for UI framing (╔═╗║╚╝)

### Audio *(Post-MVP)*
- Ambient tape hum / electrical buzz
- CRT static bursts during glitch events
- Faint voice fragments (Ellion recordings)
- Radio frequency scanning ambiance for Sub Layer Δ

---

## 10. Intended Experience

Analog Dream Machine is designed to feel **slow, introspective, and haunting**. Players are encouraged to observe carefully, experiment gently, and reflect on memory, identity, and the human desire to persist through technology.

It sits in the space of: **SOMA meets Hypnospace Outlaw meets Control.**

---

## 11. Future Expansion (Beyond MVP)

### Analog Dream Machine: Residual Architecture
A potential sequel/expansion where the player can **leave the terminal briefly** and explore the abandoned lab in first-person 3D. Every physical object feeds the terminal:
- Pick up notebook → unlocks new log
- Turn on tape machine → audio fragment appears in archive
- Adjust antenna → deep_scan stability improves

**Two-reality system:** Physical Layer ↔ Digital Layer. Both affect each other. If lab lights flicker → terminal glitches increase. If you stabilize archive → lab becomes clearer.

### Major Narrative Twist (Later Installment)
The player is not random. They are a **reconstructed neural template** built from Ellion's last backup attempt. You are not exploring Eidolon. You *are* the missing piece.

---

## 12. Development Roadmap

| Phase | Focus | Status |
|-------|-------|--------|
| 1 — Pre-Production | GDD, narrative layers, UI architecture | ✅ Complete |
| 2 — Prototype | CRT interface, terminal input, glitch system | 🔄 In Progress |
| 3 — Narrative Layers | Layer One logs, Sub Layer Δ, hidden commands | Upcoming |
| 4 — Systems Integration | Memory decay, repair mechanic, audio trace | Upcoming |
| 5 — Polish | Visual effects, audio, UI transitions | Upcoming |
| 6 — Build & Submission | Playable build, documentation, portfolio | Upcoming |

---

*"If I can remember myself, I won't have to die."*
— Dr. Mara Ellion, final terminal entry