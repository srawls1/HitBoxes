using System;
using UnityEngine;

public class DamageNearbyEnemiesStatusEffect : StatusEffect
{
	private DamageStruct damage;
	private float distance;
	private ParameterStackType distanceStackType;

	public DamageNearbyEnemiesStatusEffect(string name, float totalDuration, ParameterStackType durationStackType,
		DamageStruct damage, float distance, ParameterStackType distanceStackType)
		: base(name, totalDuration, durationStackType)
	{
		this.damage = damage;
		this.distance = distance;
		this.distanceStackType = distanceStackType;
	}

	public override void OnInflict(StatusEffectsContainer container)
	{
		container.AddReceiveDamageInterceptor(name, (damageBuilder) =>
		{
			Collider2D[] colliders = Physics2D.OverlapCircleAll(damageBuilder.hurtbox.transform.position, distance);
			for (int i = 0; i < colliders.Length; ++i)
			{
				HurtBox target = colliders[i].GetComponent<HurtBox>();
				if (target != null && target != damageBuilder.hurtbox)
				{
					Damage.Builder builder = new Damage.Builder(damage.type, null, target)
						.WithDamage(damage.amount)
						.WithKnockback(damage.knockback);
					target.PreprocessHit(builder);
					target.TakeDamage(builder.Build());
				}
			}
		}, 10);
	}

	public override void StackAdditionalEffect(StatusEffect additionalEffect)
	{
		DamageNearbyEnemiesStatusEffect effect = additionalEffect as DamageNearbyEnemiesStatusEffect;
		if (effect == null)
		{
			return;
		}

		base.StackAdditionalEffect(additionalEffect);
		damage.amount = CalculateStackedParameter(damage.amount, effect.damage.amount, damage.amountStackType);
		damage.knockback = CalculateStackedParameter(damage.knockback, effect.damage.knockback, damage.knockbackStackType);
		distance = CalculateStackedParameter(distance, effect.distance, distanceStackType);

	}
}

[CreateAssetMenu(menuName = "Status Effects/Damage Nearby Enemies")]
public class DamageNearbyEnemiesScriptableObject : StatusEffectScriptableObject
{
	[SerializeField] private DamageStruct damage;
	[SerializeField] private float distance;
	[SerializeField] private ParameterStackType distanceStackType;

	public override StatusEffect GetEffectObject()
	{
		return new DamageNearbyEnemiesStatusEffect(name, duration, durationStackType, damage, distance, distanceStackType);
	}
}
