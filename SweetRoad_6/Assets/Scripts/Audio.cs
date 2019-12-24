using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio : MonoBehaviour
{
    public static Audio instance;
    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
            instance = this;
    }

    public void PlayMatchSound()
    {
        GetComponents<AudioSource>()[0].Play();
    }
    public void PlayDropSound()
    {
        GetComponents<AudioSource>()[1].Play();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
