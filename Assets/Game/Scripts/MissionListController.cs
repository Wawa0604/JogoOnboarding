using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MissionListController : MonoBehaviour
{
    [SerializeField] private VisualTreeAsset itemTemplate; // Arraste o MissionEntry.uxml aqui
    private ListView _listView;
    private List<string> _missions = new List<string>(); // Sua lista de dados

    void OnEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        _listView = root.Q<ListView>("ListaMissoes"); // Busca o ListView no documento

        // 1. Dados de teste (Aqui você carregaria seu sistema de missões)
        _missions.Add("Encontrar com o presidente");
        _missions.Add("Explorar o laboratório");
        _missions.Add("Coletar amostras de solo");

        // 2. Setup do ListView
        _listView.makeItem = () => itemTemplate.Instantiate();

        _listView.bindItem = (VisualElement element, int index) => {
            // Aqui associamos o texto da lista ao Label do template
            var label = element.Q<Label>("MissionTaskLabel");
            label.text = _missions[index];
        };

        // 3. Passar a fonte de dados
        _listView.itemsSource = _missions;
    }
}