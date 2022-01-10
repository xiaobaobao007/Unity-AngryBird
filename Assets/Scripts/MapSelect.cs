using System;
using UnityEngine;
using UnityEngine.UI;

public class MapSelect : MonoBehaviour
{
    private int _starNum;
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

        _starNum = Convert.ToInt32(_starCount.GetComponent<Text>().text);

        if (PlayerPrefs.GetInt("totalNum", 0) >= _starNum)
        {
            _locks.SetActive(false);
            _stars.SetActive(true);
        }
    }

    void Update()
    {
    }
}