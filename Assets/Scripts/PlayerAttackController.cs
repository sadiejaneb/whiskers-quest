using UnityEngine;

public class PlayerAttackController : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            animator.SetTrigger("Punch");
        }

        if (Input.GetButtonDown("Fire2"))
        {
            animator.SetTrigger("Kick");
        }
    }
}