using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButton : MonoBehaviour
{

    //[HideInInspector]
    public GameObject[] MenuList;

    public void Start()
    {
        MenuList = GameObject.FindGameObjectsWithTag("Menu");

        for (int i = 0; i < MenuList.Length; ++i)
        {
            if (MenuList[i].name == "Upgrade") continue;
            MenuList[i].SetActive(false);
        }
    }

    public void ChangeMenu(GameObject gameobject)
    {
        for(int i = 0; i < MenuList.Length; ++i)
        {
            MenuList[i].SetActive(false);
            if (gameobject.name.CompareTo(MenuList[i].name) == 0)
            {
                MenuList[i].SetActive(true);
            }
        }
    }
}
