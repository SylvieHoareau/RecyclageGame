using UnityEngine;
using UnityEngine.UI;
using TMPro; // si tu utilises TextMeshPro

public class LoopBar : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Image loopFill;
    [SerializeField] private TextMeshProUGUI loopCounterText; // ou Text si pas TMP

    [Header("Loop Settings")]
    [SerializeField] private float loopDuration = 10f; // durée d'une boucle en secondes

    private float timer = 0f;
    private int loopCount = 1;

    void Update()
    {
        // Avance le timer
        timer += Time.deltaTime;

        // Calcul du pourcentage de la barre
        float fillAmount = Mathf.Clamp01(timer / loopDuration);
        loopFill.fillAmount = fillAmount;

        // Check si la boucle est terminée
        if (timer >= loopDuration)
        {
            timer = 0f;
            loopCount++;
            loopCounterText.text = $"Boucle : {loopCount}";
            OnLoopReset();
        }
    }

    private void OnLoopReset()
    {
        // Ici tu peux déclencher des effets : flash, son, reset d’objets, etc.
        Debug.Log("Nouvelle boucle !");
    }
}
