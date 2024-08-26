using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.U2D;

public class BreakableObject : MonoBehaviour
{
    Color spriteOriginalColor;
    [SerializeField] float health;
    [SerializeField] SpriteRenderer sr;
    [SerializeField] SpriteShapeRenderer srShape;
    [SerializeField] float scaleMultiplier;
    [SerializeField] float scaleTime;

    Vector2 srStartScale;

    private void Start()
    {

        if (sr != null)
        {
            srStartScale = sr.transform.localScale;
            spriteOriginalColor = sr.color;
        }
        else
        {
            srStartScale = srShape.transform.localScale;
            spriteOriginalColor = srShape.color;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        TakeDamage();
    }

    void TakeDamage()
    {
        health--;
        ToOriginalSize();
        if (sr != null)
        {
            sr.color = Color.white;
            sr.transform.DOScale(scaleMultiplier, scaleTime).SetId("scale").OnComplete(ToOriginalSize);
        }
        else
        {
            srShape.color = Color.white;
            srShape.transform.DOScale(scaleMultiplier, scaleTime).SetId("scale").OnComplete(ToOriginalSize);
        }
        if (health <= 0)
        {
            DestroyMyself();
        }
    }
    void ToOriginalSize()
    {
        DOTween.Kill("scale");
        if (sr != null)
        {
            sr.color = spriteOriginalColor;
            sr.transform.localScale = srStartScale;
        }
        else
        {
            srShape.color = spriteOriginalColor;
            srShape.transform.localScale = srStartScale;
        }
    }

    void DestroyMyself()
    {
        DOTween.Kill("scale");
        if (transform.parent != null)
        {
            Destroy(transform.parent.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
