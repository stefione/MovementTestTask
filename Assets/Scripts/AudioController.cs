using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioController : MonoBehaviour
{
    public static AudioController Instance { get; private set; }
    private AudioSource _source;


    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        _source = GetComponent<AudioSource>();
    }

    public void PlayOneShot(AudioClip clip,float volume)
    {
        _source.PlayOneShot(clip, volume);
    }
}
