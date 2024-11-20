using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 6f; // Vitesse de déplacement
    public float mouseSensitivity = 100f; // Sensibilité ajustée de la souris
    public float gravity = 20f; // Gravité appliquée

    private Vector3 moveDirection = Vector3.zero; // Direction de mouvement
    private CharacterController characterController; // Composant CharacterController
    private Animator playerAnimator; // Composant Animator pour gérer les animations

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        playerAnimator = GetComponent<Animator>();

        if (playerAnimator == null)
        {
            Debug.LogWarning("Aucun composant Animator trouvé sur le GameObject.");
        }

        // Bloquer le curseur pour un contrôle fluide
        Cursor.lockState = CursorLockMode.Locked;
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
            // Récupérer les inputs pour le déplacement
            float moveHorizontal = Input.GetAxis("Horizontal"); // Gauche/Droite
            float moveVertical = Input.GetAxis("Vertical"); // Avant/Arrière

            moveDirection = new Vector3(moveHorizontal, 0, moveVertical);
            moveDirection = transform.TransformDirection(moveDirection) * speed;

            // Mettre à jour les animations en fonction de la direction du mouvement
            if (playerAnimator != null)
            {
                playerAnimator.SetFloat("MoveSpeed", moveDirection.magnitude); // Animation de marche (avant/arrière)
                playerAnimator.SetFloat("MoveDirection", moveHorizontal); // Animation gauche/droite
            }
        }

        // Appliquer la gravité
        moveDirection.y -= gravity * Time.deltaTime;

        // Déplacer le joueur
        characterController.Move(moveDirection * Time.deltaTime);
    }

    void HandleCameraControl()
    {
        // Récupérer le mouvement horizontal de la souris
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;

        // Réduire l'effet de la souris en divisant par une valeur ajustable (100 dans ce cas)
        mouseX = mouseX / 2f;

        // Rotation horizontale du joueur
        transform.Rotate(0, mouseX, 0);
    }
}
