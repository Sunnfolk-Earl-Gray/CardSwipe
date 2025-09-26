using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public PlayerMovData Data;
    public Rigidbody2D RB;
    public bool isDashing;
    public bool isStun;
    public bool isFacingRight;
    private Vector2 _moveInput;
    public InputSystem_Actions Actions;
    
    private void Awake()
    {
        RB = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        isFacingRight = true;
    }

    private void Update()
    {
        #region INPUT HANDLER
        _moveInput.x = Input.GetAxisRaw("Horizontal");
        _moveInput.y = Input.GetAxisRaw("Vertical");

        if (_moveInput.x != 0) CheckDirectionToFace(_moveInput.x > 0);

        if(Actions.Player.Jump.WasPressedThisFrame())
        {
            //Dash();
        }
        #endregion
    }

    private void FixedUpdate()
    {
      
    }
    
    private void Run(float lerpAmount)
    {
        //calc the direction we want to move in and our desired velocity
        float targetSpeed = _moveInput.x * Data.RunMaxSpeed;
        //We can reduce are control using Lerp() this smooths changes to are direction and speed
        targetSpeed = Mathf.Lerp(RB.linearVelocity.x, targetSpeed, lerpAmount);

        #region Calculate AccelRate
        float accelRate;

        //Gets an acceleration value based on if we are accelerating (includes turning) 
        //or trying to decelerate (stop).
        accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? Data.RunAccelAmount : Data.RunDeccelAmount;

        #endregion
    }

    private void Turn()
    {
        //stores scale and flips the player along the x-axis, 
        Vector3 scale = transform.localScale; 
        scale.x *= -1;
        transform.localScale = scale;
        isFacingRight = !isFacingRight;
    }
        
    public void CheckDirectionToFace(bool isMovingRight) 
    {
        if (isMovingRight != isFacingRight) 
        {
            Turn();
        }
    }

    public bool CanDash()
    {
        if (Data.DashCooldown == 0)
        {
            return true;
        }
        return false; //might fuck with dash.
    }
}
