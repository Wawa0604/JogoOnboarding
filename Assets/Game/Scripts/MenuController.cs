using UnityEngine;
using UnityEngine.UIElements; // Essencial para UI Toolkit
using UnityEngine.SceneManagement;
using System.Collections;

public class MenuController : MonoBehaviour
{
    private UIDocument _uiDocument;
    
    // Elementos da UI
    private TextField _txtUsuario;
    private TextField _txtSenha;
    private Button _btnEntrar;
    private VisualElement _loginContainer;
    private VisualElement _loadingContainer;
    private ProgressBar _progressBar;

    [Header("Configurações de Navegação")]
    [Tooltip("O número da cena do jogo no Build Settings")]
    [SerializeField] private int _gameSceneIndex = 1; 

    private void OnEnable()
    {
        // 1. Referenciando a Raiz da UI
        _uiDocument = GetComponent<UIDocument>();
        VisualElement root = _uiDocument.rootVisualElement;

        // 2. Buscando elementos pelos IDs (#) que você definiu
        _txtUsuario = root.Q<TextField>("usuario-input");
        _txtSenha = root.Q<TextField>("senha-input");
        _btnEntrar = root.Q<Button>("btn-entrar");
        
        // Buscando os containers para alternar visibilidade
        _loginContainer = root.Q<VisualElement>("Container");
        _loadingContainer = root.Q<VisualElement>("LoadingContainer");
        
        // Buscando a ProgressBar (como só tem uma, o tipo basta)
        _progressBar = root.Q<ProgressBar>();

        // 3. Registrando o evento de clique
        if (_btnEntrar != null)
        {
            _btnEntrar.clicked += OnLoginClick;
        }
    }

    private void OnDisable()
    {
        // Boa prática Sênior: sempre desinscrever eventos ao desativar o objeto
        if (_btnEntrar != null)
        {
            _btnEntrar.clicked -= OnLoginClick;
        }
    }

    private void OnLoginClick()
    {
        // Validação simples de preenchimento
        if (string.IsNullOrEmpty(_txtUsuario.value) || string.IsNullOrEmpty(_txtSenha.value))
        {
            Debug.LogWarning("Por favor, preencha o login e a senha.");
            return;
        }

        // Se passar na validação, inicia o carregamento
        StartCoroutine(LoadGameAsync());
    }

    private IEnumerator LoadGameAsync()
    {
        // 1. Troca de telas: Esconde login, mostra loading
        if (_loginContainer != null) _loginContainer.style.display = DisplayStyle.None;
        if (_loadingContainer != null) _loadingContainer.style.display = DisplayStyle.Flex;

        // 2. Carregamento Assíncrono pelo ÍNDICE
        AsyncOperation operation = SceneManager.LoadSceneAsync(_gameSceneIndex);
        
        // Impede a ativação automática para garantir que a barra chegue a 100%
        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            // Normalizando o progresso (0.9f é o máximo antes da ativação)
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            
            if (_progressBar != null)
            {
                _progressBar.value = progress * 100f; // UI Toolkit ProgressBar usa 0-100
                _progressBar.title = $"Carregando: {Mathf.RoundToInt(progress * 100)}%";
            }

            // Se o carregamento terminou na memória (0.9)
            if (operation.progress >= 0.9f)
            {
                // Delay de 1 segundo para o jogador ver que completou
                yield return new WaitForSeconds(1f);
                operation.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}