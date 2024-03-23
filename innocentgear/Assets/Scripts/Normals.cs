using UnityEngine;

// Controls the activation of the player's regular attacks.
public class Normals : MonoBehaviour
{
    // See https://www.dustloop.com/w/Notation for fighting game notation
    
    [Header("Keys")]
    [SerializeField] private KeyCode punchKey;
    [Header("Attacks")]
    [SerializeField] private AttackMove fiveP;
    [SerializeField] private AttackMove twoP;
    [SerializeField] private AttackMove jumpP;

    private PlayerInput _playerInput;
    private PlayerState _playerState;
    private PlayerMovement _playerMovement;
    
    // Start is called before the first frame update
    void Start()
    {
        _playerInput = GetComponent<PlayerInput>();
        _playerState = GetComponent<PlayerState>();
        _playerMovement = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_playerMovement.IsGrounded())
        {
            if (Input.GetKeyDown(punchKey))
            {
                if (_playerInput.CurrentDirection is 5 or 4)
                {
                    _playerState.Move(fiveP);
                }
                else if (_playerInput.CurrentDirection is 1 or 2 or 3)
                {
                    _playerState.Move(twoP);
                }
            }
        }
        else
        {
            if (Input.GetKeyDown(punchKey))
            {
                _playerState.Move(jumpP);
            }
        }
    }
}
