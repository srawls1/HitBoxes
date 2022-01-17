using UnityEngine;

public class DealDamageInterceptorStatusEffect : StatusEffect
{
	private DamageInterceptor interceptor;
	private int priority;

	public DealDamageInterceptorStatusEffect(string name, float totalDuration, ParameterStackType durationStackType, DamageInterceptor interceptor, int priority)
		: base(name, totalDuration, durationStackType)
	{
		this.interceptor = interceptor;
		this.priority = priority;
	}

	public override void OnInflict(StatusEffectsContainer container)
	{
		container.AddDealDamageInterceptor(name, interceptor, priority);
	}
}

[CreateAssetMenu(menuName = "Status Effects/Deal Damage Interceptor")]
public class DealDamageInterceptorScriptableObject : StatusEffectScriptableObject
{
	[SerializeField] private DamageInterceptorScriptableObject damageInterceptor;

	public override StatusEffect GetEffectObject()
	{
		return new DealDamageInterceptorStatusEffect(effectName, duration, durationStackType, damageInterceptor.Process, damageInterceptor.priority);
	}
}
