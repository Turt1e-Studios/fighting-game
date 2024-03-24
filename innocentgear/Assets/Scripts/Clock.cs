using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Clock : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private GameOverScreen gameOverScreen;
    private int _time;
    private bool _activated;
    private float _initialTime;
    private float _previousTime;

    // Start is called before the first frame update
    void Start()
    {
        _time = 99;
        UpdateDisplay();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_activated) return;
        if (Time.time - _previousTime > 1f)
        {
            _time--;
            if (_time <= 0)
            {
                gameOverScreen.RoundOver(true, true);
            }
            
            _previousTime = Time.time;
            UpdateDisplay();
        }
    }

    public void ResetClock()
    {
        _time = 99;
        UpdateDisplay();
    }

    public void SetStatus(bool active)
    {
        _activated = active;
        _previousTime = Time.time;
    }

    private void UpdateDisplay()
    {
        timeText.text = "" + _time;
    }
}
