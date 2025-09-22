using UnityEngine;
using TMPro; // si tu utilises TextMeshPro

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private GameObject dialoguePanel; // Panel UI
    [SerializeField] private TextMeshProUGUI dialogueText; // Texte du tutoriel

    private int currentStep = 0;

    void Start()
    {
        ShowMessage("Pousse la caisse vers le carré orange avec les touches ZQSD de ton clavier");
    }

    private void ShowMessage(string message)
    {
        dialoguePanel.SetActive(true);
        dialogueText.text = message;
    }

    private void HideMessage()
    {
        dialoguePanel.SetActive(false);
    }

    // Appelé par GameFlowManager ou TriggerObject
    public void OnCratePlaced()
    {
        if (currentStep == 0)
        {
            ShowMessage("Passe sur le carré mauve pour faire pousser des fleurs");
            currentStep++;
        }
    }

    public void OnSpawnerActivated()
    {
        if (currentStep == 1)
        {
            ShowMessage("Mission accomplie ! Dirige-toi vers la sortie");
            currentStep++;
        }
    }

    public void OnExitReached()
    {
        if (currentStep == 2)
        {
            HideMessage();
        }
    }
}
