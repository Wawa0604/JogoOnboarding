using UnityEngine;
using System;

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
}