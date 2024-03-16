using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    // See https://www.dustloop.com/w/Notation for fighting game notation
    public int CurrentDirection { get; private set; }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            if (Input.GetKey(KeyCode.S))
            {
                CurrentDirection = 5;
            }
            else if (Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
            {
                CurrentDirection = 7;
            }
            else if (Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.S))
            {
                CurrentDirection = 9;
            }
            else
            {
                CurrentDirection = 8;
            }
        }
        else if (Input.GetKey(KeyCode.A))
        {
            if (Input.GetKey(KeyCode.D))
            {
                CurrentDirection = 5;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                CurrentDirection = 1;
            }
            else
            {
                CurrentDirection = 4;
            }
        }
        else if (Input.GetKey(KeyCode.D))
        {
            CurrentDirection = Input.GetKey(KeyCode.S) ? 3 : 6;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            CurrentDirection = 2;
        }
        else
        {
            CurrentDirection = 5;
        }
    }
}
