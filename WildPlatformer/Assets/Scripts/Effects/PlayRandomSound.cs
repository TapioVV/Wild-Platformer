using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class PlayRandomSound : MonoBehaviour
{
    [SerializeField] AudioClip[] audioClips;
    AudioSource audioSource;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    // Start is called before the first frame update
    public void PlayRandomAudioClip()
    {
        int randomNumber = Random.Range(0, audioClips.Length);
        audioSource.PlayOneShot(audioClips[randomNumber]);
    }
}
