using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPool : MonoBehaviour
{
	[SerializeField] private int m_maxAmmo;
	[SerializeField] private int currentAmmo;

	public int maxAmmo
	{
		get { return m_maxAmmo; }
		set { m_maxAmmo = value; }
	}

	public bool CanUse(int cost)
	{
		return currentAmmo >= cost;
	}

    public bool UseAmmo(int cost)
	{
		if (currentAmmo < cost)
		{
			return false;
		}

		currentAmmo -= cost;
		return true;
	}

	public void ReplenishAmmo(int amount)
	{
		currentAmmo = Mathf.Min(currentAmmo + amount, maxAmmo);
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
