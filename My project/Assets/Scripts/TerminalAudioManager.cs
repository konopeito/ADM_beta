using UnityEngine;

public class TerminalAudioManager : MonoBehaviour
{
    public static TerminalAudioManager Instance { get; private set; }

    [Header("Audio Sources")]
    public AudioSource sfxSource;
    public AudioSource ambientSource;
    public AudioSource typingSource;

    [Header("Typing Sounds")]
    public AudioClip[] keyPressSounds;
    public AudioClip enterKeySound;

    [Header("Command Feedback")]
    public AudioClip commandAccepted;
    public AudioClip commandError;
    public AudioClip helpOpen;

    [Header("Scanning")]
    public AudioClip scanSweep;
    public AudioClip deepScanSweep;
    public AudioClip deltaUnlocked;

    [Header("Memory / Logs")]
    public AudioClip logOpen;
    public AudioClip logCorrupted;
    public AudioClip memoryDecayTick;

    [Header("Eidolon")]
    public AudioClip stateShift;
    public AudioClip eidolonWhisper;
    public AudioClip eidolonRecognize;
    public AudioClip ellionTrigger;

    [Header("Glitch")]
    public AudioClip glitchBurst;
    public AudioClip staticCrackle;
    public AudioClip signalDistortion;

    [Header("Ambient")]
    public AudioClip ambientHum;
    public AudioClip tapeHiss;

    [Header("Volume Settings")]
    [Range(0f, 1f)] public float typingVolume = 0.4f;
    [Range(0f, 1f)] public float sfxVolume = 0.6f;
    [Range(0f, 1f)] public float ambientVolume = 0.15f;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        StartAmbient();
    }

    public void StartAmbient()
    {
        if (ambientHum != null)
        {
            ambientSource.clip = ambientHum;
            ambientSource.loop = true;
            ambientSource.volume = ambientVolume;
            ambientSource.Play();
        }
    }

    public void PlayKeyPress()
    {
        if (keyPressSounds.Length == 0) return;
        typingSource.pitch = Random.Range(0.95f, 1.05f);
        typingSource.PlayOneShot(
            keyPressSounds[Random.Range(0, keyPressSounds.Length)],
            typingVolume
        );
    }

    public void PlayEnterKey()
    {
        if (enterKeySound != null)
            typingSource.PlayOneShot(enterKeySound, typingVolume * 1.2f);
    }

    public void PlayCommandAccepted()
    {
        if (commandAccepted != null)
            sfxSource.PlayOneShot(commandAccepted, sfxVolume);
    }

    public void PlayCommandError()
    {
        if (commandError != null)
            sfxSource.PlayOneShot(commandError, sfxVolume);
    }

    public void PlayHelpOpen()
    {
        if (helpOpen != null)
            sfxSource.PlayOneShot(helpOpen, sfxVolume * 0.8f);
    }

    public void PlayScan()
    {
        if (scanSweep != null)
            sfxSource.PlayOneShot(scanSweep, sfxVolume);
    }

    public void PlayDeepScan()
    {
        if (deepScanSweep != null)
            sfxSource.PlayOneShot(deepScanSweep, sfxVolume);
    }

    public void PlayDeltaUnlocked()
    {
        if (deltaUnlocked != null)
            sfxSource.PlayOneShot(deltaUnlocked, sfxVolume * 1.5f);
    }

    public void PlayLogOpen()
    {
        if (logOpen != null)
            sfxSource.PlayOneShot(logOpen, sfxVolume);
    }

    public void PlayLogCorrupted()
    {
        if (logCorrupted != null)
            sfxSource.PlayOneShot(logCorrupted, sfxVolume);
    }

    public void PlayDecayTick()
    {
        if (memoryDecayTick != null)
            sfxSource.PlayOneShot(memoryDecayTick, sfxVolume * 0.3f);
    }

    public void PlayStateShift()
    {
        if (stateShift != null)
            sfxSource.PlayOneShot(stateShift, sfxVolume);
    }

    public void PlayWhisper()
    {
        if (eidolonWhisper != null)
        {
            sfxSource.pitch = Random.Range(0.85f, 1f);
            sfxSource.PlayOneShot(eidolonWhisper, sfxVolume * 0.5f);
            sfxSource.pitch = 1f;
        }
    }

    public void PlayRecognize()
    {
        if (eidolonRecognize != null)
            sfxSource.PlayOneShot(eidolonRecognize, sfxVolume);
    }

    public void PlayEllionTrigger()
    {
        if (ellionTrigger != null)
            sfxSource.PlayOneShot(ellionTrigger, sfxVolume * 1.3f);
    }

    public void PlayGlitchBurst()
    {
        if (glitchBurst != null)
            sfxSource.PlayOneShot(glitchBurst, sfxVolume);
    }

    public void PlayStaticCrackle()
    {
        if (staticCrackle != null)
            sfxSource.PlayOneShot(staticCrackle, sfxVolume * 0.4f);
    }

    public void PlaySignalDistortion()
    {
        if (signalDistortion != null)
        {
            sfxSource.pitch = Random.Range(0.7f, 1.3f);
            sfxSource.PlayOneShot(signalDistortion, sfxVolume * 0.7f);
            sfxSource.pitch = 1f;
        }
    }

    public void UpdateAmbientForDecay(float decayPercent)
    {
        ambientSource.volume = Mathf.Lerp(ambientVolume, ambientVolume * 2f, decayPercent / 100f);

        if (decayPercent > 60f)
        {
            ambientSource.pitch = 1f + Random.Range(-0.02f, 0.02f);
        }
    }
}