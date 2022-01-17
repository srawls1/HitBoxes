using UnityEngine;

public class DamageOverTimeStatusEffect : StatusEffect
{
	private DamageStruct damage;
	private float tickPeriod;
	private ParameterStackType tickPeriodStackType;

	public DamageOverTimeStatusEffect(string name, float totalDuration, ParameterStackType durationStackType, DamageStruct damage, float tickPeriod, ParameterStackType tickPeriodStackType)
		: base(name, totalDuration, durationStackType)
	{
		this.damage = damage;
		this.tickPeriod = tickPeriod;
		this.tickPeriodStackType = tickPeriodStackType;
	}

	public override void OnInflict(StatusEffectsContainer container)
	{
		container.AddTickCallback(name, container.InflictDamage(damage), tickPeriod);
	}

	public override void StackAdditionalEffect(StatusEffect additionalEffect)
	{
		DamageOverTimeStatusEffect effect = additionalEffect as DamageOverTimeStatusEffect;
		if (effect == null)
		{
			return;
		}

		base.StackAdditionalEffect(effect);
		damage.amount = CalculateStackedParameter(damage.amount, effect.damage.amount, damage.amountStackType);
		damage.knockback = CalculateStackedParameter(damage.knockback, effect.damage.knockback, damage.knockbackStackType);
		tickPeriod = CalculateStackedParameter(tickPeriod, effect.tickPeriod, tickPeriodStackType);
	}
}

[CreateAssetMenu(menuName = "Status Effects/Damage Over Time")]
public class DamageOverTimeScriptableObject : StatusEffectScriptableObject
{
	[SerializeField] private DamageStruct damagePerTick;
	[SerializeField] private float tickPeriod;
	[SerializeField] private ParameterStackType tickPeriodStackType;

	public override StatusEffect GetEffectObject()
	{
		return new DamageOverTimeStatusEffect(effectName, duration, durationStackType, damagePerTick, tickPeriod, tickPeriodStackType);
	}
}
