using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.U2D;

public class BreakableObject : MonoBehaviour
{
    [SerializeField] Color spriteColor;
    [SerializeField] SpriteRenderer sr;
    [SerializeField] float health;
    [SerializeField] SpriteShapeRenderer srShape;

    Vector2 srStartScale;

    private void Start()
    {
        if (sr != null)
        {
            srStartScale = sr.transform.localScale;
        }
        else
        {
            srStartScale = srShape.transform.localScale;
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
        if(sr != null)
        {
            sr.color = Color.white;
            sr.transform.DOScale(0.6f, 0.1f).SetId("scale").OnComplete(ToOriginalSize);
        }
        else
        {
            srShape.color = Color.white;
            srShape.transform.DOScale(0.6f, 0.1f).SetId("scale").OnComplete(ToOriginalSize);
        }
    }
    void ToOriginalSize()
    {
        DOTween.Kill("scale");
        if (sr != null)
        {
            sr.color = spriteColor;
            sr.transform.localScale = srStartScale;
        }
        else
        {
            srShape.color = spriteColor;
            srShape.transform.localScale = srStartScale;
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
