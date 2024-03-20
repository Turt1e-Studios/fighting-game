using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    // See https://www.dustloop.com/w/Notation for fighting game notation

    [SerializeField] private KeyCode upMovementKey;
    [SerializeField] private KeyCode downMovementKey;
    [SerializeField] private KeyCode leftMovementKey;
    [SerializeField] private KeyCode rightMovementKey;
        
    public int CurrentDirection { get; private set; }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(upMovementKey))
        {
            if (Input.GetKey(downMovementKey))
            {
                CurrentDirection = 5;
            }
            else if (Input.GetKey(leftMovementKey) && !Input.GetKey(rightMovementKey))
            {
                CurrentDirection = 7;
            }
            else if (Input.GetKey(rightMovementKey) && !Input.GetKey(leftMovementKey))
            {
                CurrentDirection = 9;
            }
            else
            {
                CurrentDirection = 8;
            }
        }
        else if (Input.GetKey(leftMovementKey))
        {
            if (Input.GetKey(rightMovementKey))
            {
                CurrentDirection = 5;
            }
            else if (Input.GetKey(downMovementKey))
            {
                CurrentDirection = 1;
            }
            else
            {
                CurrentDirection = 4;
            }
        }
        else if (Input.GetKey(rightMovementKey))
        {
            CurrentDirection = Input.GetKey(downMovementKey) ? 3 : 6;
        }
        else if (Input.GetKey(downMovementKey))
        {
            CurrentDirection = 2;
        }
        else
        {
            CurrentDirection = 5;
        }
    }

    public void ChangeDirection()
    {
        print("changed direction?");
        (leftMovementKey, rightMovementKey) = (rightMovementKey, leftMovementKey);
    }
}
