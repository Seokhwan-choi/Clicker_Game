using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClickButton : MonoBehaviour
{
    public Slider slider;
    public Text levelText;
    public Text expText;

    private void Awake()
    {
        slider.minValue = 0;
        slider.maxValue = PlayerManager.Instance.expMax;
        levelText.text = "Lv. " + PlayerManager.Instance.playerLevel;
        expText.text = PlayerManager.Instance.exp + " / " + PlayerManager.Instance.expMax;
    }

    public void OnMouseDown()
    {
        // 클릭하면 gold 상승
        DataManager.Instance.gold += DataManager.Instance.goldPerClick;
        PlayerManager.Instance.exp += PlayerManager.Instance.expPerClick;
  
        slider.value = PlayerManager.Instance.exp;
        expText.text = PlayerManager.Instance.exp + " / " + PlayerManager.Instance.expMax;


        // 클릭하면 exp 상승
        if (PlayerManager.Instance.exp >= PlayerManager.Instance.expMax)
        {
            PlayerManager.Instance.LevelUp();
            slider.maxValue = PlayerManager.Instance.expMax;

            levelText.text = "Lv. " + PlayerManager.Instance.playerLevel;
        }
    }
}
