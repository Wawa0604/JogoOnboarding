using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MissionListController : MonoBehaviour
{
    private UIDocument _uiDocument;
    private ListView _listView;
    private Button _button;

    private void OnEnable()
    {
        _uiDocument = GetComponent<UIDocument>();
        var root = _uiDocument.rootVisualElement;

        _button = root.Q<Button>("Missions");
        _listView = root.Q<ListView>("ListView");

        if (_listView != null)
        {
            // Como o Item Template já está no UI Builder, não precisamos de makeItem.
            // Precisamos apenas do bindItem para conectar os dados ao visual.
            _listView.bindItem = (VisualElement element, int index) => 
            {
                // 1. Pega os dados ordenados do Manager
                var missions = MissionDataManager.Instance.GetSortedMissions();
                
                // Segurança: verifica se o índice existe na lista
                if (index < missions.Count)
                {
                    var mission = missions[index];

                    // 2. Busca o Label dentro do seu StyleSheetMissionEntry
                    // Se o seu Label tiver um nome no UI Builder, use element.Q<Label>("Nome")
                    var label = element.Q<Label>(); 
                    
                    if (label != null)
                    {
                        label.text = mission.title;
                        
                        // 3. Aplica o estilo visual de acordo com o status
                        label.style.color = mission.isCompleted ? Color.gray : Color.white;
                        label.style.unityFontStyleAndWeight = mission.isCompleted ? FontStyle.Italic : FontStyle.Normal;
                    }
                }
            };
        }

        // Configuração do botão para abrir/fechar e atualizar a lista
        if (_button != null)
        {
            _button.clicked += () => {
                if (_listView.style.display == DisplayStyle.Flex) 
                {
                    _listView.style.display = DisplayStyle.None;
                } 
                else 
                {
                    // Fornece a lista de dados para o ListView
                    _listView.itemsSource = MissionDataManager.Instance.GetSortedMissions();
                    
                    // Força a atualização visual
                    _listView.Rebuild();
                    
                    // Mostra a lista
                    _listView.style.display = DisplayStyle.Flex;
                }
            };
        }
    }
}