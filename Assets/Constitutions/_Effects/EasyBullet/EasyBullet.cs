using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class EasyBullet : MonoBehaviour
{
    private float damage;
    private Vector2 direction;
    private BoxCollider2D boxCollider2D;
    private Animator animator;
    public AnimationClip hit;
    

    [SerializeField]
    private float speed = 5;

    public void Set(Vector2 dir, int dam)
    {
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
        animator = GetComponent<Animator>();

        hit.events = null;
        AnimationEvent hitEndEvent = new AnimationEvent {
            time = hit.length,
            functionName = "HitEnd"
        };
        hit.AddEvent(hitEndEvent);
    }

    void Update()
    {
        if (animator.GetBool("isHit")) {
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
    }

    public void HitEnd() {
        Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.layer!=LayerMask.NameToLayer("Default")&& collision.gameObject.layer != LayerMask.NameToLayer("solid")
            && collision.gameObject.layer != LayerMask.NameToLayer("wall") && collision.gameObject.layer != LayerMask.NameToLayer("door"))
            return;
        animator.SetBool("isHit", true);
        if (collision.transform.GetComponent<UsrState>() != null) {
            collision.transform.GetComponent<UsrState>().Hurted(direction, (int)damage);
        }
    }
}
