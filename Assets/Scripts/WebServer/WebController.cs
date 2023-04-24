using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

[System.Serializable]
public class MemberForm
{
    public string name;
    public string age;
    public int gender;

	public MemberForm(string name, string age)
	{
		this.name = name;
		this.age = age;
	}
}

public class WebController : MonoBehaviour
{
	string URL = "https://script.google.com/macros/s/AKfycbxvXNUX56vzM8Catfgv-MnTPDpRW1Koxb_5DTQVfWJmVUGKgf0n8wxlwPP0mD9GbzeC/exec";

	public Text AttempText;

	// ** �α��ο� ��ǲ�ʵ�
	public InputField ID, Password;
	// ** ȸ������, ��й�ȣ ����� ��ǲ�ʵ�
	public InputField R_ID,R_Password,R_Email;


	public GameObject LoginAttemp;
	public GameObject RegisterForm;

	public Text checkText1, checkText2, checkText3;

	private bool register, passwordchange;

	private bool is_register;

	private string emailPattern = @"^[\w-.]+@([\w-]+.)+[\w-]{2,4}$";

	private void Awake()
	{
		register = passwordchange = false;
	}

	IEnumerator Registor()
	{
		MemberForm member = new MemberForm(R_ID.text, Security(R_Password.text));
		WWWForm form = new WWWForm();
		form.AddField("Name", member.name);
		form.AddField("Age", member.age);
		form.AddField("Email", R_Email.text);
		form.AddField("Time", DateTime.Now.ToString());
		form.AddField("order", "sign up");

		using (UnityWebRequest request = UnityWebRequest.Post(URL, form))
		{
			AttempText.text = "ȸ������ �����...";

			yield return request.SendWebRequest();

			if (request.downloadHandler.text == "True")
			{
				AttempText.text = "ȸ������ ����!";
				yield return new WaitForSeconds(1.0f);
				LoginAttemp.SetActive(false);
			}
			else
			{
				AttempText.text = "�ߺ��� ���̵� �Դϴ�.";
				yield return new WaitForSeconds(1.0f);
				LoginAttemp.SetActive(false);
			}
		}
	}

	IEnumerator Login()
	{
		MemberForm member = new MemberForm(ID.text, Security(Password.text));

		WWWForm form = new WWWForm();

		form.AddField("Name", member.name);
		form.AddField("Age", member.age);
		form.AddField("Time", DateTime.Now.ToString());
		form.AddField("order", "sign in");

		using (UnityWebRequest request = UnityWebRequest.Post(URL, form))
		{

			yield return request.SendWebRequest();

			if (request.downloadHandler.text == "False")
			{
				AttempText.text = "�α��� ����! ��й�ȣ�� �ٸ��ϴ�!";
				yield return new WaitForSeconds(1.0f);
				LoginAttemp.SetActive(false);
			}
			else
			{
				AttempText.text = "�α��� ����!";
				gameManager.Getinstance.PlayerID = int.Parse(request.downloadHandler.text);
				yield return new WaitForSeconds(1.0f);
				SceneManager.LoadScene("MainMenu");
			}
		}
	}

	public void Login1()
	{
		LoginAttemp.SetActive(true);

		if(!register && !passwordchange)
		{
			AttempText.text = "�α��� �õ���...";
			StartCoroutine(Login());
			return;
		}
		if (!InputFieldCheck(R_ID.text, R_Password.text, R_Email.text))
		{
			LoginAttemp.SetActive(false);
			return;
		}
		if (register)
		{
			AttempText.text = "ȸ������ �õ���...";
			RegisterForm.SetActive(false);
			register = false;
			StartCoroutine(Registor());
			return;
		}
		if(passwordchange)
		{
			AttempText.text = "��й�ȣ ������...";
			RegisterForm.SetActive(false);
			passwordchange = false;
			StartCoroutine(PasswordReset());
			return;
		}
	}

	private bool InputFieldCheck(string id, string password, string EMail)
	{
		bool Check = true;
		if (string.IsNullOrEmpty(id))
		{
			checkText1.text = "���̵� �Է��ϼ���";
			Check = false;
		}
		if(string.IsNullOrEmpty(password))
		{
			checkText2.text = "��й�ȣ�� �Է��ϼ���";
			Check = false;
		}
		if(string.IsNullOrEmpty(EMail))
		{
			checkText3.text = "�̸����� �Է��ϼ���";
			Check = false;
		}
		if (!Regex.IsMatch(EMail, emailPattern))
		{
			checkText3.text = "�̸��� ������ Ȯ�����ּ���";
			Check = false;
		}
		return Check;
	}

	public void ReTouching(int i)
	{
		if (i == 0)
			register = true;
		if (i == 1)
			passwordchange = true;
		RegisterForm.SetActive(true);
	}

	public void closeForm()
	{
		RegisterForm.SetActive(false);
		checkText1.text = checkText2.text = checkText3.text = "";
		register = passwordchange = false;
	}

	public void GameExit()
	{
		Application.Quit();
	}

	private void Update()
	{
		if(Input.GetKeyDown(KeyCode.Return))
		{
			Login1();
		}
	}

	IEnumerator PasswordReset()
	{
		MemberForm member = new MemberForm(R_ID.text, Security(R_Password.text));

		WWWForm form = new WWWForm();

		form.AddField("Name", member.name);
		form.AddField("Age", member.age);
		form.AddField("Email", R_Email.text);
		form.AddField("Time", DateTime.Now.ToString());
		form.AddField("order", "passwordreset");

		using (UnityWebRequest request = UnityWebRequest.Post(URL, form))
		{

			yield return request.SendWebRequest();

			if (request.downloadHandler.text == "False")
			{
				AttempText.text = "��й�ȣ ���� ����! ��ġ�ϴ� ���̵� �����ϴ�.";
				yield return new WaitForSeconds(1.0f);
				LoginAttemp.SetActive(false);
			}
			else if(request.downloadHandler.text == "WrongEmail")
			{
				AttempText.text = "��й�ȣ ���� ����! ȸ�� ������ �̸����� �ٸ��ϴ�.";
				yield return new WaitForSeconds(1.0f);
				LoginAttemp.SetActive(false);
			}
			else
			{
				AttempText.text = "��й�ȣ ���� ����! �ٽ� �α��� ���ּ���.";
				yield return new WaitForSeconds(1.0f);
				LoginAttemp.SetActive(false);
			}
		}
	}

	string Security(string password)
	{
		if (string.IsNullOrEmpty(password))
		{
			// ** true
			AttempText.text = "��й�ȣ�� �ʼ� �Է� �� �Դϴ�.";

			return "null";
		}
		else
		{
			// ** false
			// ** ��ȣȭ & ��ȣȭ
			SHA256 sha = new SHA256Managed();
			byte[] hash = sha.ComputeHash(Encoding.ASCII.GetBytes(password));
			StringBuilder stringBuilder = new StringBuilder();

			foreach (byte b in hash)
			{
				stringBuilder.AppendFormat("{0:x2}", b);
			}
			return stringBuilder.ToString();
		}
	}
}
