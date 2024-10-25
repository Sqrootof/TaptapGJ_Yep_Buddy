using System;
using System.Collections;
using System.Collections.Generic;
using NodeCanvas.DialogueTrees;
using UnityEngine;

public class Trycol : MonoBehaviour
{
    private bool isTrigger = true;
    public DialogueTreeController _dialogueTreeController;//获取对话树（就那个预制体）
    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("1");
        if (other.CompareTag("Player") && isTrigger)
        {
            _dialogueTreeController.StartDialogue();//启动对话
            isTrigger = false;
        }
        
    }
}
