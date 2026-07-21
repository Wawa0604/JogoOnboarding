using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CollectibleUIPopup : MonoBehaviour
{
    public static CollectibleUIPopup Instance;

    [Header("UI Elementos")]
    public GameObject painelPopup;
    public TextMeshProUGUI textoNome;
    public TextMeshProUGUI textoDescricao;
    public Image imagemFoto;
    public Button botaoLink;
    public Button botaoFechar;

    private string linkAtual = "";

    private void Awake()
    {
        Instance = this;
        
        botaoFechar.onClick.AddListener(Fechar);
        botaoLink.onClick.AddListener(AbrirLink);
        painelPopup.SetActive(false);
    }

    public void ExibirItem(CollectibleData data)
    {
        textoNome.text = data.nomeItem;
        textoDescricao.text = data.descricao;
        imagemFoto.sprite = data.foto;
        linkAtual = data.urlExterno;

        // Se não tiver link cadastrado, esconde o botão
        botaoLink.gameObject.SetActive(!string.IsNullOrEmpty(linkAtual));
        
        painelPopup.SetActive(true);
    }

    private void AbrirLink()
    {
        if (!string.IsNullOrEmpty(linkAtual))
        {
            Application.OpenURL(linkAtual); // Abre a nova aba mágica no navegador!
        }
    }

    private void Fechar() => painelPopup.SetActive(false);
}