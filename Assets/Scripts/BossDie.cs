using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using NodeCanvas.DialogueTrees;

public class BossDie : MonoBehaviour
{
    public DialogueTreeController _dialogueTreeController;
    public void OnDestroy()
    {
        SaveManager.Round += 1;
        foreach (var a in Whole.anchorPoints)
        {
            a.isUnlocked = false;
        }
        _dialogueTreeController.StartDialogue();
    }
}
