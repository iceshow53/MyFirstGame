using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

[System.Serializable]
class DataForm
{
	// ** 데이터 목록
	public string kill;
	public string mostKill;
	public string Died;
	public string Clear;

	public DataForm(string _kill, string _mostKill, string _Died, string _Clear)
	{
		kill = _kill;
		mostKill = _mostKill;
		Died = _Died;
		Clear = _Clear;
	}
}

class PlayerStats
{
	// ** 플레이어 업그레이드 정보 저장
	public string MaxHP;
	public string Damage;
	public string Ammo;
	public string EXP;
	public string Movement;
	public string Reload;
	public string Fire;
	public string MoreBullet;
	public string Piercing;
	public string Point;

    public PlayerStats(string maxHP, string damage, string ammo, string eXP, string movement, string reload, string fire, string moreBullet, string piercing, string point)
    {
        MaxHP = maxHP;
        Damage = damage;
        Ammo = ammo;
        EXP = eXP;
        Movement = movement;
        Reload = reload;
        Fire = fire;
        MoreBullet = moreBullet;
        Piercing = piercing;
		Point = point;
    }
}

// ** 업적 정보
class Achive
{
	public int achive1;
	public int achive2;
	public int achive3;
	public int achive4;
	public int achive5;
	public int achive6;
	public int achive7;
	public int achive8;
	public int achive9;
	public int achive10;

	public Achive(int achive1, int achive2, int achive3, int achive4, int achive5, int achive6, int achive7, int achive8, int achive9, int achive10)
	{
		this.achive1 = achive1;
		this.achive2 = achive2;
		this.achive3 = achive3;
		this.achive4 = achive4;
		this.achive5 = achive5;
		this.achive6 = achive6;
		this.achive7 = achive7;
		this.achive8 = achive8;
		this.achive9 = achive9;
		this.achive10 = achive10;
	}
}

//class WebPlayerStats
//{
//	// ** 플레이어 업그레이드 정보 저장
//	public int MaxHP;
//	public int Damage;
//	public int Ammo;
//	public int EXP;
//	public int Movement;
//	public int Reload;
//	public int Fire;
//	public int MoreBullet;
//	public int Piercing;
//	public int Point;
//}

public class SaveController : MonoBehaviour
{
	public SaveController() { }
	public static SaveController instance = null;

	public static SaveController Getinstance
	{
		get
		{
			if (instance == null)
				return null;
			return instance;
		}
	}

	private DataForm playInfo;
	private TextAsset JsonData;

	private PlayerStats playerstats;
	private TextAsset PlayerJsonData;


	public bool hardmod;

	public bool Achive_load,Stats_load;

	public float Sound1, Sound2, Sound3;

	public int KILL, DIE, CLEAR, MOSTKILL;
	private int I_HP, I_Damage, I_Ammo, I_EXP, I_Movement, I_Reload, I_Fire, I_MoreBullet, I_Piercing, statPoint;
	public List<int> AchiveList = new List<int>();

	private void Awake()
	{
		if (instance == null)
		{
			instance = this;

			Achive_load = Stats_load = false;

			fileCheck();

			LoadData();

			WebDataCheck();
			

			hardmod = false;
			Sound1 = Sound2 = Sound3 = -20.0f;

			DontDestroyOnLoad(gameObject);
		}
	}

	private void AchiveCheck()
	{
		if (DIE >= 2)
			AchiveList[8] = 1;
		if (CLEAR >= 5)
			AchiveList[7] = 1;
	}

	// ** 파일 없으면 생성
	public void fileCheck()
    {
		string file1 = Application.dataPath + "/Resources/saveFile/playerinfo.json";
		string file2 = Application.dataPath + "/Resources/saveFile/saveFile.json";

		if(!File.Exists(file1))
        {
			StatSaveData("0", "0", "0", "0", "0", "0", "0", "0", "0", "0");
        }
		if(!File.Exists(file2))
        {
			SaveData("0", "0", "0", "0");
        }
	}

	// ** 플레이어 업그레이드 정보 받아오기
	public void WebDataCheck()
	{
		// ** 받아오기전 업그레이드 항목 비활성화
		GameObject.Find("UpgradeScene").GetComponent<Button>().interactable = false;
		StartCoroutine(WebDataDownload());
		StartCoroutine(WebAchiveData());
	}

	IEnumerator WebDataDownload()
	{
		string URL = "https://script.google.com/macros/s/AKfycbxvXNUX56vzM8Catfgv-MnTPDpRW1Koxb_5DTQVfWJmVUGKgf0n8wxlwPP0mD9GbzeC/exec";

		WWWForm form = new WWWForm();

		form.AddField("Pk", gameManager.Getinstance.PlayerID);
		form.AddField("order", "Find");

		using (UnityWebRequest request = UnityWebRequest.Post(URL, form))
		{
			yield return request.SendWebRequest();

			PlayerStats json = JsonUtility.FromJson<PlayerStats>(request.downloadHandler.text);

			I_HP = int.Parse(json.MaxHP);
			I_Damage = int.Parse(json.Damage);
			I_Ammo = int.Parse(json.Ammo);
			I_EXP = int.Parse(json.EXP);
			I_Movement = int.Parse(json.Movement);
			I_Reload = int.Parse(json.Reload);
			I_Fire = int.Parse(json.Fire);
			I_MoreBullet = int.Parse(json.MoreBullet);
			I_Piercing = int.Parse(json.Piercing);
			statPoint = int.Parse(json.Point);

			Stats_load = true;

			// ** 정보를 받아와서 적용이 끝났을 때 다시 버튼 활성화
			if(GameObject.Find("UpgradeScene") != null)
				GameObject.Find("UpgradeScene").GetComponent<Button>().interactable = true;
		}
	}

	IEnumerator WebAchiveData()
	{
		string URL = "https://script.google.com/macros/s/AKfycbxvXNUX56vzM8Catfgv-MnTPDpRW1Koxb_5DTQVfWJmVUGKgf0n8wxlwPP0mD9GbzeC/exec";

		WWWForm form = new WWWForm();

		form.AddField("Pk", gameManager.Getinstance.PlayerID);
		form.AddField("order", "getAchive");

		using (UnityWebRequest request = UnityWebRequest.Post(URL, form))
		{
			yield return request.SendWebRequest();

			Achive json = JsonUtility.FromJson<Achive>(request.downloadHandler.text);

			AchiveList.Add(json.achive1);
			AchiveList.Add(json.achive2);
			AchiveList.Add(json.achive3);
			AchiveList.Add(json.achive4);
			AchiveList.Add(json.achive5);
			AchiveList.Add(json.achive6);
			AchiveList.Add(json.achive7);
			AchiveList.Add(json.achive8);
			AchiveList.Add(json.achive9);
			AchiveList.Add(json.achive10);

			Achive_load = true;

			AchiveCheck();
		}
	}

	IEnumerator AchiveUpdate()
	{
		string URL = "https://script.google.com/macros/s/AKfycbxvXNUX56vzM8Catfgv-MnTPDpRW1Koxb_5DTQVfWJmVUGKgf0n8wxlwPP0mD9GbzeC/exec";

		WWWForm form = new WWWForm();

		form.AddField("Pk", gameManager.Getinstance.PlayerID);
		form.AddField("order", "setAchive");
		for(int i = 0; i<AchiveList.Count;++i)
			form.AddField("achive"+(i+1).ToString(), AchiveList[i]);

		using (UnityWebRequest request = UnityWebRequest.Post(URL, form))
		{
			yield return request.SendWebRequest();
		}
	}

	public void StatUpdate()
	{
		StartCoroutine(StatsUpdate());
	}

	public void AchiveUp()
	{
		StartCoroutine(AchiveUpdate());
	}

	IEnumerator StatsUpdate()
	{
		string URL = "https://script.google.com/macros/s/AKfycbxvXNUX56vzM8Catfgv-MnTPDpRW1Koxb_5DTQVfWJmVUGKgf0n8wxlwPP0mD9GbzeC/exec";

		WWWForm form = new WWWForm();

		form.AddField("MaxHP", I_HP);
		form.AddField("Damage", I_Damage);
		form.AddField("Ammo", I_Ammo);
		form.AddField("EXP", I_EXP);
		form.AddField("Movement", I_Movement);
		form.AddField("Reload", I_Reload);
		form.AddField("Fire", I_Fire);
		form.AddField("MoreBullet", I_MoreBullet);
		form.AddField("Piercing", I_Piercing);
		form.AddField("Point", statPoint);
		form.AddField("order", "change");
		form.AddField("Pk", gameManager.Getinstance.PlayerID);

		using (UnityWebRequest request = UnityWebRequest.Post(URL, form))
		{

			yield return request.SendWebRequest();
		}
	}


	// ** 데이터 저장
    public void onSave()
    {
		SaveData(KILL.ToString(), MOSTKILL.ToString(), DIE.ToString(), CLEAR.ToString());
		StatUpdate();
	}

	public void onSaveStat()
    {
		StatSaveData(I_HP.ToString(), I_Damage.ToString(), I_Ammo.ToString(), I_EXP.ToString(), I_Movement.ToString(), I_Reload.ToString(), I_Fire.ToString(), I_MoreBullet.ToString(), I_Piercing.ToString(), statPoint.ToString());
    }

	public void StatSaveData(string maxHP, string damage, string ammo, string eXP, string movement, string reload, string fire, string moreBullet, string piercing, string point)
    {
		PlayerStats _form = new(maxHP, damage, ammo, eXP, movement, reload, fire, moreBullet, piercing, point);

		string Jsondata = JsonUtility.ToJson(_form);

		FileStream fileStream = new(Application.dataPath + "/Resources/saveFile/playerinfo.json", FileMode.Create);

		byte[] data = Encoding.UTF8.GetBytes(Jsondata);

		fileStream.Write(data, 0, data.Length);
		fileStream.Close(); // ** 항상 볼일이 끝나면 닫아주어야 함.(리소스 차지)
	}

	// ** 데이터 저장
	public void SaveData(string _kill, string _mostKill, string _Died, string _Clear)
	{
		DataForm _form = new(_kill, _mostKill, _Died, _Clear);

		string Jsondata = JsonUtility.ToJson(_form);

		FileStream fileStream = new(Application.dataPath + "/Resources/saveFile/saveFile.json", FileMode.Create);

		byte[] data = Encoding.UTF8.GetBytes(Jsondata);

		fileStream.Write(data, 0, data.Length);
		fileStream.Close(); // ** 항상 볼일이 끝나면 닫아주어야 함.(리소스 차지)
	}

	// ** 데이터 불러오기
	public void LoadData()
    {
		string path = Application.dataPath + "/Resources/saveFile/saveFile.json";
		FileStream fileStream = new(path, FileMode.Open);

		byte[] data = new byte[path.Length];

		fileStream.Read(data, 0, data.Length);
		fileStream.Close();

		string json = Encoding.UTF8.GetString(data);

		playInfo = JsonUtility.FromJson<DataForm>(json);

		KILL = int.Parse(playInfo.kill);
		DIE = int.Parse(playInfo.Died);
		CLEAR = int.Parse(playInfo.Clear);
		MOSTKILL = int.Parse(playInfo.mostKill);
	}

	// ** 데이터 초기화
	public void ResetData()
    {
		SaveData("0", "0", "0", "0");
		StatSaveData("0", "0", "0", "0", "0", "0", "0", "0", "0", "0");
		KILL = DIE = CLEAR = MOSTKILL = 0;
		statPoint = I_HP + I_Damage + I_Ammo + I_EXP + I_Movement + I_Reload + I_Fire + I_MoreBullet + I_Piercing;
		I_HP = I_Damage = I_Ammo = I_EXP = I_Movement = I_Reload = I_Fire = I_MoreBullet = I_Piercing = 0;
		StatUpdate();
	}

	// ** 데이터를 리스트 형태로 바로 받아오기
	public List<string> getData()
    {
		List<string> data = new List<string>();
		data.Add(playInfo.kill);
		data.Add(playInfo.Died);
		data.Add(playInfo.Clear);
		data.Add(playInfo.mostKill);

		return data;
    }

	public List<int> getStatData()
    {
		List<int> data = new List<int>();
		data.Add(I_HP);
		data.Add(I_Damage);
		data.Add(I_Ammo);
		data.Add(I_EXP);
		data.Add(I_Movement);
		data.Add(I_Reload);
		data.Add(I_Fire);
		data.Add(I_MoreBullet);
		data.Add(I_Piercing);
		data.Add(statPoint);

		return data;
	}

	public void UpdateStatData(List<int> data)
    {
		I_HP = data[0];
		I_Damage = data[1];
		I_Ammo = data[2];
		I_EXP = data[3];
		I_Movement = data[4];
		I_Reload = data[5];
		I_Fire = data[6];
		I_MoreBullet = data[7];
		I_Piercing = data[8];
		statPoint = data[9];
	}

	// ** 하드모드 설정
	public void hardMode()
    {
		hardmod = !hardmod;
    }

	public void StatAdd(int i) { statPoint += i; }
}
