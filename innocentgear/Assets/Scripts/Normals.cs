using System.Collections.Generic;
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
    [SerializeField] private AttackMove sixD;
    [SerializeField] private AttackMove jumpSixD;
    [Header("Specials")]
    [SerializeField] private AttackMove twoThreeSixP;
    [SerializeField] private AttackMove twoOneFourP;
    [SerializeField] private AttackMove sixTwoThreeK;
    [SerializeField] private AttackMove fourOneTwoThreeSixH;
    [SerializeField] private AttackMove twoOneFourK;

    private PlayerInput _playerInput;
    private PlayerState _playerState;
    private PlayerMovement _playerMovement;
    private SpecialInput _specialInput;
    private bool _didCloseS;
    
    // Start is called before the first frame update
    void Start()
    {
        _playerInput = GetComponent<PlayerInput>();
        _playerState = GetComponent<PlayerState>();
        _playerMovement = GetComponent<PlayerMovement>();
        _specialInput = GetComponent<SpecialInput>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_playerMovement.IsGrounded())
        {
            if (Input.GetKeyDown(punchKey))
            {
                if (_specialInput.CheckCombo(new List<int> {2, 3, 4}))
                {
                    _playerState.Move(twoThreeSixP);
                }
                else if (_specialInput.CheckCombo(new List<int> {2, 1, 4}))
                {
                    _playerState.Move(twoOneFourP);
                }
                else switch (_playerInput.CurrentDirection)
                {
                    case 5 or 4:
                        _playerState.Move(fiveP);
                        break;
                    case 1 or 2 or 3:
                        _playerState.Move(twoP);
                        break;
                    case 6:
                        _playerState.Move(sixP);
                        break;
                }
            }
            else if (Input.GetKeyDown(kickKey))
            {
                if (_specialInput.CheckCombo(new List<int> {2, 1, 5}))
                {
                    _playerState.Move(twoOneFourK);
                }
                else if (_specialInput.CheckCombo(new List<int> {3, 2, 3, 5}))
                {
                    _playerState.Move(sixTwoThreeK);
                }
                else switch (_playerInput.CurrentDirection)
                {
                    case 6 or 5 or 4:
                        _playerState.Move(fiveK);
                        break;
                    case 1 or 2 or 3:
                        _playerState.Move(twoK);
                        break;
                }
            }
            else if (Input.GetKeyDown(slashKey))
            {
                switch (_playerInput.CurrentDirection)
                {
                    case 5 or 4 when Mathf.Abs(_playerMovement.GetDisplacement()) < 3f && !_didCloseS:
                        _playerState.Move(cS);
                        _didCloseS = true;
                        break;
                    case 5 or 4:
                        _playerState.Move(fS);
                        _didCloseS = false;
                        break;
                    case 1 or 2 or 3:
                        _playerState.Move(twoS);
                        break;
                    case 6:
                        _playerState.Move(sixS);
                        break;
                }
            }
            else if (Input.GetKeyDown(heavyKey))
            {
                if (_specialInput.CheckCombo(new List<int> {1, 2, 3, 7}))
                {
                    _playerState.Move(fourOneTwoThreeSixH);
                }
                else switch (_playerInput.CurrentDirection)
                {
                    case 5 or 4:
                        _playerState.Move(fiveH);
                        break;
                    case 1 or 2 or 3:
                        _playerState.Move(twoH);
                        break;
                    case 6:
                        _playerState.Move(sixH);
                        break;
                }
            }
            else if (Input.GetKeyDown(dustKey))
            {
                switch (_playerInput.CurrentDirection)
                {
                    case 5 or 4:
                        _playerState.Move(fiveD);
                        break;
                    case 1 or 2 or 3:
                        _playerState.Move(twoD);
                        break;
                    case 6:
                        _playerState.Move(sixD);
                        break;
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
                if (_playerInput.CurrentDirection is 6)
                {
                    _playerState.Move(jumpSixD);
                }
                else
                {
                    _playerState.Move(jumpD);
                }
            }
        }
    }

    public void CompletedMove()
    {
        _didCloseS = false;
    }
}
