using UnityEngine;
using UnityEngine.UI;
public class BShuffle : MonoBehaviour
{
    private Button _button;
    private ArrayManager _arrayManager;
    private SortTimer _timer;

    private void Start()
    {
        _arrayManager = FindFirstObjectByType<ArrayManager>();

        _button = GetComponent<Button>();
        _button.onClick.AddListener((UnityEngine.Events.UnityAction)(() => _arrayManager.ShuffleArray()));

        _timer = FindFirstObjectByType<SortTimer>();
        _timer.AddButton(_button);
    }
}
