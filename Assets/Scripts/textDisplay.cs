using UnityEngine;
using TMPro;
using JetBrains.Annotations;

public class textDisplay : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public TextMeshProUGUI timerText;
    private betterMovement playerScript;

    void Start()
    {
        // 2. Search for the betterMovement script in the scene
        playerScript = FindFirstObjectByType<betterMovement>();

        // Automatically grab TextMeshPro if this script is attached directly to the Text object
        if (timerText == null)
        {
            timerText = GetComponent<TextMeshProUGUI>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (playerScript != null && timerText != null)
        {
            // "F0" rounds the float to 0 decimal places (whole seconds)
            timerText.text = "Cooldown: " + playerScript.TitanaCooldown.ToString("F0") + "s" +
                             " | Duration: " + playerScript.TitanaDuration.ToString("F0") + "s";
        }
    }
}
