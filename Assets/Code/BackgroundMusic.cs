using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    [SerializeField]
    List<AudioClip> songs;
    void Start()
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        int index = Random.Range(0,songs.Count);
        audioSource.clip = songs[index];
        audioSource.Play();
    }
}
