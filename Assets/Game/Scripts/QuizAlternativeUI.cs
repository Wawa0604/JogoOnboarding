using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class QuizAlternativeUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI campoTexto;
    [SerializeField] private Image campoImagem;
    [SerializeField] private TextMeshProUGUI campoJustificativa; 
    
    public QuizAlternative Dados { get; private set; }
    public bool IsSelected { get; private set; } 
    
    private QuizManager gerenciador;
    private Button botao;

    public void Configurar(QuizAlternative alternativa, QuizManager manager)
    {
        Dados = alternativa;
        gerenciador = manager;
        IsSelected = false;
        botao = GetComponent<Button>();

        // CORRIGIDO: Agora usa 'textoAlternative' para bater com o ScriptableObject unificado
        if (campoTexto != null)
        {
            campoTexto.text = alternativa.textoAlternative;
            campoTexto.gameObject.SetActive(!string.IsNullOrEmpty(alternativa.textoAlternative));
        }

        if (campoImagem != null)
        {
            campoImagem.sprite = alternativa.spriteAlternativa;
        }

        if (campoJustificativa != null)
        {
            campoJustificativa.gameObject.SetActive(false); 
        }

        AtualizarVisualSelecao();

        botao.onClick.RemoveAllListeners();
        botao.onClick.AddListener(OnToggleSelect);
    }

    private void OnToggleSelect()
    {
        if (gerenciador.ExibindoJustificativa) return;

        IsSelected = !IsSelected;
        AtualizarVisualSelecao();
        
        // CORRIGIDO: Alterado de 'AtualizarBotaoConfirmar' para 'AtivarBotaoConfirmar'
        gerenciador.AtivarBotaoConfirmar(); 
    }

    private void AtualizarVisualSelecao()
    {
        ColorBlock cb = botao.colors;

        Color corAlvo = IsSelected ? new Color(0.2f, 0.6f, 1f) : Color.white;

        cb.normalColor = corAlvo;
        cb.selectedColor = corAlvo;
        cb.highlightedColor = corAlvo;
        botao.colors = cb;
    }

    public void RevelarResultado()
    {
        botao.interactable = false; 

        if (campoJustificativa != null && !Dados.ehCorreta && !string.IsNullOrEmpty(Dados.justificativa))
        {
            campoJustificativa.text = Dados.justificativa;
            campoJustificativa.gameObject.SetActive(true);
        }

        ColorBlock cb = botao.colors;
        if (Dados.ehCorreta)
        {
            cb.disabledColor = new Color(0.2f, 0.8f, 0.2f); 
        }
        else if (IsSelected && !Dados.ehCorreta)
        {
            cb.disabledColor = new Color(0.8f, 0.2f, 0.2f); 
        }
        else
        {
            cb.disabledColor = Color.gray; 
        }
        botao.colors = cb;
    }
}