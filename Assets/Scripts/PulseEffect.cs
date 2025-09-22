using UnityEngine;

public class PulseEffect : MonoBehaviour
{
    [SerializeField] private float pulseSpeed = 2f; // vitesse de pulsation
    [SerializeField] private float pulseAmount = 0.1f; // intensité de l’agrandissement

    private Vector3 baseScale;

    void Start()
    {
        baseScale = transform.localScale;
    }

    void Update()
    {
        float scale = 1 + Mathf.Sin(Time.time * pulseSpeed) * pulseAmount;
        transform.localScale = baseScale * scale;
    }
}
