using UnityEngine;
using System;

public static class GameEvents 
{

    // Evento para Diálogos (passa o ID do diálogo que acabou)
    public static Action<string> OnDialogueEnded;

    // Evento para Missões (passa o ID da missão)
    public static Action<string> OnMissionCompleted;

    // evento para quizzes
    public static System.Action<QuizSequence> OnQuizRequested;

    //evento do quiz de arrasta
    public static System.Action<QuizDragSequence> OnDragQuizRequested;
}

