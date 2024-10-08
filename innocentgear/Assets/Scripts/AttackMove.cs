using System;
using System.Collections.Generic;
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
    public int damage;
    public int level;
    [Range(0, 3)] public int counterHit;

    public bool isLow;
    public bool isHigh;
    public bool isThrow;
    public bool isAirThrow;
    public bool isSpecial;
    public bool hasNoHitbox;
    public bool changesPosition;
    public bool isProjectile;
    public bool switchesSides;

    public Vector2 initialForce;
    public List<AttackMove> gatlings;
    public List<GameObject> otherBoxes; // also includes base attack.
    public List<int> otherBoxFrames; // necessary if there are other boxes in an attack.
    public AttackMove rekka;
}
