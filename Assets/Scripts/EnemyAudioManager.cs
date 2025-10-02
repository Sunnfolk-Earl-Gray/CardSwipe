using UnityEngine;
using UnityEngine.Audio;

public class EnemyAudioManager : MonoBehaviour
{
    private AudioManager audioManager
    {
        get { return AudioManager.instance; }
    }

    [SerializeField] private AudioResource Death;

    private void OnDestroy()
    {
        audioManager.sfxSource.resource = Death;
        audioManager.sfxSource.Play();
    }
}
