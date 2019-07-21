using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapReborn : MonoBehaviour
{
    public UsrState usrState;
    public Transform finalPos;
    public MapManager mapManager;

    public AudioClip audioTh;
    public void Set() {
        StartCoroutine(WaitForDashEnd());
    }

    IEnumerator WaitForDashEnd() {
        while (usrState.currentAni.IsName("dashing")) {
            yield return new WaitForEndOfFrame();
        }
        usrState.goldenKeyTime = 0;
        usrState.transform.position = finalPos.position;
        usrState.climbSlideSpeed = 0.8f;
        Camera.main.GetComponent<CameraFollow>().target.transform.localPosition = new Vector3(0, 3, 0);
        FindObjectOfType<CameraBlurShape>().downSample = 1;
        FindObjectOfType<CameraBlurShape>().blurSpread = 0;


        mapManager.GetComponent<AudioSource>().Stop();
        mapManager.GetComponent<AudioSource>().clip = audioTh;
        mapManager.GetComponent<AudioSource>().loop = false;
        mapManager.GetComponent<AudioSource>().Play();
    }
}
