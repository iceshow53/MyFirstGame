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

	// ** 로그인용 인풋필드
	public InputField ID, Password;
	// ** 회원가입, 비밀번호 변경용 인풋필드
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
			AttempText.text = "회원가입 대기중...";

			yield return request.SendWebRequest();

			if (request.downloadHandler.text == "True")
			{
				AttempText.text = "회원가입 성공!";
				yield return new WaitForSeconds(1.0f);
				LoginAttemp.SetActive(false);
			}
			else
			{
				AttempText.text = "중복된 아이디 입니다.";
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
				AttempText.text = "로그인 실패! 비밀번호가 다릅니다!";
				yield return new WaitForSeconds(1.0f);
				LoginAttemp.SetActive(false);
			}
			else
			{
				AttempText.text = "로그인 성공!";
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
			AttempText.text = "로그인 시도중...";
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
			AttempText.text = "회원가입 시도중...";
			RegisterForm.SetActive(false);
			register = false;
			StartCoroutine(Registor());
			return;
		}
		if(passwordchange)
		{
			AttempText.text = "비밀번호 변경중...";
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
			checkText1.text = "아이디를 입력하세요";
			Check = false;
		}
		if(string.IsNullOrEmpty(password))
		{
			checkText2.text = "비밀번호를 입력하세요";
			Check = false;
		}
		if(string.IsNullOrEmpty(EMail))
		{
			checkText3.text = "이메일을 입력하세요";
			Check = false;
		}
		if (!Regex.IsMatch(EMail, emailPattern))
		{
			checkText3.text = "이메일 형식을 확인해주세요";
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
				AttempText.text = "비밀번호 변경 실패! 일치하는 아이디가 없습니다.";
				yield return new WaitForSeconds(1.0f);
				LoginAttemp.SetActive(false);
			}
			else if(request.downloadHandler.text == "WrongEmail")
			{
				AttempText.text = "비밀번호 변경 실패! 회원 정보와 이메일이 다릅니다.";
				yield return new WaitForSeconds(1.0f);
				LoginAttemp.SetActive(false);
			}
			else
			{
				AttempText.text = "비밀번호 변경 성공! 다시 로그인 해주세요.";
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
			AttempText.text = "비밀번호는 필수 입력 값 입니다.";

			return "null";
		}
		else
		{
			// ** false
			// ** 암호화 & 복호화
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
