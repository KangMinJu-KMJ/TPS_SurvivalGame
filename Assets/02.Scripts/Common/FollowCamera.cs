using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField]
    private Transform Target;
    [SerializeField]
    private float height = 4.0f;
    [SerializeField]
    private float distance = 5.0f;
    [SerializeField]
    private float moveDamping = 15.0f;
    [SerializeField]
    private float rotateDamping = 10f;
    public float targetOffset = 2.0f;
    //추적 좌표의 오프셋
    [SerializeField]
    private Transform tr;

    void Start()
    {
        tr = GetComponent<Transform>();
        moveDamping = 15.0f;
        rotateDamping = 10.0f;

    }
    void LateUpdate()
    {
        //float angle = Mathf.LerpAngle(Target.localEulerAngles.y,
        //       tr.localEulerAngles.y, Time.deltaTime * Damping);
        //Quaternion rot = Quaternion.Euler(0f, angle, 0f);

        //tr.position = Target.position - (rot * Vector3.forward * distance) + (Vector3.up * height);
        //tr.LookAt(Target.transform);
        var camPos = Target.position
                    - (Target.forward * distance)
                    + (Target.up * height);
        tr.position = Vector3.Slerp(tr.position,camPos
                                , Time.deltaTime * moveDamping);
        tr.rotation = Quaternion.Slerp(tr.rotation, Target.rotation,
                Time.deltaTime * rotateDamping);
        //카메라 추적 대상으로 Z축을  회전 시킴
        tr.LookAt(Target.position + (Target.up * targetOffset));
        
    }
   
}
