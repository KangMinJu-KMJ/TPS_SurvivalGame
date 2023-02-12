using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveBullet : MonoBehaviour
{
    public GameObject sparkEffect;


    private void OnCollisionEnter(Collision coll)
    {
        if(coll.gameObject.tag.Equals("BULLET"))
        {
            //ShowEffect(coll.transform.position);
            ShowEffect(coll);
            Destroy(coll.gameObject);
        }


    }
    //void ShowEffect(Vector3 hitPos) //첫번째 방법
    //{

    //   Instantiate(sparkEffect, hitPos, Quaternion.LookRotation(-Vector3.forward));


    //}
    void ShowEffect(Collision coll)
    {
        //충돌지점 정보를 추출
        ContactPoint contact = coll.contacts[0];
        Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, contact.normal);
        Instantiate(sparkEffect, contact.point, rot);


    }
}
