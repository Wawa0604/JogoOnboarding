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
    public GameObject check; 

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
        // CORREÇÃO: Encontra o teu gerenciador de botões real que está rodando na cena
        buttonsManager managerBotoes = FindAnyObjectByType<buttonsManager>();

        // Se o manager e o painel existirem, abre o painel automaticamente para o jogador ver o risco acontecer
        if (managerBotoes != null && managerBotoes.painelMissoes != null)
        {
            managerBotoes.painelMissoes.SetActive(true);
        }

        checkbox.isOn = true;
        float progresso = 0;

        check.SetActive(true);

        // Animação da caneta riscando (preenchimento horizontal)
        while (progresso < 1)
        {
            progresso += Time.deltaTime * velocidadeRisco;
            imagemRisco.fillAmount = progresso;
            yield return null;
        }

        textoDescricao.alpha = 0.5f;
        yield return new WaitForSeconds(0.3f); // Pausa curta para o jogador ver o risco feito

        // CORREÇÃO: Fecha o painel de missões novamente após o término da animação
        if (managerBotoes != null && managerBotoes.painelMissoes != null)
        {
            managerBotoes.painelMissoes.SetActive(false);
        }
    }
}