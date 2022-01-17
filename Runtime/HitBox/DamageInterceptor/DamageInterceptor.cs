using UnityEngine;

public delegate void DamageInterceptor(Damage.Builder builder);

public abstract class DamageInterceptorScriptableObject : ScriptableObject
{
	[SerializeField] private int m_priority;

	public int priority
	{
		get { return m_priority; }
		protected set { m_priority = value; }
	}

	public abstract void Process(Damage.Builder builder);
}
