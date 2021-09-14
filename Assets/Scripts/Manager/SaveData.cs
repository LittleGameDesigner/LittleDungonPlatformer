using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class SaveData
{
    public string sceneName;
    public string whichPlayer;
    public float positionX;
    public float positionY;
    public float positionZ;
    public int level;
    public float AttackDemage;
    public float maxHealth;
    public float maxMagic;
    public float health;
    public float magic;
    public float exp;
    public float expGap;
    public float totalEXP;

    public bool canSwitchToBow;
    public bool canSwitchToSword;
    public bool GotFireBall;
    public bool GotBerserkerRage;

    public SaveData(StaticPlayerData Player){
        sceneName = SceneManager.GetActiveScene().name;
        whichPlayer = Player.whichPlayer;
        maxMagic = Player.maxMagic;
        magic = Player.magic;
        maxHealth = Player.maxHealth;
        health = Player.health;
        level = Player.level;
        AttackDemage = Player.AttackDemage;
        exp = Player.exp;
        expGap = Player.expGap;
        totalEXP = Player.totalEXP;
        GotFireBall = Player.GotFireBall;
        GotBerserkerRage = Player.GotBerserkerRage;
        canSwitchToBow = Player.canSwitchToBow;
        canSwitchToSword = Player.canSwitchToSword;
        positionX = Player.positionX;
        positionY = Player.positionY;
        positionZ = Player.positionZ;
    }
}
