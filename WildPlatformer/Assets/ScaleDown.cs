using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

public class ScaleDown : MonoBehaviour
{
    [SerializeField] UnityEvent done;
    [SerializeField] AnimationCurve curve;
    [SerializeField] float scaleTime;
    // Start is called before the first frame update
    void Start()
    {
        transform.DOScale(Vector2.zero, scaleTime).SetEase(curve).OnComplete(DoneScaling);
    }

    void DoneScaling()
    {
        DOTween.Kill(this);
        done.Invoke();
    }
}
