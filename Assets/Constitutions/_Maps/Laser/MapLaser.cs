using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapLaser : MonoBehaviour
{
    private LineRenderer lineRederer;
    public SpriteRenderer hitSprite;
    public SpriteRenderer lauchSprite;
    public float hitPosOffsetY = 0.0f;
    public float lauchPosOffsetY = 0.0f;

    public bool destroyEverything = false;
    public Material destroyMat;
    public float damage;

    void Awake()
    {
        lineRederer = GetComponentInChildren<LineRenderer>();
        lineRederer.enabled = true;
        lineRederer.useWorldSpace = true;

        if (destroyEverything) {
            lineRederer.startColor = new Color(1, 0, 0);
            lauchSprite.color = new Color(1, 0, 0);
        }
    }
    
    void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -transform.up);
        Debug.DrawLine(transform.position, hit.point);
        hitSprite.transform.position = hit.point + new Vector2(0, hitPosOffsetY);
        lauchSprite.transform.position = transform.position + new Vector3(0, lauchPosOffsetY);
        lineRederer.SetPosition(0, transform.position);
        lineRederer.SetPosition(1, hit.point);
        if (hit.collider.GetComponent<UsrState>()) {
            hit.collider.GetComponent<UsrState>().Hurted(new Vector2(1, 0), (int)damage);
        }
        else if (hit.collider.GetComponent<AliveInLaser>()) {
        }
        else if (!hit.collider.GetComponent<SpriteRenderer>()&&destroyEverything) {
            Destroy(hit.collider.gameObject);
        }
        else if (hit.collider.GetComponent<SpriteRenderer>().material.name != destroyMat.name&&destroyEverything) {
            StartCoroutine(DestroyEverything(hit.collider.GetComponent<SpriteRenderer>()));
        }
    }

    IEnumerator DestroyEverything(Renderer renderer) {
        renderer.material = destroyMat;
        renderer.material.name = destroyMat.name;
        while (renderer.material.GetFloat("_DissolveThreshold") < 1) {
            float temp = renderer.material.GetFloat("_DissolveThreshold");
            temp += 1 * Time.deltaTime;
            if (temp > 1)
                temp = 1;
            renderer.material.SetFloat("_DissolveThreshold", temp);
            yield return new WaitForEndOfFrame();
        }
        Destroy(renderer.gameObject);
    }
}
