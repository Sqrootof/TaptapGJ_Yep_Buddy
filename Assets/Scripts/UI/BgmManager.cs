using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgmManager : MonoBehaviour
{
    public static AudioSource Bgm;
    private static AudioClip sampleMusic;
    private static AudioClip Music2;
    private static AudioClip Boss;
    
    public static float BgmVol = 1f;
    
    public int NowBgm = -1;
    public static int NeedBgm;

    private void Start()
    {
        Bgm = GetComponent<AudioSource>();
        sampleMusic = Resources.Load<AudioClip>("sounds/Easy_Slow_BGM");
        Music2 = Resources.Load<AudioClip>("sounds/battle_music");
        Boss = Resources.Load<AudioClip>("sounds/BOOS_BGM_1");
    }

    private void Update()
    {
        if (NeedBgm != NowBgm)
        {
            NowBgm = NeedBgm;
            switch (NowBgm)
            {
                case 0:
                    Bgm.clip = sampleMusic; Bgm.Play(); break;
                case 1:
                    Bgm.clip = Music2; Bgm.Play(); break;
                case 2:
                    Bgm.clip = Boss; Bgm.Play(); break;
            }
        }
    }
}
