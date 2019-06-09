using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorSwitch : MonoBehaviour
{
    public Sprite spriteOn;
    public Sprite spriteOff;
    public SpriteRenderer cube;
    public SpriteRenderer eye;

    private AudioSource audioSource;
    public AudioClip audioUnlock;

    public DoorSwitchCallBack callBack;

    [SerializeField]
    private bool isOn = false;
    [SerializeField]
    private float rotateSpeed = 90;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (isOn) {
            eye.sprite = spriteOn;
        }
        else {
            eye.sprite = spriteOff;
        }
    }

    void Update()
    {
        if (isOn) {
            Vector3 tempRot = cube.transform.localEulerAngles;
            tempRot.z += Time.deltaTime * rotateSpeed;
            cube.transform.localEulerAngles = tempRot;
        }
    }

    public void SetSwitchOn(bool withCallBack = true) {
        isOn = true;
        eye.sprite = spriteOn;
        audioSource.clip = audioUnlock;
        audioSource.Play();
        if(withCallBack)
            callBack.OnSwitchOn();
    }

    public void SetSwitchOff(bool withCallBack = true) {
        isOn = false;
        eye.sprite = spriteOff;
        if (withCallBack)
            callBack.OnSwitchOff();
    }

    public void Change() {
        if (isOn)
            SetSwitchOff(false);
        else
            SetSwitchOn(false);
        callBack.OnChange();
    }
}
