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
}
