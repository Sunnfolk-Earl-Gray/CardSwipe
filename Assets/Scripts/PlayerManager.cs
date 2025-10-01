using System;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public int health;
    public int maxHealth;
    public TMP_Text healthText;
    public TMP_Text scoreText;
    public Animator healthAnim;
    private void Start()
    {
        health = maxHealth;
        healthText.text = health.ToString();
    }

    public void ChangeHealth(int amount) //use negative numbers for dmg and positive ones for heals
    {
        health += amount;
        healthAnim.Play("HPAnimation"); 
        healthText.text = health.ToString();
        if (health <= 0)
        {
            gameObject.SetActive(false);
        }
    }
}
