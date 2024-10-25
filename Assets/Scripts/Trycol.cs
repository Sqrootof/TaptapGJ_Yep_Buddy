using System;
using System.Collections;
using System.Collections.Generic;
using NodeCanvas.DialogueTrees;
using UnityEngine;

public class Trycol : MonoBehaviour
{
    private bool isTrigger = true;
    public  DialogueTreeController _dialogueTreeController;
    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("1");
        if (other.CompareTag("Player") && isTrigger)
        {
            _dialogueTreeController.StartDialogue();
            isTrigger = false;
        }
        
    }
}
