using UnityEngine;

[CreateAssetMenu(menuName = "Player Data")]
public class PlayerMovData : ScriptableObject
{
    [Header("Run")]
    public float RunMaxSpeed; //Target speed we want the player to reach.
    public float RunAcceleration; //The speed at which our player accelerates to max speed, can be set to runMaxSpeed for instant acceleration down to 0 for none at all
    [HideInInspector] public float RunAccelAmount; //The actual force (multiplied with speedDiff) applied to the player.
    public float RunDecceleration; //The speed at which our player decelerates from their current speed, can be set to runMaxSpeed for instant deceleration down to 0 for none at all
    [HideInInspector] public float RunDeccelAmount; //Actual force (multiplied with speedDiff) applied to the player .
    
    [Space(20)]
    
    [Header("Dash")]
    public float DashSpeed;
    public float DashLength;
    public float DashAcceleration;
    public float DashAccelAmount;
    public float DashDecceleration;
    public float DashDeccelAmount;
    public float DashCooldown;
    
}
