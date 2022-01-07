using UnityEngine;

public class GreenBird : Bird
{
    protected override void ShowSkill()
    {
        Vector3 speed = Rb.velocity;
        speed.x *= -1;
        Rb.velocity = speed;
    }
}