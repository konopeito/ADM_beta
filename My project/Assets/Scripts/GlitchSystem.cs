using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text;

public class GlitchSystem : MonoBehaviour
{
    public static GlitchSystem Instance { get; private set; }

    [Header("References")]
    public TextMeshProUGUI terminalOutput;
    public CanvasGroup screenCanvasGroup;

    [Header("Glitch Characters")]
    private static readonly string glitchChars = "Δ░▒▓█▄▀■□◊╬╫╪┼┤├┴┬│─≡≈∞∅∇";

    private static readonly string[] eidolonWhispers = new string[]
    {
        "...she was here...",
        "...signal lost...",
        "...I remember the warmth...",
        "...don't look away now...",
        "...tape is running...",
        "...who are you?...",
        "...the reels still spin...",
        "...Ellion...",
    };

    void Awake()
    {
        if (Instance == null) Instance = this;
    }

    /// <summary>
    /// Corrupts text based on corruption level (0-100).
    /// Higher = more characters replaced with glitch symbols.
    /// </summary>
    public static string CorruptText(string original, int corruptionLevel)
    {
        if (corruptionLevel <= 0) return original;

        StringBuilder corrupted = new StringBuilder(original);
        float corruptionChance = corruptionLevel / 100f;

        for (int i = 0; i < corrupted.Length; i++)
        {
            if (corrupted[i] != ' ' && corrupted[i] != '\n' && Random.value < corruptionChance * 0.4f)
            {
                corrupted[i] = glitchChars[Random.Range(0, glitchChars.Length)];
            }
        }

        return corrupted.ToString();
    }

    /// <summary>
    /// Inserts a whispered Eidolon fragment into the terminal output.
    /// Called during deep scans or high-decay moments.
    /// </summary>
    public IEnumerator WhisperGlitch()
    {
        string whisper = eidolonWhispers[Random.Range(0, eidolonWhispers.Length)];
        string original = terminalOutput.text;

        terminalOutput.text += "\n<color=#1a5c2a>" + whisper + "</color>\n";

        yield return new WaitForSeconds(Random.Range(1.5f, 3f));

        // Whisper fades — optional: remove it or leave it
        // terminalOutput.text = original;
    }

    /// <summary>
    /// Brief screen disruption — flicker + jitter.
    /// </summary>
    public IEnumerator ScreenGlitch(float duration = 0.3f)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            screenCanvasGroup.alpha = Random.Range(0.4f, 1f);
            yield return new WaitForSeconds(0.05f);
            elapsed += 0.05f;
        }
        screenCanvasGroup.alpha = 1f;
    }
}