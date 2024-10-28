using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveGamePanel : MonoBehaviour
{
    public GameObject startDialog;
    // Start is called before the first frame update
    void Start()
    {
        if (GetSave.CountTrueAnchors(SaveManager.NowSD.Anchor) > 0)
        {
            startDialog.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
