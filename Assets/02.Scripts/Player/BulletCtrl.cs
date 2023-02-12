using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCtrl : MonoBehaviour
{

    public int damage = 20;
    public float speed = 1000f;

    void Start()
    {
        GetComponent<Rigidbody>().AddForce(transform.forward * speed);
       // GetComponent<Rigidbody>().AddRelativeForce()
         //Rigibody 컴퍼넌트에 힘을 가하는 함수는  위의 두개가 있다.
         //맞은 위치에서 발사위치를 빼면 입사각 실제적으로 물리가 발생하는 방향을 알수 있다.

    }

    
  
}
