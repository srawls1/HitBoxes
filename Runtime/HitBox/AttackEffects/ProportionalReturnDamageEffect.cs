using UnityEngine;

[CreateAssetMenu(menuName = "Attack Effects/Proportional Return Damage")]
public class ProportionalReturnDamageEffect : AttackEffectScriptableObject
{
	[SerializeField] private float returnPortion;

	public override void Apply(Damage damage)
	{
		HurtBox attackerHurtbox = damage.hitbox.GetComponentInParent<HurtBox>();
		attackerHurtbox?.TakeDamage(
			new Damage.Builder(DamageType.Magic, null, attackerHurtbox)
				.WithDamage(returnPortion * damage.damage)
				.Build());
	}
}
