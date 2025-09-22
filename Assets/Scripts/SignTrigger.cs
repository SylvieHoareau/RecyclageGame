using UnityEngine;
using TMPro; // si tu utilises TextMeshPro

public class SignTrigger : MonoBehaviour
{
    [Header("Texte du panneau")]
    [TextArea]
    [SerializeField] private string message;

    private bool playerInRange = false;
    private static TextMeshProUGUI signText; 

    void Start()
    {
        // On récupère le Text UI une seule fois
        if (signText == null)
        {
            GameObject textObj = GameObject.FindWithTag("SignText"); 
            if (textObj != null)
            {
                signText = textObj.GetComponent<TextMeshProUGUI>();
            }
            else
            {
                Debug.LogWarning("Aucun objet UI avec le tag 'SignText' trouvé !");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            ShowMessage();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            HideMessage();
        }
    }

    private void ShowMessage()
    {
        if (signText != null)
        {
            signText.text = message;
            signText.enabled = true;
        }
    }

    private void HideMessage()
    {
        if (signText != null)
        {
            signText.enabled = false;
        }
    }
}
