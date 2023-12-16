using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAudio : MonoBehaviour
{
    [SerializeField]
    AudioClip jump, run, landing;
    AudioSource jumpAudio,runAudio,landingAudio;

    int runAudioCounter = 0;

    // Start is called before the first frame update
    void Awake()
    {
        jumpAudio = gameObject.AddComponent<AudioSource>();
        runAudio = gameObject.AddComponent<AudioSource>();
        landingAudio = gameObject.AddComponent<AudioSource>();

        jumpAudio.clip = jump;
        runAudio.clip = run;
        landingAudio.clip = landing;


        SetupSettings();
    }

    void SetupSettings()
    {
        //volume
        jumpAudio.volume = 0.15f;
        runAudio.volume = 0.2f;
        landingAudio.volume = 0.35f;

        //pitch
        jumpAudio.pitch = 0.9f;
        landingAudio.pitch = 2.1f;     
                

        runAudio.loop = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (runAudioCounter > 0) runAudioCounter--;
    }

    public void PlayJumpSound()
    {        
        jumpAudio.Play();
    }

    public void PlayRunSound()
    {
        if(runAudioCounter == 0)
        {            
            runAudio.time = Random.Range(0,run.length);
            runAudio.Play();
        }
        else
        {
            runAudioCounter = 2;
        }
        
    }

    public void PlayLandingSound()
    {
        landingAudio.Play();
    }
}
