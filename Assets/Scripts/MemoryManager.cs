using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MemoryManager : MonoBehaviour
{
    private const string NowBigLevel = "nowBigLevel";
    private const string NowSmallLevel = "nowSmallLevel";
    private const string TotalStarNum = "totalStarNum";
    private const string BigStarNum = "bigStarNum_";
    private const string SmallStarNum = "smallStarNum_";
    private const string BigOpenNum = "bigOpenNum_";

    public static string GetGameResources()
    {
        return "level_" + GetNowBigLevel() + "_" + GetNowSmallLevel();
    }

    public void ClearAllMemory()
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene(1);
        print("所有数据已清空");
    }

    public static void SetBigOpenNum(int openNum)
    {
        PlayerPrefs.SetInt(BigOpenNum + GetNowBigLevel(), openNum);
    }

    public static int GetBigOpenNum()
    {
        return PlayerPrefs.GetInt(BigOpenNum + GetNowBigLevel(), 1);
    }

    public static void SetNowBigLevel(string bigLevel)
    {
        PlayerPrefs.SetString(NowBigLevel, bigLevel);
    }

    public static string GetNowBigLevel()
    {
        return PlayerPrefs.GetString(NowBigLevel);
    }

    public static void SetNowSmallLevel(string smallLevel)
    {
        PlayerPrefs.SetString(NowSmallLevel, smallLevel);
    }

    public static string GetNowSmallLevel()
    {
        return PlayerPrefs.GetString(NowSmallLevel);
    }


    private static void SetTotalStarNum(int totalStarNum)
    {
        PlayerPrefs.SetInt(TotalStarNum, totalStarNum);
    }

    public static int GetTotalStarNum()
    {
        return PlayerPrefs.GetInt(TotalStarNum, 0);
    }

    public static int GetBigAllStarNum(string bigLevel)
    {
        return PlayerPrefs.GetInt(BigStarNum + bigLevel, 0);
    }

    private static void SetBigAllStarNum(string bigLevel, int star)
    {
        PlayerPrefs.SetInt(BigStarNum + bigLevel, star);
    }

    public static int GetSmallStarNum(string smallLevel)
    {
        return PlayerPrefs.GetInt(SmallStarNum + GetNowBigLevel() + "_" + smallLevel, 0);
    }

    private static void SetSmallStarNum(string bigLevel, string smallLevel, int star)
    {
        PlayerPrefs.SetInt(SmallStarNum + bigLevel + "_" + smallLevel, star);
    }

    public static void RefreshSmallStarNum(int star)
    {
        var bigLevel = GetNowBigLevel();
        var smallLevel = GetNowSmallLevel();
        var oldStarNum = GetSmallStarNum(smallLevel);
        if (star <= oldStarNum)
        {
            return;
        }

        //设置小关卡星星数据
        SetSmallStarNum(bigLevel, smallLevel, star);

        var addNum = star - oldStarNum;

        //设置大关卡星星数据
        var oldBigStarNum = GetBigAllStarNum(GetNowBigLevel());
        SetBigAllStarNum(GetNowBigLevel(), oldBigStarNum + addNum);

        //设置总星星数据
        var oldTotalStarNum = GetTotalStarNum();
        SetTotalStarNum(oldTotalStarNum + addNum);

        //设置开放的关卡数
        var openNum = GetBigOpenNum();
        if (openNum <= Convert.ToInt32(GetNowSmallLevel()))
        {
            SetBigOpenNum(openNum + 1);
        }
    }
}