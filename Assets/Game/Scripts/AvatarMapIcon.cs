using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

[RequireComponent(typeof(RectTransform))]
public class AvatarMapIcon : MonoBehaviour
{
    [Header("Configurações")]
    public float velocidadeNavegacao = 500f;
    
    private RectTransform rectTransform;
    private bool estaViajando = false;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        // 1. Ao iniciar ativo, puxa a última posição salva e aplica imediatamente
        if (Game_Manager.Instance != null)
        {
            rectTransform.anchoredPosition = Game_Manager.Instance.ultimaPosicaoSalva;
        }

        // Inscreve-se no evento de viagem
        GameEvents.OnTravelRequested += IniciarViagem;
    }

    private void OnDisable()
    {
        GameEvents.OnTravelRequested -= IniciarViagem;
    }

    private void IniciarViagem(Vector2 posicaoAlvo, string nomeDaCena)
    {
        if (estaViajando) return; // Evita duplos cliques
        StartCoroutine(AnimarIconeEViajar(posicaoAlvo, nomeDaCena));
    }

    private IEnumerator AnimarIconeEViajar(Vector2 destino, string nomeDaCena)
    {
        estaViajando = true;

        // 2. Animação lenta até o botão
        while (Vector2.Distance(rectTransform.anchoredPosition, destino) > 0.5f)
        {
            rectTransform.anchoredPosition = Vector2.MoveTowards(
                rectTransform.anchoredPosition, 
                destino, 
                velocidadeNavegacao * Time.deltaTime
            );
            yield return null;
        }

        rectTransform.anchoredPosition = destino; // Crava na posição

        // 3. Manda salvar a nova posição via evento
        GameEvents.OnMapPositionSaved?.Invoke(destino);

        yield return new WaitForSeconds(0.2f); // Pausa dramática para feedback visual

        // 4. Troca de cena assim que a animação terminar
        SceneManager.LoadScene(nomeDaCena);
    }
}