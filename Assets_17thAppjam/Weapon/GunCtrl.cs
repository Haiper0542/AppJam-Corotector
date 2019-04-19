using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunCtrl : MonoBehaviour {

    [Header("[Shot]")]
    public float shotDelay = 0.5f;
    private float shotTime = 0;

    public int bulletCount = 10;
    public int maxBulletCount = 10;

    public int damage = 3;

    [Header("[Reload]")]
    public float reloadDelay = 1f;
    private float reloadTime = 0;
    public bool isReloading = false;

    [Header("[사운드 관리]")]
    public AudioSource audioSource;
    public AudioClip shotClip;
    public AudioClip reloadClip;

    public Transform muzzle;
    public ParticleSystem effect;
    RaycastHit hit;
    Ray ray;
    private bool isShot = false;

    public LayerMask droneLayer;

    void Start () {
        effect.Stop();
        bulletCount = maxBulletCount;
        audioSource = GetComponent<AudioSource>();
	}

    private void Update()
    {
        Debug.DrawRay(muzzle.position, muzzle.forward*100f, Color.green);
        ray = new Ray(muzzle.position, muzzle.forward * 100f);
        if (shotTime > 0)
        {
            shotTime -= Time.deltaTime;
        }
        if (isReloading)//재장전
        {
            if (bulletCount < maxBulletCount)
            {
                reloadTime += Time.deltaTime;

                if(reloadTime > reloadDelay)
                {
                    reloadTime = 0;
                    isReloading = false;
                    bulletCount = maxBulletCount;
                    try
                    {
                        audioSource.PlayOneShot(reloadClip);
                    }
                    catch { }
                }
            }
        }
        else
        {
            reloadTime = 0;
        }
    }

    public void Shot () {
		if(shotTime <= 0 && bulletCount > 0)
        {
            bulletCount--;
            try { 
            audioSource.PlayOneShot(shotClip);
            }
            catch { }
            if (Physics.Raycast(ray, out hit, droneLayer))
            {
                Debug.Log(hit.transform.name);
                if (hit.transform.CompareTag("NDrone"))
                {
                    Debug.Log("NAttack");
                    hit.transform.GetComponent<NormalDrone>().TakeDamage(damage);
                }
                else if (hit.transform.CompareTag("SDrone"))
                {
                    Debug.Log("SAttack");
                    hit.transform.GetComponent<GunDrone>().TakeDamage(damage);
                }
                else if (hit.transform.CompareTag("BDrone"))
                {
                    Debug.Log("BAttack");
                    hit.transform.GetComponent<SuicideDrone>().TakeDamage(damage);
                }
            }

        }
        effect.Stop();
        effect.Play();
	}
}
