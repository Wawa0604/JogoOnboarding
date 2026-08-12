using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class DialogueController : MonoBehaviour
{
    [Header("Controle Físico do Painel (Manual por Cena)")]
    [SerializeField] private GameObject painelDeDialogoManual; 

    [Header("Referências de UI Locais")]
    [SerializeField] private InteractionUI ui;
    
    private DialogueSequence sequence;
    private int index;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    private void Start()
    {
        if (ui != null && ui.GetAudioButton() != null)
        {
            ui.GetAudioButton().onClick.AddListener(ToggleAudio);
        }
    }

    public void StartDialogue(DialogueSequence newSequence)
    {
        if (newSequence == null) return;
        sequence = newSequence;
        index = 0;

        if (painelDeDialogoManual != null)
        {
            painelDeDialogoManual.SetActive(true);
        }

        if (ui != null) 
        {
            ui.Show();
        }
        
        UpdateUI();
    }

    public void Next()
    { 
        if (sequence == null) return;

        StopAudio();

        if (index < sequence.lines.Length - 1)
        {
            index++;
            UpdateUI();
        }
        else
        {
            EndDialogue();
        }
    }

    public void Previous()
    {
        if (index > 0)
        {
            StopAudio();
            index--;
            UpdateUI();
        }
    }

    private void UpdateUI()
    {
        if (ui == null || sequence == null) return;
        
        DialogueLine line = sequence.lines[index];
        ui.SetDialogue(line.characterName, line.text, sequence.iconeAvatar);

        bool hasPrevious = index > 0; 
        bool hasNext = true;         

        ui.SetButtonState(hasPrevious, hasNext);

        // Configura o áudio atual
        if (line.audioClip != null)
        {
            audioSource.clip = line.audioClip;
            PlayAudio(); // Toca automaticamente ao mudar de frase (opcional, remova se quiser só no clique)
        }
        else
        {
            audioSource.clip = null;
            ui.SetAudioButtonState(false, false);
        }
    }

    // Alterna entre Play e Pause
    public void ToggleAudio()
    {
        if (audioSource.clip == null) return;

        if (audioSource.isPlaying)
        {
            audioSource.Pause();
            ui.SetAudioButtonState(true, false);
        }
        else
        {
            audioSource.Play();
            ui.SetAudioButtonState(true, true);
        }
    }

    private void PlayAudio()
    {
        if (audioSource.clip != null)
        {
            audioSource.Play();
            ui.SetAudioButtonState(true, true);
        }
    }

    private void StopAudio()
    {
        if (audioSource != null)
        {
            audioSource.Stop();
        }
    }

    private void Update()
    {
        // Garante que o ícone do botão volte para "Play" quando o áudio terminar sozinho
        if (audioSource != null && audioSource.clip != null && !audioSource.isPlaying && ui != null)
        {
            if (audioSource.time == 0 || audioSource.time >= audioSource.clip.length)
            {
                ui.SetAudioButtonState(true, false);
            }
        }
    }

    public void EndDialogue()
    {
        StopAudio();

        if (ui != null) ui.Hide();

        if (painelDeDialogoManual != null)
        {
            painelDeDialogoManual.SetActive(false);
        }

        if (sequence != null)
        {
            GameEvents.OnDialogueEnded?.Invoke(sequence.id); 
            sequence.OnSequenceComplete?.Invoke();

            if (sequence.missaoParaConcluir != null && MissionManager.Instance != null)
            {
                MissionManager.Instance.ConcluirMissao(sequence.missaoParaConcluir.id);
            }

            if (sequence.quizParaIniciar != null)
            {
                Debug.Log($"[SISTEMA UNIFICADO] Diálogo concluído. Iniciando a sequência de quiz: {sequence.quizParaIniciar.id}");
                GameEvents.OnQuizRequested?.Invoke(sequence.quizParaIniciar);
            }
        }
    }
}