using UnityEngine;

public class ReceiveDamageInterceptorStatusEffect : StatusEffect
{
	private DamageInterceptor damageInterceptor;
	private int priority;

	public ReceiveDamageInterceptorStatusEffect(string name, float totalDuration, ParameterStackType durationStackType, DamageInterceptor damageInterceptor, int priority)
		: base(name, totalDuration, durationStackType)
	{
		this.damageInterceptor = damageInterceptor;
		this.priority = priority;
	}

	public override void OnInflict(StatusEffectsContainer container)
	{
		container.AddReceiveDamageInterceptor(name, damageInterceptor, priority);
	}
}

[CreateAssetMenu(menuName = "Status Effects/Weak")]
public class ReceiveDamageInterceptorScriptableObject : StatusEffectScriptableObject
{
	[SerializeField] private DamageInterceptorScriptableObject damageInterceptor;

	public override StatusEffect GetEffectObject()
	{
		return new ReceiveDamageInterceptorStatusEffect(effectName, duration, durationStackType, damageInterceptor.Process, damageInterceptor.priority);
	}
}
