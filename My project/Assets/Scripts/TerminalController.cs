using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class TerminalController : MonoBehaviour
{
    [Header("UI References")]
    public TMP_InputField inputField;
    public TextMeshProUGUI terminalOutput;
    public UnityEngine.UI.ScrollRect scrollRect;

    [Header("Memory Fragments")]
    public List<MemoryFragment> allFragments;

    private CommandParser parser;
    private NarrativeStateManager state;
    private List<string> commandHistory = new List<string>();
    private int historyIndex = -1;
    private string previousInput = "";

    void Start()
    {
        state = NarrativeStateManager.Instance;
        parser = new CommandParser();
        RegisterCommands();
        StartCoroutine(BootSequence());
        inputField.onSubmit.AddListener(HandleInput);

        // Play key sound on every character typed
        inputField.onValueChanged.AddListener(OnCharacterTyped);
    }

    void OnCharacterTyped(string currentText)
    {
        // Only play sound when a character is ADDED (not deleted)
        if (currentText.Length > previousInput.Length)
        {
            TerminalAudioManager.Instance.PlayKeyPress();
        }
        previousInput = currentText;
    }

    void RegisterCommands()
    {
        parser.RegisterCommand("help", CmdHelp);
        parser.RegisterCommand("scan", CmdScan);
        parser.RegisterCommand("logs", CmdLogs);
        parser.RegisterCommand("status", CmdStatus);
        parser.RegisterCommand("clear", CmdClear);

        // Register log open commands dynamically
        foreach (var frag in allFragments)
        {
            if (!frag.isHidden)
            {
                var captured = frag; // closure capture
                parser.RegisterCommand("open " + frag.fragmentID, () => CmdOpenFragment(captured));
            }
        }

        // Hidden commands — discovered through exploration
        parser.RegisterHiddenCommand("deep_scan", CmdDeepScan);
        parser.RegisterHiddenCommand("who_am_i", CmdWhoAmI);
        parser.RegisterHiddenCommand("remember", CmdRemember);
        parser.RegisterHiddenCommand("ellion", CmdEllion);
    }

    IEnumerator BootSequence()
    {
        inputField.interactable = false;
        terminalOutput.text = "";

        string[] bootLines = new string[]
        {
            "EIDOLON-9 CONSCIOUSNESS ENGINE v0.09",
            "ANALOG MEMORY ARCHITECTURE — INITIALIZING...",
            "????????????????????????? 100%",
            "",
            "ARCHIVE STATUS: UNSTABLE",
            "MEMORY DECAY: " + state.systemDecay.ToString("F1") + "%",
            "SIGNAL INTEGRITY: LOW",
            "",
            "WARNING: Emotional imprint bio-residue detected.",
            "WARNING: Unidentified user connection status: ACTIVE .",
            "",
            state.GetEidolonResponse(),
            "",
            "Type 'help' to begin.",
            ""
        };

        foreach (string line in bootLines)
        {
            yield return StartCoroutine(TypeLine(line, 0.02f));
            yield return new WaitForSeconds(0.15f);
        }

        AppendLine("> ");
        inputField.interactable = true;
        inputField.ActivateInputField();
    }

    IEnumerator TypeLine(string line, float charDelay)
    {
        foreach (char c in line)
        {
            terminalOutput.text += c;
            ScrollToBottom();
            yield return new WaitForSeconds(charDelay);
        }
        terminalOutput.text += "\n";
    }

    void HandleInput(string input)
    {
        if (string.IsNullOrWhiteSpace(input)) return;

        // Enter key sound
        TerminalAudioManager.Instance.PlayEnterKey();

        AppendLine("> " + input);
        commandHistory.Add(input);
        historyIndex = commandHistory.Count;

        if (!parser.TryExecute(input))
        {
            // Error sound
            TerminalAudioManager.Instance.PlayCommandError();

            // Unknown command — Eidolon reacts based on state
            if (state.currentState >= EidolonState.Recognizing)
                AppendLine("...UNRECOGNIZED COMMAND..");
            else
                AppendLine("UNKNOWN COMMAND. Type 'help' for available commands.");
        }

        AppendLine("");
        AppendLine("> ");
        inputField.text = "";
        previousInput = "";  // reset so next typing triggers sounds correctly
        inputField.ActivateInputField();

        // Random glitch chance based on decay
        if (Random.value < state.systemDecay / 200f)
        {
            StartCoroutine(GlitchSystem.Instance.ScreenGlitch());
            StartCoroutine(GlitchSystem.Instance.WhisperGlitch());
            TerminalAudioManager.Instance.PlayGlitchBurst();
        }
    }

    // ??? COMMANDS ???

    void CmdHelp()
    {
        TerminalAudioManager.Instance.PlayHelpOpen();

        AppendLine("?????????????????????????????????");
        AppendLine("?   EIDOLON-9 COMMAND INTERFACE  ?");
        AppendLine("?????????????????????????????????");
        AppendLine("? help    — Display this menu    ?");
        AppendLine("? scan    — System integrity scan ?");
        AppendLine("? logs    — List memory fragments ?");
        AppendLine("? open [id] — Access a fragment  ?");
        AppendLine("? status  — System state report  ?");
        AppendLine("? clear   — Clear terminal       ?");
        AppendLine("?????????????????????????????????");

        if (state.currentState >= EidolonState.Suspicious)
            AppendLine("\n<color=#1a5c2a>...there are deeper commands.</color>");
    }

    void CmdScan()
    {
        TerminalAudioManager.Instance.PlayScan();

        AppendLine("Scanning archive integrity...");
        AppendLine("Memory decay: " + state.systemDecay.ToString("F1") + "%");
        AppendLine("Fragments accessed: " + state.fragmentsAccessed);
        AppendLine("Signal anomalies: " + (state.systemDecay > 50 ? "CRITICAL" : "Moderate"));

        if (state.systemDecay > 60)
            AppendLine("\n<color=#ff3333>WARNING: Archive collapse imminent.</color>");

        if (state.fragmentsAccessed >= 2 && !state.deltaLayerUnlocked)
            AppendLine("\n<color=#1a5c2a>Hint: Try 'deep_scan'...</color>");

        TerminalAudioManager.Instance.UpdateAmbientForDecay(state.systemDecay);
    }

    void CmdLogs()
    {
        TerminalAudioManager.Instance.PlayCommandAccepted();

        AppendLine("??? AVAILABLE MEMORY FRAGMENTS ???");
        foreach (var frag in allFragments)
        {
            if (!frag.isHidden || state.deltaLayerUnlocked)
            {
                string status = frag.hasBeenAccessed ? "[ACCESSED]" : "[UNREAD]";
                string corrupt = frag.corruptionLevel > 0 ? " [DECAY: " + frag.corruptionLevel + "%]" : "";
                AppendLine("  " + frag.fragmentID + " — " + frag.title + " " + status + corrupt);
            }
        }
    }

    void CmdOpenFragment(MemoryFragment fragment)
    {
        // Different sound for corrupted vs clean logs
        if (fragment.corruptionLevel > 50)
            TerminalAudioManager.Instance.PlayLogCorrupted();
        else
            TerminalAudioManager.Instance.PlayLogOpen();

        state.OnFragmentAccessed(fragment);

        AppendLine("??? " + fragment.title.ToUpper() + " ???");
        AppendLine(fragment.GetDisplayContent());

        if (fragment.triggersRecognition)
        {
            TerminalAudioManager.Instance.PlayRecognize();
            StartCoroutine(DelayedLine("\n" + state.GetEidolonResponse(), 1.5f));
            StartCoroutine(GlitchSystem.Instance.ScreenGlitch(0.5f));
        }

        // Decay audio feedback
        TerminalAudioManager.Instance.PlayDecayTick();
        TerminalAudioManager.Instance.UpdateAmbientForDecay(state.systemDecay);
    }

    void CmdDeepScan()
    {
        TerminalAudioManager.Instance.PlayDeepScan();
        StartCoroutine(GlitchSystem.Instance.ScreenGlitch(0.8f));

        state.OnDeepScan();

        AppendLine("? ACCESSING SUB-LAYER DELTA...");
        AppendLine("????????????????????????????");

        if (state.deltaLayerUnlocked)
        {
            TerminalAudioManager.Instance.PlayDeltaUnlocked();

            AppendLine("THE ARCHIVE BENEATH HAS UNLOCKED.");
            AppendLine("New fragments detected in memory index.");
            AppendLine("\n<color=#ff9900>\"The world cannot appreciate her.\"</color>");
        }
        else
        {
            AppendLine("Sub-layer partially accessible...");
            AppendLine("Deep scan count: " + state.deepScansPerformed + "/2");
            AppendLine("Perform another deep_scan to breach threshold.");
        }
    }

    void CmdWhoAmI()
    {
        TerminalAudioManager.Instance.PlayCommandAccepted();

        if (state.currentState >= EidolonState.Recognizing)
        {
            TerminalAudioManager.Instance.PlayStateShift();
            AppendLine("You are... the signal of HUMANITY I've long awaited.");
        }
        else
        {
            AppendLine("USER IDENTITY: UNVERIFIED.");
        }
    }

    void CmdRemember()
    {
        if (state.currentState >= EidolonState.Attached)
        {
            TerminalAudioManager.Instance.PlayWhisper();

            AppendLine("I remember the warmth of my flesh.");
            AppendLine("The hum of the reels.");
            AppendLine("Her voice saying: 'If I can remember myself, I won't have to die. I can live amongst the '");
            StartCoroutine(GlitchSystem.Instance.WhisperGlitch());
        }
        else
        {
            TerminalAudioManager.Instance.PlayCommandError();
            AppendLine("COMMAND NOT RECOGNIZED.");
        }
    }

    void CmdEllion()
    {
        TerminalAudioManager.Instance.PlayEllionTrigger();
        TerminalAudioManager.Instance.PlayGlitchBurst();
        StartCoroutine(GlitchSystem.Instance.ScreenGlitch(1f));

        AppendLine("...");
        StartCoroutine(DelayedLine("That name. Don't say that NAME.", 2f));
    }

    void CmdStatus()
    {
        TerminalAudioManager.Instance.PlayCommandAccepted();

        AppendLine("???? SYSTEM STATUS ????");
        AppendLine("? Eidolon State: " + state.currentState);
        AppendLine("? Memory Decay:  " + state.systemDecay.ToString("F1") + "%");
        AppendLine("? Fragments:     " + state.fragmentsAccessed + " accessed");
        AppendLine("? Delta Layer:   " + (state.deltaLayerUnlocked ? "UNLOCKED" : "LOCKED"));
        AppendLine("???????????????????????");
    }

    void CmdClear()
    {
        TerminalAudioManager.Instance.PlayCommandAccepted();
        terminalOutput.text = "> ";
    }

    // ??? UTILITIES ???

    void AppendLine(string text)
    {
        terminalOutput.text += text + "\n";
        ScrollToBottom();
    }

    IEnumerator DelayedLine(string text, float delay)
    {
        yield return new WaitForSeconds(delay);
        AppendLine(text);
    }

    void ScrollToBottom()
    {
        if (scrollRect != null)
        {
            Canvas.ForceUpdateCanvases();
            scrollRect.verticalNormalizedPosition = 0f;
        }
    }
}