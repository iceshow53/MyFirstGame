using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hidden : MonoBehaviour
{
	// ** 대화창
	public GameObject textBox, interaction;
    public Sprite icon;

	private Text text;
	public Sprite HALL;

	// ** 트래커
    private GameObject SpawnedTracker, Tracker;
	private ShieldController shieldController;


	// ** 대화 리스트
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

		HiddenFound.Add("바닥을 파다보니 재미가 들렸다.");
		HiddenFound.Add("삽 데미지 + 10% 증가!");

		SignText.Add("무언가 바닥에 묻혀있음");
		SignText.Add("아마 삽이 필요할 것 같다.");

		AfterFound.Add("이미 파해쳐진 후다");
		AfterFound.Add("다음에 오면 뭔가 있을지도?");

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

	// ** NPC 근처일 때 상호작용 가능하다는 표시
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

	// ** 상호작용 가능한 범위에서 벗어났을 때 표시 제거
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

	// ** 대화 표시
	private void dialog()
	{
		if(SpawnedTracker != null)
			is_interactor(); // ** 상호작용 가능한 거리까지 가까워 지면 표시 삭제
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
		// ** 나중에 추가적으로 대화 내용이 늘어나면 사용 더 괜찮은 코드 작성바람
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

	// ** 조건을 달성했을 때
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

	// ** 조건에 맞지 않을 때
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

	// ** 이미 파해쳤을 때
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

	// ** 상자를 열었을 때 UI를 움직이고 상자를 제거
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
