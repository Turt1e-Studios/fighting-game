using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    [SerializeField] private RoundStart roundStart;
    [SerializeField] private GameObject winScreen;
    [SerializeField] private TextMeshProUGUI winText;
    [SerializeField] private GameObject player1;
    [SerializeField] private GameObject player2;
    [SerializeField] private List<GameObject> hearts1;
    [SerializeField] private List<GameObject> hearts2;
    [SerializeField] private Clock clock;

    private int _p1Lives = 2;
    private int _p2Lives = 2;

    void Start()
    {
        //if (!PlayerPrefs.HasKey("P1Lives"))
        //{
            //PlayerPrefs.SetInt("P1Lives", 2);
        //}
        //if (!PlayerPrefs.HasKey("P2Lives"))
        //{
            //PlayerPrefs.SetInt("P2Lives", 2);
        //}
        winScreen.SetActive(false);
    }
    
    public void RoundOver(bool p1Winner, bool draw = false)
    {
        if (p1Winner && !draw)
        {
            _p2Lives--;
            Destroy(hearts2[_p2Lives]);
        }
        else if (!draw)
        {
            _p1Lives--;
            Destroy(hearts1[_p1Lives]);
            //hearts1.RemoveAt(_p1Lives - 1);
        }

        if (_p1Lives <= 0 || _p2Lives <= 0)
        {
            winScreen.SetActive(true);
            winText.text = _p1Lives <= 0 ? "PLAYER 2 WINS!" : "PLAYER 1 WINS!";
            clock.SetStatus(false);
            return;
        }

        roundStart.IncreaseRound();
        gameObject.SetActive(true);
        
        clock.SetStatus(false);

        StartCoroutine(WaitForTime(3f));
    }

    // Perform an action after a certain amount of frames.
    private IEnumerator WaitForTime(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        ResetStage();
    }

    private void ResetStage()
    {
        clock.ResetClock();
        gameObject.SetActive(false);
        player1.transform.position = new Vector2(-4.32f, -1.49f);
        player2.transform.position = new Vector2(4.17f, -1.49f);
        player1.GetComponent<Health>().ResetHealth();
        player2.GetComponent<Health>().ResetHealth();
        roundStart.StartRound();
    }

    public void ResetGame()
    {
        SceneManager.LoadScene("Game");
    }
}
