using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// InputField에서 Tab과 Shift+Tab키를 누르는 것을 관리하는 클래스
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
    /// Focus 할 InputField를 설정한다.
    /// </summary>
    /// <param name="idx">Focus 할 InputField의 index 번호</param>
    public void SetFocus(int idx = 0)
    {
        if (idx >= 0 && idx < list.Count)
            list[idx].Select();
    }

    /// <summary>
    /// Tab, Shift+Tab 키를 눌렀을 때 반응 할 InputField를 추가한다.
    /// </summary>
    /// <param name="inputField">추가 할 InputField</param>
    public void Add(InputField inputField)
    {
        list.Add(inputField);
    }

    /// <summary>
    /// 현재 위치를 얻는다.
    /// </summary>
    /// <returns>현재 위치의 Index</returns>
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
    /// 다음 InputField로 Focus한다.
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
    /// 이전 InputField로 Focus한다.
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
    /// Tab키와 Shift + Tab키를 눌렀는지 체크하여 눌렀으면 Focus를 옮긴다.
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
    //InputFieldTabManager inputFieldTabMrg = new InputFieldTabManager(); 를 이용하여 선언 후.
    //inputFieldTabMrg.Add(인풋필드); 로 인풋필드를 추가해준다.
    //inputFieldTabMrg.SetFocus(); 를 이용하여 포커스를 맞춘 후
    //Update() 함수 안에서 inputFieldTabMrg.CheckFocus() 함수를 실행해주면 된다.

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
