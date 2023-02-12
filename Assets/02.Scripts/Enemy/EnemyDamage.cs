using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    private const string bulletTag = "BULLET";
    private int hp = 100;
    private GameObject bloodEffect;
    void Start()
    {
        bloodEffect = Resources.Load<GameObject>("Effect/BloodEffect");
    }
    private void OnCollisionEnter(Collision coll)
    {
        if(coll.gameObject.tag == bulletTag)
        {
            ShowBloodEffect(coll);
            Destroy(coll.gameObject);
            hp -= coll.gameObject.GetComponent<BulletCtrl>().damage;
            if(hp <=0)
            {
                GetComponent<EnemyAI>().state = State.DIE;
            }

        }

    }
    void ShowBloodEffect(Collision coll)
    {   //총알이 충돌한 지점 산출 
        Vector3 pos = coll.contacts[0].point;
        Vector3 _normal = coll.contacts[0].normal;
        //총알이 충돌 했을 때의 위치 방향 각도  전문용어로는 법선 벡터 
        //총알 충돌시 방향 벡터의 회전값 계산
        Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, _normal);
        //혈흔 효과 생성
        GameObject blood = Instantiate<GameObject>(bloodEffect, pos, rot);
        Destroy(blood, 1.0f);
    }
    void Update()
    {
        
    }
}
