using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    public AudioSource buttonSound;
    public AudioSource upgradeSound;
    public AudioClip[] audioClips;
}
