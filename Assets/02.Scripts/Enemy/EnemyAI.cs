using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public enum State
{
    PARTROL=1,TRACE, ATTACK,DIE
}
public class EnemyAI : MonoBehaviour
{
    public State state = State.PARTROL;
    private Transform playerTr;
    private Transform enemyTr;
    public float attackDist = 5.0f;
    public float traceDist = 10f;
    public bool isDie = false;
    private WaitForSeconds ws; //코루틴에서
    //사용할 지연 시간 변수 
    private MoveAgent moveAgent;
    private Animator animator;
    private readonly int hashMove = Animator.StringToHash("IsMove");
    private readonly int hashSpeed = Animator.StringToHash("Speed");
    private readonly int hashDie = Animator.StringToHash("Die");
    private readonly int hashDieIdx = Animator.StringToHash("DieIdx");
    private EnemyFire enemyfire;

    void Awake() //제일 먼저 호출 네트워크나 오브젝트 풀링시 필수 
    {            // 싱글턴 패턴으로 할때 

        var player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
            playerTr = player.GetComponent<Transform>();

        enemyTr = GetComponent<Transform>();
        animator = GetComponent<Animator>();
        moveAgent = GetComponent<MoveAgent>();
        enemyfire = GetComponent<EnemyFire>();
        ws = new WaitForSeconds(0.3f);
       
    }
    private void OnEnable() //오브젝트가 비활성화 되었다가
    {                       //활성화되었을 때 스타트 함수보다
                            //먼저 호출
        isDie = false;
        //적이나 enemy에서는 Update나 FixedUpdate를 구현하지 않고
        //스타트 코루틴이라는 것을 써야 한다.
        //이유는 여러개가 생성될때 프레임을 많이 소모되기 때문에 
        //모바일 게임에서는 플레이어도 스타트 코루틴으로 구현하는 경우가
        //많타 
        StartCoroutine(CheckState());
        StartCoroutine(Action());
    }
    IEnumerator CheckState()
    {
        while(!isDie)
        {    //사망한 상태를 상태라면 코루틴 함수종료
            if (state == State.DIE) yield break;
            //주인공과 적캐릭터간의 거리를 계산
            float dist = Vector3.Distance(playerTr.position, enemyTr.position);
            if (dist <= attackDist)
                state = State.ATTACK;
            else if (dist <= traceDist)
                state = State.TRACE;
            else
                state = State.PARTROL;
                           
            yield return ws;//0.3초 동안 대기하는 동안 제어권을 양보 
           
        }

    }
    IEnumerator Action()
    {
        while(!isDie)
        {
            yield return ws;
            switch(state)
            {
                case State.PARTROL:
                    enemyfire.isFire = false;
                    moveAgent.patrolling = true;
                    animator.SetBool(hashMove, true);
                    break;
                case State.TRACE:
                    enemyfire.isFire = false;
                    moveAgent.traceTarget = playerTr.position;
                    animator.SetBool(hashMove, true);
                    break;
                case State.ATTACK:
                 
                    moveAgent.Stop();
                    animator.SetBool(hashMove, false);
                    if(enemyfire.isFire ==false)
                      enemyfire.isFire = true;
                    Vector3 at = playerTr.position - enemyTr.position;
                    enemyTr.rotation = Quaternion.Slerp(enemyTr.rotation, Quaternion.LookRotation(at), Time.deltaTime * 3.0f);
                    break;
                case State.DIE:
                    isDie = true;
                    enemyfire.isFire = false;
                    moveAgent.Stop();
                    //사망 애니메이션 종류를 지정 
                    animator.SetInteger(hashDieIdx, Random.Range(0, 2));
                    animator.SetTrigger(hashDie);
                    break;
            }

        }

    }
    private void Update()
    {

        animator.SetFloat(hashSpeed, moveAgent.speed);

    }


}
