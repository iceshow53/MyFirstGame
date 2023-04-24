using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Medicine : MonoBehaviour
{
    private int heal_mount;

    private void Start()
    {
        heal_mount = 10;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            if (PlayerInfo.Getinstance.getHP() == PlayerInfo.Getinstance.getMHP())
                return;
            else
            {
                if (PlayerInfo.Getinstance.getHP() + heal_mount >= PlayerInfo.Getinstance.getMHP())
                    PlayerInfo.Getinstance.setHP(PlayerInfo.Getinstance.getMHP());
                else
                    PlayerInfo.Getinstance.addHP(heal_mount);

                Destroy(gameObject);
            }
        }
    }
}
