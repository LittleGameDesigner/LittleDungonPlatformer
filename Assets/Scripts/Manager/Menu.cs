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
        PlayerData = GameObject.Find("StaticPlayerData");
        SaveManager.Save(PlayerData.GetComponent<StaticPlayerData>());
    }

    public void LoadGame(){
        PlayerData = GameObject.Find("StaticPlayerData");
        SaveData data = SaveManager.Load();
        ReadLoadData(data);
        SceneManager.LoadScene(data.sceneName);
        DontDestroyOnLoad(PlayerData);
    }

    public void ReadLoadData(SaveData data){
        PlayerData = GameObject.Find("StaticPlayerData");
        PlayerData.GetComponent<StaticPlayerData>().maxMagic = data.maxMagic;
        PlayerData.GetComponent<StaticPlayerData>().magic = data.magic;
        PlayerData.GetComponent<StaticPlayerData>().maxHealth = data.maxHealth;
        PlayerData.GetComponent<StaticPlayerData>().health = data.health;
        PlayerData.GetComponent<StaticPlayerData>().level = data.level;
        PlayerData.GetComponent<StaticPlayerData>().AttackDemage = data.AttackDemage;
        PlayerData.GetComponent<StaticPlayerData>().exp = data.exp;
        PlayerData.GetComponent<StaticPlayerData>().expGap = data.expGap;
        PlayerData.GetComponent<StaticPlayerData>().totalEXP = data.totalEXP;
        PlayerData.GetComponent<StaticPlayerData>().GotFireBall = data.GotFireBall;
        PlayerData.GetComponent<StaticPlayerData>().GotBerserkerRage = data.GotBerserkerRage;
        PlayerData.GetComponent<StaticPlayerData>().canSwitchToSword = data.canSwitchToSword;
        PlayerData.GetComponent<StaticPlayerData>().canSwitchToBow = data.canSwitchToBow;
    }

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
