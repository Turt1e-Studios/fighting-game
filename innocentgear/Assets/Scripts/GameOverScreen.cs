using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    [SerializeField] private RoundStart roundStart;
    
    public void RoundOver(bool winner)
    {
        roundStart.IncreaseRound();
        gameObject.SetActive(true);
        
        StartCoroutine(WaitForTime(3f));
    }
    
    // Perform an action after a certain amount of frames.
    private IEnumerator WaitForTime(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        SceneManager.LoadScene("Game");
    }
}
