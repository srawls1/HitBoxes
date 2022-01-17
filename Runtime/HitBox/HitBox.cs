using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
	[SerializeField] private bool allowPenetration;
	[SerializeField] private float damage;
	[SerializeField] private float knockback;
	[SerializeField] private Vector3 direction;
	[SerializeField] private DamageType type;
	[SerializeField] private List<DamageInterceptorScriptableObject> interceptorObjects;

	private List<Pair<DamageInterceptor, int>> interceptors;
	private HashSet<HurtBox> alreadyHurtBoxes;
	private List<HurtBox> currentlyOverlappedHurtboxes;

	public void AddDamageInterceptor(DamageInterceptor interceptor, int priority)
	{
		interceptors.Add(new Pair<DamageInterceptor, int>(interceptor, priority));
	}

	public void RemoveDamageInterceptor(DamageInterceptor interceptor)
	{
		interceptors.RemoveAll((pair) => pair.First == interceptor);
	}

	private void Awake()
	{
		interceptors = new List<Pair<DamageInterceptor, int>>();
		for (int i = 0; i < interceptorObjects.Count; ++i)
		{
			interceptors.Add(new Pair<DamageInterceptor, int>(interceptorObjects[i].Process, interceptorObjects[i].priority));
		}

		alreadyHurtBoxes = new HashSet<HurtBox>();
		currentlyOverlappedHurtboxes = new List<HurtBox>();
	}

	private void Update()
	{
		if (!allowPenetration && alreadyHurtBoxes.Count > 0)
		{
			return;
		}
		for (int i = 0; i < currentlyOverlappedHurtboxes.Count; ++i)
		{
			HurtBox hurtBox = currentlyOverlappedHurtboxes[i];
			if (alreadyHurtBoxes.Contains(hurtBox))
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

	private void ProcessHit(HurtBox hurtBox)
	{
		Damage.Builder builder = new Damage.Builder(type, this, hurtBox);
		hurtBox.PreprocessHit(builder);
		PreprocessHit(builder);
		Damage damage = builder.Build();
		hurtBox.TakeDamage(damage);
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
}
