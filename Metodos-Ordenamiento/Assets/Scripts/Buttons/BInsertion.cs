using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BInsertion : MonoBehaviour
{
    private Button _button;
    private ArrayManager _arrayManager;
    private SortTimer _timer;

    private void Start()
    {
        _arrayManager = FindFirstObjectByType<ArrayManager>();

        _button = GetComponent<Button>();
        _button.onClick.AddListener(() => StartCoroutine(InsertionSort()));

        _timer = FindFirstObjectByType<SortTimer>();
        _timer.AddButton(_button);
    }

    private IEnumerator InsertionSort()
    {
        _timer.StartTimer();
        Element[] array = _arrayManager.Array;

        // Al inicio todos grises
        foreach (var e in array) e.ResetColor();

        for (int i = 1; i < array.Length; i++)
        {
            // marcar el elemento elegido como rojo
            array[i].HighlightRed();
            yield return new WaitForSeconds(0.3f);

            int j = i;

            while (j > 0 && array[j - 1].transform.localScale.x > array[j].transform.localScale.x)
            {
                // marcar comparados como verdes
                array[j].HighlightGreen();
                array[j - 1].HighlightGreen();
                yield return new WaitForSeconds(0.3f);

                // intercambiar visualmente
                Vector3 posJ = array[j].transform.localPosition;
                Vector3 posPrev = array[j - 1].transform.localPosition;

                array[j].MoveTo(posPrev);
                array[j - 1].MoveTo(posJ);

                yield return new WaitForSeconds(0.5f);

                _arrayManager.Swap(ref array[j - 1], ref array[j]);
                _arrayManager.ShowArray();

                // resetear colores después de mover (vuelven a gris)
                array[j].ResetColor();
                array[j - 1].ResetColor();

                j--;
            }

            // el elegido vuelve a gris si no quedó moviéndose
            array[j].ResetColor();
        }

        // aquí al final, cuando ya está ordenado,
        // se pintan de azul uno por uno con efecto
        for (int i = 0; i < array.Length; i++)
        {
            array[i].HighlightBlue();
            yield return new WaitForSeconds(0.2f);
        }

        _timer.StopTimer();
    }
}
