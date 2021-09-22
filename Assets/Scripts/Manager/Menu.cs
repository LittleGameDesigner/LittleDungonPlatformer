using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public GameObject Setting;
    public AudioMixer audioMixer;
    private GameObject PlayerData;
    private GameObject whichPlayer;
    public SaveData saveData;

    public void PauseGame(){
        if(Time.timeScale == 1){
            Time.timeScale = 0;
        }else if(Time.timeScale == 0){
            Time.timeScale = 1;
        }
    }

    public void NewGame(){
        SaveManager.DeleteSaveFile();
        SceneManager.LoadScene("Chapter1");
    }

    public void OpenCloseSetting(){
        if(Setting.activeSelf){
            Setting.SetActive(false);
        }else{
            Setting.SetActive(true);
        }
    }

    public void SaveGame(){
        SaveManager.Save(GameObject.Find("PlayerController").GetComponent<PlayerController>());
    }

    public void LoadGame(){
        var playerController = GameObject.Find("PlayerController").GetComponent<PlayerController>();
        PlayerData data = SaveManager.Load(playerController);
        SceneManager.LoadScene(data.sceneName);
    }

    // public void ReadLoadData(PlayerData data){
    //     var playerController = GameObject.Find("PlayerController").GetComponent<PlayerController>();
    //     playerController.maxMagic = data.maxMagic;
    //     playerController.magic = data.magic;
    //     playerController.maxHealth = data.maxHealth;
    //     playerController.health = data.health;
    //     playerController.level = data.level;
    //     playerController.AttackDemage = data.AttackDemage;
    //     playerController.exp = data.exp;
    //     playerController.expGap = data.expGap;
    //     playerController.totalEXP = data.totalEXP;
    //     playerController.GotFireBall = data.GotFireBall;
    //     playerController.GotBerserkerRage = data.GotBerserkerRage;
    //     playerController.canSwitchToBow = data.canSwitchToBow;
    //     playerController.canSwitchToSword = data.canSwitchToSword;
    // }

    public void SetMenuThemeVolume(float volume){
        audioMixer.SetFloat("volume", volume);
    }

    public void SetThemeVolume(float volume){
        FindObjectOfType<AudioManager>().SetVolume("Theme", volume);
        FindObjectOfType<AudioManager>().SetVolume("SkeletonBossTheme", volume);
        FindObjectOfType<AudioManager>().SetVolume("Win", volume);
    }

    public void SetEffectVolume(float volume){
        FindObjectOfType<AudioManager>().SetVolume("SwordSwing", volume);
        FindObjectOfType<AudioManager>().SetVolume("ArrowHitMetal", volume);
        FindObjectOfType<AudioManager>().SetVolume("ArrowFly", volume);
        FindObjectOfType<AudioManager>().SetVolume("DrawSword", volume);
        FindObjectOfType<AudioManager>().SetVolume("SkeletonDie", volume);
        FindObjectOfType<AudioManager>().SetVolume("SpiderDie", volume);
        FindObjectOfType<AudioManager>().SetVolume("Electric", volume);
        FindObjectOfType<AudioManager>().SetVolume("OpenDoor", volume);
        FindObjectOfType<AudioManager>().SetVolume("Ding", volume);
        FindObjectOfType<AudioManager>().SetVolume("DrinkPotion", volume);
        FindObjectOfType<AudioManager>().SetVolume("LevelUp", volume);
        FindObjectOfType<AudioManager>().SetVolume("DrawBow", volume);
        FindObjectOfType<AudioManager>().SetVolume("FireBallExplosion", volume);
        FindObjectOfType<AudioManager>().SetVolume("CastFireBall", volume);
        FindObjectOfType<AudioManager>().SetVolume("Block", volume);
        FindObjectOfType<AudioManager>().SetVolume("BerserkerRage", volume);
    }

    public void QuitGame(){
        Application.Quit();
    }

    public void QuitToStartMenu(){
        SceneManager.LoadScene("MainMenu");
    }

    public void Back(){
        gameObject.SetActive(false);
    }
}
