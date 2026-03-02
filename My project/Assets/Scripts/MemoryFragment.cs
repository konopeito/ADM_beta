using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewMemoryFragment", menuName = "ADM/Memory Fragment")]
public class MemoryFragment : ScriptableObject
{
    public string fragmentID;
    public string title;
    [TextArea(5, 20)]
    public string content;
    public int corruptionLevel; // 0 = clean, 100 = fully corrupted
    public bool isHidden; // requires deep_scan to find
    public bool hasBeenAccessed;

    [Header("Narrative Flags")]
    public bool triggersRecognition; // does this fragment make Eidolon "recognize" the player?
    public bool isEllionLog; // is this a Dr. Ellion personal log?

    public string GetDisplayContent()
    {
        if (corruptionLevel >= 75)
            return GlitchSystem.CorruptText(content, corruptionLevel);
        return content;
    }
}
