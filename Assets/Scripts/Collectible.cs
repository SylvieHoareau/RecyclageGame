using UnityEngine;

public class Collectible : MonoBehaviour
{
    [Header("Paramètres de collecte")]
    public string itemName;
    public Sprite itemSprite;
    public bool destroyOnCollect = true;

    public void Collect(GameObject collector)
    {
        if (collector.CompareTag("Player"))
        {
            GameFlowManager.Instance.HandleCollectible(this, collector);
        }
    }
}
