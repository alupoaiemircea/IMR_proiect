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
    [Header("Health")]
    public float maxHealth;
    public Slider sliderHealth;
    public float currentHealth;
    public AudioSource player_hurt = null;

    [Header("Stamina")]
    public float maxStamina;
    public Slider sliderStamina;
    public float currentStamina;
    public float sprintvalue;
    private bool sprinting = false;
    private bool attacking = false;
    private bool penalty = false;
    public float increaseStaminaValue = 2f;
    public AudioSource tiredSoundEffect;

    [Header("Attack")]
    public float attackDamage;
    public float attackValue;

    [Header("Frenzy")]
    private float maxFrenzy;
    public Slider sliderFrenzy;
    private float currentFrenzy=0;
    private bool frenzyModeOn = false;
    public float frenzyTimer = 10f;
    public int increaseFrenzyValue = 1;
    public float frenzyDecreaseTime=5f;
    public GameObject x2DamageText;

    [Header("Xp")]
    public int current_lvl = 1;
    public float current_xp;
    private float xp_to_lvl_up = 10f;
    public int lvl_up_points = 0;
    public GameObject lvl_up_indicator;

    [Header("UI")]
    bool escMenu = false;
    public GameOverScreen gameOverScreen;
    public EscMenuScreen escMenuScreen;
  
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
        currentHealth = maxHealth;
        sliderHealth.value = currentHealth;
        currentStamina = maxStamina;
        sliderStamina.value = maxStamina;
        current_lvl = 1;
        current_xp = 0;
        lvl_up_points = 0;
        maxFrenzy = 10;
    }

    // Update is called once per frame
    void Update()
    {

        DecreaseFrenzyTimer();   
       

        if (penalty)
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
        if (!frenzyModeOn)
        {
            if (currentStamina != 0)
            {
                currentStamina -= value * Time.deltaTime;
            }
            if (currentStamina < 0.05f)
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
            if (currentStamina < 0)
            {
                currentStamina = 0.0001f;
            }
        }
    }

    public void IncreaseFrenzy()
    {
        if (!frenzyModeOn)
        {
            if (currentFrenzy < maxFrenzy)
            {
                currentFrenzy += increaseFrenzyValue;
                sliderFrenzy.value = currentFrenzy;

            }
            if (currentFrenzy >= maxFrenzy)
            {
                currentFrenzy = maxFrenzy;
                sliderFrenzy.value = currentFrenzy;
                FrenzyMode();
                Invoke(nameof(FrenzyModeReset), frenzyTimer);
            }
        }
        
    }

    public void ResetFrenzyTimer()
    {
        frenzyDecreaseTime = 5f;
    }
    private void DecreaseFrenzyTimer()
    {
        if (frenzyDecreaseTime > 0)
        {
            frenzyDecreaseTime -= Time.deltaTime;
        }
        else
        {

            frenzyDecreaseTime = 5f;
            DecreaseFrenzy();
        }
    }
    public void DecreaseFrenzy()
    {
        if(currentFrenzy>0)
        {
            currentFrenzy -= increaseFrenzyValue;
            sliderFrenzy.value = currentFrenzy;
        }
        if(currentFrenzy < 0)
        {
            currentFrenzy = 0;
            sliderFrenzy.value = currentFrenzy;         
        }
    }

    private void FrenzyMode()
    {
        currentStamina = maxStamina;
        attackDamage=attackDamage*2;
        frenzyModeOn = true;
        x2DamageText.SetActive(true);
    }

    private void FrenzyModeReset()
    {
        frenzyModeOn= false;
        attackDamage = attackDamage/2;
        sliderFrenzy.value = 0;
        currentFrenzy = 0;
        x2DamageText.SetActive(false);
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
