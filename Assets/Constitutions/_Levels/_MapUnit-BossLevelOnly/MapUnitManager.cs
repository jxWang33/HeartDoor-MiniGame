using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapUnitManager : MonoBehaviour
{
    public List<MapUnit> mapUnitList;
    public List<GameObject> keyList;
    public UsrState usrState;
    public HeartBoss heartBoss;
    public Transform cameraPos;
    public Transform envCameraPos;

    public GameObject pbGoldenKey;

    void Awake()
    {
        keyList = new List<GameObject>();
    }
    
    void Update()
    {
        if (usrState == null)
            return;

        if (usrState.transform.position.y < mapUnitList[1].transform.position.y - mapUnitList[1].unitSize.y / 2)  {
            MapUnit tempUnit = mapUnitList[0];
            mapUnitList.RemoveAt(0);
            Vector3 tempPos = tempUnit.transform.position;
            tempPos.y -= tempUnit.unitSize.y * 3;
            tempUnit.transform.position = tempPos;
            mapUnitList.Add(tempUnit);

            int tempI = keyList.Count;
            for (int i = 0; i < tempI; i++) {
                Destroy(keyList[0]);
                keyList.RemoveAt(0);
            }

            float tempX = (float)GMManager.rd.NextDouble() * 8 - 4;
            GameObject tempGameObject = Instantiate(pbGoldenKey, new Vector2(tempX, mapUnitList[0].transform.position.y), Quaternion.identity);
            keyList.Add(tempGameObject);
            tempX = (float)GMManager.rd.NextDouble() * 8 - 4;
            tempGameObject = Instantiate(pbGoldenKey, new Vector2(tempX, mapUnitList[0].transform.position.y), Quaternion.identity);
            keyList.Add(tempGameObject);

            DeceptionMove(tempUnit.unitSize.y);
        }
    }

    void DeceptionMove(float d) {
        Vector3 tempPos;
        foreach (GameObject i in keyList) {
            tempPos = i.transform.position;
            tempPos.y += d;
            i.transform.position = tempPos;
        }
        foreach (MapUnit i in mapUnitList) {
            tempPos = i.transform.position;
            tempPos.y += d;
            i.transform.position = tempPos;
        }

        tempPos = usrState.transform.position;
        tempPos.y += d;
        usrState.transform.position = tempPos;
        if (heartBoss) {
            tempPos = heartBoss.transform.position;
            tempPos.y += d;
            heartBoss.transform.position = tempPos;
        }
        tempPos = cameraPos.transform.position;
        tempPos.y += d;
        cameraPos.transform.position = tempPos;

        tempPos = envCameraPos.transform.position;
        tempPos.y += d;
        envCameraPos.transform.position = tempPos;
    }
}
