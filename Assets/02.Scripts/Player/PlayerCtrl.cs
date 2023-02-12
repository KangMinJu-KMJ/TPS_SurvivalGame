using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable] //애튜리뷰트 속성(Artibute)
public class PlayerAnim //인스펙터 뷰에 멤버 변수들이 보이게한다.
{
    public AnimationClip idle;
    public AnimationClip runF;
    public AnimationClip runB;
    public AnimationClip runL;
    public AnimationClip runR;
    public AnimationClip sprintF; //빠르게 달리기
    public AnimationClip FireGun;
}

public class PlayerCtrl : MonoBehaviour
{
    public PlayerAnim playerAnim;
    [HideInInspector] //[SerializeField]의 반대 
    public Animation anim; // public 이라고 선언했지만 인스펙트에 보이지 않는다.
    private float h = 0f;
    private float v = 0f;
    private float r = 0f;
    private Transform tr;
    public float moveSpeed = 10f;
    public float rotSpeed = 80f;
    
    private void Awake()
    {
       
        tr = GetComponent<Transform>();
        anim = GetComponent<Animation>();
    }
    void Start()
    {
       
        anim.clip = playerAnim.idle;
        anim.Play();
        moveSpeed = 10f;
    }
    void Update()
    {

        PlayerMove();
        PlayerAction();
        //FireBullet();
    }
    void PlayerMove()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");
        r = Input.GetAxis("Mouse X");
        Vector3 movedir = (h * Vector3.right) + (v * Vector3.forward);
        tr.Translate(movedir.normalized * moveSpeed * Time.deltaTime, Space.Self);
        tr.Rotate(Vector3.up * r * rotSpeed * Time.deltaTime);
    }
    void PlayerAction()
    {
        if (v >= 0.1f)
        {     //연계동작인 경우 레거시에서는 크로스 페이드 쓴다.
            anim.CrossFade(playerAnim.runF.name, 0.3f);

        }
        else if (v <= -0.1f)
        {
            anim.CrossFade(playerAnim.runB.name, 0.3f);

        }
        else if (h >= 0.1f)
        {
            anim.CrossFade(playerAnim.runR.name, 0.3f);

        }
        else if (h <= -0.1f)
        {
            anim.CrossFade(playerAnim.runL.name, 0.3f);

        }
        else
        {
            anim.CrossFade(playerAnim.idle.name, 0.3f);
        }
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.W))
        {
            moveSpeed = 17f;
            anim.Play(playerAnim.sprintF.name);

        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            moveSpeed = 10f;
        }


    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("DOOR"))
        {
            other.gameObject.SendMessage("DoorCheck",
                SendMessageOptions.DontRequireReceiver);

        }


    }

}
