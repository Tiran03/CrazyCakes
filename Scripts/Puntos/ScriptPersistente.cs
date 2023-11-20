using UnityEngine;

public class ScriptPersistente : MonoBehaviour
{
    public static ScriptPersistente instance;
    public int puntajeTotal = 0;
    public int puntajePorTiempo = 0; // Nuevo campo para el puntaje por tiempo

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}

