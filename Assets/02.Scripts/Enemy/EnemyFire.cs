using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFire : MonoBehaviour
{
    private AudioSource _audio;
    private Animator _animator;
    private Transform playerTr;
    private Transform enemyTr;
    private readonly int hashFire = Animator.StringToHash("Fire");
    private readonly int hashReload = Animator.StringToHash("Reload");
    private float nextFire = 0.0f; // 다음 발사 할 시간 계산 변수 
    private readonly float fireRate = 0.1f; //발사 간격
    private readonly float damping = 10f;
    public bool isFire = false;

    private readonly float reloadTime = 2.9f; //재장전 시간
    private readonly int maxBullet = 10; //탄창의 최대 총알수 
    private int CurrBullet = 10; //초기 총알수 
    private bool isReload = false;
    private WaitForSeconds wsReload; // 재장전 시간동안 기다릴 변수 선언
    public AudioClip fireSfx;
    public AudioClip reloadSfx;
    [SerializeField]
    private GameObject Bullet;
    [SerializeField]
    private Transform firePos;
    [SerializeField]
    private MeshRenderer muzzleFlash;

    void Start()
    {
        Bullet = Resources.Load<GameObject>("Weapon/E_Bullet");
        _audio = GetComponent<AudioSource>();
        _animator = GetComponent<Animator>();
        playerTr = GameObject.FindGameObjectWithTag("Player").transform;
        enemyTr = GetComponent<Transform>();
        isFire = false;
        wsReload = new WaitForSeconds(reloadTime);
       
    }
    void FixedUpdate()
    {
        if(isFire && !isReload)
        {                   //다음 발사 시간보다 큰지 확인
            if(Time.time >= nextFire)
            {
                Fire();
                nextFire = Time.time + fireRate + Random.Range(0f, 0.3f);
            }



        }

    }
    void Fire()
    {
        _animator.SetTrigger(hashFire);
        _audio.PlayOneShot(fireSfx, 1.0f);
        GameObject _bullet = Instantiate(Bullet, firePos.position
            , firePos.rotation);
        Destroy(_bullet, 3.0f);


        isReload = (--CurrBullet % maxBullet == 0);
        if(isReload)
        {
            StartCoroutine(Reloading());
        }
        StartCoroutine(ShowMuzzleFlash());

    }
    IEnumerator Reloading()
    {
        muzzleFlash.enabled = false;
        _animator.SetTrigger(hashReload);
        _audio.PlayOneShot(reloadSfx, 1.0f);
        yield return wsReload;

        CurrBullet = maxBullet;
        isReload = false;

    }
    IEnumerator ShowMuzzleFlash()
    {
        muzzleFlash.enabled = true;
        Quaternion rot = Quaternion.Euler(Vector3.forward * Random.Range(0,360));
        muzzleFlash.transform.localRotation = rot;
        muzzleFlash.transform.localScale = Vector3.one * Random.Range(1f, 2f);
        Vector3 offset = new Vector2(Random.Range(0, 2),Random.Range(0, 2));
        muzzleFlash.material.SetTextureOffset("_MainTex", offset);
        //텍스터 ㅇffset 속성에 적용할 불규칙한 값을 생성 텍스처 종류중 Diffuse 
       yield return new WaitForSeconds(Random.Range(0.05f, 0.2f));
        muzzleFlash.enabled = false;
    }
}
