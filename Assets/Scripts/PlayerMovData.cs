using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "Player Data")]
public class PlayerMovData : ScriptableObject
{
    [Header("Run")]
    public float runMaxSpeed; //Target speed we want the player to reach.
    public float runAcceleration; //The speed at which our player accelerates to max speed, can be set to runMaxSpeed for instant acceleration down to 0 for none at all
    [HideInInspector] public float runAccelAmount; //The actual force (multiplied with speedDiff) applied to the player.
    public float runDecceleration; //The speed at which our player decelerates from their current speed, can be set to runMaxSpeed for instant deceleration down to 0 for none at all
    [HideInInspector] public float runDeccelAmount; //Actual force (multiplied with speedDiff) applied to the player .
    public bool doConserveMomentum = true;

    [FormerlySerializedAs("DashSpeed")]
    [Space(20)]
    
    [Header("Dash")]
    public float dashSpeed;
    public float dashDuration;
    public float dashCooldown;
    
    private void OnValidate()
    {
        //Calculate are run acceleration & deceleration forces using formula: amount = ((1 / Time.fixedDeltaTime) * acceleration) / runMaxSpeed
        runAccelAmount = (50 * runAcceleration) / runMaxSpeed;
        runDeccelAmount = (50 * runDecceleration) / runMaxSpeed;

        runAcceleration = Mathf.Clamp(runAcceleration, 0.01f, runMaxSpeed);
        runDecceleration = Mathf.Clamp(runDecceleration, 0.01f, runMaxSpeed);
    }
}