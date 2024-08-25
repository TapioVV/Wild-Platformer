using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class LaserBullet : MonoBehaviour
{
    public static Action<float> OnLaserJump;
    [SerializeField] float laserJumpAmount;
    [SerializeField] LineRenderer line;
    [SerializeField] LayerMask hittableThings;
    [SerializeField] float lifeTime;
    [SerializeField] AnimationCurve bulletSizeCurve;

    [SerializeField] GameObject LaserHitEffectPrefab;

    
    
    void Start()
    {
        RaycastHit2D hit2D = Physics2D.Raycast(transform.position, transform.up,10000, hittableThings);
        

        if(hit2D.collider == null)
        {
            ShootLaser(transform.position + transform.up * 200);
        }
        if (hit2D.collider != null)
        {
            ShootLaser(hit2D.point);
            Instantiate(LaserHitEffectPrefab, hit2D.point, Quaternion.identity);
        }

        transform.DOScale(Vector2.zero, lifeTime).SetEase(bulletSizeCurve).OnComplete(DestroyMyself);
    }
    void ShootLaser(Vector2 lineEndPosition)
    {
        line.SetPosition(0, transform.position);
        line.SetPosition(1, lineEndPosition);
        OnLaserJump?.Invoke(laserJumpAmount);
    }
    void DestroyMyself()
    {
        DOTween.Kill(this);
        Destroy(gameObject);
    }
}



