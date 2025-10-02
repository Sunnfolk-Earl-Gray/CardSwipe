using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class PlayerMovement : MonoBehaviour
{
    public PlayerMovData data;
    public Rigidbody2D Rb { get; private set; }
    public bool isDashing;
    private bool isRunning;
    public bool isStun;
    public bool isFacingRight;    
    public Animator anim;
    public static bool IsInvul;
    public float dashCooldownTimer;
    private Vector2 _moveInput;
    private InputSystem_Actions _actions;
    public GameObject dashTrailPrefab; 
    
    private void Awake()
    {
        Rb = GetComponent<Rigidbody2D>();
        _actions = new InputSystem_Actions();
        _actions.Enable();
    }
    
    private void Start()
    {
        isFacingRight = true;
        dashCooldownTimer = data.dashCooldown;
    }

    private void Update()
    {
        if (!isStun)
        {
            #region INPUT HANDLER

            _moveInput = _actions.Player.Move.ReadValue<Vector2>();

            if (_moveInput.x != 0) CheckDirectionToFace(_moveInput.x > 0);

            if ((_actions.Player.Jump.WasPressedThisFrame() || _actions.Player.Sprint.WasPressedThisFrame() ||
                 _actions.Player.Interact.WasPressedThisFrame()) && dashCooldownTimer <= 0)
            {
                StartCoroutine(Dash());
            }
        }

        #endregion
        
        if (dashCooldownTimer > 0)
        {
            dashCooldownTimer -= Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        if (!isDashing && !isStun)
        {
            Run(1);
        }
        
        anim.SetFloat("horizontal", Mathf.Abs(Rb.linearVelocity.x));
        anim.SetFloat("vertical", Mathf.Abs(Rb.linearVelocity.y));
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
        // gameObject.GetComponent<PlayerAudioManager>().PlaySound("Dash");
        Vector2 dashDirection = new Vector2(_moveInput.x, _moveInput.y).normalized;
        if (dashDirection != Vector2.zero)
        {
            isDashing = true;
            anim.Play("Dash");
            dashCooldownTimer = data.dashCooldown;
            gameObject.layer = LayerMask.NameToLayer("Dash");
            Rb.linearVelocity = dashDirection * data.dashSpeed;
            yield return new WaitForSeconds(data.dashDuration);
            PlayDashEffect(dashDirection, dashDistance: 0.5f);
            gameObject.layer = LayerMask.NameToLayer("Player");
            isDashing = false;
        }
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

    #region Knockback/Stun
    public void Knockback(Transform enemy, float knockbackForce, float knockbackTime, float stunTime)
    {
        isStun = true;
        StartCoroutine(StunTimer(knockbackTime, stunTime));
        Vector2 direction = (transform.position - enemy.position).normalized; 
        Rb.linearVelocity = direction * knockbackForce;
        Debug.Log("Knockback applied");
    }

    IEnumerator StunTimer(float knockbackTime, float stunTime)
    {
        IsInvul = true;
        yield return new WaitForSeconds(knockbackTime);
        Rb.linearVelocity = Vector2.zero;
        yield return new WaitForSeconds(stunTime);
        isStun = false;
        IsInvul = false;
    }
    #endregion
    
    void PlayDashEffect(Vector2 dashDirection, float dashDistance)
    {
        float angle = Mathf.Atan2(dashDirection.y, dashDirection.x) * Mathf.Rad2Deg;
        // Compute spawn position — center of the dash path
        Vector3 startPosition = transform.position;
        Vector3 endPosition = startPosition + (Vector3)(dashDirection.normalized * dashDistance);
        Vector3 midPoint = (startPosition + endPosition) / 2f;
        
        GameObject trail = Instantiate(dashTrailPrefab, midPoint, Quaternion.Euler(0, 0, angle));
        trail.transform.localScale = new Vector3(dashDistance, 1f, 1f);
        
        Destroy(trail, 1f); 
    }
        
        
    private void OnDisable()
    {
        _actions.Disable();
    }
}