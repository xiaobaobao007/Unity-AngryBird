using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public static float StayMagnitude = 0.06F;

    public GameObject lose;
    public GameObject win;
    public GameObject[] stars;

    private List<Bird> _birds;
    private List<Pig> _pigs;
    private int _birdIndex;
    private Vector3 _originPos;

    private int _calculateResult;

    private void Awake()
    {
        _birds = new List<Bird>();
        foreach (var bird in GameObject.FindGameObjectsWithTag("Bird"))
        {
            _birds.Add(bird.GetComponent<Bird>());
            _birds.Sort((x, y) => x.CompareTo(y));
        }

        _pigs = new List<Pig>();
        foreach (var pig in GameObject.FindGameObjectsWithTag("Pig"))
        {
            _pigs.Add(pig.GetComponent<Pig>());
        }

        Instance = this;

        _originPos = _birds[0].transform.position;

        for (var i = 1; i < _birds.Count; i++)
        {
            _birds[i].setBirdIsWaitToFly_0();
        }
    }

    private void Update()
    {
        for (var i = _birdIndex; i < _birds.Count; i++)
        {
            if (!_birds[i].IsStay())
            {
                return;
            }
        }

        if (_pigs.Any(pig => !pig.IsStay()))
        {
            return;
        }

        if (_birdIndex < _birds.Count && _pigs.Count > 0)
        {
            return;
        }

        if (_pigs.Count == 0)
        {
            Invoke(nameof(Win), 1F);
            return;
        }

        Invoke(nameof(Lose), 1F);
    }

    private void Win()
    {
        win.SetActive(true);
    }

    private void Lose()
    {
        lose.SetActive(true);
    }

    public void NextBird()
    {
        if (++_birdIndex >= _birds.Count)
        {
            return;
        }

        var bird = _birds[_birdIndex];
        bird.transform.position = _originPos;
        bird.setBirdIsWaitToFly_1();
    }

    public void PigDead(Pig pig)
    {
        _pigs.Remove(pig);
    }

    public void ShowStars()
    {
        StartCoroutine(nameof(Show));
    }

    private IEnumerator Show()
    {
        var starNum = 4 - _birdIndex;
        starNum = Math.Max(starNum, 3);
        starNum = Math.Min(starNum, 0);

        for (var i = 0; i < starNum; i++)
        {
            yield return new WaitForSeconds(0.2F);
            stars[i].SetActive(true);
        }
    }

    public void Replay()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(2);
    }

    public void Home()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(1);
    }
}