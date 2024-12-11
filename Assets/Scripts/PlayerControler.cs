using UnityEngine;

public class PlayerControler : MonoBehaviour
{
    [Header("Player Movement Configuration")]
    public float speed = 6f;
    public float gravity = 20f;
    public float mouseSensitivity = 150f;
    public float keyboardSensitivity = 50f;
    public float rotationSpeed = 100f;
    public float Jumpspeed = 8f;

    private Vector3 moveDirection = Vector3.zero;
    private CharacterController characterController;
    private Animator playerAnimator;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        playerAnimator = GetComponent<Animator>();

        if (playerAnimator == null)
        {
            Debug.LogWarning("Aucun Animator trouv? sur le GameObject !");
        }
    }

    void Update()
    {
        HandleMovement();
        HandleCameraControl();
    }

    void HandleMovement()
    {
        if (characterController.isGrounded)
        {
            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");

            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow) ||
                Input.GetKey(KeyCode.Keypad4) || Input.GetKey(KeyCode.Keypad6))
            {
                moveHorizontal = 0;
            }

            moveDirection = new Vector3(moveHorizontal, 0, moveVertical);
            moveDirection = transform.TransformDirection(moveDirection) * speed;

            if (playerAnimator != null && playerAnimator.HasParameter("MoveSpeed"))
            {
                playerAnimator.SetFloat("MoveSpeed", moveDirection.magnitude);
            }

            if (Input.GetButton("Jump"))
            {
                moveDirection.y = Jumpspeed;
            }
        }

        moveDirection.y -= gravity * Time.deltaTime;

        characterController.Move(moveDirection * Time.deltaTime);
    }

    void HandleCameraControl()
    {
        float rotationInput = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;

        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.Keypad4))
        {
            rotationInput -= rotationSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.Keypad6))
        {
            rotationInput += rotationSpeed * Time.deltaTime;
        }

        transform.Rotate(0, rotationInput, 0);
    }
}

public static class AnimatorExtensions
{
    public static bool HasParameter(this Animator animator, string paramName)
    {
        foreach (var param in animator.parameters)
        {
            if (param.name == paramName)
            {
                return true;
            }
        }
        return false;
    }
}

