using UnityEngine;

public class AnimationController : MonoBehaviour
{
    private Animator playerAnimator;

    // Indique si c'est le joueur local (modifiable dans un contexte multijoueur)
    public bool isLocalPlayer = true;

    // État reçu depuis le serveur pour les joueurs distants
    private bool isMovingFromServer;

    void Awake()
    {
        playerAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        if (isLocalPlayer)
        {
            // Gestion des animations en local (entrées utilisateur)
            HandleLocalAnimation();
        }
        else
        {
            // Gestion des animations pour un joueur distant
            HandleRemoteAnimation();
        }
    }

    private void HandleLocalAnimation()
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

    private void HandleRemoteAnimation()
    {
        // Mise à jour de l'animation "walk" pour les joueurs distants
        playerAnimator.SetBool("isMoving", isMovingFromServer);
    }

    // Méthode publique pour mettre à jour l'état de mouvement depuis le serveur
    public void UpdateMovementFromServer(bool isMoving)
    {
        isMovingFromServer = isMoving;
    }
}
