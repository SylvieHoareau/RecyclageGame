using UnityEngine;
using UnityEngine.UI;
using TMPro; // si tu utilises TextMeshPro
using System.Collections;

public class LoopBar : MonoBehaviour
{
    [Header("UI Elements")]
    // [SerializeField] private Image loopFill;
    [SerializeField] private Slider loopSlider; //  barre style HP
    [SerializeField] private TextMeshProUGUI loopTimerText; // pour 00:00
    [SerializeField] private TextMeshProUGUI loopCounterText; // pour Boucle 1, Boucle 2...


    void Start()
    {
        if (TimeLoopManager.Instance != null)
        {
            TimeLoopManager.Instance.OnLoopRestart += OnLoopReset;
        }

        // Initialisation de la barre
        if (loopSlider != null && TimeLoopManager.Instance != null)
        {
            loopSlider.minValue = 0f;
            loopSlider.maxValue = TimeLoopManager.Instance.LoopDuration;
        }
        
        // Initialise l'affichage au démarrage
        UpdateUI();
    }

    void Update()
    {
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (TimeLoopManager.Instance == null) return;

        // On lit les valeurs directement depuis le TimeLoopManager
        float currentLoopTime = TimeLoopManager.Instance.TimeRemaining;
        float loopDuration = TimeLoopManager.Instance.LoopDuration;
        int loopCount = TimeLoopManager.Instance.LoopCount;

        // Mise à jour du Slider
        if (loopSlider != null)
        {
            loopSlider.value = currentLoopTime;
        }

        // Formatage du texte pour le timer (00:00)
        int minutes = Mathf.FloorToInt(currentLoopTime / 60);
        int seconds = Mathf.FloorToInt(currentLoopTime % 60);
        loopTimerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

        // Mise à jour du texte du compteur de boucles
        if (loopCounterText != null)
        {
            loopCounterText.text = $"Boucle {loopCount}";
        }
    }

    private void OnDestroy()
    {
        if (TimeLoopManager.Instance != null)
        {
            TimeLoopManager.Instance.OnLoopRestart -= OnLoopReset;
        }
    }

    private void OnLoopReset()
    {
        // Ici tu peux déclencher des effets : flash, son, reset d’objets, etc.
        Debug.Log("Nouvelle boucle !");

        // 1. Jouer un son
        AudioSource audio = GetComponent<AudioSource>();
        if (audio != null)
            audio.Play();

        // 2. Déclencher un flash visuel (coroutine)
        StartCoroutine(FlashScreen());
    }

    private IEnumerator FlashScreen()
    {
        // Crée un overlay blanc temporaire
        GameObject flash = new GameObject("Flash");
        Image img = flash.AddComponent<Image>();
        img.color = new Color(1, 1, 1, 0); // transparent

        flash.transform.SetParent(transform.root, false); // UI parent
        RectTransform rt = flash.GetComponent<RectTransform>();
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;

        // Fade in/out
        float duration = 0.5f;
        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            img.color = new Color(1, 1, 1, 1 - (t / duration));
            yield return null;
        }

        Destroy(flash);
    }
}
