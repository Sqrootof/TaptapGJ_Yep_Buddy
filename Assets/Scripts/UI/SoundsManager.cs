using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundsManager : MonoBehaviour
{
    public static AudioSource effect;
    private static AudioClip panel;
    private static AudioClip a;
    
    public static float EffectVol = 1f;
    
    private void Start()
    {
        effect = GetComponent<AudioSource>();
        panel = Resources.Load<AudioClip>("sounds/weapon_handling_48");
        a = Resources.Load<AudioClip>("sounds/Magic_Bullet_Blue");
        
    }

    public static void PanelClip()
    {
        if (panel)
        {
            effect.PlayOneShot(panel);
        }
    }
    public static void Attack()
    {
        if (a)
        {
            effect.PlayOneShot(a);
        }
    }
}
