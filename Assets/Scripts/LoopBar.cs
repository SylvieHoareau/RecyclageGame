using UnityEngine;
using UnityEngine.UI;
using TMPro; // si tu utilises TextMeshPro
using System.Collections;

public class LoopBar : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Image loopFill;
    [SerializeField] private Slider loopSlider; //  barre style HP
    [SerializeField] private TextMeshProUGUI loopTimerText; // pour 00:00
    [SerializeField] private TextMeshProUGUI loopCounterText; // pour Boucle 1, Boucle 2...

    [Header("Loop Settings")]
    [SerializeField] private float loopDuration = 30f; // durée d'une boucle en secondes

    void Start()
    {
        if (GameFlowManager.Instance != null)
        {
            GameFlowManager.Instance.OnLoopRestart += OnLoopReset;
        }

         // Init du slider
        if (loopSlider != null)
        {
            loopSlider.minValue = 0f;
            loopSlider.maxValue = GameFlowManager.Instance.loopDuration;
            loopSlider.value = GameFlowManager.Instance.loopDuration;
        }
    }

    void Update()
    {
        // On s'assure que l'instance du manager existe pour éviter les erreurs
        if (GameFlowManager.Instance == null)
        {
            return;
        }

        // On lit les valeurs directement depuis le GameFlowManager
        float currentLoopTime = GameFlowManager.Instance.loopTimer;
        float loopDuration = GameFlowManager.Instance.loopDuration;

        // --- Image Fill (style radial ou horizontal)
        if (loopFill != null)
        {
            float fillAmount = Mathf.Clamp01(currentLoopTime / loopDuration);
            loopFill.fillAmount = fillAmount;
        }

        // --- Slider (style barre de vie)
        if (loopSlider != null)
        {
            loopSlider.maxValue = loopDuration;
            loopSlider.value = currentLoopTime;
        }

        // On formate le texte pour afficher le temps restant
        // Timer (00:00)
        int minutes = Mathf.FloorToInt(currentLoopTime / 60);
        int seconds = Mathf.FloorToInt(currentLoopTime % 60);
        loopTimerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

        // Numéro de boucle
        if (loopCounterText != null)
        {
            loopCounterText.text = $"Boucle {GameFlowManager.Instance.loopCount}";
        }
    }

    private void OnDestroy()
    {
        if (GameFlowManager.Instance != null)
        {
            GameFlowManager.Instance.OnLoopRestart -= OnLoopReset;
        }
    }

    private void OnLoopReset()
    {
        // Ici tu peux déclencher des effets : flash, son, reset d’objets, etc.
        Debug.Log("Nouvelle boucle !");

        // 1. 🔊 Jouer un son
        AudioSource audio = GetComponent<AudioSource>();
        if (audio != null)
            audio.Play();

        // 2. 💡 Déclencher un flash visuel (coroutine)
        StartCoroutine(FlashScreen());
    }
    gt
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
