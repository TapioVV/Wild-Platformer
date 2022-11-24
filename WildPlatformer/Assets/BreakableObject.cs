using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BreakableObject : MonoBehaviour
{
    [SerializeField] Color _spriteColor;
    [SerializeField] SpriteRenderer _sr;
    [SerializeField] float _health;
    void Start()
    {
        
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
        if(_health <= 0)
        {
            DestroyMyself();
        }
    }

    void TakeDamage()
    {
        _health--;
        ToOriginalSize();
        _sr.color = Color.white;
        _sr.transform.DOScale(0.6f, 0.1f).SetId("scale").OnComplete(ToOriginalSize);
    }
    void ToOriginalSize()
    {
        DOTween.Kill("scale");
        _sr.color = _spriteColor;
        _sr.transform.localScale = Vector2.one;
    }
    
    void DestroyMyself()
    {
        DOTween.Kill("scale");
        Destroy(gameObject);
    }
}
