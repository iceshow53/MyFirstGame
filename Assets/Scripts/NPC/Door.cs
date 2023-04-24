using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
	public GameObject Door1, Door2;

	public void interaction()
	{
		if(SaveController.Getinstance.AchiveList[5] == 1)
		{
			Destroy(Door1);
			Destroy(Door2);
			Destroy(gameObject);
		}
	}
}
