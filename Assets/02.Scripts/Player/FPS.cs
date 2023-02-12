using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPS : MonoBehaviour
{
    public float moveSpeed = 5f; //이동속도
    public float rotSpeed = 70f; //회전속도
    public float xSensitivity = 100f; //x축 위아래 회전시 감도 
    public float ySensitivity = 100f; //y축 좌우 회전시 감도 
    public float yMinLimit = -45f; //위아래 회전 시 제한값
    public float yMaxLimit = 45;
    public float yRot = 0f; //y축 회전 변수
    public float xRot = 0f; //x축 회전 변수 
    public float jumpForce = 8f; // 점프력
    private bool isJumping = false;
    private Rigidbody rbody;
    private CapsuleCollider C_controller;
    //캐릭터 컨트롤러 
    private Transform tr;
    private float h = 0f, v = 0f;
    void Start()
    {
        isJumping = false;
        rbody = GetComponent<Rigidbody>();
        C_controller = GetComponent<CapsuleCollider>();
        tr = GetComponent<Transform>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        //바닥에 닿으면 점프중이 아니다.
        isJumping = false;
    }
    void Update()
    {
        MovePlayer();
        RotationPlayer();
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
            isJumping = true;
        }
        FastMove();
    }
    void MovePlayer()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");
        Vector3 movedir = (h * Vector3.right) + (v * Vector3.forward);
        tr.Translate(movedir.normalized * Time.deltaTime * moveSpeed,Space.Self);
    }
    void RotationPlayer()
    {           //마우스는 x로 움직이지만 캐릭터는 y축으로 회전
        xRot += Input.GetAxis("Mouse X") * xSensitivity * Time.deltaTime;
                //마우스는 y로 움직이지만 캐릭터는 x축 회전 
        yRot += Input.GetAxis("Mouse Y") * ySensitivity * Time.deltaTime;

        yRot = Mathf.Clamp(yRot, yMinLimit, yMaxLimit);
                          //what,최소값, 최대값

        tr.localEulerAngles = new Vector3(yRot, xRot, 0.0f);
            //로컬 회전축 벡터값을 받아서 회전 하는 특성이 있다.
    }
    void Jump()
    {
        if (isJumping) return;
        rbody.velocity = Vector3.up * jumpForce;

    }
    void FastMove()
    {
        if(Input.GetKey(KeyCode.LeftShift)&& Input.GetKey(KeyCode.W))
        {
            moveSpeed = 15f;

            tr.Translate(Vector3.forward * v * Time.deltaTime * moveSpeed, Space.Self);
        }
        if(Input.GetKeyUp(KeyCode.LeftShift))
        {
            moveSpeed = 5f; 
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag.Equals("DOOR"))
        {

            other.gameObject.SendMessage("DoorCheck", 
                         SendMessageOptions.DontRequireReceiver);

        }


    }
}
