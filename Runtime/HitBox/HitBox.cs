using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    #region Editor Fields

    [SerializeField] private bool allowPenetration;
	[SerializeField] private float damage;
	[SerializeField] private float knockback;
	[SerializeField] private Vector3 direction;
	[SerializeField] private DamageType type;
	[SerializeField] private List<DamageInterceptorScriptableObject> interceptorObjects;

    #endregion // Editor Fields

    #region Non-Editor Fields

    private bool initialized;
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
			if (!hurtBox.enabled || alreadyHurtBoxes.Contains(hurtBox))
			{
				continue;
			}

			alreadyHurtBoxes.Add(hurtBox);
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

		interceptors = new List<Pair<DamageInterceptor, int>>();
		for (int i = 0; i < interceptorObjects.Count; ++i)
		{
			interceptors.Add(new Pair<DamageInterceptor, int>(interceptorObjects[i].Process, interceptorObjects[i].priority));
		}

		alreadyHurtBoxes = new HashSet<HurtBox>();
		currentlyOverlappedHurtboxes = new List<HurtBox>();
		initialized = true;
	}

	private void PreprocessHit(Damage.Builder builder)
	{
		builder.WithDamage(damage)
			.WithDirection(direction)
			.WithKnockback(knockback);
		
		for (int i = 0; i < interceptors.Count; ++i)
		{
			builder.WithInterceptor(interceptors[i].First, interceptors[i].Second);
		}
	}

    #endregion // Private Functions
}
