using UnityEngine;

[CreateAssetMenu(menuName = "Damage Interceptors/Multiply Knockback")]
public class MultiplyKnockbackInterceptor : DamageInterceptorScriptableObject
{
	[SerializeField] private float factor;

	public override void Process(Damage.Builder builder)
	{
		builder.knockback *= factor;
	}
}
