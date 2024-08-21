using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Bullet : MonoBehaviour
{
    [SerializeField] AnimationCurve bullerSizeCurve;
    [SerializeField] float speed;
    [SerializeField] float lifeTime;
    float lifeTimeTimer;
    [SerializeField] float hitEffectDuration;
    [SerializeField] float hitEffectSizeMultiplier;
    Rigidbody2D rb2D;
    CircleCollider2D collider;



    void Start()
    {
        lifeTimeTimer = lifeTime;

        collider = GetComponent<CircleCollider2D>();
        rb2D = GetComponent<Rigidbody2D>();
        rb2D.velocity = transform.up * speed;
        transform.DOScale(Vector2.zero, lifeTime).SetEase(bullerSizeCurve).OnComplete(DestroyMyself).SetId("normal");
    }

    private void Update()
    {
        lifeTimeTimer -= Time.deltaTime;
        if(lifeTimeTimer <= 0 && collider.enabled == true)
        {
            DestroyMyself();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Breakable")
        {
            collider.enabled = false;
            DOTween.Kill("normal");
            HitSomething();
        }
    }

    void HitSomething()
    {
        transform.localScale = Vector2.one * hitEffectSizeMultiplier;
        rb2D.velocity = Vector2.zero;
        transform.DOScale(0, hitEffectDuration).OnComplete(DestroyMyself);
    }

    void DestroyMyself()
    {
        DOTween.Kill(this);
        Destroy(gameObject);
    }
}
