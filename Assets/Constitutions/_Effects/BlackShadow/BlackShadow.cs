using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackShadow : MonoBehaviour
{
    private float damage;
    private Vector2 direction;
    private BoxCollider2D boxCollider2D;
    
    public float speed = 10;

    public void Set(Vector2 dir, int dam) {
        damage = dam;
        direction = dir;
        GetComponent<Rigidbody2D>().velocity = direction * speed;

        if (direction == new Vector2(1, 0))
            transform.localScale = new Vector3(1, 1, 1);
        else if (direction == new Vector2(-1, 0))
            transform.localScale = new Vector3(-1, 1, 1);
    }

    private void Awake() {
        gameObject.layer = LayerMask.NameToLayer("interact");
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.transform.GetComponent<UsrState>() != null) {
            collision.transform.GetComponent<UsrState>().Hurted(direction, (int)damage);
        }
        if (collision.transform.GetComponent<DoorSwitch>() != null) {
            collision.transform.GetComponent<DoorSwitch>().Change();
        }
    }
    private void OnBecameInvisible() {
        Destroy(gameObject);
    }
}
