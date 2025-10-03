using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BSelection : MonoBehaviour
{
    private Button _button;
    private ArrayManager _arrayManager;
    private SortTimer _timer;

    private void Start()
    {
        _arrayManager = FindFirstObjectByType<ArrayManager>();

        _button = GetComponent<Button>();
        _button.onClick.AddListener(() => StartCoroutine(SelectionSort()));

        _timer = FindFirstObjectByType<SortTimer>();
        _timer.AddButton(_button);
    }

    private IEnumerator SelectionSort()
    {
        _timer.StartTimer();
        _arrayManager.CreateArray(true); //crear un array random
        Element[] array = _arrayManager.Array;

        // Todos grises al inicio
        foreach (var e in array) e.ResetColor();

        for (int i = 0; i < array.Length - 1; i++)
        {
            int minIdx = i;

            // rojo: candidato actual
            array[i].HighlightRed();
            yield return new WaitForSeconds(0.3f);

            for (int j = i + 1; j < array.Length; j++)
            {
                // el anterior comparado vuelve a gris (excepto el i)
                if (j > i + 1)
                    array[j - 1].ResetColor();

                // verde: elemento que se está comparando ahora
                array[j].HighlightGreen();
                yield return new WaitForSeconds(0.3f);

                // si es nuevo mínimo, actualizar índice
                if (array[j].transform.localScale.x < array[minIdx].transform.localScale.x)
                {
                    minIdx = j;
                }
            }

            // resetear el último verde del recorrido si no es el min
            if (array[array.Length - 1] != array[minIdx])
                array[array.Length - 1].ResetColor();

            // si hay swap, animarlo
            if (minIdx != i)
            {
                Vector3 posI = array[i].transform.localPosition;
                Vector3 posMin = array[minIdx].transform.localPosition;

                array[i].MoveTo(posMin);
                array[minIdx].MoveTo(posI);

                // espera la animación 
                yield return new WaitForSeconds(1f);

                _arrayManager.Swap(ref array[minIdx], ref array[i]);
                _arrayManager.ShowArray();

                array[minIdx].ResetColor();
            }

            // azul: elemento ya en su posición definitiva
            array[i].HighlightBlue();
            yield return new WaitForSeconds(0.3f);
        }

        // último también azul
        array[array.Length - 1].HighlightBlue();
        _timer.StopTimer();
    }
}
