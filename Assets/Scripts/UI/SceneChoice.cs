using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChoice : MonoBehaviour
{
    public string sceneName; // 要跳转的场景名称
    public float ifDelayTime;
    public GameObject effect;

    // 立即跳转到指定场景
    public void SceneAlt()
    {
        Time.timeScale = 1;
        Instantiate(effect, transform.position, transform.rotation);
        SaveManager.Save(SaveManager.NowNum);
        SceneManager.LoadScene(sceneName);
    }

    // 延时跳转到指定场景
    public void SceneAltWithDelay()
    {
        Time.timeScale = 1;
        Instantiate(effect, transform.position, transform.rotation);
        StartCoroutine(DelayedSceneChange(ifDelayTime));
    }

    // 协程：在指定延迟后跳转到场景
    private IEnumerator DelayedSceneChange(float delay)
    {
        yield return new WaitForSeconds(delay); // 等待指定的时间
        SceneManager.LoadScene(sceneName); // 跳转到场景
    }
}
