using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class ItemMissaoUI : MonoBehaviour
{
    public TextMeshProUGUI textoDescricao;
    public Toggle checkbox;
    public Image imagemRisco; // Arraste o objeto "Linha_Risco" aqui no Inspector
    public float velocidadeRisco = 2.0f; // Tempo da animação

    public void Configurar(string descricao, bool estaCompleta)
    {
        textoDescricao.text = descricao;
        checkbox.isOn = estaCompleta;

        if (estaCompleta)
        {
            imagemRisco.fillAmount = 1;
            textoDescricao.alpha = 0.5f;
        }
        else
        {
            imagemRisco.fillAmount = 0;
            textoDescricao.alpha = 1.0f;
        }
    }

    // Esta função será chamada pelo MissionManager para fazer a mágica acontecer
    public IEnumerator AnimarConclusao()
    {
        checkbox.isOn = true;
        float progresso = 0;

        // Animação da caneta riscando
        while (progresso < 1)
        {
            progresso += Time.deltaTime * velocidadeRisco;
            imagemRisco.fillAmount = progresso;
            yield return null;
        }

        // Feedback visual de desativado
        textoDescricao.alpha = 0.5f;
        
        yield return new WaitForSeconds(0.5f); // Pequena pausa dramática antes de mover

        // Move para o final da lista no layout
        transform.SetAsLastSibling();
    }
}