using UnityEngine;

[CreateAssetMenu(menuName = "Attack Effects/Constant Return Damage")]
public class ConstantReturnDamageEffect : AttackEffectScriptableObject
{
	[SerializeField] private float returnAmount;

	public override void Apply(Damage damage)
	{
		HurtBox attackerHurtbox = damage.hitbox.GetComponentInParent<HurtBox>();
		attackerHurtbox?.TakeDamage(
			new Damage.Builder(DamageType.Magic, null, attackerHurtbox)
				.WithDamage(returnAmount)
				.Build());
	}
}
