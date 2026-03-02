using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class BootSequence : MonoBehaviour
{
    [Header("References")]
    public CanvasGroup blackOverlay;
    public CanvasGroup logoGroup;
    public CanvasGroup bootTextGroup;
    public TextMeshProUGUI bootText;
    public CanvasGroup screenCanvasGroup;

    [Header("Video Logo")]
    public VideoPlayer logoVideoPlayer;
    public RawImage logoRawImage;
    public RenderTexture logoRenderTexture;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip powerOnStatic;
    public AudioClip electricalHum;
    public AudioClip bootBeep;
    public AudioClip tapeSpinUp;
    public AudioClip glitchBurst;

    [Header("Settings")]
    public float charTypeSpeed = 0.015f;
    public float linePause = 0.12f;
    public float logoHoldTime = 0f; // 0 = wait for video to finish

    void Start()
    {
        // Clear the render texture so there's no garbage frame
        ClearRenderTexture();
        StartCoroutine(RunBootSequence());
    }

    void ClearRenderTexture()
    {
        RenderTexture currentRT = RenderTexture.active;
        RenderTexture.active = logoRenderTexture;
        GL.Clear(true, true, Color.black);
        RenderTexture.active = currentRT;
    }

    IEnumerator RunBootSequence()
    {
        // ─── PHASE 0: SILENCE & DARKNESS ───
        blackOverlay.alpha = 1f;
        logoGroup.alpha = 0f;
        bootTextGroup.alpha = 0f;
        bootText.text = "";
        logoRawImage.color = new Color(1f, 1f, 1f, 0f);

        yield return new WaitForSeconds(1f);

        // ─── PHASE 1: CRT POWER ON ───
        if (powerOnStatic != null)
            audioSource.PlayOneShot(powerOnStatic);

        for (int i = 0; i < 6; i++)
        {
            blackOverlay.alpha = Random.Range(0.3f, 1f);
            yield return new WaitForSeconds(Random.Range(0.05f, 0.15f));
        }

        float fadeTime = 0.5f;
        float elapsed = 0f;
        while (elapsed < fadeTime)
        {
            elapsed += Time.deltaTime;
            blackOverlay.alpha = Mathf.Lerp(1f, 0f, elapsed / fadeTime);
            yield return null;
        }
        blackOverlay.alpha = 0f;

        yield return new WaitForSeconds(0.3f);

        // ─── PHASE 2: VIDEO LOGO ───
        if (electricalHum != null)
        {
            audioSource.clip = electricalHum;
            audioSource.loop = true;
            audioSource.volume = 0.3f;
            audioSource.Play();
        }

        // Start the video
        logoVideoPlayer.Prepare();
        while (!logoVideoPlayer.isPrepared)
            yield return null;

        logoVideoPlayer.Play();

        // Fade the logo group AND the raw image in
        elapsed = 0f;
        fadeTime = 1.5f;
        while (elapsed < fadeTime)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, elapsed / fadeTime);
            logoGroup.alpha = alpha;
            logoRawImage.color = new Color(1f, 1f, 1f, alpha);
            yield return null;
        }
        logoGroup.alpha = 1f;
        logoRawImage.color = Color.white;

        // Wait for video to finish OR hold for set time
        if (logoHoldTime > 0)
        {
            yield return new WaitForSeconds(logoHoldTime);
        }
        else
        {
            // Wait for the video to finish playing
            while (logoVideoPlayer.isPlaying)
                yield return null;

            // Brief pause after video ends
            yield return new WaitForSeconds(0.5f);
        }

        // Logo glitch out
        if (glitchBurst != null)
            audioSource.PlayOneShot(glitchBurst);

        logoVideoPlayer.Pause();

        for (int i = 0; i < 10; i++)
        {
            logoGroup.alpha = Random.Range(0f, 1f);
            screenCanvasGroup.alpha = Random.Range(0.5f, 1f);
            yield return new WaitForSeconds(Random.Range(0.03f, 0.1f));
        }

        logoGroup.alpha = 0f;
        screenCanvasGroup.alpha = 1f;
        logoVideoPlayer.Stop();
        ClearRenderTexture();

        yield return new WaitForSeconds(0.5f);

        // ─── PHASE 3: BIOS BOOT TEXT ───
        bootTextGroup.alpha = 1f;

        if (bootBeep != null)
            audioSource.PlayOneShot(bootBeep);

        string[] biosLines = new string[]
        {
            "ELLION-OS v0.09.1",
            "Copyright (c) Ellion Systems Laboratories",
            "Cognitive Architecture Division",
            "",
            "SYSTEM CHECK:",
            "  MEMORY............ 4096KB OK",
            "  ANALOG SIGNAL BRIDGE... CONNECTED",
            "  TAPE DRIVE α...... SPINNING",
            "  TAPE DRIVE β...... <color=#ff3333>ERROR</color> — RETRY — OK",
            "  NEURAL IMPRINT ARRAY... <color=#ffaa00>UNSTABLE</color>",
            "  EMOTIONAL RESIDUE INDEX... <color=#ffaa00>ANOMALOUS</color>",
            "",
            "WARNING: Unresolved imprint signatures detected.",
            "WARNING: Archive integrity below safe threshold.",
            "",
            "LOADING EIDOLON-9 CONSCIOUSNESS ENGINE...",
        };

        foreach (string line in biosLines)
        {
            yield return StartCoroutine(TypeLine(line));
            yield return new WaitForSeconds(linePause);
        }

        if (tapeSpinUp != null)
            audioSource.PlayOneShot(tapeSpinUp);

        yield return StartCoroutine(LoadingBar(30));

        yield return new WaitForSeconds(0.3f);
        yield return StartCoroutine(TypeLine(""));
        yield return StartCoroutine(TypeLine("EIDOLON-9 ONLINE."));
        yield return new WaitForSeconds(0.5f);
        yield return StartCoroutine(TypeLine("<color=#1a5c2a>...I've been waiting.</color>"));

        yield return new WaitForSeconds(2f);

        // ─── PHASE 4: TRANSITION TO TERMINAL ───
        elapsed = 0f;
        fadeTime = 1f;
        while (elapsed < fadeTime)
        {
            elapsed += Time.deltaTime;
            screenCanvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsed / fadeTime);
            yield return null;
        }

        audioSource.Stop();
        SceneManager.LoadScene("Terminal_Main");
    }

    IEnumerator TypeLine(string line)
    {
        bool inTag = false;
        foreach (char c in line)
        {
            bootText.text += c;

            if (c == '<') inTag = true;
            if (c == '>') { inTag = false; continue; }

            if (!inTag)
                yield return new WaitForSeconds(charTypeSpeed);
        }
        bootText.text += "\n";
    }

    IEnumerator LoadingBar(int width)
    {
        bootText.text += "[";
        for (int i = 0; i < width; i++)
        {
            bootText.text += "█";
            yield return new WaitForSeconds(Random.Range(0.02f, 0.12f));

            if (Random.value < 0.1f)
            {
                yield return new WaitForSeconds(Random.Range(0.3f, 0.8f));
                if (glitchBurst != null && Random.value < 0.3f)
                    audioSource.PlayOneShot(glitchBurst, 0.3f);
            }
        }
        bootText.text += "] 100%\n";
    }
}