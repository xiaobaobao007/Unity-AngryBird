using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Bird : MonoBehaviour
{
    public float maxDistance = 1.3F;
    public float delayTime = 0.1F;
    public float cameraSmooth = 3;

    public Transform leftPoint;
    public LineRenderer left;
    public Transform rightPoint;
    public LineRenderer right;
    public AudioClip select;
    public AudioClip fly;

    protected Rigidbody2D _rb;

    private SpringJoint2D _sp;
    private int _status;
    private TestMyTrail _testMyTrail;
    private bool _canMove = true;
    private bool _isClick;
    private bool _isFly = false;


    private void Awake()
    {
        _sp = GetComponent<SpringJoint2D>();
        _rb = GetComponent<Rigidbody2D>();
        _testMyTrail = GetComponent<TestMyTrail>();
    }

    private void Update()
    {
        if (_isFly && Input.GetMouseButtonDown(0)) Skill();

        var posX = transform.position.x;
        Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position,
            new Vector3(Mathf.Clamp(posX, 6.35F, 16),
                Camera.main.transform.position.y,
                Camera.main.transform.position.z), cameraSmooth * Time.deltaTime);

        if (_status < 2) Line();

        if (!_isClick)
        {
            if (_status != 1) return;

            //禁用画线组件
            right.enabled = false;
            left.enabled = false;

            if (!IsStay()) return;

            _isFly = false;

            _status = 2;

            Dead();

            return;
        }

        _status = 1;

        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position -= new Vector3(0, 0, Camera.main.transform.position.z);

        if (Vector3.Distance(transform.position, rightPoint.position) > maxDistance)
        {
            var pos = (transform.position - rightPoint.position).normalized * maxDistance;
            transform.position = rightPoint.position + pos;
        }
    }

    private void OnMouseDown()
    {
        AudioPlay(select);
        if (!_canMove) return;
        //点击开始
        _isClick = true;

        _rb.isKinematic = true;
    }

    private void OnMouseUp()
    {
        _canMove = false;
        //点击结束
        _isClick = false;
        _rb.isKinematic = false;
        //延迟执行
        Invoke("Fly", delayTime);
    }

    private void Fly()
    {
        _isFly = true;
        AudioPlay(fly);
        //脱离靶子
        _sp.enabled = false;
        _testMyTrail.TrailStart();
    }

    /**
     * 划线
     */
    private void Line()
    {
        right.enabled = true;
        left.enabled = true;
        var position = transform.position;

        right.SetPosition(0, rightPoint.position);
        right.SetPosition(1, position);

        left.SetPosition(0, leftPoint.position);
        left.SetPosition(1, position);
    }

    public void setBirdIsWaitToFly_0()
    {
        enabled = false;
        _sp.enabled = false;
    }

    public void setBirdIsWaitToFly_1()
    {
        enabled = true;
        _sp.enabled = true;
    }

    private void Dead()
    {
        Destroy(gameObject);
        GameManager.Instance.NextBird();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        _isFly = false;
        _testMyTrail.TrailEnd();
    }

    public bool IsStay()
    {
        return Math.Abs(_rb.velocity.magnitude) <= GameManager.StayMagnitude;
    }

    public float GetXPoint()
    {
        return gameObject.transform.position.x;
    }

    private void AudioPlay(AudioClip clip)
    {
        AudioSource.PlayClipAtPoint(clip, transform.position);
    }

    private void Skill()
    {
        _isFly = false;
        ShowSkill();
    }

    protected virtual void ShowSkill()
    {
    }
}