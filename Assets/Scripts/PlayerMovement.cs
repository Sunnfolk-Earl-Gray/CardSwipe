using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
    public PlayerMovData data;
    public Rigidbody2D Rb { get; private set; }
    public bool isDashing;
    public bool canDash;
    public bool isStun;
    public bool isFacingRight;
    private Vector2 _moveInput;
    private InputSystem_Actions _actions;
    private void Awake()
    {
        Rb = GetComponent<Rigidbody2D>();
        _actions = new InputSystem_Actions();
        _actions.Enable();
    }
    
    private void Start()
    {
        isFacingRight = true;
        canDash = true;
    }

    private void Update()
    {
        #region INPUT HANDLER

        _moveInput = _actions.Player.Move.ReadValue<Vector2>();

        if (_moveInput.x != 0) CheckDirectionToFace(_moveInput.x > 0);

        if((_actions.Player.Jump.WasPressedThisFrame() || _actions.Player.Sprint.WasPressedThisFrame() || _actions.Player.Interact.WasPressedThisFrame()) && canDash)
        {
                StartCoroutine(Dash());
        }
        #endregion
    }

    private void FixedUpdate()
    {
        if (!isDashing)
        {
            Run(1);
        }
    }
    
    private void Run(float lerpAmount)
    {
        if (data == null)
        {
            Debug.LogError("PlayerMovData is not assigned.");
            return;
        }

        Vector2 targetSpeed = new Vector2(
            _moveInput.x * data.runMaxSpeed,
            _moveInput.y * data.runMaxSpeed
        );

        Vector2 currentVelocity = Rb.linearVelocity;
        targetSpeed = Vector2.Lerp(currentVelocity, targetSpeed, lerpAmount);

        float accelRateX = Mathf.Abs(targetSpeed.x) > 0.01f ? data.runAccelAmount : data.runDeccelAmount;
        float accelRateY = Mathf.Abs(targetSpeed.y) > 0.01f ? data.runAccelAmount : data.runDeccelAmount;

        if (data.doConserveMomentum &&
            Mathf.Abs(currentVelocity.x) > Mathf.Abs(targetSpeed.x) &&
            Mathf.Sign(currentVelocity.x) == Mathf.Sign(targetSpeed.x) &&
            Mathf.Abs(targetSpeed.x) > 0.01f)
        {
            accelRateX = 0;
        }

        if (data.doConserveMomentum &&
            Mathf.Abs(currentVelocity.y) > Mathf.Abs(targetSpeed.y) &&
            Mathf.Sign(currentVelocity.y) == Mathf.Sign(targetSpeed.y) &&
            Mathf.Abs(targetSpeed.y) > 0.01f)
        {
            accelRateY = 0;
        }

        float speedDifX = targetSpeed.x - currentVelocity.x;
        float speedDifY = targetSpeed.y - currentVelocity.y;

        float movementX = speedDifX * accelRateX;
        float movementY = speedDifY * accelRateY;

        Vector2 movement = new Vector2(movementX, movementY);

        if (float.IsNaN(movement.x) || float.IsNaN(movement.y))
        {
            Debug.LogError($"[NaN] movement={movement}, targetSpeed={targetSpeed}, currentVelocity={currentVelocity}");
            return;
        }

        Rb.AddForce(movement, ForceMode2D.Force);
    }

    IEnumerator Dash()
    {
        Vector2 dashDirection = new Vector2(_moveInput.x, _moveInput.y).normalized;
        canDash = false;
        isDashing = true;
        Rb.linearVelocity = dashDirection * data.dashSpeed;
        yield return new WaitForSeconds(data.dashDuration);
        isDashing = false;
        yield return new WaitForSeconds(data.dashCooldown);
        canDash = true;
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
        if (data.dashCooldown == 0)
        {
            return true;
        }
        return false; //might fuck with dash.
    }

    private void OnDisable()
    {
        _actions.Disable();
    }
}
