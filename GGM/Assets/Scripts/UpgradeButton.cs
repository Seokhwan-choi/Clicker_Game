using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeButton : MonoBehaviour {

    public Text upgradeDisPlayer;
    public string upgradeName;

    [HideInInspector]
    // 현재 GoldPerClick
    public int goldByUpgrade;
    public int startGoldByUpgrade;

    [HideInInspector]
    // 현재 Cost
    public int currentCost = 1;
    public int startCurrentCost = 1;

    [HideInInspector]
    // 현재 Upgrade 몇번했는지 즉, Level
    public int level = 1;

    // 강화 증가수치, 가격 증가수치
    public float upgradePow = 1.07f;
    public float costPow = 3.14f;

    void Start()
    {
        DataManager.Instance.LoadUpgradeButton(this);
        UpdateUI();
    }

    public void PurchaseUpgrade()
    {
        // 현재 소지하고 있는 gold가 충분하다면 구매한다.
        if ( DataManager.Instance.gold >= currentCost)
        {
            // 현재 소지금 - 가격
            DataManager.Instance.gold -= currentCost;
            // Level++
            level++;
            // 클릭 한번 당 골드 획득 수치 ++ GoldPerClick
            DataManager.Instance.goldPerClick += goldByUpgrade;

            UpdateUpgrade();    // 강화수치 갱신
            UpdateUI();         // UI Display 갱신

            DataManager.Instance.SaveUpgradeButton(this);
        }
    }

    public void UpdateUpgrade()
    {
        // 현재 Upgrade의 Level을 통해서 
        goldByUpgrade = startGoldByUpgrade * (int)Mathf.Pow(upgradePow, level);
        currentCost = startCurrentCost * (int)Mathf.Pow(costPow, level);
    }

    public void UpdateUI()
    {
        upgradeDisPlayer.text = upgradeName + "\nCost: " + currentCost +
            "\nLevel: " + level + "\nNext New GoldPerClick: " + goldByUpgrade;
    }
}
