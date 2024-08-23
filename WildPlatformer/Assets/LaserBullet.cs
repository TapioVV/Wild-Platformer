using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LaserBullet : MonoBehaviour
{
    [SerializeField] LineRenderer line;
    [SerializeField] LayerMask hittableThings;
    [SerializeField] float lifeTime;
    [SerializeField] AnimationCurve bulletSizeCurve;

    [SerializeField] GameObject LaserHitEffectPrefab;

    
    
    void Start()
    {
        RaycastHit2D hit2D = Physics2D.Raycast(transform.position, transform.up,10000, hittableThings);
        line.SetPosition(0, transform.position);

        if(hit2D.collider == null)
        {
            Debug.Log("EI osunut maahan");
            line.SetPosition(1, transform.position + transform.up * 200);
        }
        if (hit2D.collider != null)
        {
            Debug.Log("osui maahan");
            Instantiate(LaserHitEffectPrefab, hit2D.point, Quaternion.identity);
            line.SetPosition(1, hit2D.point);
        }

        transform.DOScale(Vector2.zero, lifeTime).SetEase(bulletSizeCurve).OnComplete(DestroyMyself);
    }
    void DestroyMyself()
    {
        DOTween.Kill(this);
        Destroy(gameObject);
    }
}



