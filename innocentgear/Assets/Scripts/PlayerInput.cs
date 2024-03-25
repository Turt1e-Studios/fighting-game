using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Condenses player input into a single number.
public class PlayerInput : MonoBehaviour
{
    // See https://www.dustloop.com/w/Notation for fighting game notation
    
    [Header("Input Keys")]
    [SerializeField] private KeyCode upMovementKey;
    [SerializeField] private KeyCode downMovementKey;
    [SerializeField] private KeyCode leftMovementKey;
    [SerializeField] private KeyCode rightMovementKey;
    [SerializeField] private float delayBetweenPresses = 0.2f;
    [SerializeField] private float delayBeforeCombo = 0.4f;
    
    private const float DelayBetweenPresses = 0.2f;
    private const float DelayBeforeCombo = 0.4f;
    private List<int> _directionList;
    private float _lastPressedTime;
    private float _finishedComboTime;
    
    // Property of the current direction of the player
    public int CurrentDirection { get; private set; }

    void Start()
    {
        _lastPressedTime = Time.time;
        _directionList = new List<int>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(upMovementKey))
        {
            if (Input.GetKey(downMovementKey))
            {
                SetCurrentDirection(5);
            }
            else if (Input.GetKey(leftMovementKey) && !Input.GetKey(rightMovementKey))
            {
                SetCurrentDirection(7);
            }
            else if (Input.GetKey(rightMovementKey) && !Input.GetKey(leftMovementKey))
            {
                SetCurrentDirection(9);
            }
            else
            {
                SetCurrentDirection(8);
            }
        }
        else if (Input.GetKey(leftMovementKey))
        {
            if (Input.GetKey(rightMovementKey))
            {
                SetCurrentDirection(5);
            }
            else if (Input.GetKey(downMovementKey))
            {
                SetCurrentDirection(1);
            }
            else
            {
                SetCurrentDirection(4);
            }
        }
        else if (Input.GetKey(rightMovementKey))
        {
            SetCurrentDirection(Input.GetKey(downMovementKey) ? 3 : 6);
        }
        else if (Input.GetKey(downMovementKey))
        {
            SetCurrentDirection(2);
        }
        else
        {
            SetCurrentDirection(5);
        }
    }

    // Change direction readings after players switch sides
    public void ChangeDirection()
    {
        (leftMovementKey, rightMovementKey) = (rightMovementKey, leftMovementKey);
    }

    private void SetCurrentDirection(int direction)
    {
        if (Time.time - _lastPressedTime <= delayBetweenPresses && (_directionList.Count == 0 || _directionList[^1] != direction))
        {
            _directionList.Add(direction);
            _finishedComboTime = Time.time;
            print($"Here's the list: ({string.Join(", ", _directionList)}).");
        }
        else if (Time.time - _finishedComboTime >= delayBeforeCombo)
        {
            print("cleared list");
            _directionList.Clear();
        }
        CurrentDirection = direction;
        _lastPressedTime = Time.time;
    }
    private List<int> GetCombo(int nums)
    {
        // check if array is valid:
        // first, remove first and last elements if they are 5 (misc combo.)
        // then compare to see if the two arrays are the same.
        // then ensure that you are already doing a combo.
        return _directionList.GetRange(0, nums);
    }

    public void ResetCombo()
    {
        _directionList.Clear();
    }

    public bool CheckCombo(List<int> combo)
    {
        //print($"combo list: ({string.Join(", ", combo)}).");

        List<int> compareList = new List<int>(_directionList);
        //print($"original list: ({string.Join(", ", compareList)}).");
        if (compareList.First() == 5)
        {
            compareList.RemoveAt(0);
        }
        if (compareList.Count != 0 && compareList.Last() == 5)
        {
            compareList.RemoveAt(compareList.Count - 1);
        }
        //print($"changed list: ({string.Join(", ", compareList)}).");

        //print("equals? " + compareList.Equals(combo));
        return compareList.Count >= 1 && compareList.SequenceEqual(combo);
    }
}
