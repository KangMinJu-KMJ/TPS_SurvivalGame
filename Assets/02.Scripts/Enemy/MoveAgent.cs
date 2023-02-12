using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
[RequireComponent(typeof(NavMeshAgent))]
public class MoveAgent : MonoBehaviour
{
    //순찰 지점들을 저장하기 위한 list타입 변수 
    public List<Transform> wayPoints;
    public int nexIdx; //순찰지점의 배열 인덱스 값
    private NavMeshAgent agent;
    

    private readonly float patrolSpeed = 1.5f;
    private readonly float traceSpeed = 4.0f;
    private float damping = 1.0f; // 회전할때 속도 조절하는 변수 
    private Transform enemyTr;
    private bool _patrolling; //패트롤 중인지 판단
    
    public bool patrolling
    {    // C#에서는 이 변수  중괄호에다가 
        //getproperty,setproperty 설 정 할수 있다. 
        get { return _patrolling; }
        set
        {
            _patrolling = value;
            if(_patrolling)
            {
                agent.speed = patrolSpeed;
                damping = 1.0f; //순찰 상태 회전 계수 
                MoveWayPoint();
            }

        }
    }
    //추적대상의 위치를 저장하는 변수 
    private Vector3 _traceTarget;
    //traceTarget 프로퍼티 정의(getter,setter)
    public Vector3 traceTarget
    {
        get { return _traceTarget; }
        set
        {
            _traceTarget = value;
            agent.speed = traceSpeed;
            damping = 7.0f;
            TraceTarget(_traceTarget);//플레이어를 추적 할때 이동 시키는 함수 
        }

    }
    public float speed
    {   //NavMeshAgent의 이동속도에 대한 프로퍼티 정의(getter)
        get { return agent.velocity.magnitude; }
    }


    void Start()
    {
        enemyTr = GetComponent<Transform>();
        agent = GetComponent<NavMeshAgent>();
        agent.autoBraking = false;
        // 목적지에 가까워 질수록 속도를 줄이는 것을 비활성
        agent.updateRotation = false; 
        //NavmeshAgent 회전 하는 어색 해서  false 처리 했다.
        agent.speed = patrolSpeed;
        var group = GameObject.Find("WayPointGroup");
        if(group != null)
        {
            group.GetComponentsInChildren<Transform>(wayPoints);
            wayPoints.RemoveAt(0);
            //첫번째 항목 삭제  //최신 방법
            //그전에 했던 방법
            //Transform[] ways = group.GetComponentsInChildren<Transform>();
            //wayPoints.AddRange(ways);
            
                
        }
        MoveWayPoint();
    }
    void MoveWayPoint()//다음 목적지 까지 이동 명령을 내리는 함수
    {   //최단 거리 경로 계산이 끝나지 않았으면 다음을 
        //수행 하지 않음 
        if (agent.isPathStale) return;
        //다음 목적지를 wayPoints 배열에서 추출하누 위치로 다음 목적지를 지정 한다.
        agent.destination = wayPoints[nexIdx].position;
        //네비게이션 기능을 활성화 해서 이동을 시작함
        agent.isStopped = false;


    }
    private void Update()
    {    
        if(agent.isStopped ==false) //적캐릭터가 이동 중일때만 회전 
        {                    
                            //NaveMeshAgent가 가야 할 방향 벡터를 쿼터니언 타입의 각도로 변환
            Quaternion rot = Quaternion.LookRotation(agent.desiredVelocity);
            enemyTr.rotation = Quaternion.Slerp(enemyTr.rotation, rot, Time.deltaTime * damping);
        }
        
        
        
        //패트롤 순찰 모드가 아닐 경우 로직 수행 하지 않음
        if (!_patrolling) return;


        //NaveMeshAgent 가 이동하고 있고 목적지에 도착했는 여부를 계산
        if(agent.velocity.sqrMagnitude >= 0.2f *0.2f 
            && agent.remainingDistance <=0.5f)
        {   //다음 목적지의 배열 첨자를 계산 
            nexIdx = ++nexIdx % wayPoints.Count;
            MoveWayPoint();
        }



    }
    void TraceTarget(Vector3 pos)
    {   //주인공을 추적 할 때 이동시키는 함수 
        //최단 경로거리를 계산 하지 못하면 빠져 나간다.
        if (agent.isPathStale) return;
        agent.destination = pos;
        agent.isStopped = false;
    }
    public void Stop() //순찰 및 추적을 정지 시키는 함수 
    {
        agent.isStopped = true;
        agent.velocity = Vector3.zero;
        //바로 정지 하기 위해 속도를 0으로 설정
        _patrolling = false;

    }
}
