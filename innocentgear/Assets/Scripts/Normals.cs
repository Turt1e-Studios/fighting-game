using UnityEngine;

// Controls the activation of the player's regular attacks.
public class Normals : MonoBehaviour
{
    // See https://www.dustloop.com/w/Notation for fighting game notation
    
    [Header("Keys")]
    [SerializeField] private KeyCode punchKey;
    [SerializeField] private KeyCode kickKey;
    [SerializeField] private KeyCode slashKey;
    [SerializeField] private KeyCode heavyKey;
    [SerializeField] private KeyCode dustKey;
    [Header("Attacks")]
    [SerializeField] private AttackMove fiveP;
    [SerializeField] private AttackMove twoP;
    [SerializeField] private AttackMove jumpP;
    [SerializeField] private AttackMove fiveK;
    [SerializeField] private AttackMove twoK;
    [SerializeField] private AttackMove jumpK;
    [SerializeField] private AttackMove cS;
    [SerializeField] private AttackMove fS;
    [SerializeField] private AttackMove twoS;
    [SerializeField] private AttackMove jumpS;
    [SerializeField] private AttackMove fiveH;
    [SerializeField] private AttackMove twoH;
    [SerializeField] private AttackMove jumpH;
    [SerializeField] private AttackMove fiveD;
    [SerializeField] private AttackMove twoD;
    [SerializeField] private AttackMove jumpD;
    [SerializeField] private AttackMove sixP;
    [SerializeField] private AttackMove sixS;
    [SerializeField] private AttackMove sixH;

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
                else if (_playerInput.CurrentDirection is 6)
                {
                    _playerState.Move(sixP);
                }
            }
            else if (Input.GetKeyDown(kickKey))
            {
                if (_playerInput.CurrentDirection is 5 or 4)
                {
                    _playerState.Move(fiveK);
                }
                else if (_playerInput.CurrentDirection is 1 or 2 or 3)
                {
                    _playerState.Move(twoK);
                }
            }
            else if (Input.GetKeyDown(slashKey))
            {
                if (_playerInput.CurrentDirection is 5 or 4)
                {
                    if (Mathf.Abs(_playerMovement.GetDisplacement()) < 3f)
                    {
                        _playerState.Move(cS);
                    }
                    else
                    {
                        _playerState.Move(fS);
                    }
                }
                else if (_playerInput.CurrentDirection is 1 or 2 or 3)
                {
                    _playerState.Move(twoS);
                }
                else if (_playerInput.CurrentDirection is 6)
                {
                    _playerState.Move(sixS);
                }
            }
            else if (Input.GetKeyDown(heavyKey))
            {
                if (_playerInput.CurrentDirection is 5 or 4)
                {
                    _playerState.Move(fiveH);
                }
                else if (_playerInput.CurrentDirection is 1 or 2 or 3)
                {
                    _playerState.Move(twoH);
                }
                else if (_playerInput.CurrentDirection is 6)
                {
                    _playerState.Move(sixH);
                }
            }
            else if (Input.GetKeyDown(dustKey))
            {
                if (_playerInput.CurrentDirection is 5 or 4)
                {
                    _playerState.Move(fiveD);
                }
                else if (_playerInput.CurrentDirection is 1 or 2 or 3)
                {
                    _playerState.Move(twoD);
                }
            }
        }
        else
        {
            if (Input.GetKeyDown(punchKey))
            {
                _playerState.Move(jumpP);
            }
            else if (Input.GetKeyDown(kickKey))
            {
                _playerState.Move(jumpK);
            }
            else if (Input.GetKeyDown(slashKey))
            {
                _playerState.Move(jumpS);
            }
            else if (Input.GetKeyDown(heavyKey))
            {
                _playerState.Move(jumpH);
            }
            else if (Input.GetKeyDown(dustKey))
            {
                _playerState.Move(jumpD);
            }
        }
    }
}
