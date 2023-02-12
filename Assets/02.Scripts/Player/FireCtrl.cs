using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//총알 발사와 재장전 오디오 클립을 저장 할
//구조체 
[System.Serializable]
public struct PlayerSfx
{
    public AudioClip[] fire;
    public AudioClip[] reload;
}


public class FireCtrl : MonoBehaviour
{
    public enum WeaponType
    {
        RIFLE = 0,
        SHOTGUN
    }

    //주인공 현재 들고 있는 무기를 저장 할 변수 
    public WeaponType currWeapon = WeaponType.RIFLE;
    public PlayerSfx playersfx;
    [SerializeField]
    private GameObject Bullet;
    [SerializeField]
    private Transform firePos;
    [SerializeField]
    private ParticleSystem cartridge;
    [SerializeField]
    private ParticleSystem muzzleFlash;
    [SerializeField]
    private Animation Anim;
    [SerializeField]
    private AudioSource _audio;
    void Start()
    {
        Bullet = Resources.Load<GameObject>("WeaPon/Bullet");
        muzzleFlash = firePos.GetComponentInChildren<ParticleSystem>();
        Anim = GetComponent<Animation>();
        _audio = GetComponent<AudioSource>();
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Fire();
        }
        
    }
    void Fire()
    {
          //함수 오버로딩
        Instantiate(Bullet, firePos.position, firePos.rotation);
        //Instantiate(Bullet);
        // Instantiate(Bulllet, firePos.position);
        // Instantiate(Bullet, firePos.position, firePos.rotation,null);
        cartridge.Play(); //파티클 이펙트  실행 
                          //cartridge.Stop();
        muzzleFlash.Play();
        //isFire = true;
        Anim.Play("IdleFireSMG");
        FireSfx();
    }
    void FireSfx()
    {   //현재 들고 있는 무기의 오디오 클립을 가져옴 
        var _sfx = playersfx.fire[(int)currWeapon];
        _audio.PlayOneShot(_sfx, 1.0f);
    }
}
