using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextModule : MonoBehaviour
{
    private TextModule() { }
    private static TextModule Instance = null;

    public static TextModule Getinstance
    {
        get
        {
            if (Instance == null)
                return null;
            return Instance;
        }
    }

    private GameObject textBox;
    private Image image;
    private Text text;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            textBox = GameObject.Find("UI").transform.Find("Dialog").gameObject;
            image = textBox.transform.Find("CharImage").GetComponent<Image>();
            text = textBox.transform.Find("Dialog").GetComponent<Text>();
        }
    }

	void Start()
    {
        
    }

    public void OpenTextBox()
	{
        if (textBox.activeSelf)
        {
            PlayerInfo.Getinstance.set_pause(-1);
            textBox.SetActive(false);
        }
        else
        {
            PlayerInfo.Getinstance.set_pause(1);
            textBox.SetActive(true);
        }
    }


    public void PrintText(string texts, Sprite icon)
	{
        text.text = texts;
        image.sprite = icon;
	}

    
}
