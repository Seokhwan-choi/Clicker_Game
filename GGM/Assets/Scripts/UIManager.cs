using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public Text goldDisplayer;
    public Text goldPerClickDisplayer;
    public Text goldPerSecDisplayer;

    void Update()
    {
        goldDisplayer.text = "GOLD: " + DataManager.Instance.gold;
        goldPerClickDisplayer.text = "GOLD PER CLICK: " + DataManager.Instance.goldPerClick;
        goldPerSecDisplayer.text = "GOLD PER SEC: " + DataManager.Instance.GetGoldPerSec();
    }
}
