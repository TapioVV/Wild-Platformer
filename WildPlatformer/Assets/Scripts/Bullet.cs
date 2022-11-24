using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Bullet : MonoBehaviour
{
    [SerializeField] AnimationCurve _bulletSizeCurve;
    [SerializeField] float _speed;
    [SerializeField] float _lifeTime;
    float _lifeTimeTimer;
    [SerializeField] float _hitEffectDuration;
    [SerializeField] float _hitEffectSizeMultiplier;
    Rigidbody2D _rb2D;
    CircleCollider2D _cc;



    void Start()
    {
        _lifeTimeTimer = _lifeTime;

        _cc = GetComponent<CircleCollider2D>();
        _rb2D = GetComponent<Rigidbody2D>();
        _rb2D.velocity = transform.up * _speed;
        transform.DOScale(Vector2.zero, _lifeTime).SetEase(_bulletSizeCurve).OnComplete(DestroyMyself).SetId("normal");
    }

    private void Update()
    {
        _lifeTimeTimer -= Time.deltaTime;
        if(_lifeTimeTimer <= 0 && _cc.enabled == true)
        {
            DestroyMyself();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Breakable")
        {
            _cc.enabled = false;
            DOTween.Kill("normal");
            HitSomething();
        }
    }

    void HitSomething()
    {
        transform.localScale = Vector2.one * _hitEffectSizeMultiplier;
        _rb2D.velocity = Vector2.zero;
        transform.DOScale(0, _hitEffectDuration).OnComplete(DestroyMyself);
    }

    void DestroyMyself()
    {
        DOTween.Kill(this);
        Destroy(gameObject);
    }
}
