using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManagerScript : MonoBehaviour
{

    public static AudioClip playerAttackSound, playerAttack2Sound, playerDeathSound, playerHealSound, playerTakeHit1, playerTakeHit2, playerTakeHit3;
    static AudioSource audioSrc;
    // Start is called before the first frame update
    void Start()
    {
        playerAttackSound = Resources.Load<AudioClip> ("playerAttack");
    
        audioSrc = GetComponent<AudioSource> ();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public static void PlaySound (string clip)
    {
        switch (clip)
        {
            case "playerAttack":
                audioSrc.PlayOneShot (playerAttackSound);
                break;
        }
    }
}
