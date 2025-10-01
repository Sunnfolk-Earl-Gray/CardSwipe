using System.Collections;
using UnityEngine;

public class HitStop : MonoBehaviour
{
    public static HitStop Instance;

    private float hitStopTimer = 0f;
    private bool isHitStopping = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (isHitStopping)
        {
            hitStopTimer -= Time.unscaledDeltaTime;

            if (hitStopTimer <= 0f)
            {
                ResumeTime();
            }
        }
    }

    public void Stop(float duration)
    {
        // Accumulate time if already stopping
        if (isHitStopping)
        {
            hitStopTimer = Mathf.Max(hitStopTimer, duration);
            return;
        }

        hitStopTimer = duration;
        isHitStopping = true;
        Time.timeScale = 0f;
    }

    private void ResumeTime()
    {
        Time.timeScale = 1f;
        isHitStopping = false;
    }
}