using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorCtrl : MonoBehaviour
{

    [SerializeField]
    private Animation ani;
    public AudioSource source;
    public AudioClip openSound;
    public AudioClip shutSound;
    private bool isOpen = false;
    private float Dtime = 0;


    void Start()
    {
        isOpen = false;
        ani = transform.parent.GetComponent<Animation>();
        Dtime = Time.time;

    }


    void Update()
    {
        DoorClose();
    }

    void DoorCheck()
    {
        if (!isOpen)
            DoorMnanager(openSound, true, "dooropen");
    }

    void DoorClose()
    {
        if (isOpen)
        {
            if (Time.time - Dtime > 5.0f)
            {
                DoorMnanager(shutSound, false, "doorshut");
                Dtime = Time.time;
            }
        }
    }

    void DoorMnanager(AudioClip aClip, bool Ischeck, string aniName)
    {
        source.PlayOneShot(aClip, 1.0f);
        isOpen = Ischeck;
        ani.Play(aniName);
    }

}
