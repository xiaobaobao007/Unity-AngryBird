using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Windows.Speech;

public class LevelSelect : MonoBehaviour
{
    public Sprite unlockBg;
    public Sprite lockBg;

    private bool _canSelect;

    void Start()
    {
        var bigOpenNum = MemoryManager.GetBigOpenNum();

        int nowLevel = Convert.ToInt32(gameObject.name);

        _canSelect = nowLevel <= bigOpenNum;

        if (_canSelect)
        {
            GetComponent<Image>().overrideSprite = unlockBg;

            transform.Find("num").gameObject.SetActive(true);

            var count = MemoryManager.GetSmallStarNum(gameObject.name);
            if (count > 0)
            {
                transform.Find("num/star-" + count).gameObject.SetActive(true);
            }
        }
        else
        {
            GetComponent<Image>().overrideSprite = lockBg;
        }
    }

    public void Selected()
    {
        if (_canSelect)
        {
            MemoryManager.SetNowSmallLevel(gameObject.name);
            SceneManager.LoadScene(2);
        }
    }
}