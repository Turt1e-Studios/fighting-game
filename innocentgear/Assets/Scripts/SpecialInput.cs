using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpecialInput : MonoBehaviour
{
    [SerializeField] private KeyCode[] keys;
    [SerializeField] private float delayBetweenPresses = 0.3f;

    private List<KeyCode> _keylist;
    private PlayerMovement _playerMovement;
    private float _lastPresssedTime;
    
    // Start is called before the first frame update
    void Start()
    {
        _keylist = new List<KeyCode>();
        _playerMovement = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - _lastPresssedTime > delayBetweenPresses)
        {
            _lastPresssedTime = Time.time;
            _keylist.Clear();
        }
        foreach (KeyCode key in keys)
        {
            if (Input.GetKeyDown(key))
            {
                _keylist.Add(key);
                _lastPresssedTime = Time.time;
                //print($"combo list: ({string.Join(", ", _keylist)}).");
            }
        }
    }
    
    // Up: 0. Left: 1. Down: 2. Right: 3. Punch: 4. Kick: 5. Slash: 6. Heavy. 7: Dust.
    public bool CheckCombo(List<int> combo)
    {
        print("checking combo");
        List<KeyCode> convertedKeys = new List<KeyCode>();
        print($"combo list: ({string.Join(", ", _keylist)}).");
        foreach (int num in combo)
        {
            if ((num == 1 || num == 3) && _playerMovement.IsFlipped())
            {
                convertedKeys.Add(num == 1 ? KeyFromInt(3) : KeyFromInt(1));
            }
            else
            {
                convertedKeys.Add(KeyFromInt(num));
            }
        }
        print($"combo list: ({string.Join(", ", convertedKeys)}).");
        return _keylist.SequenceEqual(convertedKeys);
    }

    // Have to put it in the right order in the inspector.
    private KeyCode KeyFromInt(int num)
    {
        return keys[num];
    }

    public void ResetCombo()
    {
        _keylist.Clear();
    }
}
