using System;
using TMPro;
using Unity.Mathematics;

using UnityEngine;
using UnityEngine.Events;

public class PlayerManager : MonoBehaviour
{
    public int health;
    public int maxHealth;
    public TMP_Text healthText;
    public TMP_Text scoreText;
    public Animator healthAnim;
    [SerializeField] private PlayerAudioManager _audioManager;
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
        if (amount < 0) _audioManager.PlaySound("Damaged");
        if (health <= 0)
        {
            _audioManager.PlaySound("Death");
            gameObject.SetActive(false);
        }
    }
}
