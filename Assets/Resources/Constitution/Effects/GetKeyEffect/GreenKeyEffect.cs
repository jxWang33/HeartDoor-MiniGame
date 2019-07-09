using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenKeyEffect : MonoBehaviour
{
    private Vector2 startPos = new Vector2(0, 0);
    private Vector2 endPos = new Vector2(5, 5);
    private Vector2 canvasPos;

    [SerializeField]
    private float moveSpeed = 5f;
    private Vector2 dir;

    private void Awake() {
        transform.position = startPos;
        dir = (endPos - (Vector2)transform.position).normalized;
    }

    public void Set(Vector2 sp, Vector2 ep) {
        startPos = sp;
        endPos = ep;

        transform.position = sp;
        dir = (endPos - startPos).normalized;
    }
    
    void Update()
    {
        transform.Translate(dir * moveSpeed * Time.deltaTime, Space.Self);
        if (((Vector2)transform.position - startPos).magnitude >= (endPos - startPos).magnitude) {
            Destroy(gameObject);
        }
    }
}
