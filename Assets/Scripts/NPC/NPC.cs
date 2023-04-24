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

		getQuest.Add("안녕하세요!");
		getQuest.Add("오늘 날씨가 참 좋네요.");
		getQuest.Add("좀비 20마리만 좀 잡아주시겠어요?");

		ClearQuest.Add("좀비를 모두 처리해 주셨군요!");
		ClearQuest.Add("감사합니다! (사격속도 10% UP)");

		HiddenQuest.Add("여기 어딘가에 숨겨진 정원이 있다던데 혹시 보셨나요?");
		HiddenQuest.Add("특별한 꽃이 있다길래 찾고있는데...");
		HiddenQuest.Add("혹시 찾으면 꼭 알려주세요!");

		HiddenClear.Add("정말 오랫동안 찾고 있었던건데 감사해요!");
		HiddenClear.Add("보답으로 보상을 드릴게요!");

		ShowKey.Add("묘비같이 생긴 몬스터가 열쇠같은걸 들고 있었어요!");
		ShowKey.Add("혹시 그게 정원 열쇠가 아닐까요?");

		GardenDirection.Add("열쇠를 가지고 계시군요!");
		GardenDirection.Add("정원은 북서쪽에 있다는 소식을 들었는데...");
		GardenDirection.Add("한 번 봐주실레요?");
	}

	void Update()
	{
		if(is_enter)
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
		// ** 나중에 추가적으로 대화 내용이 늘어나면 사용
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
			text.text = "안녕하세요! 지난번엔 감사했어요.";
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
				text.text = "아직인가요?";
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
					text.text = "아직 못 찾으셨나요?";
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
