using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Bullet : MonoBehaviour
{
    [SerializeField] AnimationCurve _bulletSizeCurve;
    [SerializeField] float _speed;
    [SerializeField] float _lifeTime;
    Rigidbody2D _rb2D;

    void Start()
    {
        _rb2D = GetComponent<Rigidbody2D>();
        _rb2D.velocity = transform.up * _speed;
        transform.DOScale(Vector2.zero, _lifeTime).SetEase(_bulletSizeCurve).OnComplete(DestroyMyself);
    }

    void DestroyMyself()
    {
        Destroy(gameObject);
    }
}
