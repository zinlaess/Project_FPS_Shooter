using UnityEngine;


[RequireComponent(typeof(CharacterController))]

public class SC_CharacterController : MonoBehaviour
{
    public float speed = 7.5f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;
    public Transform playerCameraParent;
    public float lookSpeed = 2.0f;
    public float lookXLimit = 45.0f;
    public SC_WeaponManager weaponManager;
    private Animator anim;
    private bool playerMov = true;
    private bool playerJump = false;
    CharacterController characterController;
    Vector3 moveDirection = Vector3.zero;
    Vector2 rotation = Vector2.zero;
    private bool Dying = false;

    [HideInInspector]
    public bool canMove = true;
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        characterController = GetComponent<CharacterController>();
        rotation.y = transform.eulerAngles.y;
    }

    void Update()
    {
        if (Dying)
            return;

        if (characterController.isGrounded)
        {
            if (playerJump)
            {
                weaponManager.WeaponPosStabilizationisGrounded();
                playerJump = false;
            }
            if (Input.GetAxis("Vertical") == 0 && Input.GetAxis("Horizontal") == 0)
            {
                if (!playerMov)
                {
                    weaponManager.WeaponPosStabilizationDefault();
                    playerMov = true;
                }
                
                anim.SetBool("Mov", false);
                anim.SetBool("Default", true);
            }
            else
            {
                if (playerMov)
                {
                    weaponManager.WeaponPosStabilizationMov();
                    playerMov = false;
                }
                anim.SetBool("Default", false);
                anim.SetBool("Mov", true);
            }
                
            if (Input.GetKey(KeyCode.LeftShift))
                speed = 11f;
            else
                speed = 7.5f;
            // We are grounded, so recalculate move direction based on axes
            Vector3 forward = transform.TransformDirection(Vector3.forward);
            Vector3 right = transform.TransformDirection(Vector3.right);
            float curSpeedX = canMove ? speed * Input.GetAxis("Vertical") : 0;
            float curSpeedY = canMove ? speed * Input.GetAxis("Horizontal") : 0;
            moveDirection = (forward * curSpeedX) + (right * curSpeedY);

            if (Input.GetButton("Jump") && canMove)
            {
                weaponManager.WeaponPosStabilizationJump();
                moveDirection.y = jumpSpeed;
                anim.SetTrigger("Jump");
                playerJump = true;
            }
        }

        // Apply gravity. Gravity is multiplied by deltaTime twice (once here, and once below
        // when the moveDirection is multiplied by deltaTime). This is because gravity should be applied
        // as an acceleration (ms^-2)
        moveDirection.y -= gravity * Time.deltaTime;

        // Move the controller
        characterController.Move(moveDirection * Time.deltaTime);

        // Player and Camera rotation
        if (canMove)
        {
            rotation.y += Input.GetAxis("Mouse X") * lookSpeed;
            rotation.x += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotation.x = Mathf.Clamp(rotation.x, -lookXLimit, lookXLimit);
            playerCameraParent.transform.localRotation = Quaternion.Euler(rotation.x, 0, 0);
            transform.eulerAngles = new Vector2(0, rotation.y);
        }
    }
    public void PlayerDyingAnim()
    {
        anim.SetBool("Mov", false);
        anim.SetBool("Default", false);
        anim.SetTrigger("Dying");
    }
}