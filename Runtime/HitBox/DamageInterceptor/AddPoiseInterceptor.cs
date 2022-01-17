using UnityEngine;

[CreateAssetMenu(menuName = "Damage Interceptors/Add Poise")]
public class AddPoiseInterceptor : DamageInterceptorScriptableObject
{
	[SerializeField]
	private float modifier;

	public override void Process(Damage.Builder builder)
	{
		builder.WithPoise(builder.poise + modifier);
	}
}
