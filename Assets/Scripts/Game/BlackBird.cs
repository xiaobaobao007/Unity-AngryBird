using UnityEngine;

public class BlackBird : Bird
{
    protected override void ShowSkill()
    {
        foreach (var block in GameObject.FindGameObjectsWithTag("CanBeBoom"))
        {
            if ((transform.position - block.transform.position).sqrMagnitude <= 16F)
            {
                block.GetComponent<Pig>().Dead();
            }
        }

        Clear();
    }

    private void Clear()
    {
        Rb.velocity = Vector3.zero;
        Instantiate(Boom, transform.position, Quaternion.identity);
        Sr.enabled = false;
        GetComponent<CircleCollider2D>().enabled = false;
        MyTrail.TrailEnd();
    }
}