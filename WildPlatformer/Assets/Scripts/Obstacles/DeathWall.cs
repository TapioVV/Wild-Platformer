using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DeathWall : MonoBehaviour
{
    [SerializeField] AnimationCurve easeCurve;
    [SerializeField] BoxCollider2D boxCollider2D;
    [SerializeField] float speed;
    [SerializeField] Transform cameraTransform;
    [SerializeField] float moveToMiddleSpeed;
    [SerializeField] AudioSource audioSource;
    [SerializeField] GameObject deathWallSoundLoop;
    void Update()
    {
        transform.position = transform.position + new Vector3(speed * Time.deltaTime, 0, 0);
    }
    public void MoveWallToMiddleOfScreen()
    {
        deathWallSoundLoop.SetActive(false);
        audioSource.Play();
        boxCollider2D.enabled = false;
        speed = 0;
        transform.DOMoveX(cameraTransform.position.x, moveToMiddleSpeed).SetEase(easeCurve);
    }
}
