using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderCheck : MonoBehaviour
{
    public Camera mainCamera;
    public Camera fpsCamera;


    void Start()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag =="Player")
        {
            mainCamera.depth = -1;
            mainCamera.transform.GetComponent<AudioListener>().enabled = false;
            fpsCamera.depth = 0;
            fpsCamera.transform.GetComponent<AudioListener>().enabled = true;

        }


    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            mainCamera.depth = 0;
            mainCamera.transform.GetComponent<AudioListener>().enabled = true;
            fpsCamera.depth = -1;
            fpsCamera.transform.GetComponent<AudioListener>().enabled = false;

        }


    }
}
