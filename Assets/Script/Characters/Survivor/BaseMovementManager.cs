using UnityEngine;

public class BaseMovementManager : MonoBehaviour
{
    /*
        implement basic horizontal movement, and better jumping.

        for better horizontal movement:
            1. use rotation instead of reverse local scale to flip the sprite.
            (decoupled facing direction from local scale. big pog..........)

        for better jumping:
            1. apply coyote time. *

            2. push survivor a bit if survivor hits the ledge
                if inner raycasts hits the ceiling but outer does not. push the survivor nearer to the outer raycast that does not hit.

            3. can adjust position mid-air *
    */
    [Header("The brain")]

    [SerializeField] protected SurvivorBase survivor;

    protected SurvivorParameters parameter;

    [Header("horizontal")]

    [SerializeField] private float walkSpeed;

    [SerializeField] private float runSpeed;

    private float horizontal;

    // flip 
    private bool isFacingRight = true;

    public bool isRunning;
    [Header("Jump")]

    [SerializeField] private float jumpForce;

    // start a timer right after first jump to stop ground check for some seconds
    [SerializeField] private float maxJumpTimer;

    [SerializeField] private float curJumpTimer;
    // ground check
    [SerializeField] private LayerMask groundLayer;

    [SerializeField] private float groundCheckRadius;

    [SerializeField] private Transform groundCheckCentre;

    // to determine animation
    public bool isInMidAir;

    [Header("Jump Coyote")]
    // a big tip to improve jumping experiences
    [SerializeField] private float curCoyoteTimer;

    [SerializeField] private float maxCoyoteTimer;

    private bool isInCoyoteTime;

    [Header("Jump Ledge Buffer")]
    // big tip to improve ledge jumping
    [SerializeField] private float raycastLength;

    [SerializeField] private Transform outerBackRayCastRoot;

    [SerializeField] private Transform outerFrontRayCastRoot;

    [SerializeField] private Transform innerBackRayCastRoot;

    [SerializeField] private Transform innerFrontRayCastRoot;

    [SerializeField] private float bufferForce;

    [SerializeField] private bool isClutched;

    [SerializeField] private bool isCaptured;

    void OnEnable()
    {
        EventManager.OnClutch += GetClutched;

        EventManager.OnRelease += GetReleased;

        EventManager.OnControlSuccessful += GetCaptured;
    }
    void OnDisable()
    {
        EventManager.OnClutch -= GetClutched;

        EventManager.OnRelease -= GetReleased;

        EventManager.OnControlSuccessful -= GetCaptured;
    }
    void Start()
    {
        this.parameter = survivor.parameter;
    }
    void Update()
    {
        // to allow dynamic jumping
        HandleHorizontalMovement();

        SetHorizontal();

        Flip();

        UpdateJumpTimer();

        UpdateCoyoteTime();

        CheckLandedFromJumping();

        BufferLedge();
    }
    private void GetClutched(ClutchData data)
    {
        if (data.receiver != this.gameObject)
        {
            return;
        }

        isClutched = true;


    }

    private void GetCaptured(DetectionData data)
    {
        if (data.receiver != this.gameObject)
        {
            return;
        }
        isCaptured = true;
    }

    // do capture break free in the future;

    private void GetReleased(ClutchData data)
    {
        if (data.receiver != this.gameObject)
        {
            return;
        }

        isClutched = false;
    }
    private void HandleHorizontalMovement()
    {
        if (isRunning)
        {
            Run();
        }
        else
        {
            Walk();
        }
    }

    private void SetHorizontal()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
    }
    public void Walk(float dir)
    {
        parameter.RB.linearVelocity = new Vector2(dir * walkSpeed, parameter.RB.linearVelocity.y);

        AutomaticFlip(dir);
    }
    public void Walk()
    {
        if (!CanMove())
        {
            return;
        }
        parameter.RB.linearVelocity = new Vector2(horizontal * walkSpeed, parameter.RB.linearVelocity.y);
    }

    public void Run()
    {
        if (!CanMove())
        {
            return;
        }
        parameter.RB.linearVelocity = new Vector2(horizontal * runSpeed, parameter.RB.linearVelocity.y);
    }
    public void AutomaticFlip(float dir)
    {
        if (dir > 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);

            isFacingRight = true;
        }
        if (dir < 0)
        {
            transform.rotation = Quaternion.Euler(0, 180f, 0);

            isFacingRight = false;
        }
    }
    private void Flip()
    {
        if (!CanFlip())
        {
            return;
        }
        bool shouldFaceRight = isFacingRight;

        float newPos = 0f;

        if (horizontal > 0)
        {
            shouldFaceRight = true;
            newPos = 0f;
        }
        else if (horizontal < 0)
        {
            shouldFaceRight = false;
            newPos = -180f;
        }

        if (shouldFaceRight != isFacingRight)
        {
            // rotate survivor on the y-axis
            transform.rotation = Quaternion.Euler(0, newPos, 0);
        }

        isFacingRight = shouldFaceRight;

        parameter.isFacingRight = isFacingRight;
    }

    public void Jump()
    {
        // add force to RB
        parameter.RB.AddForce(new Vector2(0, 1f * jumpForce), ForceMode2D.Impulse);

        curJumpTimer = maxJumpTimer;

        isInMidAir = true;

        // reset coyote time to prevent abuse
        isInCoyoteTime = false;
    }



    private void UpdateJumpTimer()
    {
        if (curJumpTimer > 0)
        {
            curJumpTimer -= Time.deltaTime;
        }
    }
    private bool HasJustJumped()
    {
        // start checking IsGrounded after a buffer
        if (curJumpTimer > 0)
        {
            return true;
        }
        return false;
    }

    private void CheckLandedFromJumping()
    {
        // implement jump buffer
        if (HasJustJumped())
        {
            return;
        }

        // from the mid air to ground
        if (isInMidAir)
        {
            if (IsGrounded())
            {
                isInMidAir = false;
            }
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheckCentre.position, groundCheckRadius, groundLayer);
    }

    private void UpdateCoyoteTime()
    {
        if (IsGrounded())
        {
            curCoyoteTimer = maxCoyoteTimer;
        }
        else
        {
            if (curCoyoteTimer > 0)
            {
                // still enable jumping
                isInCoyoteTime = true;
                curCoyoteTimer -= Time.deltaTime;
            }
            if (curCoyoteTimer < 0)
            {
                isInCoyoteTime = false;
            }

        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheckCentre.position, groundCheckRadius);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(outerBackRayCastRoot.position, outerBackRayCastRoot.position + new Vector3(0, 1f * raycastLength, 0));
        Gizmos.DrawLine(outerFrontRayCastRoot.position, outerFrontRayCastRoot.position + new Vector3(0, 1f * raycastLength, 0));
        Gizmos.DrawLine(innerBackRayCastRoot.position, innerBackRayCastRoot.position + new Vector3(0, 1f * raycastLength, 0));
        Gizmos.DrawLine(innerFrontRayCastRoot.position, innerFrontRayCastRoot.position + new Vector3(0, 1f * raycastLength, 0));

    }



    public void BufferLedge()
    {
        if (!isInMidAir)
        {
            return;
        }
        Vector2 outerBack = outerBackRayCastRoot.position;

        // RaycastHit2D hit = Physics2D.RayCast(v2 origin, V2 dir, float distance, LayerMask target);
        RaycastHit2D outerBackHit = Physics2D.Raycast(outerBack, new Vector2(0, 1f), raycastLength, groundLayer);

        Vector2 outerFront = outerFrontRayCastRoot.position;
        RaycastHit2D outerFrontHit = Physics2D.Raycast(outerFront, new Vector2(0, 1f), raycastLength, groundLayer);

        Vector2 innerFront = innerFrontRayCastRoot.position;
        RaycastHit2D innerFrontHit = Physics2D.Raycast(innerFront, new Vector2(0, 1f), raycastLength, groundLayer);

        Vector2 innerBack = innerBackRayCastRoot.position;
        RaycastHit2D innerBackHit = Physics2D.Raycast(innerBack, new Vector2(0, 1f), raycastLength, groundLayer);


        /* 
            buffer ledge logic:
            if one side of inner and outer hit, push the survivor to the other side.
        */
        // define front and back
        Vector2 front = transform.right;
        Vector2 back = -front;
        if (innerBackHit && outerBackHit)
        {
            if (!(innerFrontHit && outerBackHit))
            {
                //push to the front
                // Debug.Log("back hit and applied force to front");
                parameter.RB.AddForce(front * bufferForce, ForceMode2D.Impulse);
            }
        }
        if (outerFrontHit && innerFrontHit)
        {
            if (!(innerBackHit && outerBackHit))
            {
                //push to the front
                // Debug.Log("front hit and applied force to back");
                parameter.RB.AddForce(back * bufferForce, ForceMode2D.Impulse);
            }
        }

    }


    protected virtual bool CanMove()
    {
        if (!survivor.isPlayedByPlayer)
        {
            return false;
        }
        if (isCaptured)
        {
            return false;
        }
        if (isClutched)
        {
            return false;
        }
        return true;
    }
    protected virtual bool CanFlip()
    {
        if (!survivor.isPlayedByPlayer)
        {
            return false;
        }
        if (isCaptured)
        {
            return false;
        }

        if (isInMidAir)
        {
            return false;
        }

        return true;

    }

    public virtual bool CanJump()
    {
        if (!survivor.isPlayedByPlayer)
        {
            return false;
        }
        if (isCaptured)
        {
            return false;
        }
        if (isClutched)
        {
            return false;
        }

        // check if is grounded
        if (isInCoyoteTime)
        {
            return true;
        }
        if (!IsGrounded())
        {
            return false;
        }
        // prevent double jumping
        if (HasJustJumped())
        {
            return false;
        }
        if (isInMidAir)
        {
            return false;
        }


        return true;
    }

    public void DisableLinearVelocity()
    {
        survivor.parameter.RB.linearVelocity = Vector2.zero;
    }
}
