using UnityEngine;

public class ExitTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Niveau terminé !");

            // Affiche la boîte de dialogue pour annoncer la sortie au joueur
            FindObjectOfType<TutorialManager>()?.OnExitReached();
            // On demande au GameFlowManager de gérer la fin du niveau
            GameFlowManager.Instance.HandleLevelCompletion();
        }
    }
}
