using UnityEngine;

[CreateAssetMenu(menuName = "Damage Interceptors/Add Knockback")]
public class AddKnockbackInterceptor : DamageInterceptorScriptableObject
{
	[SerializeField]
	private float modifier;

	public override void Process(Damage.Builder builder)
	{
		builder.knockback += modifier;
	}
}
