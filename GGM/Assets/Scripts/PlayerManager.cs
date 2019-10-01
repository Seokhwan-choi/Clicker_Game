using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    #region PlayerManager 싱글톤 프로퍼티
    private static PlayerManager instance;
    public static PlayerManager Instance
    {
        get
        {
            if ( instance == null)
            {
                instance = FindObjectOfType<PlayerManager>();

                if ( instance == null)
                {
                    GameObject container = new GameObject("PlayerManager");

                    instance = container.AddComponent<PlayerManager>();
                }
            }
            return instance;
        }
    }
    #endregion

    //[HideInInspector]
    public float expPow = 2f;
    public int startExpMax = 100;

    // 플레이어 현재 경험치 프로퍼티
    public int exp
    {
        get
        {
            return PlayerPrefs.GetInt("Exp");
        }

        set
        {
            PlayerPrefs.SetInt("Exp", value);
        }
    }

    // 플레이어 클릭당 경험치 프로퍼티
    public int expPerClick
    {
        get
        {
            return PlayerPrefs.GetInt("ExpPerClick", 1);
        }

        set
        {
            PlayerPrefs.SetInt("ExpPerClick", value);
        }
    }

    // 플레이어 LevelUp에 필요한 경험치양 프로퍼티
    public int expMax
    {
        get
        {
            return PlayerPrefs.GetInt("ExpMax", startExpMax);
        }

        set
        {
            PlayerPrefs.SetInt("ExpMax", value);
        }
    }

    // 플레이어 Level 프로퍼티
    public int playerLevel
    {
        get
        {
            return PlayerPrefs.GetInt("PlayerLevel", 1);
        }

        set
        {
            PlayerPrefs.SetInt("PlayerLevel", value);
        }
    }


    // 플레이어 LevelUp 함수
    public void LevelUp()
    {
        // 플레이어 level 상승
        playerLevel++;
        // 경험치 최대치 상승
        exp = 0;
        expMax = startExpMax * (int)Mathf.Pow(expPow, playerLevel);
    }
}
