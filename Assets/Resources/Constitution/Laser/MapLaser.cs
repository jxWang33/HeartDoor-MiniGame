using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapLaser : MonoBehaviour
{
    private LineRenderer lineRederer;
    public SpriteRenderer hitSprite;
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
        hitSprite.transform.position = hit.point;
        lineRederer.SetPosition(0, transform.position);
        lineRederer.SetPosition(1, hitSprite.transform.position);
        if (hit.collider.GetComponent<UsrState>() != null) {
            hit.collider.GetComponent<UsrState>().Hurted(new Vector2(1, 0), 50);
        }
        else if (hit.collider.GetComponent<AliveInLaser>() == null) {
            Destroy(hit.collider.gameObject);
        }
    }
}
