using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MissionListController : MonoBehaviour
{
    // Esse script está na UI do jogo, que é um prefab

    // Esse script é para ligar um elemento que começa desligado e que é um prefab

    //Quando aperta o botão <BtnMissions> liga o elemento <ListaMissoes> 

    //O <ListaMissoes> deve usar <StyleSheetMissionEntry> como formato padrão para os elementos da lista

    // Precisa, quando carrega a cena, verificar quais missões não foram concluidas e coloca-las no topo, 
    // seguindo a ordemque elas foram arrumadas no array

    //As missões que ja foram concluídas precisam estar no fim da lista. A mais em baixo sendo a primeira a ser concluída. 
    // E todas elas com o "ctrl S" ligado (quando fica riscada no meio)

}