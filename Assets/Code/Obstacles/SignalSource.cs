using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignalSource : MonoBehaviour
{
    [SerializeField]
    public List<Signalable> signalables;
    public Color color = Color.white;
    [SerializeField] AudioClip signalSound;
    AudioSource audio;
    private void Start()
    {
        audio = gameObject.AddComponent<AudioSource>();
        audio.clip = signalSound;
        audio.volume = 0.2f;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        audio.Play();
        foreach(Signalable signalable in signalables)
        {
            signalable.ReceiveSignal();
        }
        AnimateSelf();
    }


    virtual protected void AnimateSelf(){}
}


