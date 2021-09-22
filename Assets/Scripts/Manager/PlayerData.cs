using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class PlayerData
{
    public string sceneName;

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


    public PlayerData(PlayerController Player, string scene = null){
        if(scene == null){
            sceneName = SceneManager.GetActiveScene().name;
        }else{
            sceneName = scene;
        }
        positionX = Player.transform.position.x;
        positionY = Player.transform.position.y;
        positionZ = Player.transform.position.z;
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
    }

    public void Reset(){
        sceneName = "Chapter1";
        maxMagic = 50;
        magic = 50;
        maxHealth = 100;
        health = 100;
        level = 1;
        AttackDemage = 10;
        exp = 0;
        expGap = 100;
        totalEXP = 0;
        GotFireBall = false;
        GotBerserkerRage = true;
        canSwitchToBow = false;
        canSwitchToSword = false;
    }
}
