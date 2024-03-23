using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RoundStart : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI numberText;
    [SerializeField] private GameObject roundText;
    [SerializeField] private GameObject fightText;

    // Start is called before the first frame update
    void Start()
    {
        if (!PlayerPrefs.HasKey("Round"))
        {
            PlayerPrefs.SetInt("Round", 1);
        }
        numberText.text = "" + PlayerPrefs.GetInt("Round");
        roundText.SetActive(true);
        
        StartCoroutine(WaitForTime(2f, ActivateFightText));
    }

    void ActivateFightText()
    {
        roundText.SetActive(false);
        fightText.SetActive(true);
        StartCoroutine(WaitForTime(1f, () => fightText.SetActive(false)));
    }
    
    // Perform an action after a certain amount of frames.
    private IEnumerator WaitForTime(float seconds, Action action)
    {
        yield return new WaitForSeconds(seconds);
        action();
    }

    public void IncreaseRound()
    {
        PlayerPrefs.SetInt("Round", PlayerPrefs.GetInt("Round") + 1);
    }
}
