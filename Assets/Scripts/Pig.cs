using System;
using UnityEngine;

public class Pig : MonoBehaviour
{
    public GameObject boom;

    public Sprite hurt;
    public float maxSpeed = 10;
    public float minSpeed = 5;
    public GameObject score;
    public bool isPig;
    public AudioClip birdCollision;
    public AudioClip dead;
    public AudioClip hurtClip;

    private SpriteRenderer _renderer;
    private Rigidbody2D _rb;

    // Start is called before the first frame update
    private void Start()
    {
        _renderer = GetComponent<SpriteRenderer>();
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void Update()
    {
    }


    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag.Equals("Bird"))
        {
            AudioPlay(birdCollision);
            other.transform.GetComponent<Bird>().Hurt();
        }

        if (other.relativeVelocity.magnitude > maxSpeed)
        {
            Dead();
        }
        else if (other.relativeVelocity.magnitude < minSpeed)
        {
        }
        else
        {
            if (isPig)
            {
                AudioPlay(hurtClip);
                _renderer.sprite = hurt;
            }
        }
    }

    private void Dead()
    {
        AudioPlay(dead);

        var position = transform.position;

        Instantiate(boom, position, Quaternion.identity);
        Destroy(gameObject);

        Destroy(Instantiate(score, position + new Vector3(0, 0.5F, 0), Quaternion.identity), 1.5F);

        GameManager.Instance.PigDead(this);
    }


    public bool IsStay()
    {
        return Math.Abs(_rb.velocity.magnitude) <= GameManager.StayMagnitude;
    }

    private void AudioPlay(AudioClip clip)
    {
        AudioSource.PlayClipAtPoint(clip, transform.position);
    }
}