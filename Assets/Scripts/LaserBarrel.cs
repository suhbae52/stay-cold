using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class LaserBarrel : MonoBehaviour
{
    // referencing gameobjects.
    private Transform player;
    public GameObject laserLine;
    public GameObject laserBeam;

    // laser creating variables.
    private bool isCoroutineExecuting;
    public float laserIntervalTime = 1.5f;
    private float laserBlastTime = 0.5f;
    
    void Start(){
        player = GameObject.FindGameObjectWithTag("Player").transform;
        MakeLasers();
    }

    void MakeLasers(){
        if(!isCoroutineExecuting){
            // setting the angle of the barrel.
            Vector3 randomPlayerPosition = player.position;
            Vector3 difference = randomPlayerPosition - transform.position;
            float angle = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
            transform.eulerAngles = Vector3.forward * angle;

            laserLine.SetActive(true);
            AudioManager.instance.RegisterAudioSource(laserLine.GetComponent<AudioSource>());
            StartCoroutine(StartLaser(laserIntervalTime, laserBlastTime));
        }
    }
    
    IEnumerator StartLaser(float time1, float time2){
        if (!gameObject.activeInHierarchy) yield break;
        // disabling laser line and enabling laser beam after time1 seconds.
        isCoroutineExecuting = true;
        yield return new WaitForSeconds(time1);
        AudioManager.instance.UnregisterAudioSource(laserLine.GetComponent<AudioSource>());
        laserLine.SetActive(false);
        laserBeam.SetActive(true);
        AudioManager.instance.RegisterAudioSource(laserBeam.GetComponent<AudioSource>());

        // disabling laser beam after time2 seconds.
        yield return new WaitForSeconds(time2);
        AudioManager.instance.UnregisterAudioSource(laserBeam.GetComponent<AudioSource>());
        laserBeam.SetActive(false);
        isCoroutineExecuting = false;
        Destroy(gameObject);
        
    }
}
