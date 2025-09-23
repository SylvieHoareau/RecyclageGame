using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    // Fonction pour charger la scène suivante
    public void NextScene()
    {

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    // Fonction pour retourner au menu principal
    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu"); // Nom exact de la scène dans Build Settings
    }
    
    // Fonction pour charger la scène Crédits
    public void LoadCredits()
    {
        SceneManager.LoadScene("Credits"); // crée une scène "Credits" et ajoute-la dans Build Settings
    }

    /// <summary>
    /// Recharge la dernière scène sauvegardée par le GameFlowManager.
    /// </summary>
    public void ReloadCurrentLevel()
    {
        // On s'assure que le GameFlowManager existe
        if (GameFlowManager.Instance != null && !string.IsNullOrEmpty(GameFlowManager.SceneToReload))
        {
            // Réinitialise l'état persistant si nécessaire avant de recharger le niveau
            // Important pour ne pas garder les objets détruits ou déplacés du niveau précédent
            if (PersistentState.Instance != null)
            {
                PersistentState.Instance.ClearState();
            }

            // Charge la scène dont le nom est stocké dans GameFlowManager
            SceneManager.LoadScene(GameFlowManager.SceneToReload);
        }
        else
        {
            Debug.LogError("Nom de la scène à recharger introuvable !");
            // Optionnel : Revenir au menu principal si on ne sait pas où aller
            LoadMainMenu();
        }
    }

    // Fonction pour quitter le jeu
    public void QuitGame()
    {
        Debug.Log("Quitter le jeu..."); // utile en mode éditeur
        Application.Quit(); // fonctionne seulement dans un build (pas dans l’éditeur Unity)
    }
}
