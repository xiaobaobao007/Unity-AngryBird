using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PausePanel : MonoBehaviour
{
    public GameObject button;

    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void Retry()
    {
        GameManager.Instance.Replay();
    }

    public void Pause()
    {
        _animator.SetBool("isPause", true);
        button.SetActive(false);
    }

    public void Home()
    {
        GameManager.Instance.Home();
    }

    public void Resume()
    {
        Time.timeScale = 1;
        _animator.SetBool("isPause", false);
    }

    public void PauseAniEnd()
    {
        Time.timeScale = 0;
    }

    public void ResumeAniEnd()
    {
        button.SetActive(true);
    }
}