using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.U2D;

public class BreakableObject : MonoBehaviour
{
    [SerializeField] Color spriteColor;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] float health;
    [SerializeField] SpriteShapeRenderer spriteShapeRenderer;

    Vector2 spriteStartScale;

    private void Start()
    {
        if (spriteRenderer != null)
        {
            spriteStartScale = spriteRenderer.transform.localScale;
        }
        else
        {
            spriteStartScale = spriteShapeRenderer.transform.localScale;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "PlayerBullet")
        {
            TakeDamage();
            
        }
    }

    private void Update()
    {
        if(health <= 0)
        {
            DestroyMyself();
        }
    } 

    void TakeDamage()
    {
        health--;
        ToOriginalSize();
        if(spriteRenderer != null)
        {
            spriteRenderer.color = Color.white;
            spriteRenderer.transform.DOScale(0.6f, 0.1f).SetId("scale").OnComplete(ToOriginalSize);
        }
        else
        {
            spriteShapeRenderer.color = Color.white;
            spriteShapeRenderer.transform.DOScale(0.6f, 0.1f).SetId("scale").OnComplete(ToOriginalSize);
        }
    }
    void ToOriginalSize()
    {
        DOTween.Kill("scale");
        if (spriteRenderer != null)
        {
            spriteRenderer.color = spriteColor;
            spriteRenderer.transform.localScale = spriteStartScale;
        }
        else
        {
            spriteShapeRenderer.color = spriteColor;
            spriteShapeRenderer.transform.localScale = spriteStartScale;
        }
    }
    
    void DestroyMyself()
    {
        DOTween.Kill("scale");
        if(transform.parent != null)
        {
            Destroy(transform.parent.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
