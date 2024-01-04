using UnityEngine;

[CreateAssetMenu(menuName = "Damage Interceptors/Conditional")]
public class ConditionalDamageInterceptor : DamageInterceptorScriptableObject
{
	[SerializeField] private DamageInterceptorCondition condition;
	[SerializeField] private DamageInterceptorScriptableObject delegateInterceptor;

	public override void Process(Damage.Builder builder)
	{
		GetFunctionalInterceptor(condition.IsMet, delegateInterceptor.Process)(builder);
	}

	public static DamageInterceptor GetFunctionalInterceptor(Condition<Damage.Builder> condition, DamageInterceptor interceptor)
	{
		return (builder) =>
		{
			if (condition(builder))
			{
				interceptor(builder);
			}
		};
	}
}
