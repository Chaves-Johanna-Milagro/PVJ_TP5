using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SortTimer : MonoBehaviour
{
    private TMP_Text _txtTime;

    private bool _running;
    private float _elapsed;

    private List<Button> _buttons = new List<Button>();

    private void Start()
    {
        _txtTime = GetComponent<TMP_Text>();
    }

    public void AddButton(Button b)
    {
        if (!_buttons.Contains(b))
            _buttons.Add(b);
    }
    public void StartTimer()
    {
        _elapsed = 0;
        _running = true;

        SetStateButtons(false);
    }

    public void StopTimer()
    {
        _running = false;

        SetStateButtons(true);
    }

    private void Update()
    {
        if (!_running) return;

        _elapsed += Time.deltaTime;
        DisplayTime(_elapsed);
    }

    private void DisplayTime(float timeToDisplay)
    {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        float milliSeconds = Mathf.FloorToInt((timeToDisplay * 100) % 100);

        _txtTime.text = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, milliSeconds);
    }

    private void SetStateButtons(bool state)
    {
        if (_buttons == null) return;

        foreach (var btn in _buttons)
        {
            if (btn != null)
                btn.interactable = state;
        }
    }
}
