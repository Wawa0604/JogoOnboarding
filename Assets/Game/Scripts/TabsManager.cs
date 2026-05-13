using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;



// para que seja o primeiro rodado na cena
[DefaultExecutionOrder(-1)]

public class TabsManager : MonoBehaviour
{
     // Singleton pattern: onde só existe um tipo desse objeto na cena
    public static TabsManager Instance = null; 

    // referencia do tab ui controller para controlar o sistema de abas
    [SerializeField] private TabController tabController;
    // lista contendo as informações de cada tab
    [SerializeField] private List<TabUIData> data;
    // lista de botões de controllers
    [SerializeField] private List<CoresSprites> colorButtons = new List<CoresSprites>();

    public event Action<SlotItemData> OnBodyPartChange;
    // evento de cor
    public event Action<string, Color> OnColorChange;

    private void Awake()
    {
            Instance = this;
    }

    void Start()
    {
        // inicializar as abas
        // para isso o for na lista de tabUIData
        data.ForEach(tabUIData =>
        {
            // chamando o método de tab page passando uma instancia de tab page
           tabController.AddTabPage(new TabPage()
           {
            // passando as informações 
                icon = tabUIData.icon,
                sprites = tabUIData.sprites,
                identificador = tabUIData.identificador,
           });
        });
        // manager precisa ser avisado quando uma tab é selecionada
        // assinar o on page selected e criar o método do handle
        // que será executado quando o evento for disparado
        tabController.OnPageSelected += HandlePageSelected;
        tabController.OnSlotButtonSelected += HandleSlotButtonSelected;
        tabController.SelectTabByIndex(0);
    }

    private void OnDestroy()
    {
        // removendo a subscrição do page selected
        tabController.OnPageSelected -= HandlePageSelected;
    }

    private void HandlePageSelected(TabPage obj)
{
    Debug.Log("Tentando selecionar aba: " + obj.identificador);
    
    var currentTabUIData = data.Find(tabUIData => tabUIData.identificador == obj.identificador);
    
    if (currentTabUIData == null)
    {
        Debug.LogError("ERRO: Não encontrei nenhum TabUIData com o ID: " + obj.identificador + 
                       ". Verifique se o nome no ScriptableObject é idêntico ao da TabPage.");
        return;
    }

    Debug.Log("Dados encontrados! UseColor: " + currentTabUIData.useColor + " | Total de cores: " + currentTabUIData.colors.Count);

    for (int i = 0; i < colorButtons.Count; i++)
    {
        // Se a aba usa cor E o índice atual existe na lista de cores
        if (currentTabUIData.useColor && i < currentTabUIData.colors.Count)
        {
            colorButtons[i].gameObject.SetActive(true);
            colorButtons[i].Setup(currentTabUIData.identificador, currentTabUIData.colors[i]);
            Debug.Log("Botão " + i + " ativado com a cor: " + currentTabUIData.colors[i]);
        }
        else
        {
            colorButtons[i].gameObject.SetActive(false);
            Debug.Log("o botão" + i + "foi desativado");
        }
    }
}

    //metodo que os botoes chamam ao serem clicados
    public void NotifyColorClick(string id, Color cor)
    {
        TabsManager.Instance.OnColorChange?.Invoke(id, cor);
    }

    private void HandleSlotButtonSelected(SlotItemData obj)
    {
        TabsManager.Instance.OnBodyPartChange?.Invoke(obj);
        // ID EXATO que você escreveu no ScriptableObject da missão
        if (MissionManager.Instance != null)
        {
            MissionManager.Instance.ConcluirMissao("montar_avatar");
        }
    }

}