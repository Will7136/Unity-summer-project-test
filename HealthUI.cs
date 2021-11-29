using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    public Text HealthText;
    public void UpdateHealthUI(int hp){
        if (hp >= 0){
            HealthText.text = "Health : " + hp.ToString();
        }
        else{
            HealthText.text = "Health : 0";
        }
    }
    
}
