using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour, IComparable<Bird>
{
    public float maxDistance = 1.3F;
    public float delayTime = 0.1F;
    public float cameraSmooth = 3;
    public AudioClip select;
    public AudioClip fly;
    public Sprite hurt;
    public GameObject Boom;
    public GameObject Road;

    protected Rigidbody2D Rb;
    protected SpriteRenderer Sr;
    protected TestMyTrail MyTrail;

    private readonly Vector2 _springJointPoint = new Vector2(0.09F, 0.7F);
    private Transform _leftPoint;
    private LineRenderer _left;
    private Transform _rightPoint;
    private Rigidbody2D _rightRg;
    private LineRenderer _right;
    private SpringJoint2D _sp;
    private int _status;
    private bool _canMove = false;
    private bool _isClick;
    private bool _isFly;
    private bool _paintRoad;
    private bool _isRelease;

    private void Awake()
    {
        var l = GameObject.Find("leftPosition");
        _leftPoint = l.transform;
        _left = l.GetComponent<LineRenderer>();

        var r = GameObject.Find("rightPosition");
        _right = r.GetComponent<LineRenderer>();
        _rightRg = GameObject.Find("right").GetComponent<Rigidbody2D>();
        _rightPoint = r.transform;

        _sp = GetComponent<SpringJoint2D>();
        _sp.connectedAnchor = _springJointPoint;

        Rb = GetComponent<Rigidbody2D>();
        MyTrail = GetComponent<TestMyTrail>();
        Sr = GetComponent<SpriteRenderer>();
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
            _right.enabled = false;
            _left.enabled = false;

            if (!IsStay()) return;

            _isFly = false;

            _status = 2;

            Dead();

            return;
        }

        _status = 1;

        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position -= new Vector3(0, 0, Camera.main.transform.position.z);

        if (Vector3.Distance(transform.position, _rightPoint.position) > maxDistance)
        {
            var pos = (transform.position - _rightPoint.position).normalized * maxDistance;
            transform.position = _rightPoint.position + pos;
        }
    }

    private void OnMouseDown()
    {
        if (!_canMove) return;
        AudioPlay(select);
        //点击开始
        _isClick = true;

        Rb.isKinematic = true;

        StartCoroutine(nameof(PaintRoad_1));
    }

    private IEnumerator PaintRoad_1()
    {
        _paintRoad = true;
        while (_paintRoad)
        {
            var clone = Instantiate(Road, transform.position, Quaternion.identity);

            var roadSj = clone.GetComponent<SpringJoint2D>();
            roadSj.connectedBody = _rightRg;
            roadSj.connectedAnchor = _springJointPoint;

            yield return new WaitForSeconds(delayTime);
            roadSj.enabled = false;
            Destroy(clone, 1.0F);
        }
    }

    // private IEnumerator PaintRoad_2()
    // {
    //     var position = transform.position;
    //     var now = new Vector3(position.x, position.y, position.z);
    //
    //     while (true)
    //     {
    //         var clone = Instantiate(Road, now, Quaternion.identity);
    //
    //         var roadSj = clone.GetComponent<SpringJoint2D>();
    //         roadSj.connectedBody = _rightRg;
    //         roadSj.connectedAnchor = _springJointPoint;
    //
    //         yield return new WaitForSeconds(delayTime);
    //         roadSj.enabled = false;
    //     }
    // }

    private void OnMouseUp()
    {
        if (!_canMove) return;
        // StartCoroutine(nameof(PaintRoad_2));

        _paintRoad = false;

        _canMove = false;
        //点击结束
        _isClick = false;
        Rb.isKinematic = false;
        //延迟执行
        Invoke(nameof(Fly), delayTime);
    }

    private void Fly()
    {
        _isRelease = true;
        _isFly = true;
        AudioPlay(fly);
        //脱离靶子
        _sp.enabled = false;
        MyTrail.TrailStart();
    }

    /**
     * 划线
     */
    private void Line()
    {
        _right.enabled = true;
        _left.enabled = true;
        var position = transform.position;

        _right.SetPosition(0, _rightPoint.position);
        _right.SetPosition(1, position);

        _left.SetPosition(0, _leftPoint.position);
        _left.SetPosition(1, position);
    }

    public void setBirdIsWaitToFly_0()
    {
        enabled = false;
        _sp.enabled = false;
        Rb.bodyType = RigidbodyType2D.Static;
    }

    public void SetCanMove()
    {
        _canMove = true;
    }

    public void setBirdIsWaitToFly_1()
    {
        enabled = true;
        _sp.enabled = true;
        Rb.bodyType = RigidbodyType2D.Dynamic;
    }

    private void Dead()
    {
        Instantiate(Boom, transform.position, Quaternion.identity);
        Destroy(gameObject);
        GameManager.Instance.NextBird();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        _isFly = false;
        MyTrail.TrailEnd();
    }

    public bool IsStay()
    {
        return Math.Abs(Rb.velocity.magnitude) <= GameManager.StayMagnitude;
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

    public void Hurt()
    {
        Sr.sprite = hurt;
    }

    public int CompareTo(Bird other)
    {
        if (GetXPoint() > other.GetXPoint())
        {
            return -1;
        }

        return 1;
    }

    private float GetXPoint()
    {
        return gameObject.transform.position.x;
    }
}