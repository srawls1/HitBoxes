using UnityEngine;

[CreateAssetMenu(menuName = "Damage Interceptors/Multiply Damage")]
public class MultiplyDamageInterceptor : DamageInterceptorScriptableObject
{
	[SerializeField] private float factor;

	public override void Process(Damage.Builder builder)
	{
		builder.damage = builder.damage * factor;
	}
}
