using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHighLight : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    /*用于按钮动画*/
    
    private Animator animator;
    public bool silenceIt;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        animator.SetBool("Highlighted",true);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        animator.SetBool("Highlighted",false);
    }
    // private void OnDisable()
    // {
    //     animator.SetBool("Highlighted",false);
    // }
    public void ClickAnim()
    {
        animator.SetTrigger("Pressed");
        if (!silenceIt)
        {
            SoundsManager.PanelClip();
        }
    }
}
