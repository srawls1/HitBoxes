using UnityEngine;

[CreateAssetMenu(menuName = "Damage Interceptors/Conditions/Probability")]
public class RandomChanceCondition : DamageInterceptorCondition
{
	[SerializeField] private float probability; 

	public override bool IsMet(Damage.Builder arg)
	{
		return GetCondition(probability)(arg);
	}

	public static Condition<Damage.Builder> GetCondition(float probability)
	{
		return (builder) => Random.Range(0f, 1f) < probability;
	}
}
