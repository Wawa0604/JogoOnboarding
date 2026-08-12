using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InteractionUI : MonoBehaviour
{
    [Header("Configuração de Exibição")]
    [SerializeField] private GameObject uiPanel; 

    [Header("UI Elements (Atribuição Manual por Cena)")]
    [SerializeField] private TextMeshProUGUI characterNameText;
    [SerializeField] private Image imagemAvatar;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private Button btnPrevious;
    [SerializeField] private Button btnNext;

    [Header("UI de Áudio (Novo)")]
    [SerializeField] private Button btnAudio;
    [SerializeField] private Image imgAudioIcon;
    [SerializeField] private Sprite iconPlay;
    [SerializeField] private Sprite iconPause;

    public void SetDialogue(string characterName, string text, Sprite avatarSprite)
    {
        if (characterNameText != null) characterNameText.text = characterName;
        if (dialogueText != null) dialogueText.text = text;
        if (imagemAvatar != null) imagemAvatar.sprite = avatarSprite;
    }

    public void SetButtonState(bool hasPrevious, bool hasNext)
    {
        if (btnPrevious != null) btnPrevious.interactable = hasPrevious;
        if (btnNext != null) btnNext.interactable = hasNext;
    }

    // Configura a visibilidade do botão de áudio e atualiza seu ícone
    public void SetAudioButtonState(bool hasAudio, bool isPlaying)
    {
        if (btnAudio != null)
        {
            btnAudio.gameObject.SetActive(hasAudio);
            if (imgAudioIcon != null)
            {
                imgAudioIcon.sprite = isPlaying ? iconPause : iconPlay;
            }
        }
    }

    // Método para vincular o clique do botão no inspector ou por código
    public Button GetAudioButton() => btnAudio;

    public void Show() 
    {
        if (uiPanel != null)
        {
            uiPanel.SetActive(true);
        }
    }

    public void Hide() => uiPanel?.SetActive(false);
}