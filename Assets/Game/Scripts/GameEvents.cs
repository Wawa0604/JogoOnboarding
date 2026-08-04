using UnityEngine;
using System;
using System.Collections.Generic; // <- Faltava esta linha para o Dictionary funcionar

public static class GameEvents 
{
    // Evento para Diálogos (passa o ID do diálogo que acabou)
    public static Action<string> OnDialogueEnded;

    // Evento para Missões (passa o ID da missão)
    public static Action<string> OnMissionCompleted;

    // PORTAL ÚNICO: O único evento necessário para iniciar QUALQUER quiz (Misto, Arrastar ou Regular)
    public static Action<QuizSequence> OnQuizRequested;
 
    // Evento dedicado para o botão "Jogar Novamente" avisar o QuizManager
    public static Action OnRestartDragQuizRequested;

    //Avisa que o quiz foi concluído com sucesso
    public static Action<string> OnQuizCompletedSuccessfully;

    //item coletado
    public static Action<string> OnItemCollected;

    // Evento disparado quando um botão é clicado (passa o destino e a cena alvo)
    public static Action<Vector2, string> OnTravelRequested;
    
    // Evento para atualizar e salvar as coordenadas finais
    public static Action<Vector2> OnMapPositionSaved;

    // Evento disparado quando o painel de avatar é salvo e fechado.
    // Dicionário 1: "identificador" -> itemIndex
    // Dicionário 2: "identificador" -> Color
    public static Action<Dictionary<string, int>, Dictionary<string, Color>> OnAvatarSaved;
}