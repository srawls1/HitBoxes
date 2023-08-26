using UnityEngine;
using UnityEngine.UI;

public class AmmoBar : MonoBehaviour
{
    [SerializeField] private AmmoPool m_ammoPool;
    [SerializeField] private AmmoChangedChannelSO channel;
	[SerializeField] private bool adjustLengthWithMaxAmmo;
	[SerializeField] private float pixelLengthPerAmmo;

	private Slider slider;

    public AmmoPool ammoPool
	{
        get { return m_ammoPool; }
		set
		{
			if (m_ammoPool != null)
			{
				m_ammoPool.ammoChangedEvent.RemoveListener(UpdateAmmo);
			}

			m_ammoPool = value;

			if (m_ammoPool != null)
			{
				UpdateAmmo(new AmmoChangedParameters()
				{
					currentAmmo = ammoPool.currentAmmo,
					maxAmmo = ammoPool.maxAmmo
				});
				ammoPool.ammoChangedEvent.AddListener(UpdateAmmo);
			}
		}
	}

	private void Awake()
	{
		slider = GetComponent<Slider>();
	}

	private void Start()
	{
		ammoPool = ammoPool;
	}

	private void OnEnable()
	{
		if (channel)
		{
			channel.Subscribe(UpdateAmmo);
		}
	}

	private void OnDisable()
	{
		if (channel)
		{
			channel.Subscribe(UpdateAmmo);
		}
	}

	private void UpdateAmmo(AmmoChangedParameters parameters)
	{
		if (adjustLengthWithMaxAmmo)
		{
			RectTransform rectTransform = transform as RectTransform;
			float startingWidth = rectTransform.rect.width;
			rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, pixelLengthPerAmmo * parameters.maxAmmo);
			float endingWidth = rectTransform.rect.width;
			rectTransform.position += Vector3.right * (endingWidth - startingWidth) * 0.5f;
		}

		slider.value = (float)parameters.currentAmmo / parameters.maxAmmo;
	}
}
