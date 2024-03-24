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

    private int _round = 1;

    // Start is called before the first frame update
    void Start()
    {
        //if (!PlayerPrefs.HasKey("Round"))
        //{
            //PlayerPrefs.SetInt("Round", 1);
        //}
        //numberText.text = "" + PlayerPrefs.GetInt("Round");
        StartRound();
    }

    public void StartRound()
    {
        numberText.text = "" + _round;
        roundText.SetActive(true);
        
        SetMovement(false);

        StartCoroutine(WaitForTime(2f, ActivateFightText));
    }

    void SetMovement(bool set)
    {
        GameObject.Find("Player").GetComponent<PlayerMovement>().enabled = set;
        GameObject.Find("Player").GetComponent<Normals>().enabled = set;
        GameObject.Find("Player2").GetComponent<PlayerMovement>().enabled = set;
        GameObject.Find("Player2").GetComponent<Normals>().enabled = set;
    }

    void ActivateFightText()
    {
        roundText.SetActive(false);
        fightText.SetActive(true);
        StartCoroutine(WaitForTime(1f, StartFight));
    }

    void StartFight()
    {
        fightText.SetActive(false);
        SetMovement(true);
    }
    
    // Perform an action after a certain amount of frames.
    private IEnumerator WaitForTime(float seconds, Action action)
    {
        yield return new WaitForSeconds(seconds);
        action();
    }

    public void IncreaseRound()
    {
        _round++;
        //PlayerPrefs.SetInt("Round", PlayerPrefs.GetInt("Round") + 1);
    }
}
