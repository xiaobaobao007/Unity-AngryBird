public class YellowBird : Bird
{
    protected override void ShowSkill()
    {
        print(_rb.velocity);
        _rb.velocity *= 2;
    }
}