using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [SerializeField] private GameOverScreen gameOverScreen;
    [SerializeField] private Slider slider;
    [SerializeField] private Gradient gradient;
    [SerializeField] private Image fill;
    
    private int _health;
    private bool _gameOver;
    
    // Start is called before the first frame update
    void Start()
    {
        _health = 420; // Do not change
        slider.maxValue = _health;
        slider.value = _health;
        //fill.color = gradient.Evaluate(1f);
    }

    public void Damage(int damage)
    {
        float multiplier;
        if (_health <= 0.1 * 420)
        {
            multiplier = 0.5f;
        }
        else if (_health <= 0.2 * 420)
        {
            multiplier = 0.6f;
        }
        else if (_health <= 0.3 * 420)
        {
            multiplier = 0.7f;
        }
        else if (_health <= 0.4 * 420)
        {
            multiplier = 0.8f;
        }
        else if (_health <= 0.5 * 420)
        {
            multiplier = 0.85f;
        }
        else if (_health <= 0.6 * 420)
        {
            multiplier = 0.9f;
        }
        else if (_health <= 0.7 * 420)
        {
            multiplier = 0.95f;
        }
        else
        {
            multiplier = 1f;
        }
        _health -= (int) (damage * multiplier * 252 / 256);

        slider.value = _health;
        //fill.color = gradient.Evaluate(slider.normalizedValue);

        if (!_gameOver && _health <= 0)
        {
            _gameOver = true;
            bool winner = gameObject.name == "Player2";
            gameOverScreen.RoundOver(winner);
        }
    }

    public void ResetHealth()
    {
        _health = 420;
        slider.value = _health;
        _gameOver = false;
    }
}
