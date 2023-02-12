using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelCtrl : MonoBehaviour
{
    public GameObject expEffect; //폭파 효과 파티클
    public Mesh[ ] meshes;
    [SerializeField]
    private MeshFilter meshFilter;
    private int hitCount = 0;
    private Rigidbody rbody;
    [SerializeField]
    private Texture[] textures;
    [SerializeField]
    private MeshRenderer renderer;
    //폭파 반경 배럴주위에 있는 콜라이더와 리지디 바디를 갖고 있는 오브젝트에 폭파 효과 구현
    public float expRadius = 25f;
    public AudioClip expSfx;
    private AudioSource _audio;
    void Start()
    {
        _audio = GetComponent<AudioSource>();
        rbody = GetComponent<Rigidbody>();
        meshFilter = GetComponent<MeshFilter>();
        textures = Resources.LoadAll<Texture>("Images/Barrels");
        renderer = GetComponent<MeshRenderer>();
        renderer.material.mainTexture = textures[Random.Range(0, textures.Length)];

    }
    private void OnCollisionEnter(Collision coll)
    {
        if(coll.gameObject.CompareTag("BULLET"))
        {
            if(++hitCount ==3)
            {
                ExpBarrel();
            }

        }

    }
    void ExpBarrel()
    {
        Instantiate(expEffect, transform.position, Quaternion.identity);
        //rbody.mass = 1f;
        //rbody.AddForce(Vector3.up * 1000f);
        IndirectDamage(transform.position);
        int idx = Random.Range(0, meshes.Length);
        meshFilter.sharedMesh = meshes[idx];
        _audio.PlayOneShot(expSfx, 1.0f);
       
    }
    void IndirectDamage(Vector3 pos)
    {
        Collider[ ] colls = Physics.OverlapSphere(pos, expRadius, 1 << 14 );
         //자기자신위치에서 25반경에 리지디바디와 콜라이더를 달고 있는 오브젝트를
         //콜라이더 배열에 넣는다.
         //C# var 자료형이 있다.  똑똑한 자료형  알아서 형을 맞춰어준다 
        foreach(var col in colls)
        {
            var _rb = col.GetComponent<Rigidbody>();
            _rb.mass = 1f;
            _rb.AddExplosionForce(1200f, pos, expRadius, 1000f);

        }
    }
}
