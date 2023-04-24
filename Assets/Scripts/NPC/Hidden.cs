using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hidden : MonoBehaviour
{
	// ** ��ȭâ
	public GameObject textBox, interaction;
    public Sprite icon;

	private Text text;
	public Sprite HALL;

	// ** Ʈ��Ŀ
    private GameObject SpawnedTracker, Tracker;
	private ShieldController shieldController;


	// ** ��ȭ ����Ʈ
    private List<string> HiddenFound = new List<string>();
	private List<string> SignText = new List<string>();
	private List<string> AfterFound = new List<string>();
	private int list;

	private bool is_enter ,is_founded;

	private void Awake()
    {
		if (SaveController.Getinstance.AchiveList[0] != 0)
			Destroy(gameObject);

        Tracker = Resources.Load("Prefabs/BoxTracking") as GameObject;
		textBox = GameObject.Find("UI").transform.Find("Dialog").gameObject;
		interaction = gameObject.transform.Find("Triangle").gameObject;
		shieldController = GameObject.Find("Player").transform.Find("SubWeapon").Find("shield").GetComponent<ShieldController>();

		text = textBox.transform.Find("Dialog").GetComponent<Text>();
	}
    void Start()
    {
		interaction.SetActive(false);

		HiddenFound.Add("�ٴ��� �Ĵٺ��� ��̰� ��ȴ�.");
		HiddenFound.Add("�� ������ + 10% ����!");

		SignText.Add("���� �ٴڿ� ��������");
		SignText.Add("�Ƹ� ���� �ʿ��� �� ����.");

		AfterFound.Add("�̹� �������� �Ĵ�");
		AfterFound.Add("������ ���� ���� ��������?");

		list = 0;
		is_enter = false;

		if (SaveController.Getinstance.AchiveList[0] == 1)
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
		if(SpawnedTracker != null)
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
				if(is_founded)
				{
					alreadyTaken();
					return;
				}
				if (shieldController.getAble())
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
		else if (shieldController.getAble())
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
			transform.GetComponent<SpriteRenderer>().sprite = HALL;
			icon = HALL;
			SaveController.Getinstance.AchiveList[0] = 1;
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

	// ** ���ڸ� ������ �� UI�� �����̰� ���ڸ� ����
	public void is_interactor()
    {
        Destroy(SpawnedTracker);
    }

    public void SpawnTacker()
    {
        SpawnedTracker = Instantiate(Tracker);
        SpawnedTracker.transform.position = PlayerInfo.Getinstance.playerPos();
        SpawnedTracker.transform.parent = GameObject.Find("Tracker").transform;
        SpawnedTracker.GetComponent<BoxTracker>().BoxPostion(transform.position);
    }
}
