using UnityEngine;

public class AnimationController : MonoBehaviour
{
    private Animator playerAnimator;

    void Awake()
    {
        playerAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        // Gestion de l'animation de la marche
         playerAnimator.SetFloat("walk", Input.GetAxis("Vertical"));

        // Gestion de l'animation de recul
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            playerAnimator.SetBool("isBacking", true);
        }
        else
        {
            playerAnimator.SetBool("isBacking", false);
        }
    }
}
