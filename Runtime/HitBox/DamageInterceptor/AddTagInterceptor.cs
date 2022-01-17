using UnityEngine;

[CreateAssetMenu(menuName = "Damage Interceptors/Add Tag")]
public class AddTagInterceptor : DamageInterceptorScriptableObject
{
	[SerializeField] private string tag;

	public override void Process(Damage.Builder builder)
	{
		builder.WithTag(tag);
	}
}
