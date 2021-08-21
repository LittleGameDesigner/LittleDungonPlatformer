using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarBehaviour : MonoBehaviour
{
    public Slider slider;

    public void SetMaxHealth(float health, float maxHealth){
        slider.maxValue = maxHealth;
        slider.value = health;
    }

    public void SetHealth(float health){
        slider.value = health;
    }

    public void Active(bool setOrNot){
        gameObject.SetActive(setOrNot);
    }

}
