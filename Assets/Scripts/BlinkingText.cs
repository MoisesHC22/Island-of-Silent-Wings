using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BlinkingText : MonoBehaviour
{
    public TextMeshProUGUI blinkingText;
    public float blinkSpeed = 0.40f; // Velocidad de parpadeo (en segundos)

    private void Start()
    {
        blinkingText = GetComponent<TextMeshProUGUI>();

        if (blinkingText != null)
        {
            InvokeRepeating("ToggleBlink", 0f, blinkSpeed);
        }
        else
        {
            Debug.LogError("No se encontró el componente TextMeshProUGUI");
        }
    }

    private void ToggleBlink()
    {
        if (blinkingText != null)
        {
            blinkingText.enabled = !blinkingText.enabled;
        }
    }
}
