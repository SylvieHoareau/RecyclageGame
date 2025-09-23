using UnityEngine;

public class ExitTrigger : MonoBehaviour
{
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
                GameFlowManager.Instance.EndLevel();
            }
            else
            {
                Debug.LogError("GameFlowManager.Instance est null. Assurez-vous qu'il existe dans la scène.");
            }
        }
    }
}
