using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hall : MonoBehaviour
{
	// ** ��ȭâ
	private GameObject textBox, interaction;
	public Sprite icon;

	private Text text;

	// ** Ʈ��Ŀ
	private GameObject SpawnedTracker, Tracker;
	private DroneController dronecontroller;


	// ** ��ȭ ����Ʈ
	private List<string> HiddenFound = new List<string>();
	private List<string> SignText = new List<string>();
	private List<string> AfterFound = new List<string>();
	private int list;

	private bool is_enter, is_founded;

	private void Awake()
	{
		if(SaveController.Getinstance.AchiveList[0] != 1)
			Destroy(gameObject);

		Tracker = Resources.Load("Prefabs/BoxTracking") as GameObject;
		textBox = GameObject.Find("UI").transform.Find("Dialog").gameObject;
		interaction = gameObject.transform.Find("Triangle").gameObject;
		dronecontroller = GameObject.Find("Player").transform.Find("SubWeapon").Find("drone").GetComponent<DroneController>();

		text = textBox.transform.Find("Dialog").GetComponent<Text>();
	}

	void Start()
	{
		interaction.SetActive(false);

		HiddenFound.Add("���� �ӿ��� ���� ���δ�.");
		HiddenFound.Add("���ο��� ������ ����� �־���!");
		HiddenFound.Add("��Ե� �����ߴ�.");

		SignText.Add("���� ���� �ʹ� ��Ӵ�.");
		SignText.Add("����� �̿��� Ž���غ���?");

		AfterFound.Add("�ȿ� ���� �ƹ��͵� ����.");

		list = 0;
		is_enter = false;

		if (SaveController.Getinstance.AchiveList[2] == 1)
			is_founded = true;
		else
			is_founded = false;

		SpawnTacker();
	}


	void Update()
	{
		if (is_enter)
		{
			dialog();
		}
	}

	// ** NPC ��ó�� �� ��ȣ�ۿ� �����ϴٴ� ǥ��
	private void OnTriggerStay2D(Collider2D collision)
	{
		if (collision.tag != "Player")
			return;
		else
		{
			is_enter = true;
			interaction.SetActive(true);
		}
	}

	// ** ��ȣ�ۿ� ������ �������� ����� �� ǥ�� ����
	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.tag != "Player")
			return;
		else
		{
			interaction.SetActive(false);
			is_enter = false;
		}
	}

	// ** ��ȭ ǥ��
	private void dialog()
	{
		if (SpawnedTracker != null)
			is_interactor(); // ** ��ȣ�ۿ� ������ �Ÿ����� ����� ���� ǥ�� ����
		if (Input.GetKeyDown(KeyCode.E))
		{
			if (textBox.activeSelf)
			{
				PlayerInfo.Getinstance.set_pause(-1);
				textBox.SetActive(false);
			}
			else
			{
				PlayerInfo.Getinstance.set_pause(1);
				list = 0;
				dialogs();
				textBox.SetActive(true);
			}
		}
		// ** ���߿� �߰������� ��ȭ ������ �þ�� ��� �� ������ �ڵ� �ۼ��ٶ�
		if (textBox.activeSelf)
		{
			if (Input.GetMouseButtonDown(0))
			{
				if (is_founded)
				{
					alreadyTaken();
					return;
				}
				if (dronecontroller.getAble())
				{
					NextDialog();
					return;
				}
				dialogs();
			}
		}
	}

	private void dialogs()
	{
		Image image = textBox.transform.Find("CharImage").GetComponent<Image>();
		image.sprite = icon;

		if (is_founded)
			alreadyTaken();
		else if (dronecontroller.getAble())
			NextDialog();
		else
			NoItem();
	}

	// ** ������ �޼����� ��
	private void NextDialog()
	{
		if (list < HiddenFound.Count)
		{
			text.text = HiddenFound[list];
			++list;
		}
		else
		{
			list = 0;
			is_founded = true;
			SaveController.Getinstance.AchiveList[2] = 1;
			SaveController.Getinstance.AchiveUp();
			PlayerInfo.Getinstance.set_pause(-1);
			textBox.SetActive(false);
		}
	}

	// ** ���ǿ� ���� ���� ��
	private void NoItem()
	{
		if (list < SignText.Count)
		{
			text.text = SignText[list];
			++list;
		}
		else
		{
			list = 0;
			PlayerInfo.Getinstance.set_pause(-1);
			textBox.SetActive(false);
		}
	}

	// ** �̹� �������� ��
	private void alreadyTaken()
	{
		if (list < AfterFound.Count)
		{
			text.text = AfterFound[list];
			++list;
		}
		else
		{
			list = 0;
			PlayerInfo.Getinstance.set_pause(-1);
			textBox.SetActive(false);
		}
	}

	// ** ��ȣ�ۿ��ϸ� Ʈ��Ŀ ����
	public void is_interactor()
	{
		Destroy(SpawnedTracker);
	}

	// ** Ʈ��Ŀ ����
	public void SpawnTacker()
	{
		SpawnedTracker = Instantiate(Tracker);
		SpawnedTracker.transform.position = PlayerInfo.Getinstance.playerPos();
		SpawnedTracker.transform.parent = GameObject.Find("Tracker").transform;
		SpawnedTracker.GetComponent<BoxTracker>().BoxPostion(transform.position);
	}
}

