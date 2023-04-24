using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Key : MonoBehaviour
{
    // ** �������� ������ �� ������ Ʈ��Ŀ
    private GameObject Tracker;
    // ** �� Ʈ��Ŀ�� ������ ������Ʈ
    private GameObject SpawnedTracker;

	private void Awake()
	{
        Tracker = Resources.Load("Prefabs/BoxTracking") as GameObject;
    }

	void Start()
    {
        SpawnTacker();
    }

    public void GetKey()
	{
        SaveController.Getinstance.AchiveList[5] = 1;
        SaveController.Getinstance.AchiveUp();
        Destroy(SpawnedTracker);
        Destroy(gameObject);
	}

	private void SpawnTacker()
    {
        SpawnedTracker = Instantiate(Tracker);
        SpawnedTracker.transform.position = PlayerInfo.Getinstance.playerPos();
        SpawnedTracker.transform.parent = GameObject.Find("Tracker").transform;
        SpawnedTracker.GetComponent<BoxTracker>().BoxPostion(transform.position);
        SpawnedTracker.GetComponent<BoxTracker>().SetColor(new Vector4(255, 242, 52, 255));
    }
}
