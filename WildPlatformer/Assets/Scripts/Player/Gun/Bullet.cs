using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Bullet : MonoBehaviour
{
    [SerializeField] AnimationCurve bulletSizeCurve;
    [SerializeField] LayerMask hittableThings;
    [SerializeField] float speed;
    [SerializeField] float lifeTime;
    [SerializeField] GameObject hitEffectPrefab;
    float lifeTimeTimer;
        
    Rigidbody2D rb2D;
    CircleCollider2D circleCollider;



    void Start()
    {
        lifeTimeTimer = lifeTime;

        circleCollider = GetComponent<CircleCollider2D>();
        rb2D = GetComponent<Rigidbody2D>();
        rb2D.velocity = transform.up * speed;
        transform.DOScale(Vector2.zero, lifeTime).SetEase(bulletSizeCurve).OnComplete(DestroyMyself).SetId("normal");
    }

    private void Update()
    {
        lifeTimeTimer -= Time.deltaTime;
        if(lifeTimeTimer <= 0 && circleCollider.enabled == true)
        {
            DestroyMyself();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
            DOTween.Kill("normal");
            HitSomething();
    }

    void HitSomething()
    {
        Instantiate(hitEffectPrefab, transform.position, Quaternion.identity);
        DestroyMyself();
    }

    void DestroyMyself()
    {
        DOTween.Kill(this);
        Destroy(gameObject);
    }
}
