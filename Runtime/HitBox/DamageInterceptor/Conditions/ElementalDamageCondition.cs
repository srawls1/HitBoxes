using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Damage Interceptors/Conditions/Element")]
public class ElementalDamageCondition : DamageInterceptorCondition
{
	[SerializeField] private List<DamageType> allowedTypes;

	public override bool IsMet(Damage.Builder arg)
	{
		return GetElementalDamageCondition(allowedTypes)(arg);
	}

	public static Condition<Damage.Builder> GetElementalDamageCondition(ICollection<DamageType> allowedTypes)
	{
		return (builder) => allowedTypes.Contains(builder.type);
	}
}
