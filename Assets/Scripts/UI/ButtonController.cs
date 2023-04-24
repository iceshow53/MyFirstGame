using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    public List<GameObject> Show = new List<GameObject>();
    public GameObject text;
    
    // ** 죽거나 클리어 했을 때 버튼 표시
    public void VisibleButton()
    {
        Show[0].SetActive(true);
        Show[1].SetActive(true);
        Show[2].SetActive(false);
        Show[3].SetActive(false);
        text.SetActive(false);
    }
}
