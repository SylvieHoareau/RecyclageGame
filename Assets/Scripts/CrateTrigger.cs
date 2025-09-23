using UnityEngine;

public class CrateTrigger : MonoBehaviour
{

     // --- TRIGGERS & INTERACTIONS ---
    // Méthode pour gérer l'interaction avec le TriggerObject

    public void HandleCrateTrigger()
    {
        // AFfiche une boîte de dialogue
        FindObjectOfType<TutorialManager>()?.OnCratePlaced();
        // Trouve l'objet Bridge avec le script StrechingObject
        // Ce code ne sera appelé que via le spawner trigger du Level 1
        var bridge = FindObjectOfType<StrechingObject>();
        if (bridge != null)
        {
            bridge.ChangeStretch();
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
