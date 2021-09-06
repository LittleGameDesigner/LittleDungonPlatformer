using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarBehaviour : MonoBehaviour
{
    public Slider sliderHealth;
    public Slider sliderMagic;
    public Slider sliderEXP;
    public Text lv;

    public void SetMaxHealth(float health, float maxHealth){
        sliderHealth.maxValue = maxHealth;
        sliderHealth.value = health;
    }

    public void SetHealth(float health){
        sliderHealth.value = health;
    }

    public void SetMaxMagic(float magic, float maxMagic){
        sliderMagic.maxValue = maxMagic;
        sliderMagic.value = magic;
    }

    public void SetMagic(float magic){
        sliderMagic.value = magic;
    }

    public void SetEXP(float exp, float maxExp, float level){
        sliderEXP.maxValue = maxExp;
        sliderEXP.value = exp;
        lv.text = level.ToString();
    }

    public void Active(bool setOrNot){
        gameObject.SetActive(setOrNot);
    }

}
