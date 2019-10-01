using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DataManager : MonoBehaviour
{
    #region DataManager 싱글톤 프로퍼티 ( 속성 )
    // 획득 Gold 관리하는 DataManager 싱글톤
    private static DataManager instance;
    public static DataManager Instance
    {
        get
        {
            // instance가 null일 때
            if ( instance == null)
            {
                // DataManager를 찾는다.
                instance = FindObjectOfType<DataManager>();
                // DataManager를 찾아도 없으면 새로 생성한다.
                if ( instance == null)
                {
                    GameObject container = new GameObject("DataManager");

                    instance = container.AddComponent<DataManager>();
                }
            }
            return instance;
        }
    }
    #endregion

    #region Gold 관리 프로퍼티 ( 속성 )
    // Gold 관리
    public long gold
    {
        get
        {
            if ( !PlayerPrefs.HasKey("Gold"))
            {
                return 0;
            }

            string tmpGold = PlayerPrefs.GetString("Gold");
            return long.Parse(tmpGold);
        }

        set
        {
            PlayerPrefs.SetString("Gold", value.ToString());
        }
    }
    #endregion

    #region 클릭 당 Gold 관리 프로퍼티 ( 속성 )
    // 클릭 당 Gold 획득 량
    public int goldPerClick
    {
        get
        {
            // GoldPerClick
            return PlayerPrefs.GetInt("GoldPerClick", 1);
        }

        set
        {
            PlayerPrefs.SetInt("GoldPerClick", value);
        }
    }
    #endregion

    #region 게임 종료 시점 부터, 얼마나 시간이 지났나 확인 후 보상
    // 마지막 종료시간을 반환해주는 함수
    DateTime GetLastPlayDate()
    {
        // 저장된 종료시간이 없다면
        // 현재 시간을 return
        if (!PlayerPrefs.HasKey("Time"))
        {
            return DateTime.Now;
        }

        // 저장된 종료시간이 있다면
        // 문자열로 저장된 DateTime을 꺼내와 long형으로 변경 후
        string timeBinaryInString = PlayerPrefs.GetString("Time");
        long timeBinaryInLong = Convert.ToInt64(timeBinaryInString);

        return DateTime.FromBinary(timeBinaryInLong);
    }

    // 마지막 종료 시점으로 부터
    // 얼마나 시간이 지났는지 계산해주는 프로퍼티 ( 속성 )
    public int timeAfterLastPlay
    {
        get
        {
            DateTime currentTime = DateTime.Now;
            DateTime lastPlayDate = GetLastPlayDate();

            return (int)currentTime.Subtract(lastPlayDate).TotalSeconds;
        }
    }

    // 종료 시점의 시간을 PlayerPrefs에 저장
    void UpdateLastPlayDate()
    {
        PlayerPrefs.SetString("Time", DateTime.Now.ToBinary().ToString());
    }

    // 어플이 종료될 때 반드시 호출
    // 비정상 종료가되면 호출되지 않음
    private void OnApplicationQuit()
    {
        UpdateLastPlayDate();
    }
    #endregion

    #region Upgrade Button Load,Save 함수

    // UpgradeButton들을 Load하는 함수
    public void LoadUpgradeButton(UpgradeButton upgradeButton)
    {
        string key = upgradeButton.upgradeName;

        // Level, GoldPerClick, Cost를 불러온다.
        upgradeButton.level = PlayerPrefs.GetInt(key + "_level", 1);
        upgradeButton.goldByUpgrade = PlayerPrefs.GetInt(key + "_goldByUpgrade",
            upgradeButton.startGoldByUpgrade);
        upgradeButton.currentCost = PlayerPrefs.GetInt(key + "_cost",
            upgradeButton.startCurrentCost);
    }

    // UpgradeButton들을 Save하는 함수
    public void SaveUpgradeButton(UpgradeButton upgradeButton)
    {
        string key = upgradeButton.upgradeName;

        // Level, GoldPerClick, Cost를 저장한다.
        PlayerPrefs.SetInt(key + "_level", upgradeButton.level);
        PlayerPrefs.SetInt(key + "_goldByUpgrade", upgradeButton.goldByUpgrade);
        PlayerPrefs.SetInt(key + "_cost", upgradeButton.currentCost);
    }

    #endregion

    #region Hero Button Load,Save 함수
    // HeroButton들을 Load하는 함수
    public void LoadHeroButton(HeroButton heroButton)
    {
        string key = heroButton.heroName;
        heroButton.level = PlayerPrefs.GetInt(key + "_level", 1);
        heroButton.currentCost = PlayerPrefs.GetInt(key + "_cost",
            heroButton.startCurrentCost);
        heroButton.goldPerSec = PlayerPrefs.GetInt(key + "_goldPerSec");

        if (PlayerPrefs.GetInt(key +"_isPurchased") == 1)
        {
            heroButton.isPurchased = true;
        }
        else
        {
            heroButton.isPurchased = false;
        }
    }

    // HeroButton들을 Save하는 함수
    public void SaveHeroButton(HeroButton heroButton)
    {
        string key = heroButton.heroName;
        PlayerPrefs.SetInt(key + "_level", heroButton.level);
        PlayerPrefs.SetInt(key + "_cost", heroButton.currentCost);
        PlayerPrefs.SetInt(key + "_goldPerSec", heroButton.goldPerSec);

        if ( heroButton.isPurchased == true)
        {
            PlayerPrefs.SetInt(key + "_isPurchased", 1);
        }
        else
        {
            PlayerPrefs.SetInt(key + "_isPurchased", 0);
        }
    }

    #endregion

    // 고용된 영웅들을 담는 배열
    public HeroButton[] heroButtons;

    private void Awake()
    {
        // HeroButton
        heroButtons = FindObjectsOfType<HeroButton>();
        // 코루틴 실행
        StartCoroutine("AddGoldLoop");             

    }

    private void Start()
    {
        gold += GetGoldPerSec() * timeAfterLastPlay;
        // InvokeRepeating ->
        // UpdateLastPlayDate 함수를 0초 후에 5초마다 실행시켜라
        InvokeRepeating("UpdateLastPlayDate", 0f, 5f);
    }

    // 초당 Gold 획득 함수
    public int GetGoldPerSec()
    {
        int goldPerSec = 0;
        for(int i = 0; i < heroButtons.Length; ++i)
        {
            // 구매가 완료된 Hero에 한해서만 goldPerSec 추가
           if ( heroButtons[i].isPurchased == true)
            {
                goldPerSec += heroButtons[i].goldPerSec;
            }
        }
        return goldPerSec;
    }

    // 초당 골드 획득 코루틴 함수
    IEnumerator AddGoldLoop()
    {
        while (true)
        {
            for(int i = 0; i < heroButtons.Length; ++i)
            {
                if (heroButtons[i].isPurchased)
                {
                    DataManager.Instance.gold += heroButtons[i].goldPerSec;
                }
            }
            // 1초 대기 후 다시 시작한다.
            // 즉, 1초마다 반복된다.
            yield return new WaitForSeconds(1.0f);
        }
    }
}
