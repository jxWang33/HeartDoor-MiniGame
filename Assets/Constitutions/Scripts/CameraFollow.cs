using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {
    //目标对象
    public Transform target;
    public float angleX, angleY, angleZ;
    public float distance = 10.0f;
    public float showScale = 1.0f;
    public float rotateSpeed = 1.0f;
    // 需要锁定的坐标
    public bool freezeX, freezeY, freezeZ;

    // 跟随的平滑时间
    public float smoothTime = 0.3F;
    private float xVelocity, yVelocity, zVelocity = 0.0F;

    // 跟随的偏移量
    private Vector3 offset;
    private Vector3 currentOffset;
    
    private Vector3 oldPosition;
    private Vector3 startPosition;
    float ToArc(float angle) {
        float arc = angle * Mathf.PI / 180.0f;
        return arc;
    }

    public MapManager mapOn;
    void Start() {
        transform.eulerAngles = new Vector3(angleX, angleY, angleZ);
        float tempX = -distance * Mathf.Cos(ToArc(angleX)) * Mathf.Sin(ToArc(angleY));
        float tempY = distance * Mathf.Sin(ToArc(angleX));
        float tempZ = -distance * Mathf.Cos(ToArc(angleX)) * Mathf.Cos(ToArc(angleY));

        transform.position = new Vector3(target.position.x+tempX, target.position.y + tempY, target.position.z + tempZ);
        startPosition = transform.position;
        offset = transform.position - target.position;
    }

    void FixedUpdate() {
        if (target == null) { return; }
        oldPosition = transform.position;
        currentOffset = offset * showScale;
        if (!freezeX) {
            oldPosition.x = Mathf.SmoothDamp(transform.position.x, target.position.x + currentOffset.x, ref xVelocity, smoothTime);
        }

        if (!freezeY) {
            oldPosition.y = Mathf.SmoothDamp(transform.position.y, target.position.y + currentOffset.y, ref yVelocity, smoothTime);
        }

        if (!freezeZ) {
            oldPosition.z = Mathf.SmoothDamp(transform.position.z, target.position.z + currentOffset.z, ref zVelocity, smoothTime);
        }
        transform.position = oldPosition;
        LimitOnMap();
    }
    public void ResetPosition() {
        transform.position = startPosition;
    }

    private void LimitOnMap() {

        if (transform.position.x + Camera.main.orthographicSize / 9 * 16 > mapOn.mapSize.x / 2)
            transform.position = new Vector3(mapOn.mapSize.x / 2 - Camera.main.orthographicSize / 9 * 16, transform.position.y, -10);
        if (transform.position.x - Camera.main.orthographicSize / 9 * 16 < -mapOn.mapSize.x / 2)
            transform.position = new Vector3(-mapOn.mapSize.x / 2 + Camera.main.orthographicSize / 9 * 16, transform.position.y, -10);

        if (mapOn.mapSize.y == -1)
            return;

        if (transform.position.y + Camera.main.orthographicSize > mapOn.mapSize.y / 2)
            transform.position = new Vector3(transform.position.x, mapOn.mapSize.y / 2 - Camera.main.orthographicSize, -10);
        if (transform.position.y - Camera.main.orthographicSize < -mapOn.mapSize.y / 2)
            transform.position = new Vector3(transform.position.x, -mapOn.mapSize.y / 2 + Camera.main.orthographicSize, -10);
    }
}
