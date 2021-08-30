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
        SaveManager.Save();
    }

    public void LoadGame(){
        string sceneName = SaveManager.Load();
        SceneManager.LoadScene(sceneName);
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
