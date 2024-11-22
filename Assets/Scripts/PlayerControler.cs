using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player Movement Configuration")]
    public float speed = 6f; // Vitesse de déplacement du joueur
    public float gravity = 20f; // Gravité appliquée au joueur
    public float mouseSensitivity = 150f; // Sensibilité de la caméra avec la souris
    public float keyboardSensitivity = 100f; // Sensibilité de la caméra avec les touches

    [Header("Shooting Configuration")]
    public Transform bulletSpawnPoint; // Point de départ des projectiles
    public GameObject bulletPrefab; // Préfabriqué de la balle
    public float bulletSpeed = 10f; // Vitesse de la balle

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

    // Gère le mouvement du joueur
    void HandleMovement()
    {
        if (characterController.isGrounded)
        {
            float moveHorizontal = Input.GetAxis("Horizontal");
            float moveVertical = Input.GetAxis("Vertical");

            moveDirection = new Vector3(moveHorizontal, 0, moveVertical);
            moveDirection = transform.TransformDirection(moveDirection) * speed;

            // Mettre à jour les animations uniquement si les paramètres existent
            if (playerAnimator != null && playerAnimator.HasParameter("MoveSpeed"))
            {
                playerAnimator.SetFloat("MoveSpeed", moveDirection.magnitude);
            }
        }

        // Appliquer la gravité
        moveDirection.y -= gravity * Time.deltaTime;

        // Déplacer le joueur
        characterController.Move(moveDirection * Time.deltaTime);
    }

    // Gère le contrôle de la caméra
    void HandleCameraControl()
    {
        // Contrôle de la caméra avec la souris
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;

        // Contrôle de la caméra avec les touches directionnelles ou numériques
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.Keypad4))
        {
            mouseX -= keyboardSensitivity * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.Keypad6))
        {
            mouseX += keyboardSensitivity * Time.deltaTime;
        }

        // Appliquer la rotation
        transform.Rotate(0, mouseX, 0);
    }

}

// Extension pour vérifier si un paramètre Animator existe
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
