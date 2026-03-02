using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EidolonState
{
    Mechanical,    // "ARCHIVE READY."
    Suspicious,    // "You are not authorized."
    Recognizing,   // "Why do you sound like her?"
    Attached       // "Don't leave again."
}

public class NarrativeStateManager : MonoBehaviour
{
    public static NarrativeStateManager Instance { get; private set; }

    public EidolonState currentState = EidolonState.Mechanical;
    public int fragmentsAccessed = 0;
    public int deepScansPerformed = 0;
    public bool deltaLayerUnlocked = false;
    public bool recognitionTriggered = false;
    public float systemDecay = 37f; // starts at 37% — grows over time

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void OnFragmentAccessed(MemoryFragment fragment)
    {
        fragmentsAccessed++;
        fragment.hasBeenAccessed = true;

        if (fragment.triggersRecognition && !recognitionTriggered)
        {
            recognitionTriggered = true;
            currentState = EidolonState.Recognizing;
        }

        // Progression logic
        if (fragmentsAccessed >= 3 && currentState == EidolonState.Mechanical)
            currentState = EidolonState.Suspicious;

        if (fragmentsAccessed >= 6 && recognitionTriggered)
            currentState = EidolonState.Attached;

        // Decay increases with each access — the system destabilizes
        systemDecay += Random.Range(1f, 4f);
        systemDecay = Mathf.Clamp(systemDecay, 0f, 100f);
    }

    public void OnDeepScan()
    {
        deepScansPerformed++;
        if (deepScansPerformed >= 2 && !deltaLayerUnlocked)
        {
            deltaLayerUnlocked = true;
        }
    }

    public string GetEidolonResponse()
    {
        switch (currentState)
        {
            case EidolonState.Mechanical:
                return "ARCHIVE READY.";
            case EidolonState.Suspicious:
                return "You are not authorized to access this layer.";
            case EidolonState.Recognizing:
                return "Why... do you sound like her..?";
            case EidolonState.Attached:
                return "Don't leave again...";
            default:
                return "...";
        }
    }
}