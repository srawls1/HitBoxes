using System;
using UnityEngine;

public enum ParameterStackType
{
	KeepOriginal,
	TakeNew,
	Max,
	Min,
	Sum,
	Product
}

[Serializable]
public struct DamageStruct
{
	public float amount;
	public DamageType type;
	public float knockback;
	public ParameterStackType amountStackType;
	public ParameterStackType knockbackStackType;
}

public abstract class StatusEffect
{
	public readonly string name;
	public float totalDuration { get; protected set; }
	public float remainingDuration;
	public ParameterStackType durationStackType;

	public StatusEffect(string name, float totalDuration, ParameterStackType durationStackType)
	{
		this.name = name;
		this.totalDuration = totalDuration;
		this.remainingDuration = totalDuration;
		this.durationStackType = durationStackType;
	}

	public abstract void OnInflict(StatusEffectsContainer container);

	public virtual void StackAdditionalEffect(StatusEffect additionalEffect)
	{
		float newTotalDuration = CalculateStackedParameter(totalDuration, additionalEffect.totalDuration, durationStackType);
		float durationDiff = newTotalDuration - totalDuration;
		totalDuration = newTotalDuration;
		remainingDuration += durationDiff;
	}

	protected float CalculateStackedParameter(float original, float stacked, ParameterStackType stackType)
	{
		switch (stackType)
		{
			case ParameterStackType.KeepOriginal:	return original;
			case ParameterStackType.TakeNew:		return stacked;
			case ParameterStackType.Max:			return Mathf.Max(original, stacked);
			case ParameterStackType.Min:			return Mathf.Min(original, stacked);
			case ParameterStackType.Sum:			return original + stacked;
			case ParameterStackType.Product:		return original * stacked;
			default:								return 0f;
		}
	}
}

public abstract class StatusEffectScriptableObject : ScriptableObject
{
	[SerializeField] protected string effectName;
	[SerializeField] protected float duration;
	[SerializeField] protected ParameterStackType durationStackType;

	public abstract StatusEffect GetEffectObject();
}
