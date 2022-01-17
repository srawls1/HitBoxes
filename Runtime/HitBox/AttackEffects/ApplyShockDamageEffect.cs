using UnityEngine;

[CreateAssetMenu(menuName = "Attack Effects/Apply Shock Damage")]
public class ApplyShockDamageEffect : AttackEffectScriptableObject
{
	[SerializeField] private float arcDistance;
	[SerializeField] private int shockDamage;

	public ApplyShockDamageEffect(float arcDistance, int shockDamage)
	{
		this.arcDistance = arcDistance;
		this.shockDamage = shockDamage;
	}

	public override void Apply(Damage damage)
	{
		Collider2D[] colliders = Physics2D.OverlapCircleAll(damage.hurtbox.transform.position, arcDistance);
		for (int i = 0; i < colliders.Length; ++i)
		{
			HurtBox arcTarget = colliders[i].GetComponent<HurtBox>();
			if (arcTarget != null && arcTarget != damage.hurtbox)
			{
				Damage.Builder damageBuilder = new Damage.Builder(DamageType.Lightning, null, null);
				damageBuilder.damage = shockDamage;
				arcTarget.PreprocessHit(damageBuilder);
				arcTarget.TakeDamage(damageBuilder.Build());
			}
		}
	}
}
