using UnityEngine;

[CreateAssetMenu(menuName = "Damage Interceptors/Add Damage")]
public class AddDamageInterceptor : DamageInterceptorScriptableObject
{
	[SerializeField]
	private float modifier;

	public override void Process(Damage.Builder builder)
	{
		builder.damage = builder.damage + modifier;
	}
}
