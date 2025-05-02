using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FirstPersonController : MonoBehaviour
{
    [Header("Movimiento")]
    public float speed = 5f;
    public float jumpHeight = 2f;
    public float gravity = -9.81f;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;


    [Header("Efectos")]
    public AnimationCurve bobCurve;
    public float bobDuration = 1f; 

    public float bobAmplitude = 0.05f;
    public Transform cameraHolder;

    private float bobTimer = 0f;
    private Vector3 defaultCamLocalPos;


    [Header("Dash")]
    public float dashSpeed = 20f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;

    private bool isDashing = false;
    private float dashTimer = 0f;
    private float dashCooldownTimer = 0f;
    private Vector3 dashDirection;

    [Header("Ladder")]
    public float climbSpeed = 3f;

    private bool isClimbing = false;
    private Collider currentLadder;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        defaultCamLocalPos = cameraHolder.localPosition;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        //La verificación SIEMPRE inicial en el Update
        isGrounded = controller.isGrounded;


        if (dashCooldownTimer > 0f)
            dashCooldownTimer -= Time.deltaTime;

        if (currentLadder && Input.GetKey(KeyCode.W))
        {
            isClimbing = true;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && dashCooldownTimer <= 0f && !isDashing)
        {
            isDashing = true;
            dashTimer = dashDuration;
            dashCooldownTimer = dashCooldown;

            Vector3 inputDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
            dashDirection = inputDir.magnitude > 0.1f ? inputDir.normalized : transform.forward;
            dashDirection = transform.TransformDirection(dashDirection);
        }

        if (isClimbing)
        {
            if (Input.GetAxis("Vertical") < 0f && isGrounded)
            {
                isClimbing = false;
                return;
            }

            if (Input.GetAxis("Vertical") > 0f)
            {
                Vector3 climbDirection = Vector3.up;
            controller.Move(climbDirection * climbSpeed * Time.deltaTime);

                velocity.y = 0f;
            }

            if (Input.GetAxis("Vertical") < 0f)
            {
                Vector3 climbDirection = -transform.up;
                controller.Move(climbDirection * climbSpeed * Time.deltaTime);

                velocity.y = 0f;
            }

            if (Input.GetAxis("Horizontal") > 0f)
            {
                Vector3 climbDirection = transform.right;
                controller.Move(climbDirection * climbSpeed * Time.deltaTime);

                velocity.y = 0f;
            }

            if (Input.GetAxis("Horizontal") < 0f)
            {
                Vector3 climbDirection = -transform.right;
                controller.Move(climbDirection * climbSpeed * Time.deltaTime);

                velocity.y = 0f;
            }

            


            if (Input.GetKeyDown(KeyCode.Space))
            {
                isClimbing = false;
            }

            return; 
        }

        if (isDashing)
        {
            controller.Move(dashDirection * dashSpeed * Time.deltaTime);

            dashTimer -= Time.deltaTime;
            if (dashTimer <= 0f)
            {
                isDashing = false;
            }

            return; 
        }

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 move = transform.right * horizontal + transform.forward * vertical;
        controller.Move(move * speed * Time.deltaTime);



        if (isGrounded && velocity.y < 0)
        {
            velocity.y = 0f;
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y += Mathf.Sqrt(jumpHeight * -2.0f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
        bool isMoving = move.magnitude > 0.1f && controller.isGrounded;



        if (isMoving)
        {
            bobTimer += Time.deltaTime;
            if (bobTimer > bobDuration)
                bobTimer -= bobDuration;

            float curveTime = bobTimer / bobDuration;
            float bobValue = bobCurve.Evaluate(curveTime);
            cameraHolder.localPosition = defaultCamLocalPos + new Vector3(0f, bobValue * bobAmplitude, 0f);
        }
        else
        {
            bobTimer = 0f;
            cameraHolder.localPosition = Vector3.Lerp(cameraHolder.localPosition, defaultCamLocalPos, Time.deltaTime * 10f);
        }

        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ladder"))
        {
            currentLadder = other;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other == currentLadder)
        {
            isClimbing = false;
            currentLadder = null;
        }
    }
}
