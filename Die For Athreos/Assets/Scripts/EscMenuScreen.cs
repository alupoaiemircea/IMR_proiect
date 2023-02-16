using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class EscMenuScreen : MonoBehaviour
{
    public GameObject player;
    public float healthIncrement = 0.3f;
    public float staminaIncrement = 0.2f;
    public float attackIncrement = 0.5f;

    public GameObject healthBar;
    public GameObject staminaBar;
   

    public TextMeshProUGUI healthValue;
    public TextMeshProUGUI staminaValue;
    public TextMeshProUGUI attackValue;

    public TextMeshProUGUI level;
    public TextMeshProUGUI points;

    public GameObject lvl_up_indicator;
    public void Setup()
    { 
        gameObject.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Camera.main.GetComponent<CameraLook>().enabled = false;
        level.text = player.GetComponent<PlayerStats>().current_lvl.ToString();
        points.text = player.GetComponent<PlayerStats>().lvl_up_points.ToString();
    }
    public void Hide()
    {
        Camera.main.GetComponent<CameraLook>().enabled = true;
        gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public void Restart()
    {
        SceneManager.LoadScene("MainScene");
    }
    public void Exit()
    {
        
        SceneManager.LoadScene("MainMenu");
     

    }
    public void UpgradeHealth()
    {
       if(player.GetComponent<PlayerStats>().lvl_up_points > 0)
        {
            float currentHealth = player.GetComponent<PlayerStats>().GetMaxHealth();
            player.GetComponent<PlayerStats>().SetMaxHealth(currentHealth + currentHealth * healthIncrement);
            int val = int.Parse(healthValue.text) + 1;
            healthValue.text = val.ToString();
            player.GetComponent<PlayerStats>().lvl_up_points--;
            points.text = player.GetComponent<PlayerStats>().lvl_up_points.ToString();

            RectTransform rect = healthBar.GetComponent<RectTransform>();
            Vector2 pivot = rect.pivot;
            rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, rect.sizeDelta.x + rect.sizeDelta.x * healthIncrement);
            rect.pivot = pivot;
            if (player.GetComponent<PlayerStats>().lvl_up_points==0)
            {
                lvl_up_indicator.SetActive(false);
            }
        }
       
    }
    public void UpgradeStamina()
    {
        if(player.GetComponent<PlayerStats>().lvl_up_points>0)
        {
            float currentStamina = player.GetComponent<PlayerStats>().GetMaxStamina();
            player.GetComponent<PlayerStats>().SetMaxStamina(currentStamina + currentStamina * staminaIncrement);
            int val = int.Parse(staminaValue.text) + 1;
            staminaValue.text = val.ToString();
            player.GetComponent<PlayerStats>().lvl_up_points--;
            points.text = player.GetComponent<PlayerStats>().lvl_up_points.ToString();

            RectTransform rect =staminaBar.GetComponent<RectTransform>();
            Vector2 pivot = rect.pivot;
            rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, rect.sizeDelta.x + rect.sizeDelta.x * staminaIncrement);
            rect.pivot = pivot;

            if (player.GetComponent<PlayerStats>().lvl_up_points == 0)
            {
                lvl_up_indicator.SetActive(false);
            }
        }
        
    }
    public void UpgradeAttack()
    {
        if(player.GetComponent<PlayerStats>().lvl_up_points>0)
        {
            float currentAttackDamage = player.GetComponent<PlayerStats>().GetAttackDamage();
            player.GetComponent<PlayerStats>().SetAttackDamage(currentAttackDamage + currentAttackDamage * attackIncrement);
            int val = int.Parse(attackValue.text) + 1;
            attackValue.text = val.ToString();
            player.GetComponent<PlayerStats>().lvl_up_points--;
            points.text = player.GetComponent<PlayerStats>().lvl_up_points.ToString();

            if (player.GetComponent<PlayerStats>().lvl_up_points == 0)
            {
                lvl_up_indicator.SetActive(false);
            }
        }
       
    }
    public void ResizeBar(bool type,float maxValue)
    {
        if(type)//healthbar
        {
            RectTransform rect = healthBar.GetComponent<RectTransform>();
            Vector2 pivot = rect.pivot;
            rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, rect.sizeDelta.x + rect.sizeDelta.x * (0.1f*(maxValue-10)));
            rect.pivot = pivot;
        }
        else
        {
            RectTransform rect = staminaBar.GetComponent<RectTransform>();
            Vector2 pivot = rect.pivot;
            rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, rect.sizeDelta.x + rect.sizeDelta.x * (0.1f * (maxValue - 10)));
            rect.pivot = pivot;
        }
    }
}
