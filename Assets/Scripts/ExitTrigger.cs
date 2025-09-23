using UnityEngine;

public class ExitTrigger : MonoBehaviour
{
    [Tooltip("Nom de la scène suivante à charger.")]
    [SerializeField] private string nextSceneName;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Joueur a atteint la sortie. Fin du niveau.");

            // Affiche la boîte de dialogue pour annoncer la sortie au joueur
            // Utilisez FindObjectOfType de manière plus robuste
            TutorialManager tutorialManager = FindObjectOfType<TutorialManager>();
            if (tutorialManager != null)
            {
                tutorialManager.OnExitReached();
            }

            // On demande au GameFlowManager de gérer la fin du niveau.
            // La méthode à appeler est 'EndLevel()', pas 'HandleLevelCompletion()'.
            if (GameFlowManager.Instance != null)
            {
                // On appelle la méthode de fin de niveau et on lui passe le nom de la scène à charger.
                // Cela permet au GameFlowManager de ne pas se soucier de la prochaine scène,
                // il se contente d'exécuter l'action.
                GameFlowManager.Instance.EndLevel(nextSceneName);
            }
            else
            {
                Debug.LogError("GameFlowManager.Instance est null. Assurez-vous qu'il existe dans la scène.");
            }
        }
    }
}
