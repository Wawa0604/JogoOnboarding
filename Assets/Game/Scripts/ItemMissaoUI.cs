using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class ItemMissaoUI : MonoBehaviour
{
    public TextMeshProUGUI textoDescricao;
    public Toggle checkbox;
    public Image imagemRisco; 
    public float velocidadeRisco = 2.0f;

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

    public IEnumerator AnimarConclusao()
    {
        checkbox.isOn = true;
        float progresso = 0;

        // Animação da caneta riscando (preenchimento horizontal)
        while (progresso < 1)
        {
            progresso += Time.deltaTime * velocidadeRisco;
            imagemRisco.fillAmount = progresso;
            yield return null;
        }

        textoDescricao.alpha = 0.5f;
        yield return new WaitForSeconds(0.3f); // Pausa curta para o jogador ver o risco
    }
}