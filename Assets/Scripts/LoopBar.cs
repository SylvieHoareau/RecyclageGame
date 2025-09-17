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

        // Calcul du pourcentage de la barre basé sur le temps restant
        // On inverse le calcul pour que la barre se vide avec le temps
        float fillAmount = Mathf.Clamp01(currentLoopTime / loopDuration);
        loopFill.fillAmount = fillAmount;

        // On formate le texte pour afficher le temps restant
        int minutes = Mathf.FloorToInt(currentLoopTime / 60);
        int seconds = Mathf.FloorToInt(currentLoopTime % 60);
        loopCounterText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    private void OnLoopReset()
    {
        // Ici tu peux déclencher des effets : flash, son, reset d’objets, etc.
        Debug.Log("Nouvelle boucle !");
    }
}
