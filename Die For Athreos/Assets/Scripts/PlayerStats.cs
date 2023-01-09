using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//https://www.youtube.com/watch?v=BLfNP4Sc_iA (used this for health_bar slider)
//https://www.youtube.com/watch?v=f81F2onEtY8 for stamina (adapted)
public class PlayerStats : MonoBehaviour
{
    public float maxHealth;
    public float currentHealth;
    public AudioSource player_hurt = null;
    public Slider sliderHealth;
    public GameOverScreen gameOverScreen;
    public Slider sliderStamina;
    public float maxStamina=10;
    public float currentStamina;

    private bool sprinting=false;
    private bool attacking=false;
    private bool penalty = false;

    public float sprintvalue;
    public float attackValue;
    public float increaseStaminaValue=2;


    public AudioSource tiredSoundEffect;
    void Start()
    {
        currentHealth = 3;
        sliderHealth.value = currentHealth;
        currentStamina = maxStamina;
        sliderStamina.value = maxStamina;
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
}
