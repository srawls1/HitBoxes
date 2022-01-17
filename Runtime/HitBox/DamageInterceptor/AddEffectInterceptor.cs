using UnityEngine;

[CreateAssetMenu(menuName = "Damage Interceptors/Add Effect")]
public class AddEffectInterceptor : DamageInterceptorScriptableObject
{
	[SerializeField] private AttackEffectScriptableObject effectToAdd;

	public override void Process(Damage.Builder builder)
	{
		builder.WithEffect(effectToAdd);
	}
}
