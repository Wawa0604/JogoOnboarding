using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MissionListController : MonoBehaviour
{
    private UIDocument _uiDocument;
    private VisualElement _wrapper;
    private ListView _listView;
    private Button _button;

    private void OnEnable()
{
    _uiDocument = GetComponent<UIDocument>();
    var root = _uiDocument.rootVisualElement;

    // Nomes corrigidos de acordo com o seu UXML
    _button = root.Q<Button>("Missions");
    _listView = root.Q<ListView>("ListView");
    // O Wraper não precisa ser escondido, pois ele contém o botão que precisamos clicar!

    if (_button != null)
    {
        _button.clicked += () => {
            // Inverte o display apenas da LISTA
            if (_listView.style.display == DisplayStyle.Flex) {
                _listView.style.display = DisplayStyle.None;
            } else {
                _listView.itemsSource = MissionDataManager.Instance.GetSortedMissions();
                _listView.Rebuild();
                _listView.style.display = DisplayStyle.Flex;
            }
        };
    }
}}