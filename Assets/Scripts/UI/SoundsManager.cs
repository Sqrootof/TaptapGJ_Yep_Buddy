using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundsManager : MonoBehaviour
{
    public static AudioSource effect;
    private static AudioClip panel;
    
    public static float EffectVol = 1f;
    
    private void Start()
    {
        effect = GetComponent<AudioSource>();
        panel = Resources.Load<AudioClip>("sounds/weapon_handling_48");
    }

    public static void PanelClip()
    {
        effect.PlayOneShot(panel);
    }
}
