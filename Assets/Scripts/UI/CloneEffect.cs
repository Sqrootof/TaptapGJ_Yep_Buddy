using UnityEngine;

public class CloneEffect : MonoBehaviour
{
    /*原地克隆一个特效物件*/

    public GameObject cloneObject;
    public GameObject canvas;

    public void Clone()
    {
        canvas = GameObject.Find("Canvas");
        var triggerUIRect = gameObject.GetComponent<RectTransform>();

        // 在Canvas下面克隆UI物体，并设置其位置信息
        var newUIObject = Instantiate(cloneObject, canvas.transform);
        var newUIRect = newUIObject.GetComponent<RectTransform>();
        newUIRect.position = triggerUIRect.position;
    }
}
