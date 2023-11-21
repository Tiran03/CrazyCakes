using System.Collections;
using UnityEngine;
using TMPro;

public class CandySortingGame : MonoBehaviour
{
    public Transform[] candyPositions;
    //public GameObject correctParticle;
    //public GameObject nextLevelPortal;

    private IQuickSort<string> quickSort;

    private string[] flavors = { "Chocolate", "Vainilla", "Fresa", "Caramelo", "Limon" };
    private int currentIndex = 0;
    public int correctCandies = 0;

    public float gameTimer = 40.0f; // El tiempo en segundos
    private bool isGameRunning = true; // Variable para verificar si el juego está en curso
    public TextMeshProUGUI timerText;

    private int puntajePorTiempo = 0; // Puntaje del nivel basado en el tiempo

    void Start()
    {
        quickSort = new QuickSort<string>();
        ShuffleCandies();
        isGameRunning = true;
        gameTimer = 40.0f;
        timerText.text = "Tiempo: " + gameTimer.ToString("F1");
        CalcularPuntajePorTiempo();

    }

    void Update()
    {
        if (currentIndex < flavors.Length)
        {
            QuicksortCandies(0, flavors.Length - 1, flavors[currentIndex]);
        }


        if (correctCandies == 5)
        {
            Debug.Log("Ganaste");
            SumarPuntosGanados();

            LoadingManager.Instance.LoadScene(6, 7);

        }
        else
        {
            // Verifica si todos los dulces están en su posición correcta antes de declarar la victoria
            bool allCandiesCorrect = CheckAllCandiesCorrect();

           



            if (allCandiesCorrect)
            {
                Debug.Log("Ganaste");
                // Activa aquí las partículas de éxito y el portal al siguiente nivel si es necesario
                // correctParticle.SetActive(true);
                // nextLevelPortal.SetActive(true);
                SumarPuntosGanados();

                LoadingManager.Instance.LoadScene(6, 7);
            }
            else
            {
                Debug.Log("Aún no todos los caramelos están en su posición correcta.");
            }
        }



        if (isGameRunning)
        {
            gameTimer -= Time.deltaTime;

            // Actualiza el texto del temporizador en el objeto de texto
            timerText.text = "Tiempo: " + Mathf.Max(0, gameTimer).ToString("F1"); // Muestra el tiempo con un decimal

            if (gameTimer <= 0)
            {
                isGameRunning = false;
                LoadingManager.Instance.LoadScene(2, 6);
            }
        }
        CalcularPuntajePorTiempo();

    }

    void ShuffleCandies()
    {
        for (int i = 0; i < candyPositions.Length; i++)
        {
            int randomIndex = Random.Range(i, candyPositions.Length);
            Vector3 tempPosition = candyPositions[i].position;
            candyPositions[i].position = candyPositions[randomIndex].position;
            candyPositions[randomIndex].position = tempPosition;
        }
    }

    void QuicksortCandies(int left, int right, string targetFlavor)
    {
        if (left < right)
        {
            int pivotIndex = Partition(left, right, targetFlavor);

            if (flavors[pivotIndex] == targetFlavor)
            {
                currentIndex++;
            }

            QuicksortCandies(left, pivotIndex - 1, targetFlavor);
            QuicksortCandies(pivotIndex + 1, right, targetFlavor);
        }
    }

    int Partition(int left, int right, string targetFlavor)
    {
        string pivot = flavors[right];
        int i = left - 1;

        for (int j = left; j < right; j++)
        {
            if (string.Compare(flavors[j], targetFlavor) < 0)
            {
                i++;
                SwapCandies(i, j);
            }
        }

        SwapCandies(i + 1, right);
        return i + 1;
    }

    void SwapCandies(int indexA, int indexB)
    {
        if (indexA >= 0 && indexA < flavors.Length && indexB >= 0 && indexB < flavors.Length)
        {
            string tempFlavor = flavors[indexA];
            flavors[indexA] = flavors[indexB];
            flavors[indexB] = tempFlavor;
        }
    }

    public void CandyPlacedCorrectly(DragAndDrop candy)
    {
        if (candy.isCorrect)
        {
            correctCandies++;
            Debug.Log("Candy placed correctly!");
        }
        else
        {
            Debug.Log("Candy placed incorrectly!");
        }
    }

    bool CheckAllCandiesCorrect()
    {
        foreach (Transform position in candyPositions)
        {
            // Obtiene el componente DragAndDrop del caramelo en la posición actual
            DragAndDrop candy = position.GetComponentInChildren<DragAndDrop>();

            // Verifica si el caramelo está en la posición correcta y si no, devuelve falso
            if (candy == null || !candy.isCorrect)
            {
                return false;
            }
        }
        // Si todos los dulces están en su posición correcta, devuelve verdadero
        return true;
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
