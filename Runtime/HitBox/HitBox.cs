using System.Collections.Generic;
using UnityEngine;

public enum KnockbackType
{
	Radial,
	DirectionOfVelocity,
	FixedDirection
}

public class HitBox : MonoBehaviour
{
    #region Editor Fields

    [SerializeField] private bool allowPenetration;
	[SerializeField] private float damage;
	[SerializeField] private float knockback;
	[SerializeField, Tooltip("It's actually period." +
		"The time in seconds between damaging the same the hurtbox when they're still overlapping." +
		"Set to 0 to not allow duplicate damaging at all.")] private float damageFrequency;
	[SerializeField] private KnockbackType knockbackType;
	[SerializeField] private Vector3 direction;
	[SerializeField] private DamageType type;
	[SerializeField] private List<DamageInterceptorScriptableObject> interceptorObjects;

    #endregion // Editor Fields

    #region Non-Editor Fields

    private bool initialized;
	private RelativeTime time;
	new private Rigidbody rigidbody;
	private List<Pair<DamageInterceptor, int>> interceptors;
	private HashSet<HurtBox> alreadyHurtBoxes;
	private List<HurtBox> currentlyOverlappedHurtboxes;

    #endregion // Non-Editor Fields

    #region Unity Functions

    private void Awake()
	{
		Initialize();
	}

	private void Update()
	{
		if (!allowPenetration && alreadyHurtBoxes.Count > 0)
		{
			return;
		}
		for (int i = currentlyOverlappedHurtboxes.Count; i-- > 0;)
		{
			HurtBox hurtBox = currentlyOverlappedHurtboxes[i];
			if (!hurtBox)
			{
				currentlyOverlappedHurtboxes.RemoveAt(i);
				continue;
			}
			if (!hurtBox.enabled || hurtBox.inInvulnerabilityWindow || alreadyHurtBoxes.Contains(hurtBox))
			{
				continue;
			}

			AddAlreadyHurtHurtbox(hurtBox);
			ProcessHit(hurtBox);
			if (!allowPenetration)
			{
				break;
			}
		}
	}

	private void OnEnable()
	{
		currentlyOverlappedHurtboxes.Clear();
		alreadyHurtBoxes.Clear();
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.transform.root == transform.root)
		{
			return;
		}

		HurtBox hurtBox = other.GetComponent<HurtBox>();
		if (hurtBox)
		{
			currentlyOverlappedHurtboxes.Add(hurtBox);
		}
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		HurtBox hurtBox = other.GetComponent<HurtBox>();
		if (hurtBox)
		{
			currentlyOverlappedHurtboxes.Remove(hurtBox);
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.transform.root == transform.root)
		{
			return;
		}

		HurtBox hurtBox = other.GetComponent<HurtBox>();
		if (hurtBox)
		{
			currentlyOverlappedHurtboxes.Add(hurtBox);
		}

	}

	private void OnTriggerExit(Collider other)
	{
		HurtBox hurtBox = other.GetComponent<HurtBox>();
		if (hurtBox)
		{
			currentlyOverlappedHurtboxes.Remove(hurtBox);
		}
	}

	#endregion // Unity Functions

	#region Public Functions

	public void AddDamageInterceptor(DamageInterceptor interceptor, int priority)
	{
		Initialize();
		if (interceptors == null)
		{
			interceptors = new List<Pair<DamageInterceptor, int>>();
		}
		interceptors.Add(new Pair<DamageInterceptor, int>(interceptor, priority));
	}

	public void RemoveDamageInterceptor(DamageInterceptor interceptor)
	{
		Initialize();
		interceptors.RemoveAll((pair) => pair.First == interceptor);
	}

	public void RemoveAllDamageInterceptors()
	{
		Initialize();
		interceptors.Clear();
	}

	#endregion // Public Functions

	#region Private Functions

	private void ProcessHit(HurtBox hurtBox)
	{
		Damage.Builder builder = new Damage.Builder(type, this, hurtBox);
		hurtBox.PreprocessHit(builder);
		PreprocessHit(builder);
		Damage damage = builder.Build();
		hurtBox.TakeDamage(damage);
	}

	private void Initialize()
    {
		if (initialized)
        {
			return;
        }

		rigidbody = GetComponentInParent<Rigidbody>();
		time = GetComponentInParent<RelativeTime>();
		interceptors = new List<Pair<DamageInterceptor, int>>();
		for (int i = 0; i < interceptorObjects.Count; ++i)
		{
			interceptors.Add(new Pair<DamageInterceptor, int>(interceptorObjects[i].Process, interceptorObjects[i].priority));
		}

		alreadyHurtBoxes = new HashSet<HurtBox>();
		currentlyOverlappedHurtboxes = new List<HurtBox>();
		initialized = true;
	}

	private void AddAlreadyHurtHurtbox(HurtBox hurtBox)
	{
		alreadyHurtBoxes.Add(hurtBox);
		if (damageFrequency > 0)
		{
			time.SetTimer(damageFrequency, () => alreadyHurtBoxes.Remove(hurtBox));
		}
	}

	private void PreprocessHit(Damage.Builder builder)
	{
		builder.WithDamage(damage)
			.WithDirection(GetKnockbackDirection(builder))
			.WithKnockback(knockback);
		
		for (int i = 0; i < interceptors.Count; ++i)
		{
			builder.WithInterceptor(interceptors[i].First, interceptors[i].Second);
		}
	}

	private Vector3 GetKnockbackDirection(Damage.Builder builder)
	{
		switch (knockbackType)
		{
			case KnockbackType.FixedDirection:
				return direction.normalized;
			case KnockbackType.DirectionOfVelocity:
				if (!rigidbody)
				{
					Debug.LogError("This HitBox has knockbackType of DirectionOfVelocity but there is no rigidbody component attached. Defaulting to no knockback.");
					return Vector3.zero;
				}
				return rigidbody.velocity.normalized;
			case KnockbackType.Radial:
				return (builder.hurtbox.transform.position - transform.position).normalized;
		}

		return Vector3.zero;
	}

    #endregion // Private Functions
}
