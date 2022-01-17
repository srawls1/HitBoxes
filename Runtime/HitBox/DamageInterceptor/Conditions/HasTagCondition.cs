using UnityEngine;

[CreateAssetMenu(menuName = "Damage Interceptors/Conditions/Check Tag")]
public class HasTagCondition : DamageInterceptorCondition
{
	[SerializeField] private string tag;

	public override bool IsMet(Damage.Builder arg)
	{
		return arg.HasTag(tag);
	}
}
