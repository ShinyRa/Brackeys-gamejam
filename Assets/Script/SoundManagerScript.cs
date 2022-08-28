using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManagerScript : MonoBehaviour
{

    public List<string> clipNames;

    public List<AudioClip> audioClips;
    public AudioSource audioSrc;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {

    }

    public void PlaySound(string clip)
    {
        int index = this.clipNames.IndexOf(clip);
        if (index >= 0) {
            this.audioSrc.PlayOneShot(audioClips[index]);
        }
    }
}
