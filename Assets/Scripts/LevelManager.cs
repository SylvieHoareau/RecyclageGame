using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private string nextSceneName;

    // Appelez cette méthode pour déclencher la fin du niveau
    public void FinishLevel()
    {
        if (GameFlowManager.Instance != null)
        {
            GameFlowManager.Instance.EndLevel(nextSceneName);
        }
    }
}

