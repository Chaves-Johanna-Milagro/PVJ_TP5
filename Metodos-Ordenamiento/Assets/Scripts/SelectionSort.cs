using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using TMPro;

public class SelectionSort : MonoBehaviour
{
    [SerializeField] private Button buttonSort = null;
    [SerializeField] private Button buttonShuffle = null;
    [SerializeField] private Button buttonRandom = null;
    [SerializeField] private TMP_Text txtTime = null;
    [SerializeField] private Element elementPrefab; // Prefab asignado en el Inspector

    private Element[] array = new Element[10];

    private float posX = -7.8f;
    private float posY = 2.15f;
    private const float offsetScale = 1.4f;

    private bool clickTwoElements = false;
    private int numElementsMoving = 0;

    private bool timerSorting = false;
    private byte firstClickedElementID;

    private void OnEnable()
    {
        Element.elementClick += ClickElementEvent;
        Element.elementMove += OnElementMove;
    }

    private void Start()
    {
        InitArray();
        ShowArray();

        buttonSort.onClick.AddListener(SortArray);
        buttonRandom.onClick.AddListener(() => InitArray(true));
        buttonShuffle.onClick.AddListener(ShuffleArray);
    }

    /// <summary>
    /// Inicializa el array, ordenado o aleatorio
    /// </summary>
    private void InitArray(bool random = false)
    {
        float elementPosX = posX;
        System.Random rnd = new System.Random();

        for (int i = 0; i < array.Length; i++)
        {
            if (array[i] != null) Destroy(array[i].gameObject);

            array[i] = Instantiate(elementPrefab, new Vector3(elementPosX, posY, 0f), Quaternion.identity);

            float scaleValue = random ? rnd.Next(1, 9) : (i + 1);
            array[i].transform.localScale = new Vector3(scaleValue, scaleValue, 1f);

            // Color inicial gris
            Color c = new Color(i / 10f, i / 10f, i / 10f);
            array[i].InitColor = c;

            elementPosX += offsetScale;
        }

        ShowArray();
    }

    /// <summary>
    /// Reacomoda la posición de los elementos en pantalla
    /// </summary>
    private void ShowArray()
    {
        float elementPosX = posX;
        float offsetPosX = Math.Abs(posX * 2) / (array.Length - 1);

        for (int i = 0; i < array.Length; i++)
        {
            array[i].transform.localPosition = new Vector3(elementPosX, posY, 0f);
            elementPosX += offsetPosX;
        }
    }

    /// <summary>
    /// Intercambia dos elementos en el array lógico
    /// </summary>
    private void SwapElementsArray(ref Element element1, ref Element element2)
    {
        byte tempID = element1.elementID;
        Element temp = element1;

        element1.elementID = element2.elementID;
        element1 = element2;

        element2.elementID = tempID;
        element2 = temp;
    }

    /// <summary>
    /// Desordena aleatoriamente los elementos
    /// </summary>
    private void ShuffleArray()
    {
        if (numElementsMoving != 0) return;

        System.Random rnd = new System.Random();
        int n = array.Length;
        while (n > 1)
        {
            int k = rnd.Next(n--);
            SwapElementsArray(ref array[n], ref array[k]);
        }

        ShowArray();
    }

    private void SortArray()
    {
        StartCoroutine(StartSortingTimer());
        StartCoroutine(SelectionSortArray());
    }

    /// <summary>
    /// Controla si los elementos se están moviendo para bloquear botones
    /// </summary>
    private void OnElementMove(byte elementID, bool isMoving)
    {
        numElementsMoving = isMoving ? ++numElementsMoving : --numElementsMoving;

        if (!timerSorting)
        {
            bool interactable = (numElementsMoving == 0);
            buttonRandom.interactable = interactable;
            buttonShuffle.interactable = interactable;
            buttonSort.interactable = interactable;

            if (interactable) ShowArray();
        }
    }

    /// <summary>
    /// Maneja los clics en elementos para intercambio manual
    /// </summary>
    private void ClickElementEvent(byte elementID)
    {
        if (numElementsMoving != 0 || timerSorting) return;

        if (!clickTwoElements)
        {
            // Primer clic
            firstClickedElementID = elementID;
            clickTwoElements = true;
            array[elementID].HighlightRed();
        }
        else
        {
            // Segundo clic
            clickTwoElements = false;

            if (firstClickedElementID != elementID)
            {
                // Intercambio visual
                array[firstClickedElementID].MoveTo(array[elementID].transform.localPosition);
                array[elementID].MoveTo(array[firstClickedElementID].transform.localPosition);

                StartCoroutine(WaitAndSwap(firstClickedElementID, elementID));
            }
            else
            {
                array[elementID].ResetColor();
            }
        }
    }

    private IEnumerator WaitAndSwap(int id1, int id2)
    {
        while (numElementsMoving != 0) yield return null;

        array[id1].ResetColor();
        array[id2].ResetColor();

        SwapElementsArray(ref array[id1], ref array[id2]);
        ShowArray();
    }

    private void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        float milliSeconds = Mathf.FloorToInt((timeToDisplay * 100) % 100);

        txtTime.text = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, milliSeconds);

        if (minutes == 10) timerSorting = false;
    }

    private IEnumerator StartSortingTimer()
    {
        timerSorting = true;
        float timeSorting = 0;
        while (timerSorting)
        {
            timeSorting += Time.deltaTime;
            DisplayTime(timeSorting);
            yield return null;
        }
    }

    /// <summary>
    /// Algoritmo Selection Sort con visualización y coloreado
    /// </summary>
    private IEnumerator SelectionSortArray()
    {
        buttonRandom.interactable = false;
        buttonShuffle.interactable = false;
        buttonSort.interactable = false;

        // Al inicio → todo gris
        for (int j = 0; j < array.Length; j++)
            array[j].ResetColor();

        for (int i = 0; i < array.Length - 1; i++)
        {
            int min_idx = i;

            // Marcar el primer elemento del recorrido como rojo
            array[i].HighlightRed();
            yield return new WaitForSeconds(0.3f);

            for (int j = i + 1; j < array.Length; j++)
            {
                // El anterior vuelve a gris (excepto el i en rojo)
                if (j > i + 1)
                    array[j - 1].ResetColor();

                // Marcar el que se está comparando como verde
                array[j].HighlightGreen();
                yield return new WaitForSeconds(0.3f);

                // Verificar si es el nuevo menor
                if (array[j].transform.localScale.x < array[min_idx].transform.localScale.x)
                {
                    min_idx = j;
                }
            }

            // Resetear el último verde del recorrido antes del swap
            if (array[array.Length - 1] != array[min_idx])
                array[array.Length - 1].ResetColor();

            if (min_idx != i)
            {
                // Intercambiar posiciones visuales
                Vector3 posI = array[i].transform.localPosition;
                Vector3 posMin = array[min_idx].transform.localPosition;

                array[i].MoveTo(posMin);
                array[min_idx].MoveTo(posI);

                while (numElementsMoving != 0)
                    yield return null;

                SwapElementsArray(ref array[min_idx], ref array[i]);
                ShowArray();
            }

            // El elemento definitivo queda azul
            array[i].HighlightBlue();
            yield return new WaitForSeconds(0.3f);

            // Resetear todos los demás (excepto los ya azules)
            for (int j = i + 1; j < array.Length; j++)
                array[j].ResetColor();
        }

        // Último elemento también azul
        array[array.Length - 1].HighlightBlue();

        buttonRandom.interactable = true;
        buttonShuffle.interactable = true;
        buttonSort.interactable = true;

        timerSorting = false;
    }
}