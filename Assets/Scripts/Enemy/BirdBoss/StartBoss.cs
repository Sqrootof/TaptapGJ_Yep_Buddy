using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartBoss : MonoBehaviour
{
    public GameObject boss;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void OnTriggerEnter(Collider other)
    {
        // 检查碰撞的物体是否是玩家
        if (other.CompareTag("Player"))
        {
            boss.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
