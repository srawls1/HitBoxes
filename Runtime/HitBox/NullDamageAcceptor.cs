

public class NullDamageAcceptor : DamageAcceptor
{
	public override int AcceptDamage(int damage, DamageType type)
	{
		return 0;
	}

	public override int AcceptHealing(int amount)
	{
		return 0;
	}
}
