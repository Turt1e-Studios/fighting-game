using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blocking : MonoBehaviour
{
    [SerializeField] private GameObject standingBoxes;
    [SerializeField] private GameObject crouchingBoxes;
    
    private PlayerInput _playerInput;
    private GameObject _activeBoxes;
    
    // Start is called before the first frame update
    void Start()
    {
        _playerInput = GetComponent<PlayerInput>();
        standingBoxes.SetActive(true);
        crouchingBoxes.SetActive(false);
    }

    public GameObject GetCurrentBoxes()
    {
        return _activeBoxes;
    }

    // Update is called once per frame
    void Update()
    {
        if (_playerInput.CurrentDirection is 1 or 2)
        {
            standingBoxes.SetActive(false);
            crouchingBoxes.SetActive(true);
            _activeBoxes = crouchingBoxes;
        }
        else
        {
            if (_activeBoxes == crouchingBoxes)
            {
                standingBoxes.SetActive(true);
                crouchingBoxes.SetActive(false);
                _activeBoxes = standingBoxes;
            }
        }
    }
}
