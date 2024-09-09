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
    [SerializeField] float scaleTimeOnDeath;
    float scaleTimerOnDeath;

    BoxCollider2D boxCollider2D;

    Vector2 srStartScale;

    private void Start()
    {
        scaleTimerOnDeath = scaleTimeOnDeath;
        boxCollider2D = GetComponent<BoxCollider2D>();
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
        sr.color = Color.white;
        if (health <= 0)
        {
            DestroyMyself();
        }
    }
    void Update()
    {
        if (health <= 0)
        {
            scaleTimerOnDeath -= Time.deltaTime;
        }
        if (scaleTimerOnDeath <= 0)
        {
            DOTween.Kill("scale");
            Destroy(gameObject);
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
        boxCollider2D.enabled = false;
        sr.transform.DOScale(0, scaleTimeOnDeath).SetId("scale").OnComplete(ActuallyDestroyMyself);
    }
    void ActuallyDestroyMyself()
    {
        DOTween.Kill("scale");

        Destroy(gameObject);
    }
}
