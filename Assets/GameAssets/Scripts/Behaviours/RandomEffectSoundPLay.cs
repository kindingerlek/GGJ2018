using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomEffectSoundPLay : MonoBehaviour {

    public List<AudioClip> audios;
    public float hitVol = 0.8f;
    public AudioSource source;
    public int  oneInToPlay = 4;
    public float timeToTry = 3;

    void Awake()
    {
        InvokeRepeating("RandomRun", timeToTry, timeToTry);
    }

    void RandomRun()
    {
        int x = Random.Range(0, oneInToPlay -1);

        if (x == 0)
        {

            source.PlayOneShot(audios.PickRandom<AudioClip>(), hitVol);

        }
    }
}
