using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticPlayerData : MonoBehaviour
{
    public string whichPlayer = "Player";
    public float positionX;
    public float positionY;
    public float positionZ;
    public int level = 1;
    public float AttackDemage = 10;
    public float maxHealth = 100;
    public float maxMagic = 50;
    public float health = 100;
    public float magic = 50;
    public float exp = 0;
    public float expGap = 100;
    public float totalEXP = 0;

    public bool canSwitchToBow = false;
    public bool canSwitchToSword = false;
    public bool GotFireBall = false;
    public bool GotBerserkerRage = false;
    
}
