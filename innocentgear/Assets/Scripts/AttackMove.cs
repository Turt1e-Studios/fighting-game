using UnityEngine;

[CreateAssetMenu]
public class AttackMove : ScriptableObject
{
    public GameObject boxes;
    
    public int startupFrames;
    public int activeFrames;
    public int recoveryFrames;
    public int onBlock;

    public bool isLow;
    public bool isHigh;

    public bool hasStandingAppearance;
}
