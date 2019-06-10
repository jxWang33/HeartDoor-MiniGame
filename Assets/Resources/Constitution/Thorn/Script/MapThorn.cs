using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class MapThorn : MonoBehaviour
{
    private SpriteRenderer spriteRender;
    private Animator animator;
    const float perLenth = 0.41f;
    public string activeName= "thorn_0";
    public int damage = 10;
    public int count = 5;

    public bool upOnStart = false;

    void Start() {
        gameObject.layer = LayerMask.NameToLayer("interact");
        spriteRender = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        Vector2 tempSize = spriteRender.size;
        tempSize.x = count * perLenth;
        spriteRender.size = tempSize;

        GetComponent<BoxCollider2D>().size = spriteRender.size;
        GetComponent<BoxCollider2D>().isTrigger = true;

        if (upOnStart)
            SetUp();
    }

    private void OnTriggerStay2D(Collider2D collision) {
        if (spriteRender.sprite.name != activeName)
            return;
        UsrState tempState = collision.GetComponent<UsrState>();
        if (tempState != null) {
            Vector2 hurtDir = tempState.currentDir;
            hurtDir.x = -hurtDir.x;

            tempState.Hurted(hurtDir,damage);
        }
        NumbFish tempFish = collision.GetComponent<NumbFish>();
        if (tempFish != null) {
            tempFish.Dead();
        }
    }

    public void SetUp() {
        animator.SetBool("isUp", true);
    }
    public void SetDown() {
        animator.SetBool("isUp", false);
    }

    public void Change() {
        if (animator.GetBool("isUp"))
            SetDown();
        else
            SetUp();
    }
}
