using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor.SceneManagement;
using TMPro;

public class Level1manager : MonoBehaviour
{
    public Disc[] GameDiscs => _discs;
    [SerializeField] private Disc[] _discs;
    private Disc _currentSelectedDisc;
    private Tower _currentSelectedDiscTowerOwner;

    public static Level1manager Instance;

    public float gameTimer = 40.0f; // El tiempo en segundos
    private bool isGameRunning = true; // Variable para verificar si el juego está en curso
    public TextMeshProUGUI timerText;


    private void Awake()
    {
        Instance = this;
    }

    private int puntajePorTiempo = 0; // Puntaje del nivel basado en el tiempo

    private void Start()
    {
        isGameRunning = true;
        gameTimer = 40.0f;
        timerText.text = "Tiempo: " + gameTimer.ToString("F1");

        // Inicializa el puntaje por tiempo
        CalcularPuntajePorTiempo();
    }

    void Update()
    {
        if (isGameRunning)
        {
            gameTimer -= Time.deltaTime;

            // Actualiza el texto del temporizador en el objeto de texto
            timerText.text = "Tiempo: " + Mathf.Max(0, gameTimer).ToString("F1"); // Muestra el tiempo con un decimal

            // Actualiza el puntaje por tiempo en cada fotograma
            CalcularPuntajePorTiempo();

            if (gameTimer <= 0)
            {
                isGameRunning = false;

                // Carga la escena de pérdida
                LoadingManager.Instance.LoadScene(2, 6);
            }
            
        }
    }


    public void OnDiscClick(Disc clickedDisc)
    {
        _currentSelectedDisc = clickedDisc;
        _currentSelectedDiscTowerOwner = clickedDisc.TowerOwner;
        print("click" + clickedDisc.name);
    }

    public void OnTowerClick(Tower clickedTower)
    {
        print("click" + clickedTower.name);

        if (_currentSelectedDisc == null)
        {
            print("No hay Disco seleccionado");
        }
        else
        {
            clickedTower.TryPlaceDiscInNewTower(_currentSelectedDiscTowerOwner, _currentSelectedDisc);

            // Verificar si se ha ganado el juego después de cada movimiento
            if (CheckVictoryCondition())
            {
                // Aquí puedes mostrar un mensaje de victoria o realizar otras acciones
                print("¡Has ganado el juego!");
                SumarPuntosGanados();
                LoadingManager.Instance.LoadScene(2, 3);
            }
        }
    }

    // Método para verificar si se ha ganado el juego
    //private bool CheckVictoryCondition()
    //{
    //    foreach (var tower in FindObjectsOfType<Tower>())
    //    {
    //        if (!IsTowerOrderedBySize(tower))
    //        {
    //            print("Todavia no");
    //            return false; // Si alguna torre no está ordenada, el juego no se ha ganado
    //        }
    //    }

    //    return true; // Si todas las torres están ordenadas, el juego se ha ganado
    //}


    private bool CheckVictoryCondition()
    {
        Dictionary<string, List<Tower>> towersByType = new Dictionary<string, List<Tower>>();

        foreach (var tower in FindObjectsOfType<Tower>())
        {
            if (tower.isStarterTower)
            {
                if (tower.towerStack.Count > 0)
                {
                    return false;
                }
            }
            else
            {
                if (!IsTowerOrderedBySize(tower))
                {
                    print("Todavia no");
                    return false;
                }

                // Verificar si el tipo de objeto requerido está presente en la torre
                if (tower.RequiredObjectType != null)
                {
                    bool hasRequiredObject = false;

                    foreach (Disc disc in tower.towerStack)
                    {
                        // Accede a los elementos de la pila usando el enumerador personalizado
                        // Esto supone que tower.towerStack implementa un método GetEnumerator()
                        if (disc.DiscType == tower.RequiredObjectType)
                        {
                            hasRequiredObject = true;
                            break;
                        }
                    }

                    if (!hasRequiredObject)
                    {
                        return false; // El tipo de objeto requerido no está presente en la torre
                    }
                }

                foreach (Disc disc in tower.towerStack)
                {
                    if (!towersByType.ContainsKey(disc.DiscType))
                    {
                        towersByType[disc.DiscType] = new List<Tower>();
                    }
                    towersByType[disc.DiscType].Add(tower);
                }
            }
        }

        foreach (var type in towersByType.Keys)
        {
            List<Tower> towers = towersByType[type];

            for (int i = 1; i < towers.Count; i++)
            {
                if (towers[i] != towers[0])
                {
                    return false;
                }
            }

            foreach (var otherType in towersByType.Keys)
            {
                if (otherType != type)
                {
                    List<Tower> otherTowers = towersByType[otherType];
                    foreach (var otherTower in otherTowers)
                    {
                        if (otherTower == towers[0])
                        {
                            return false;
                        }
                    }
                }
            }
        }

        return true;
    }



    // Método para verificar si una torre está ordenada por tamaño
    private bool IsTowerOrderedBySize(Tower tower)
    {
        PilaTDA<Disc> stack = tower.towerStack;
        string prevType = null; // Inicializar prevType con null

        foreach (Disc disc in stack)
        {
            if (prevType != null && disc.DiscType != prevType)
            {
                return false; // Los discos de diferentes tipos de objetos no están ordenados
            }
            prevType = disc.DiscType;
        }

        return true; // Todos los discos están ordenados por tipo de objeto
    }


    private void SumarPuntosGanados()
    {
        // Suma el puntaje por tiempo al puntaje total
        ScriptPersistente.instance.puntajeTotal += puntajePorTiempo;
    }



    private void CalcularPuntajePorTiempo()
    {
        // Puntaje directamente relacionado con el tiempo restante
        puntajePorTiempo = Mathf.RoundToInt(gameTimer);
    }
}
