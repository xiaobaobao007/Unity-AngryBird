using System;
using UnityEngine;
using UnityEngine.UI;

public class MapSelect : MonoBehaviour
{
    public GameObject panel;
    public int maxAllSmallLevelStarNum;
    private int _inStarLimit;
    private GameObject _stars;
    private GameObject _locks;
    private GameObject _star;
    private GameObject _starCount;

    void Start()
    {
        var myTransforms = GetComponentsInChildren<Transform>();
        foreach (var child in myTransforms)
        {
            switch (child.name)
            {
                case "stars":
                    _stars = child.gameObject;
                    break;
                case "lock":
                    _locks = child.gameObject;
                    break;
                case "star":
                    _star = child.gameObject;
                    break;
                case "star_count":
                    _starCount = child.gameObject;
                    break;
            }
        }

        _inStarLimit = Convert.ToInt32(_starCount.GetComponent<Text>().text);

        var totalStarNum = MemoryManager.GetTotalStarNum();

        if (totalStarNum >= _inStarLimit)
        {
            _locks.SetActive(false);
            _star.SetActive(false);
            _starCount.SetActive(false);

            _stars.GetComponentInChildren<Text>().text =
                MemoryManager.GetBigAllStarNum(gameObject.name) + "/" + maxAllSmallLevelStarNum;
        }
        else
        {
            _stars.SetActive(false);
        }


        GetComponent<Button>().onClick.AddListener(Select);
    }

    private void Select()
    {
        MemoryManager.SetNowBigLevel(gameObject.name);
        transform.parent.gameObject.SetActive(false);
        panel.SetActive(true);
    }

    public void ReturnToSelect()
    {
        transform.parent.gameObject.SetActive(true);
        panel.SetActive(false);
    }
}