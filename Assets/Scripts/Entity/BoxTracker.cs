using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxTracker : MonoBehaviour
{
	private Vector3 BoxPos;
	private float angle, speed, traceangle;
	public GameObject Hidden;

	void Start()
	{
		speed = 25.0f;
	}

	// ** 상자의 위치를 표시해주는 화살표
	void Update()
	{
		if (Hidden != null)
			BoxPos = Hidden.transform.position;

		if (PlayerInfo.Getinstance.is_paused() == 0)
		{
			angle = Mathf.Atan2(BoxPos.y - PlayerInfo.Getinstance.playerPos().y, BoxPos.x - PlayerInfo.Getinstance.playerPos().x) * Mathf.Rad2Deg;
			transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
			if (Vector3.Distance(PlayerInfo.Getinstance.playerPos(), BoxPos) > 8)
			{
				transform.position = PlayerInfo.Getinstance.playerPos() + (new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0.0f)) * 1.0f;
			}
			else
            {
				if (Vector3.Distance(transform.position, BoxPos) > 1.5f)
				{
					traceangle = Mathf.Atan2(BoxPos.y - transform.position.y, BoxPos.x - transform.position.x) * Mathf.Rad2Deg;
					transform.position += new Vector3(Mathf.Cos(traceangle * Mathf.Deg2Rad), Mathf.Sin(traceangle * Mathf.Deg2Rad), 0.0f) * Time.deltaTime * speed;
				}
				else
					transform.position = BoxPos - (new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0.0f)) * 1.0f;
			}
		}
	}

	// ** 상자 위치 받아오기
	public void BoxPostion(Vector3 boxPos)
	{
		BoxPos = boxPos;
	}

	// ** 상자 열리면 삭제
	public void Delete()
	{
		Destroy(gameObject);
	}

	public void SetColor(Color color)
	{
		transform.GetComponent<SpriteRenderer>().color = color;
	}
}
