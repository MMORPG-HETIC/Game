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
         playerAnimator.SetFloat("walk", Input.GetAxis("Vertical"));


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
