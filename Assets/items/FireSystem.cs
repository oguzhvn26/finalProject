using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FireSystem : MonoBehaviour
{
    RaycastHit hit;

    public float reloadcooldown;
    public float AmmoInGun;
    public float AmmoInPocket;
    public float AmmoMax;
    float AddableAmmo;
    float reloadtimer;
    public Text AmmoCounter;
    public Text PocketAmmoCounter;
    public GameObject impactEffect;
    public AudioClip ReloadSound;

    public GameObject RayPoint;
    public bool CanFire;
    public CharacterController Karakter;
    Animator GunAnimset;

    float gunTimer;
    public float gunCooldown;

    public ParticleSystem MuzzleFlash;

    AudioSource SesKaynak;
    public AudioClip FireSound; 
    public float range;

    void Start()
    {
        SesKaynak = GetComponent<AudioSource>();
        GunAnimset = GetComponent<Animator>();
    }


    void Update()
    {
        GunAnimset.SetFloat("hız", Karakter.velocity.magnitude);

        AmmoCounter.text = AmmoInGun.ToString();
        PocketAmmoCounter.text = AmmoInPocket.ToString();
        AddableAmmo = AmmoMax - AmmoInGun;

        if (AddableAmmo > AmmoInPocket)
        {
            AddableAmmo = AmmoInPocket;
        }


        if (Input.GetKeyDown(KeyCode.R) && AddableAmmo > 0 && AmmoInPocket > 0)
        {
            if (Time.time > reloadtimer && !GunAnimset.GetBool("isReloading"))
            {
                StartCoroutine(Reload());
                reloadtimer = Time.time + reloadcooldown;
            }
        }


        if (Input.GetKey(KeyCode.Mouse0) && CanFire == true && Time.time > gunTimer && AmmoInGun > 0)
        {
            Fire();
            gunTimer = Time.time + gunCooldown;
        }
    }

    void Fire()
    {
        AmmoInGun--;
        if (Physics.Raycast(RayPoint.transform.position, RayPoint.transform.forward, out hit, range))
        {
            MuzzleFlash.Play();
            SesKaynak.Play();
            SesKaynak.clip = FireSound;
            Debug.Log(hit.transform.name);
            GunAnimset.Play("fire", -1, 0f);
            Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
        }
    }

    IEnumerator Reload()
    {
        GunAnimset.SetBool("isReloading", true);
        SesKaynak.clip = ReloadSound;
        SesKaynak.Play();

        yield return new WaitForSeconds(0.3f);
        
        
        yield return new WaitForSeconds(1.4f);

        AmmoInGun = AmmoInGun + AddableAmmo;
        AmmoInPocket = AmmoInPocket - AddableAmmo;

        GunAnimset.SetBool("isReloading", false);
    }
}
