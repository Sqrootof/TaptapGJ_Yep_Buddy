using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackScreen : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    public bool fadeInOut = false; // ���ƽ���Ĺ�������
    private bool fadingIn = true; // �Ƿ��ڽ��䵽��͸��
    private bool fading = false; // �Ƿ����ڽ��н������

    public float stayTime;
    public float fadeSpeed = 1.0f; // �����ٶ�
    private float currentAlpha = 0f; // ��ǰ͸����
    private float time;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = new Color(0, 0, 0, 0); // ��ʼ��Ϊ��ȫ͸��
    }

    private void Update()
    {
        // ������������Ϊ true �Ҳ��ڽ��н���ʱ����ʼ�������
        if (fadeInOut && !fading)
        {
            fading = true; // ��ʼ����
            currentAlpha = 0f; // ���õ�ǰ͸����
            fadingIn = true; // ����Ϊ��͸������͸��
        }

        // �������
        if (fading && time<=0f)
        {
            // �޸�͸����
            currentAlpha = Mathf.MoveTowards(currentAlpha, fadingIn ? 1f : 0f, fadeSpeed * Time.deltaTime);
            spriteRenderer.color = new Color(0, 0, 0, currentAlpha);

            // ����Ƿ�ﵽ��Ŀ��͸����
            if (currentAlpha == (fadingIn ? 1f : 0f))
            {
                // ����Ѿ���ɽ��䣬ֹͣ����
                if (fadingIn)
                {
                    fadingIn = false; // ��תΪ�����͸��
                    time = stayTime;
                }
                else
                {
                    fading = false; // ��������������
                    fadeInOut = false; // �������ÿ��Ʊ���Ϊ false
                }
            }
        }
        if (time>=0f)
        {
            time -= Time.deltaTime;
        }
    }
}
