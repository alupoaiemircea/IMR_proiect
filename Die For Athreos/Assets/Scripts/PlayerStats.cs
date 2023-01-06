using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//https://www.youtube.com/watch?v=BLfNP4Sc_iA (used this for health_bar slider)
public class PlayerStats : MonoBehaviour
{
    public float maxHealth;
    public float currentHealth;
    public AudioSource player_hurt = null;
    public Slider slider;
    public GameOverScreen gameOverScreen;
    //public float stamina;
    void Start()
    {
        currentHealth = maxHealth;
        slider.value = currentHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void TakeDamage(float amount)
    {
        player_hurt.Play();
        currentHealth -= amount;
        slider.value = currentHealth;
        //Debug.Log(currentHealth);
        if (currentHealth <= 0)
        {
            Camera.main.transform.parent = null;
            Camera.main.GetComponent<CameraLook>().enabled = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Destroy(gameObject);
            gameOverScreen.Setup();
            slider.value = 0;
        }
    }
}
