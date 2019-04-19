using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionCtrl : MonoBehaviour {

    public AudioClip bombClip;

    void Start () {
        GetComponent<AudioSource>().PlayOneShot(bombClip);
        Destroy(gameObject, 2);
    }
}
