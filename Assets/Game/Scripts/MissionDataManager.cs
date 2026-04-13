using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class Mission 
{
    public string id;
    public string title;
    public bool isCompleted;
}

public class MissionDataManager : MonoBehaviour
{
    public static MissionDataManager Instance; // Adicionei o Singleton aqui para a UI te achar

    public List<Mission> missions = new List<Mission>();

    void Awake()
    {
        if (Instance == null) Instance = this;
        LoadMissionProgress();
    }

    public List<Mission> GetSortedMissions()
    {
        // Ordena: Não concluídas primeiro. 
        // Para as concluídas ficarem "mais recente no fim", o LINQ já mantém a ordem original da lista
        return missions.OrderBy(m => m.isCompleted).ToList();
    }

    public void CompleteMission(string id)
    {
        var mission = missions.Find(m => m.id == id);
        if (mission != null)
        {
            mission.isCompleted = true;
            PlayerPrefs.SetInt("Mission_" + id, 1);
            PlayerPrefs.Save();
        }
    }

    private void LoadMissionProgress()
    {
        foreach (var mission in missions) 
        {
            // PlayerPrefs retorna 1 ou 0, precisamos comparar para virar bool
            mission.isCompleted = PlayerPrefs.GetInt("Mission_" + mission.id, 0) == 1;
        }
    }
}