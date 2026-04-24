using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ItemMissaoUI : MonoBehaviour
{
    public TextMeshProUGUI textoDescricao;
    public Toggle checkbox;

    public void Configurar(string descricao, bool estaCompleta)
    {
        textoDescricao.text = descricao;
        checkbox.isOn = estaCompleta;

        // Se estiver completa, aplica o estilo de "Riscado" (Strikethrough)
        if (estaCompleta)
        {
            textoDescricao.fontStyle = FontStyles.Strikethrough;
            textoDescricao.alpha = 0.5f; // Efeito visual de "desativado"
        }
        else
        {
            textoDescricao.fontStyle = FontStyles.Normal;
            textoDescricao.alpha = 1.0f;
        }
    }
}