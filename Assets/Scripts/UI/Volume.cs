using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Volume : MonoBehaviour
{
    /*此代码用于调节音量，分为bgm和音效两部分*/
    /*游戏内部也使用两个物体播放*/

    public int bgmValue;

    public GameObject bgm;
    public Slider bgmSlider;
    public GameObject effect;
    public Slider effectSlider;

    public Text bgmPercent;
    public Text effectPercent;
    
    private void Start()
    {
        BgmManager.NeedBgm = bgmValue;
        bgm = GameObject.Find("BGM");
        effect = GameObject.Find("Effect");
        
        /*下方是bgm音量代码*/
        if (bgm)
        {
            bgmSlider.value = BgmManager.BgmVol;
            bgmPercent.text = Mathf.Round(bgmSlider.value * 100).ToString("F0") + "%";
            bgmSlider.onValueChanged.AddListener(UpdateVolume);
            BgmManager.Bgm.volume = BgmManager.BgmVol;
        }
        
        /*下方是音效音量代码*/
        if (effect)
        {
            effectSlider.value = SoundsManager.EffectVol;
            effectPercent.text = Mathf.Round(effectSlider.value * 100).ToString("F0") + "%";
            effectSlider.onValueChanged.AddListener(UpdateVolume2);
            SoundsManager.effect.volume = SoundsManager.EffectVol;
        }
    }

    private void UpdateVolume(float value)  //更新音量
    {
        bgmPercent.text = Mathf.Round(bgmSlider.value * 100).ToString("F0") + "%";
        BgmManager.BgmVol = value;
        BgmManager.Bgm.volume = BgmManager.BgmVol;
    }
    private void UpdateVolume2(float value)  //更新音量2
    {
        effectPercent.text = Mathf.Round(effectSlider.value * 100).ToString("F0") + "%";
        SoundsManager.EffectVol = value;
        SoundsManager.effect.volume = SoundsManager.EffectVol;
    }
}