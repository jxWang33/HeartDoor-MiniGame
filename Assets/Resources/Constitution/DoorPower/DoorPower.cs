using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorPower : MonoBehaviour
{
    public Sprite spriteLock;
    public Sprite spriteUnlock;
    public SpriteRenderer spriteRenderer;
    public GameObject cube;

    private AudioSource audioSource;
    public AudioClip audioUnlock;

    public bool isLock = true;
    private bool inPlace = false;
    private TextMesh text;
    public float textColorAShowSpeed = 1f;
    public float textColorADisappearSpeed = 2f;
    [SerializeField]
    private float rotateSpeed = 90;
    private float textColorA;

    private UsrState record;

    void Awake()
    {        
        text = GetComponentInChildren<TextMesh>();
        audioSource = GetComponent<AudioSource>();
    }
    
    void Update()
    {
        if (inPlace && isLock) {
            textColorA += textColorAShowSpeed * Time.deltaTime;
            textColorA = Mathf.Clamp(textColorA, 0, 1);
            Color tempColor = text.color;
            tempColor.a = textColorA;
            text.color = tempColor;
        }
        else if (textColorA != 0){
            textColorA -= textColorADisappearSpeed * Time.deltaTime;
            textColorA = Mathf.Clamp(textColorA, 0, 1);
            Color tempColor = text.color;
            tempColor.a = textColorA;
            text.color = tempColor;
        }

        if (!isLock) {
            Vector3 tempRot = cube.transform.localEulerAngles;
            tempRot.z += Time.deltaTime * rotateSpeed;
            cube.transform.localEulerAngles = tempRot;
        }

        if (Input.GetKeyDown(GMManager.UP_KEY) && inPlace && isLock) {
            if (record.UseGreenKey()) {
                isLock = false;
                spriteRenderer.sprite = spriteUnlock;
                audioSource.clip = audioUnlock;
                audioSource.Play();
                cube.SetActive(true);
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.GetComponent<UsrState>()) {
            inPlace = true;
            record = collision.GetComponent<UsrState>();
        }
    }
    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.GetComponent<UsrState>())
            inPlace = false;
    }
}
