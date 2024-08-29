using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class WinWall : MonoBehaviour
{
    [SerializeField] Transform cameraTransform;
    [SerializeField] AnimationCurve easeCurve;
    [SerializeField] float moveToMiddleSpeed;
    [SerializeField] AudioSource audioSource;
    public void MoveWallToMiddleOfScreen()
    {
        audioSource.Play();
        transform.DOMoveY(cameraTransform.position.y, moveToMiddleSpeed).SetEase(easeCurve);
    }
}
