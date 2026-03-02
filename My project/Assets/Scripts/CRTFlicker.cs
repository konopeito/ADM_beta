using UnityEngine;

public class CRTFlicker : MonoBehaviour
{
    private CanvasGroup canvasGroup;

    [Range(0.9f, 1f)]
    public float minAlpha = 0.96f;

    [Range(0f, 0.1f)]
    public float jitterAmount = 0.003f;

    private RectTransform rectTransform;
    private Vector2 originalPosition;

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();
        if (rectTransform != null)
            originalPosition = rectTransform.anchoredPosition;
    }

    void Update()
    {
        // Subtle alpha flicker
        canvasGroup.alpha = Random.Range(minAlpha, 1f);

        // Subtle screen jitter
        if (rectTransform != null)
        {
            float jitterX = Random.Range(-jitterAmount, jitterAmount) * Screen.width;
            float jitterY = Random.Range(-jitterAmount, jitterAmount) * Screen.height;
            rectTransform.anchoredPosition = originalPosition + new Vector2(jitterX, jitterY);
        }
    }
}