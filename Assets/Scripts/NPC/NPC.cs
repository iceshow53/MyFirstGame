using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPC : MonoBehaviour
{
	public GameObject textBox;
	public GameObject interaction;
	private GameObject tracker;
	private GameObject SpawnedTracker;
	private bool is_enter, is_item, is_accept, is_clear, is_Hidden, hidden_accept, is_hiddenClear;
	public Sprite icon;
	private List<string> getQuest = new List<string>();
	private List<string> ClearQuest = new List<string>();
	private List<string> HiddenQuest = new List<string>();
	private List<string> HiddenClear = new List<string>();
	private List<string> ShowKey = new List<string>();
	private List<string> GardenDirection = new List<string>();
	private Text text;
	private int list;

	private void Awake()
	{
		tracker = Resources.Load("Prefabs/BoxTracking") as GameObject;
	}

	void Start()
	{
		SpawnTacker();

		is_enter = is_item = is_accept = hidden_accept = is_hiddenClear = false;

		if (SaveController.Getinstance.AchiveList[4] == 1)
			is_clear = true;
		else
			is_clear = false;

		is_Hidden = SaveController.Getinstance.hardmod;

		text = textBox.transform.Find("Dialog").GetComponent<Text>();

		list = 0;

		getQuest.Add("�ȳ��ϼ���!");
		getQuest.Add("���� ������ �� ���׿�.");
		getQuest.Add("���� 20������ �� ����ֽðھ��?");

		ClearQuest.Add("���� ��� ó���� �ּ̱���!");
		ClearQuest.Add("�����մϴ�! (��ݼӵ� 10% UP)");

		HiddenQuest.Add("���� ��򰡿� ������ ������ �ִٴ��� Ȥ�� ���̳���?");
		HiddenQuest.Add("Ư���� ���� �ִٱ淡 ã���ִµ�...");
		HiddenQuest.Add("Ȥ�� ã���� �� �˷��ּ���!");

		HiddenClear.Add("���� �������� ã�� �־����ǵ� �����ؿ�!");
		HiddenClear.Add("�������� ������ �帱�Կ�!");

		ShowKey.Add("������ ���� ���Ͱ� ���谰���� ��� �־����!");
		ShowKey.Add("Ȥ�� �װ� ���� ���谡 �ƴұ��?");

		GardenDirection.Add("���踦 ������ ��ñ���!");
		GardenDirection.Add("������ �ϼ��ʿ� �ִٴ� �ҽ��� ����µ�...");
		GardenDirection.Add("�� �� ���ֽǷ���?");
	}

	void Update()
	{
		if(is_enter)
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
		// ** ���߿� �߰������� ��ȭ ������ �þ�� ���
		if (textBox.activeSelf)
		{
			if (Input.GetMouseButtonDown(0))
			{
				if (is_Hidden)
				{
					Hidden();
					return;
				}
				if (is_accept)
				{
					Quest();
					return;
				}
				if (is_clear)
				{
					KeyShow();
					return;
				}
				NextDialog();
			}
		}
	}

	private void dialogs()
	{
		Image image = textBox.transform.Find("CharImage").GetComponent<Image>();
		image.sprite = icon;

		if (is_Hidden)
			Hidden();
		else if (is_hiddenClear)
			text.text = "�ȳ��ϼ���! �������� �����߾��.";
		else if (is_accept)
			Quest();
		else if (is_clear)
			KeyShow();
		else
			NextDialog();
	}

	private void NextDialog()
	{
		if (list < getQuest.Count)
		{
			text.text = getQuest[list];
			++list;
		}
		else
		{
			list = 0;
			is_accept = true;
			PlayerInfo.Getinstance.set_pause(-1);
			textBox.SetActive(false);
		}
	}

	private void KeyShow()
	{
		if (SaveController.Getinstance.AchiveList[5] != 1)
		{
			if (list < ShowKey.Count)
			{
				text.text = ShowKey[list];
				++list;
			}
			else
			{
				list = 0;
				PlayerInfo.Getinstance.set_pause(-1);
				textBox.SetActive(false);
			}
		}
		else
		{
			if (list < GardenDirection.Count)
			{
				text.text = GardenDirection[list];
				++list;
			}
			else
			{
				list = 0;
				PlayerInfo.Getinstance.set_pause(-1);
				textBox.SetActive(false);
			}
		}
	}

	private void Quest()
	{
		if (PlayerInfo.Getinstance.getKilled() >= 20)
		{
			if (list < ClearQuest.Count)
			{
				text.text = ClearQuest[list];
				++list;
			}
			else
			{
				list = 0;
				is_clear = true;
				is_accept = false;
				SaveController.Getinstance.AchiveList[4] = 1;
				SaveController.Getinstance.AchiveUp();
				PlayerInfo.Getinstance.set_pause(-1);
				textBox.SetActive(false);
			}
		}
		else
		{
			if (list < 1)
			{
				text.text = "�����ΰ���?";
				++list;
			}
			else
			{
				list = 0;
				PlayerInfo.Getinstance.set_pause(-1);
				textBox.SetActive(false);
			}
		}
	}
	private void Hidden()
	{
		if (!hidden_accept)
		{
			{
				if (list < HiddenQuest.Count)
				{
					text.text = HiddenQuest[list];
					++list;
				}
				else
				{
					list = 0;
					hidden_accept = true;
					PlayerInfo.Getinstance.set_pause(-1);
					textBox.SetActive(false);
				}
			}
		}
		else if (hidden_accept)
		{
			if (is_item)
			{
				if (list < HiddenClear.Count)
				{
					text.text = HiddenClear[list];
					++list;
				}
				else
				{
					list = 0;
					is_Hidden = false;
					is_hiddenClear = true;
					PlayerInfo.Getinstance.set_pause(-1);
					textBox.SetActive(false);
				}
			}
			else
			{
				if (list < 1)
				{
					text.text = "���� �� ã���̳���?";
					++list;
				}
				else
				{
					list = 0;
					PlayerInfo.Getinstance.set_pause(-1);
					textBox.SetActive(false);
				}
			}
		}
	}


	public void SpawnTacker()
	{
		SpawnedTracker = Instantiate(tracker);
		SpawnedTracker.transform.position = PlayerInfo.Getinstance.playerPos();
		SpawnedTracker.transform.parent = GameObject.Find("Tracker").transform;
		SpawnedTracker.GetComponent<BoxTracker>().BoxPostion(transform.position);
		SpawnedTracker.GetComponent<BoxTracker>().SetColor(new Vector4(251, 143, 236, 255));
	}
}
