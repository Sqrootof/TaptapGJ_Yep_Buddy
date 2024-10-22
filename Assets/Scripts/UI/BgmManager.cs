using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgmManager : MonoBehaviour
{
    public static AudioSource Bgm;
    private static AudioClip sampleMusic;
    
    public static float BgmVol = 1f;
    
    public int NowBgm = -1;
    public static int NeedBgm;

    private void Start()
    {
        Bgm = GetComponent<AudioSource>();
        sampleMusic = Resources.Load<AudioClip>("sounds/SublimeWeakness");
    }

    private void Update()
    {
        if (NeedBgm != NowBgm)
        {
            NowBgm = NeedBgm;
            switch (NeedBgm)
            {
                case 0:
                    Bgm.clip = sampleMusic; Bgm.Play(); break;
            }
        }
    }
}
