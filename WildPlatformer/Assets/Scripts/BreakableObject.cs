using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.U2D;

public class BreakableObject : MonoBehaviour
{
    [SerializeField] Color _spriteColor;
    [SerializeField] SpriteRenderer _sr;
    [SerializeField] float _health;
    [SerializeField] SpriteShapeRenderer _srShape;

    Vector2 _srStartScale;

    private void Start()
    {
        if (_sr != null)
        {
            _srStartScale = _sr.transform.localScale;
        }
        else
        {
            _srStartScale = _srShape.transform.localScale;
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
        if(_health <= 0)
        {
            DestroyMyself();
        }
    } 

    void TakeDamage()
    {
        _health--;
        ToOriginalSize();
        if(_sr != null)
        {
            _sr.color = Color.white;
            _sr.transform.DOScale(0.6f, 0.1f).SetId("scale").OnComplete(ToOriginalSize);
        }
        else
        {
            _srShape.color = Color.white;
            _srShape.transform.DOScale(0.6f, 0.1f).SetId("scale").OnComplete(ToOriginalSize);
        }
    }
    void ToOriginalSize()
    {
        DOTween.Kill("scale");
        if (_sr != null)
        {
            _sr.color = _spriteColor;
            _sr.transform.localScale = _srStartScale;
        }
        else
        {
            _srShape.color = _spriteColor;
            _srShape.transform.localScale = _srStartScale;
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
