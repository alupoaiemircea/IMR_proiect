using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using Newtonsoft.Json;

//https://www.youtube.com/watch?v=BLfNP4Sc_iA (used this for health_bar slider)
//https://www.youtube.com/watch?v=f81F2onEtY8 for stamina (adapted)
public class PlayerStats : MonoBehaviour
{
    public float maxHealth;
    public float maxStamina;
    public float attackDamage;
    bool escMenu = false;


    public float currentHealth;
    public AudioSource player_hurt = null;
    public Slider sliderHealth;
    public GameOverScreen gameOverScreen;
    public EscMenuScreen escMenuScreen;
    public Slider sliderStamina;
   
    
    public float currentStamina;

    private bool sprinting=false;
    private bool attacking=false;
    private bool penalty = false;

    public float sprintvalue;
    public float attackValue;
    public float increaseStaminaValue=2;

    public int current_lvl=1;
    public float current_xp;
    private float xp_to_lvl_up=10;
    public int lvl_up_points=0;
    public AudioSource tiredSoundEffect;
    public GameObject lvl_up_indicator;

    public void LoadStats()
    {
        string data = File.ReadAllText("playerStatsStart");
        PlayerInfo playerInfo = JsonConvert.DeserializeObject<PlayerInfo>(data);
        SetMaxHealth(playerInfo.maxHealth);
        SetAttackDamage(playerInfo.attackDamage);
        SetMaxStamina(playerInfo.maxStamina);
        escMenuScreen.GetComponent<EscMenuScreen>().ResizeBar(true,playerInfo.maxHealth);
        escMenuScreen.GetComponent<EscMenuScreen>().ResizeBar(false,playerInfo.maxStamina);
    }
    void Start()
    {
        LoadStats();
        currentHealth = 3;
        sliderHealth.value = currentHealth;
        currentStamina = maxStamina;
        sliderStamina.value = maxStamina;
        current_lvl = 1;
        current_xp = 0;
        lvl_up_points = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(penalty)
        {
            FatiquePenalty();
        }
        if(sprinting)
        {
            DecreaseStamina(sprintvalue);
           
        }
        else
            if (attacking)
        {
            DecreaseStamina(attackValue);
            attacking = false;
           
        }
        else
        if(currentStamina<maxStamina && !gameObject.GetComponent<PlayerMovement>().fatigue)
        {
            IncreaseStamina();
        }
        sliderStamina.value = currentStamina;
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(escMenu)
            {
                escMenuScreen.Hide();
                escMenu=false;
            }
            else
            escMenuScreen.Setup();
        }
    }
    public void TakeDamage(float amount)
    {
        player_hurt.Play();
        currentHealth -= amount;
        sliderHealth.value = currentHealth;
        //Debug.Log(currentHealth);
        if (currentHealth <= 0)
        {
            Camera.main.transform.parent = null;
            Camera.main.GetComponent<CameraLook>().enabled = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Destroy(gameObject);
            gameOverScreen.Setup();
            sliderHealth.value = 0;
        }
    }
    public void Heal(int amount)
    { 
        currentHealth += amount;
        sliderHealth.value = currentHealth;
        if (currentHealth>maxHealth)
        {
            currentHealth=maxHealth;
        }
    }

    private void IncreaseStamina()
    {
        if(currentStamina!=0)
        {
            currentStamina += increaseStaminaValue*Time.deltaTime;
            
        }
        if(currentStamina>=0.01)
        {
            gameObject.GetComponent<PlayerAttackSword>().fatigue = false;
        }
    }
    private void DecreaseStamina(float value)
    {
        if (currentStamina != 0)
        {
            currentStamina -= value * Time.deltaTime;
        }
        if(currentStamina < 0.05f)
        {
            //Debug.Log("FATIGUE");
           
             tiredSoundEffect.Play(); 

            if (Input.GetKey(KeyCode.Space))
            {
                gameObject.GetComponent<PlayerMovement>().fatigue = true;
                gameObject.GetComponent<PlayerMovement>().SetCurrentSpeed(gameObject.GetComponent<PlayerMovement>().GetWalkSpeed());
            }
            gameObject.GetComponent<PlayerAttackSword>().fatigue = true;
            penalty = true;
        }
        if(currentStamina<0)
        {
            currentStamina = 0.0001f;
        }
    }
    private void FatiquePenalty()
    {
        increaseStaminaValue = 0.0001f;
        Invoke(nameof(EndPenalty), 3f);
    }
    private void EndPenalty()
    {
       increaseStaminaValue = 2f;
        penalty = false;
    }
    public void SetSprinting(bool value)
    {
        sprinting = value;
    }
    public void SetAttacking(bool value)
    {
        attacking = value;
    }
    public float GetStamina()
    {
        return currentStamina;
    }
    public void AddXp(int xp)
    {
        current_xp += xp;
        if(current_xp>xp_to_lvl_up)
        {
            current_lvl++;
            lvl_up_points++;
            current_xp=current_xp-xp_to_lvl_up;
            xp_to_lvl_up = xp_to_lvl_up * 1.3f;
            lvl_up_indicator.SetActive(true);
        }
    }

    public void SetMaxHealth(float value)
    { maxHealth = value;
        sliderHealth.maxValue = value;
    }
    public void SetMaxStamina(float value)
    { maxStamina = value;
        sliderStamina.maxValue = value;
    }
    public void SetAttackDamage(float value)
    { attackDamage = value; }
    public float GetMaxStamina()
    { return maxStamina; }
    public float GetMaxHealth()
    { return maxHealth; }
    public float GetAttackDamage()
    { return attackDamage; }
}
