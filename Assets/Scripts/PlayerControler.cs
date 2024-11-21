using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 6f;
    public float gravity = 20f;
    public float mouseSensitivity = 150f;

    private Vector3 moveDirection = Vector3.zero;
    private CharacterController characterController;
    private Animator playerAnimator;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        playerAnimator = GetComponent<Animator>();

        if (playerAnimator == null)
        {
            Debug.LogWarning("Aucun Animator trouvé sur le GameObject !");
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

            moveDirection = new Vector3(moveHorizontal, 0, moveVertical);
            moveDirection = transform.TransformDirection(moveDirection) * speed;

            // Mettre à jour les animations uniquement si les paramètres existent
            if (playerAnimator != null)
            {
                if (playerAnimator.HasParameter("MoveSpeed"))
                {
                    playerAnimator.SetFloat("MoveSpeed", moveDirection.magnitude);
                }
            }
        }

        moveDirection.y -= gravity * Time.deltaTime;
        characterController.Move(moveDirection * Time.deltaTime);
    }

    void HandleCameraControl()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        transform.Rotate(0, mouseX, 0);
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