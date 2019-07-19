using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveMapP2P : MonoBehaviour
{
    private Vector2 startPos;
    private Vector2 endPos;
    private Vector2 target;

    private List<Transform> contactsTransform;
    public float moveSpeed = 0;

    [SerializeField]
    private Vector2 distance = Vector2.zero;
    [SerializeField]
    private float stayTimeInter = 0;

    public float nearAngleLoss = 0.1f;

    void Awake()
    {
        contactsTransform = new List<Transform>();
        startPos = transform.position;
        endPos = startPos + distance;
        target = endPos;

        StartCoroutine(MapPlatMove());
    }

    IEnumerator MapPlatMove() {
        while (true) {
            Vector3 prePos = transform.position;
            transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
            Vector3 movement = transform.position - prePos;
            for (int i = 0; i < contactsTransform.Count; i++) {
                if (contactsTransform[i])
                    contactsTransform[i].position += movement;
                else
                    contactsTransform.RemoveAt(i--);
            }

            if ((Vector2)transform.position == target) {
                if (target == endPos)
                    target = startPos;
                else if (target == startPos)
                    target = endPos;
                yield return new WaitForSeconds(stayTimeInter);
            }
            yield return new WaitForEndOfFrame();
        }
    }
    private void OnCollisionEnter2D(Collision2D collision) {
        if (Mathf.Abs(collision.GetContact(0).normal.y + 1) <= nearAngleLoss) {
            contactsTransform.Add(collision.transform);
        }
    }
    private void OnCollisionExit2D(Collision2D collision) {
        if (contactsTransform.Contains(collision.transform)) {
            contactsTransform.Remove(collision.transform);
        }
    }
}
