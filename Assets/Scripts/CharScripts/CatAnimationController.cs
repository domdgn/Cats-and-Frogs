using System.Collections;
using UnityEngine;

public class CatAnimationController : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    public void Initialise(Sprite idleSprite, RuntimeAnimatorController animController)
    {
        if (spriteRenderer && idleSprite)
        {
            spriteRenderer.sprite = idleSprite;
        }

        if (animator && animController)
        {
            animator.runtimeAnimatorController = animController;
        }
    }

    public void PlayFireAnimation()
    {
        if (animator)
        {
            animator.SetTrigger("FireTr");
        }
    }
}