using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class QuizAlternativeUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI campoTexto;
    [SerializeField] private Image campoImagem;
    [SerializeField] private TextMeshProUGUI campoJustificativa; // <--- COLOQUE UM TEXTO PARA A JUSTIFICATIVA NO PREFAB
    
    public QuizAlternative Dados { get; private set; }
    public bool IsSelected { get; private set; } // Guarda se o jogador marcou esta opção
    
    private QuizManager gerenciador;
    private Button botao;

    public void Configurar(QuizAlternative alternativa, QuizManager manager)
    {
        Dados = alternativa;
        gerenciador = manager;
        IsSelected = false;
        botao = GetComponent<Button>();

        // Configura o Texto
        if (campoTexto != null)
        {
            campoTexto.text = alternativa.textoAlternativa;
            campoTexto.gameObject.SetActive(!string.IsNullOrEmpty(alternativa.textoAlternativa));
        }

        // Configura o Sprite/Imagem
        if (campoImagem != null)
        {
            campoImagem.sprite = alternativa.spriteAlternativa;
           // campoImagem.gameObject.SetActive(alternativa.spriteAlternativa != null);
        }

        if (campoJustificativa != null)
        {
            campoJustificativa.gameObject.SetActive(false); // Começa escondido
        }

        AtualizarVisualSelecao();

        // configura o OnClick do próprio botão
        botao.onClick.RemoveAllListeners();
        botao.onClick.AddListener(OnToggleSelect);
    }

    private void OnToggleSelect()
    {
        // Se já confirmou e está vendo a justificativa, não deixa desmarcar
        if (gerenciador.ExibindoJustificativa) return;

        IsSelected = !IsSelected;
        AtualizarVisualSelecao();
        gerenciador.AtualizarBotaoConfirmar(); // Avisa o gerente para ver se libera o botão "Confirmar"
    }

    private void AtualizarVisualSelecao()
    {
        ColorBlock cb = botao.colors;

        // define a cor base: azul se marcado, branco se desmarcado
        Color corAlvo = IsSelected ? new Color(0.2f, 0.6f, 1f) : Color.white;

        // Aplicamos a cor para todos os estados de repouso/foco do botão
        cb.normalColor = corAlvo;
        cb.selectedColor = corAlvo;
        cb.highlightedColor = corAlvo;
        botao.colors = cb;
    }

    public void RevelarResultado()
    {
        botao.interactable = false; // Desativa o clique

        // Se NÃO for correta e tiver uma justificativa, mostra ela na tela!
        if (campoJustificativa != null && !Dados.ehCorreta && !string.IsNullOrEmpty(Dados.justificativa))
        {
            campoJustificativa.text = Dados.justificativa;
            campoJustificativa.gameObject.SetActive(true);
        }

        // Feedback Visual de Cores do Botão (Certo/Errado)
        ColorBlock cb = botao.colors;
        if (Dados.ehCorreta)
        {
            cb.disabledColor = new Color(0.2f, 0.8f, 0.2f); // Verde (Era a certa)
        }
        else if (IsSelected && !Dados.ehCorreta)
        {
            cb.disabledColor = new Color(0.8f, 0.2f, 0.2f); // Vermelho (Você marcou a errada)
        }
        else
        {
            cb.disabledColor = Color.gray; // Cinza (Errada que você não marcou)
        }
        botao.colors = cb;
    }

}