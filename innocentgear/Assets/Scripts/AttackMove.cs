using UnityEngine;

// An object that can be used to hold information about an attack.
[CreateAssetMenu]
public class AttackMove : ScriptableObject
{
    public GameObject boxes;
    
    public int startupFrames;
    public int activeFrames;
    public int recoveryFrames;
    public int onHit;
    public int onBlock;

    public bool isLow;
    public bool isHigh;

    public int damage;
    public int level;

    public AttackMove[] gatlings;
}
