using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using Newtonsoft.Json;

public class MainMenuScript : MonoBehaviour
{
    
    public GameObject characterSelectScreen;
    public GameObject mainMenuScreen;
    public void NewGame()
    {
        mainMenuScreen.SetActive(false);
        characterSelectScreen.SetActive(true);
     
    }
    public void Exit()
    {
        Application.Quit();
    }
    public void Back()
    {
        characterSelectScreen.SetActive(false);
        mainMenuScreen.SetActive(true);
    }
    public void SelectAssasin()
    {
        PlayerInfo playerInfo = new PlayerInfo(6, 13, 2f);
        string jsonString = JsonConvert.SerializeObject(playerInfo);
        File.WriteAllText("playerStatsStart", jsonString);
        SceneManager.LoadScene("MainScene");
    }
    public void SelectMage()
    {
        PlayerInfo playerInfo = new PlayerInfo(14, 6, 0.5f);
        string jsonString = JsonConvert.SerializeObject(playerInfo);
        File.WriteAllText("playerStatsStart", jsonString);
        SceneManager.LoadScene("MainScene");
    }
    public void SelectWarrior()
    {
       PlayerInfo playerInfo = new PlayerInfo(10,10,1);
        string jsonString = JsonConvert.SerializeObject(playerInfo);
        File.WriteAllText("playerStatsStart", jsonString);
        SceneManager.LoadScene("MainScene");
    }
}
public class PlayerInfo
{
    public float maxHealth;
    public float maxStamina;
    public float attackDamage;
    public PlayerInfo(float maxHealth, float maxStamina, float attackDamge)
    {
       this.maxHealth = maxHealth;  
       this.maxStamina = maxStamina;    
       this.attackDamage = attackDamge;
    }
}
