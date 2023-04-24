using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoxContoller : MonoBehaviour
{
    private GameObject BoxObject;
    private BoxUIController BoxUI;
    public Sprite sprite;
    private SpriteRenderer Render;
    private GameObject Tracker;
    private GameObject SpawnedTracker;


    private void Awake()
    {
        BoxObject = GameObject.Find("BoxOpen");
        Render = transform.GetComponent<SpriteRenderer>();
        Tracker = Resources.Load("Prefabs/BoxTracking") as GameObject;
    }

    void Start()
    {
        BoxUI = BoxObject.GetComponent<BoxUIController>();
        SpawnTacker();
        
    }

    // ** 상자를 열었을 때 UI를 움직이고 상자를 제거
    public void OpenBox()
    {
        Render.sprite = sprite;
        transform.GetComponent<BoxCollider2D>().enabled = false;
        BoxUI.OnActivate();
        Destroy(SpawnedTracker);
        Destroy(gameObject, 5.0f);
    }

    public void SpawnTacker()
    {
        SpawnedTracker = Instantiate(Tracker);
        SpawnedTracker.transform.position = PlayerInfo.Getinstance.playerPos();
        SpawnedTracker.transform.parent = GameObject.Find("Tracker").transform;
        SpawnedTracker.GetComponent<BoxTracker>().BoxPostion(transform.position);
    }
}
