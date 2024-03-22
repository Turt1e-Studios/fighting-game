using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class AttackMove : ScriptableObject
{
    public GameObject boxes;
    
    public int startupFrames;
    public int activeFrames;
    public int recoveryFrames;
    public int onBlock;
}
