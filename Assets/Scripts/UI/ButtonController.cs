using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    public List<GameObject> Show = new List<GameObject>();
    public GameObject text;
    
    // ** �װų� Ŭ���� ���� �� ��ư ǥ��
    public void VisibleButton()
    {
        Show[0].SetActive(true);
        Show[1].SetActive(true);
        Show[2].SetActive(false);
        Show[3].SetActive(false);
        text.SetActive(false);
    }
}
