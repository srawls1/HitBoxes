using UnityEngine;

public class SlowDownStatusEffect : StatusEffect
{
	private float slowdownFactor;
	private ParameterStackType slowdownStackType;

	public SlowDownStatusEffect(string name, float totalDuration, ParameterStackType durationStackType,
		float slowdownFactor, ParameterStackType slowdownStackType) 
		: base(name, totalDuration, durationStackType)
	{
		this.slowdownFactor = slowdownFactor;
		this.slowdownStackType = slowdownStackType;
	}

	public override void OnInflict(StatusEffectsContainer container)
	{
		RelativeTime time = container.GetComponent<RelativeTime>();
		if (time == null)
		{
			return;
		}

		time.timeScale *= slowdownFactor;
		container.AddExpireCallback(name, () => time.timeScale /= slowdownFactor);
	}

	public override void StackAdditionalEffect(StatusEffect additionalEffect)
	{
		SlowDownStatusEffect effect = additionalEffect as SlowDownStatusEffect;
		if (effect == null)
		{
			return;
		}

		base.StackAdditionalEffect(effect);
		slowdownFactor = CalculateStackedParameter(slowdownFactor, effect.slowdownFactor, slowdownStackType);
	}
}

[CreateAssetMenu(menuName = "Status Effects/Slow Down")]
class SlowDownScriptableObject : StatusEffectScriptableObject
{
	[SerializeField] private float slowdownFactor;
	[SerializeField] private ParameterStackType slowdownStackType;

	public override StatusEffect GetEffectObject()
	{
		return new SlowDownStatusEffect(name, duration, durationStackType, slowdownFactor, slowdownStackType);
	}
}
