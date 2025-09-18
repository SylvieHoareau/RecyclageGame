using UnityEngine;

public class ExitTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Niveau terminé !");
            // On demande au GameFlowManager de gérer la fin du niveau
            GameFlowManager.Instance.HandleLevelCompletion();
        }
    }
}
