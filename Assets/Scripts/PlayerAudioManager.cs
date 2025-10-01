using UnityEngine;
using UnityEngine.Audio;

public class PlayerAudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioResource DeathSound;
    [SerializeField] private AudioResource DashSound;
    [SerializeField] private AudioResource DamagedSound;
    [SerializeField] private AudioResource HitSound;


    public void PlaySound(string soundName)
    {
        switch (soundName)
        {
            case "Death":
                DontDestroyOnLoad(gameObject);
                audioSource.resource = DeathSound;
                Destroy(gameObject, 4f);
                break;
            case "Dash":
                audioSource.resource = DashSound;
                break;
            case "Damaged":
                audioSource.resource = DamagedSound;
                break;
            case "Hit":
                audioSource.resource = HitSound;
                break;
        }
        audioSource.Play();
    }
}