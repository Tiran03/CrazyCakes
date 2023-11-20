using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor.SceneManagement;
using TMPro;

public class Level2manager : MonoBehaviour
{
    [SerializeField] private GameObject[] pieces;
    [SerializeField] private Transform panel;
    [SerializeField] private Button[] shapeButtons; // Los botones que generan las formas
    [SerializeField] private Transform buttonPanel;
    [SerializeField] private Button checkButton;
    [SerializeField] private Button clearButton;


    private float totalTime = 60f; // Tiempo total en segundos
    private float currentTime; // Variable para verificar si el juego está en curso
    [SerializeField] private TextMeshProUGUI timerText;


    private QueueTDA<GameObject> _piecesQueue = new QueueTDA<GameObject>();
    private int successfulVerifications = 0;

    private int puntajePorTiempo = 0; // Puntaje del nivel basado en el tiempo

    private void Start()
    {
        // Asignar las funciones a los botones
        for (int i = 0; i < shapeButtons.Length; i++)
        {
            int index = i; // Captura el valor de 'i' para que sea único en cada bucle
            shapeButtons[i].onClick.AddListener(() => GenerateShape(index));
        }
        checkButton.onClick.AddListener(VerifyGame);
        clearButton.onClick.AddListener(ClearButtonPanel);

        currentTime = totalTime;

        // Actualizar el TextMeshPro con el tiempo inicial
        UpdateTimerText();
        CalcularPuntajePorTiempo();
    }



    private void Update()
    {
        // Restar tiempo en cada actualización
        currentTime -= Time.deltaTime;

        // Actualizar el TextMeshPro del temporizador
        UpdateTimerText();
        CalcularPuntajePorTiempo();


        // Verificar si el tiempo se agotó
        if (currentTime <= 0)
        {
            ScriptPersistente.instance.puntajePorTiempo += puntajePorTiempo;

            // El jugador perdió el juego, aquí puedes mostrar un mensaje de pérdida o realizar otras acciones
            LoadingManager.Instance.LoadScene(4, 6);
        }
        
    }


    //public void DequeueAndEnqueue()
    //{
    //    if (QueueIsEmpty())
    //    {
    //        DeQueuePiece();
    //        EnqueuePiece();
    //    }
    //}

    //private void DeQueuePiece()
    //{
    //    if (QueueIsEmpty())
    //    {
    //        var obj = _piecesQueue.Dequeue();
    //        print(obj.name);
    //        Destroy(obj);
    //    }
    //}

    private void EnqueuePiece()
    {
        
        GameObject obj = Instantiate(pieces[Random.Range(0, pieces.Length)], this.panel);
        _piecesQueue.Enqueue(obj);
    }

    public void GenerateQueue()
    {
        foreach (Transform child in panel)
        {
            Destroy(child.gameObject);
            _piecesQueue.Clear();
        }
        for (int i = 0; i < 5; i++)
        {
            EnqueuePiece();
        }
    }

    

    // Generar una forma específica cuando se hace clic en un botón
    private void GenerateShape(int shapeIndex)
    {
        if (shapeIndex >= 0 && shapeIndex < pieces.Length)
        {
            // Eliminar cualquier forma actual en el panel de botones
            //foreach (Transform child in buttonPanel)
            //{
            //    Destroy(child.gameObject);
            //}

            // Generar la forma seleccionada en el panel de botones
            GameObject obj = Instantiate(pieces[shapeIndex], this.buttonPanel);
            //_piecesQueue.Clear(); // Limpiar la cola
        }
    }

    private void VerifyGame()
    {
        // Obtener los objetos en el panel de botones
        List<GameObject> buttonObjects = new List<GameObject>();
        foreach (Transform child in buttonPanel)
        {
            buttonObjects.Add(child.gameObject);
        }

        // Verificar si hay objetos en ambos paneles
        if (_piecesQueue.Count > 0 && buttonObjects.Count > 0)
        {
            // Verificar si los objetos son iguales en tipo y orden
            bool gameWon = true;
            for (int i = 0; i < buttonObjects.Count; i++)
            {
                if (i >= _piecesQueue.Count || buttonObjects[i].name != _piecesQueue.ElementAt(i).name)
                {
                    gameWon = false;
                    break;
                }
            }

            if (gameWon)
            {
                // Si la verificación fue exitosa, aumentar el contador
                successfulVerifications++;
                GenerateQueue();

                // Comprobar si el jugador ha verificado dos veces con éxito
                if (successfulVerifications >= 2)
                {
                    // El jugador ganó el juego después de dos verificaciones exitosas
                    Debug.Log("¡Ganaste el juego!");
                    SumarPuntosGanados();
                    LoadingManager.Instance.LoadScene(4, 5);
                }
                else
                {
                    // El jugador aún no ha ganado, puedes mostrar un mensaje de progreso
                    Debug.Log("Verificación exitosa. Verifica una vez más para ganar.");
                }
            }
            else
            {
                // La verificación falló, restablecer el contador
                successfulVerifications = 0;

                // El jugador no ganó, puedes mostrar un mensaje de fallo o realizar otras acciones
                Debug.Log("Verificación fallida. Sigue intentando.");
            }
        }
        else
        {
            // Si no hay objetos en ambos paneles, muestra un mensaje de advertencia
            Debug.Log("Debes tener objetos en ambos paneles para verificar.");
        }
    }

    private void ClearButtonPanel()
    {
        // Eliminar todos los objetos hijos del panel de botones
        foreach (Transform child in buttonPanel)
        {
            Destroy(child.gameObject);
        }
    }

    private void UpdateTimerText()
    {
        // Mostrar el tiempo restante en el TextMeshPro
        timerText.text = "Tiempo: " + Mathf.CeilToInt(currentTime).ToString();
    }

    private void SumarPuntosGanados()
    {
        // Suma el puntaje por tiempo al puntaje total
        ScriptPersistente.instance.puntajeTotal += puntajePorTiempo;
    }



    private void CalcularPuntajePorTiempo()
    {
        // Puntaje directamente relacionado con el tiempo restante
        puntajePorTiempo = Mathf.RoundToInt(currentTime);
    }
}

