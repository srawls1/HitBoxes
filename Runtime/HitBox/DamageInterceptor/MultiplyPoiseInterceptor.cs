using UnityEngine;

[CreateAssetMenu(menuName = "Damage Interceptors/Multiply Poise")]
public class MultiplyPoiseInterceptor : DamageInterceptorScriptableObject
{
	[SerializeField] private float factor;

	public override void Process(Damage.Builder builder)
	{
		builder.WithPoise(builder.poise * factor);
	}
}
