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
    void Awake()
    {
        lineRederer = GetComponentInChildren<LineRenderer>();
        lineRederer.enabled = true;
        lineRederer.useWorldSpace = true;
    }
    
    void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -transform.up);
        Debug.DrawLine(transform.position, hit.point);
        hitSprite.transform.position = hit.point + new Vector2(0, hitPosOffsetY);
        lauchSprite.transform.position = transform.position + new Vector3(0, lauchPosOffsetY);
        lineRederer.SetPosition(0, transform.position);
        lineRederer.SetPosition(1, hit.point);
        if (hit.collider.GetComponent<UsrState>() != null) {
            hit.collider.GetComponent<UsrState>().Hurted(new Vector2(1, 0), 50);
        }
        else if (hit.collider.GetComponent<AliveInLaser>() == null) {
            Destroy(hit.collider.gameObject);
        }
    }
}
