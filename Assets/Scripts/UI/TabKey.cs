using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// InputField���� Tab�� Shift+TabŰ�� ������ ���� �����ϴ� Ŭ����
/// </summary>
public class InputFieldTabManager
{
    private List<InputField> list;
    private int curPos;

    public InputFieldTabManager()
    {
        list = new List<InputField>();
    }

    /// <summary>
    /// Focus �� InputField�� �����Ѵ�.
    /// </summary>
    /// <param name="idx">Focus �� InputField�� index ��ȣ</param>
    public void SetFocus(int idx = 0)
    {
        if (idx >= 0 && idx < list.Count)
            list[idx].Select();
    }

    /// <summary>
    /// Tab, Shift+Tab Ű�� ������ �� ���� �� InputField�� �߰��Ѵ�.
    /// </summary>
    /// <param name="inputField">�߰� �� InputField</param>
    public void Add(InputField inputField)
    {
        list.Add(inputField);
    }

    /// <summary>
    /// ���� ��ġ�� ��´�.
    /// </summary>
    /// <returns>���� ��ġ�� Index</returns>
    private int GetCurerntPos()
    {
        for (int i = 0; i < list.Count; ++i)
        {
            if (list[i].isFocused == true)
            {
                curPos = i;
                break;
            }
        }
        return curPos;
    }

    /// <summary>
    /// ���� InputField�� Focus�Ѵ�.
    /// </summary>
    private void MoveNext()
    {
        GetCurerntPos();
        if (curPos < list.Count - 1)
        {
            ++curPos;
            list[curPos].Select();
        }
        else
		{
            curPos = 0;
            list[curPos].Select();
		}
    }

    /// <summary>
    /// ���� InputField�� Focus�Ѵ�.
    /// </summary>
    private void MovePrev()
    {
        GetCurerntPos();
        if (curPos > 0)
        {
            --curPos;
            list[curPos].Select();
        }
    }

    /// <summary>
    /// TabŰ�� Shift + TabŰ�� �������� üũ�Ͽ� �������� Focus�� �ű��.
    /// </summary>
    public void CheckFocus()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && !Input.GetKey(KeyCode.LeftShift))
        {
            MoveNext();
        }
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Tab))
        {
            MovePrev();
        }
    }
}

public class TabKey : MonoBehaviour
{
    //InputFieldTabManager inputFieldTabMrg = new InputFieldTabManager(); �� �̿��Ͽ� ���� ��.
    //inputFieldTabMrg.Add(��ǲ�ʵ�); �� ��ǲ�ʵ带 �߰����ش�.
    //inputFieldTabMrg.SetFocus(); �� �̿��Ͽ� ��Ŀ���� ���� ��
    //Update() �Լ� �ȿ��� inputFieldTabMrg.CheckFocus() �Լ��� �������ָ� �ȴ�.

    InputFieldTabManager inputFieldTabMrg = new InputFieldTabManager();
    public List<InputField> fields;

    void Start()
    {
        for (int i = 0; i < fields.Count; ++i)
            inputFieldTabMrg.Add(fields[i]);
        inputFieldTabMrg.SetFocus();
    }

    // Update is called once per frame
    void Update()
    {
        inputFieldTabMrg.CheckFocus();
    }
}
