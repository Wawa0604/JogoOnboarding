using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InteractionUI : MonoBehaviour
{
    [Header("Configuração de Exibição")]
    [SerializeField] private GameObject uiPanel; 

    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI characterNameText;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private Button btnPrevious;
    [SerializeField] private Button btnNext;

    public void SetDialogue(string characterName, string text)
    {
        if (characterNameText != null) characterNameText.text = characterName;
        if (dialogueText != null) dialogueText.text = text;
    }

    public void SetButtonState(bool hasPrevious, bool hasNext)
    {
        if (btnPrevious != null) btnPrevious.interactable = hasPrevious;
        if (btnNext != null) btnNext.interactable = hasNext;
    }

   public void Show() 
    {
        if (uiPanel != null)
        {
            uiPanel.SetActive(true);
            Debug.Log("<color=cyan>UI: Painel ativado com sucesso!</color>");
        }
        else
        {
            Debug.LogError("UI: O campo 'uiPanel' está VAZIO no Inspector!");
        }
    }
    public void Hide() => uiPanel?.SetActive(false);
}