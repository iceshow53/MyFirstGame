using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameManager : MonoBehaviour
{
	public gameManager() { }
	public static gameManager instance = null;

	public static gameManager Getinstance
	{
		get
		{
			if (instance == null)
				return null;
			return instance;
		}
	}

	public int PlayerID;

	private void Awake()
	{
		if (instance == null)
		{
			instance = this;

			DontDestroyOnLoad(gameObject);
		}
	}
}
