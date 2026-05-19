using UnityEngine;
using UnityEditor; // Esta biblioteca permite-nos modificar o menu do Unity

public class LimparDadosEditor
{
    // Este atributo cria um novo menu no topo do Unity: "Minhas Ferramentas > Apagar Missões"
    [MenuItem("Minhas Ferramentas/Apagar Missões (PlayerPrefs)")]
    public static void ApagarPlayerPrefs()
    {
        // O comando que elimina absolutamente tudo o que guardaste no PlayerPrefs
        PlayerPrefs.DeleteAll();
        
        // Garante que a limpeza é gravada imediatamente
        PlayerPrefs.Save();
        
        // Mostra uma mensagem de confirmação na Consola do Unity
        Debug.Log("✨ Todos os dados de missões foram apagados! Podes testar novamente.");
    }
}