public class YellowBird : Bird
{
    protected override void ShowSkill()
    {
        Rb.velocity *= 2;
    }
}