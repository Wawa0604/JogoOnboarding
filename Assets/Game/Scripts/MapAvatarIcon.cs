using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class MapAvatarIcon : MonoBehaviour
{
    [System.Serializable]
    public class AvatarLayer
    {
        public string identificador;
        public Transform parentObject;
        public bool dependeDoCorpo;
    }

    [Header("Configuração das Partes do Ícone")]
    public List<AvatarLayer> camadasDoAvatar;

    private void OnEnable()
    {
        Debug.Log("<color=cyan>[MapIcon]</color> Ícone do mapa ativado. Iniciando atualização...");
        AtualizarIcone();
    }

    public void AtualizarIcone()
    {
        if (Game_Manager.Instance == null)
        {
            Debug.LogError("<color=cyan>[MapIcon]</color> Game_Manager não encontrado! Cancelando atualização.");
            return;
        }

        Debug.Log($"<color=cyan>[MapIcon]</color> Lendo dados do Game_Manager... Peças salvas: {Game_Manager.Instance.avatarParts.Count} | Cores salvas: {Game_Manager.Instance.avatarColors.Count}");

        int corpoIndex = 0;
        if (Game_Manager.Instance.avatarParts.ContainsKey("Body"))
        {
            corpoIndex = Game_Manager.Instance.avatarParts["Body"];
            Debug.Log($"<color=cyan>[MapIcon]</color> Corpo atual (Thin/Large) detectado: índice {corpoIndex}");
        }
        else
        {
            Debug.LogWarning("<color=cyan>[MapIcon]</color> Nenhum 'Body' salvo no Game_Manager. Usando índice 0 (padrão).");
        }

        foreach (var camada in camadasDoAvatar)
        {
            if (camada.parentObject == null) continue;

            string id = camada.identificador;
            int itemEscolhidoIndex = 0;

            if (Game_Manager.Instance.avatarParts.ContainsKey(id))
            {
                itemEscolhidoIndex = Game_Manager.Instance.avatarParts[id];
                Debug.Log($"<color=cyan>[MapIcon]</color> Camada '{id}' encontrou índice salvo: {itemEscolhidoIndex}");
            }
            else
            {
                Debug.Log($"<color=cyan>[MapIcon]</color> Camada '{id}' NÃO possui save. Usando índice 0.");
            }

            for (int i = 0; i < camada.parentObject.childCount; i++)
            {
                GameObject filho = camada.parentObject.GetChild(i).gameObject;
                bool deveAtivar = (i == itemEscolhidoIndex);

                if (deveAtivar && camada.dependeDoCorpo)
                {
                    for (int j = 0; j < filho.transform.childCount; j++)
                    {
                        GameObject subFilho = filho.transform.GetChild(j).gameObject;
                        bool deveAtivarSub = (j == corpoIndex);
                        subFilho.SetActive(deveAtivarSub);
                    }
                }

                filho.SetActive(deveAtivar);

                if (deveAtivar)
                {
                    AplicarCorNaCamada(camada, filho);
                }
            }
        }
    }

    private void AplicarCorNaCamada(AvatarLayer camada, GameObject filhoAtivo)
    {
        if (Game_Manager.Instance.avatarColors.ContainsKey(camada.identificador))
        {
            Color corSalva = Game_Manager.Instance.avatarColors[camada.identificador];
            Debug.Log($"<color=cyan>[MapIcon]</color> Aplicando cor salva na camada '{camada.identificador}': {corSalva}");
            Transform alvoDaCor = filhoAtivo.transform;

            if (camada.dependeDoCorpo)
            {
                for (int j = 0; j < filhoAtivo.transform.childCount; j++)
                {
                    if (filhoAtivo.transform.GetChild(j).gameObject.activeSelf)
                    {
                        alvoDaCor = filhoAtivo.transform.GetChild(j);
                        break;
                    }
                }
            }

            Image img = alvoDaCor.GetComponent<Image>();
            if (img != null) { img.color = corSalva; }
            else
            {
                SpriteRenderer sr = alvoDaCor.GetComponent<SpriteRenderer>();
                if (sr != null) { sr.color = corSalva; }
            }
        }
    }
}