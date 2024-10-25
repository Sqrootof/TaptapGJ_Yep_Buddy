using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class BulletDrop : MonoBehaviour
{
    public Bullet BulletInfo;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<SpriteRenderer>().sprite = BulletInfo.Icon;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            WeaponBackpack.Instance.GetNewBullet(BulletInfo);
            Destroy(gameObject);
        }
    }
}
