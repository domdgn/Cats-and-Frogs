using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class CatAnimationController : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    //private Sprite altSprite;
    private Sprite idleSprite;
    private float lerpFactor = 1f;
    //private bool isAltSpriteActive = false;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    public void Initialise(Sprite idle, Sprite alt, RuntimeAnimatorController animController)
    {
        if (idle == null)
        {
            Debug.LogError("Idle sprite is null during initialization");
            return;
        }

        //if (alt == null)
        //{
        //    Debug.LogError("Alt sprite is null during initialization for " + idle.name);
        //    alt = idle;
        //}

        idleSprite = idle;
        //altSprite = alt;
        spriteRenderer.sprite = idleSprite;
        //isAltSpriteActive = false;

        if (animator && animController)
        {
            animator.runtimeAnimatorController = animController;
        }
    }

    public void PlayDefaultAnimation()
    {
        spriteRenderer.sprite = idleSprite;
        animator.SetTrigger("DefaultTr");
    }

    public void PlayWaitAnimation()
    {
        animator.SetTrigger("WaitTr");
    }

    public void SetSpriteColor(Color color)
    {
        spriteRenderer.color = color;
    }

    public IEnumerator HurtAnim()
    {
        SetSpriteColor(Color.red);
        yield return new WaitForSeconds(0.1f);

        while (lerpFactor > 0f)
        {
            lerpFactor -= Time.deltaTime * 5f;
            lerpFactor = Mathf.Clamp(lerpFactor, 0f, 1f);

            spriteRenderer.color = Color.Lerp(Color.red, Color.white, lerpFactor);
            lerpFactor = 0f;
        }
        SetSpriteColor(Color.white);
        lerpFactor = 1f;
    }
}
