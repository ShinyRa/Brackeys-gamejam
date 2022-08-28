using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManagerScript : MonoBehaviour
{

    public List<string> clipNames;

    public List<AudioClip> audioClips;
    public AudioSource audioSrc;

    public Slider volumeSlider;

    private float volume = 0.8f;
    void Start()
    {
        volume = PlayerPrefs.GetFloat("volume");
        this.audioSrc.volume = volume;
        this.volumeSlider.value = volume;
    }
    
    void Update()
    {
        this.audioSrc.volume = volume;
        PlayerPrefs.SetFloat("volume", this.volume);
    }

    public void VolumeUpdater() {
        this.volume = this.volumeSlider.value;
    }

    public void PlaySound(string clip)
    {
        int index = this.clipNames.IndexOf(clip);
        if (index >= 0) {
            this.audioSrc.PlayOneShot(audioClips[index]);
        }
    }
}
