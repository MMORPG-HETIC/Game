using UnityEngine;

public class AnimationController : MonoBehaviour
{
    private Animator playerAnimator;

    void Awake()
    {
        // Initialiser l'Animator
        playerAnimator = GetComponent<Animator>();
        if (playerAnimator == null)
        {
            Debug.LogError("Aucun composant Animator trouvé sur le GameObject !");
        }
    }

    void Update()
    {
        if (playerAnimator == null) return;

        // Gestion des animations basées sur les entrées utilisateur
        if (HasParameter(playerAnimator, "walk"))
        {
            playerAnimator.SetFloat("walk", Input.GetAxis("Vertical"));
        }

        if (HasParameter(playerAnimator, "strafe"))
        {
            playerAnimator.SetFloat("strafe", Input.GetAxis("Horizontal"));
        }

        // Animation pour la marche arrière
        if (HasParameter(playerAnimator, "isBacking"))
        {
            playerAnimator.SetBool("isBacking", Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow));
        }
    }

    // Vérifie si un paramètre existe dans l'Animator
    private bool HasParameter(Animator animator, string paramName)
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