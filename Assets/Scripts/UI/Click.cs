using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Click : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void setTrue()
    {
        OnEmpty.ClickButton = true;
        Debug.Log("1");
    }

    public void setFalse()
    {
        OnEmpty.ClickButton = false;
    }
}
