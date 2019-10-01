using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroButton : MonoBehaviour {

    // Hero 정보를 보여줄 Text
    public Text HeroDisplayer;

    public CanvasGroup canvasGroup;

    // 고용한 Hero Name
    public string heroName;

    // 고용한 Hero Level
    public int level;

    [HideInInspector]
    public int currentCost;          // 현재 가격
    public int startCurrentCost = 1; // 시작 가격

    [HideInInspector]
    public int goldPerSec;          // 현재 초당 골드 증가
    public int startGoldPerSec = 1; // 시작 초당 골드 증가

    // 가격 증가 수치 폭
    public float costPow = 3.14f;
    // 초당 골드 증가 수치 폭
    public float upgradePow = 1.07f;

    [HideInInspector]
    // 아이템 구매 여부
    public bool isPurchased = false;

    private void Start()
    {
        DataManager.Instance.LoadHeroButton(this);  // Hero 초기화
        
        UpdateUI();
    }

    private void Update()
    {
        UpdateUI();
    }

    public void PurchaseHero()
    {
        // 현재 소지금이 현재 가격보다 많으면 구매 !!!
        if (DataManager.Instance.gold >= currentCost)
        {
            // 구매상태로 변경
            isPurchased = true;
            // 소지금 감소, level++
            DataManager.Instance.gold -= currentCost;
            level++;

            UpdateHero();
            UpdateUI();

            DataManager.Instance.SaveHeroButton(this);
        }
    }

    public void UpdateHero()
    {
        // 증가 수치에 따라 --> 초당 획등 골드, 가격 증가
        goldPerSec = startGoldPerSec * (int)Mathf.Pow(upgradePow, level);
        currentCost = startCurrentCost * (int)Mathf.Pow(costPow, level);
    }

    public void UpdateUI()
    {
        HeroDisplayer.text = heroName
            + "\nLevel: " + level
            + "\nCost: " + currentCost
            + "\nGold Per Sec: " + goldPerSec;

        if (isPurchased)
        {
            canvasGroup.alpha = 1.0f;
        }
        else
        {
            canvasGroup.alpha = 0.6f;
        }
    }

   
}
