using UnityEngine;

public class DamageOnExpireStatusEffect : StatusEffect
{
	private DamageStruct damageInfo;

	public DamageOnExpireStatusEffect(string name, float delay, ParameterStackType delayStackType, DamageStruct damageInfo)
		: base(name, delay, delayStackType)
	{
		this.damageInfo = damageInfo;
	}

	public override void OnInflict(StatusEffectsContainer container)
	{
		container.AddExpireCallback(name, container.InflictDamage(damageInfo));
	}

	public override void StackAdditionalEffect(StatusEffect additionalEffect)
	{
		DamageOnExpireStatusEffect effect = additionalEffect as DamageOnExpireStatusEffect;
		if (effect == null)
		{
			return;
		}

		base.StackAdditionalEffect(effect);

		damageInfo.amount = CalculateStackedParameter(damageInfo.amount, effect.damageInfo.amount, damageInfo.amountStackType);
		damageInfo.knockback = CalculateStackedParameter(damageInfo.knockback, effect.damageInfo.knockback, damageInfo.knockbackStackType);
	}
}

[CreateAssetMenu(menuName = "Status Effects/Delayed Damage")]
public class DamageOnExpireScriptableObject : StatusEffectScriptableObject
{
	[SerializeField] private DamageStruct damage;

	public override StatusEffect GetEffectObject()
	{
		return new DamageOnExpireStatusEffect(effectName, duration, durationStackType, damage);
	}
}
