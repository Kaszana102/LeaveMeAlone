using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignalSource : MonoBehaviour
{
    [SerializeField]
    public List<Signalable> signalables;
    public Color color = Color.white;


    private void OnTriggerEnter2D(Collider2D other)
    {
        foreach(Signalable signalable in signalables)
        {
            signalable.ReceiveSignal();
        }
        AnimateSelf();
    }


    virtual protected void AnimateSelf(){}
}


