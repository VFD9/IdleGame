public interface IObject
{
    public void Attack();
    public float currentAtk();
    public float currentAtk(float addAtk);
    public void AttackDamage(float dmg);
    public float currentHp();
    public float currentHp(float _hp);
    public float HpUp(float _hp);
}
