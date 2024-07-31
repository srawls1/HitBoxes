using UnityEngine;
using UnityEngine.Events;

public struct AmmoChangedParameters
{
	public int currentAmmo;
	public int maxAmmo;
	public int ammoUsed;
}

public class AmmoPool : MonoBehaviour
{
	[SerializeField] private int m_maxAmmo;
	[SerializeField] private int m_currentAmmo;
	[SerializeField] private UnityEvent<AmmoChangedParameters> m_ammoChangedEvent;
	[SerializeField] private AmmoChangedChannelSO ammoChangedChannel;
	[SerializeField] private UnityEvent m_insufficientAmmoEvent;

	public int maxAmmo
	{
		get { return m_maxAmmo; }
		set { m_maxAmmo = value; }
	}

	public int currentAmmo
	{
		get { return m_currentAmmo; }
		set { m_currentAmmo = value; }
	}

	public UnityEvent<AmmoChangedParameters> ammoChangedEvent
	{
		get { return m_ammoChangedEvent; }
	}

	public UnityEvent insufficientAmmoEvent
	{
		get { return m_insufficientAmmoEvent; }
	}

	private void OnEnable()
	{
		if (ammoChangedChannel)
		{
			ammoChangedEvent.AddListener(ammoChangedChannel.Broadcast);
		}
	}

	private void OnDisable()
	{
		if (ammoChangedChannel)
		{
			ammoChangedEvent.RemoveListener(ammoChangedChannel.Broadcast);
		}
	}

	public bool CanUse(int cost)
	{
		bool canUse = currentAmmo >= cost;
		if (!canUse)
		{
			insufficientAmmoEvent.Invoke();
		}
		return canUse;
	}

    public bool UseAmmo(int cost)
	{
		if (currentAmmo < cost)
		{
			return false;
		}

		currentAmmo -= cost;
		ammoChangedEvent.Invoke(new AmmoChangedParameters()
		{
			currentAmmo = currentAmmo,
			maxAmmo = maxAmmo,
			ammoUsed = cost
		});

		return true;
	}

	public void ReplenishAmmo(int amount)
	{
		currentAmmo = Mathf.Min(currentAmmo + amount, maxAmmo);
		ammoChangedEvent.Invoke(new AmmoChangedParameters()
		{
			currentAmmo = currentAmmo,
			maxAmmo = maxAmmo,
			ammoUsed = -amount
		});
	}

	public int GetCurrentUses(int cost)
	{
		return currentAmmo / cost;
	}

	public int GetMaxUses(int cost)
	{
		return maxAmmo / cost;
	}
}
