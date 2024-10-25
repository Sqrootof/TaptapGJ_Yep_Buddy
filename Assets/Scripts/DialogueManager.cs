using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NodeCanvas.DialogueTrees;
public class DialogueManager : MonoBehaviour
{
    public DialogueTreeController _dialogueTreeController;
    
    public void Start()
    {

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            _dialogueTreeController.StartDialogue();
        }
    }
}
