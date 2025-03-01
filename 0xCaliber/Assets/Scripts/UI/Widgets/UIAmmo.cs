using TMPro;
using UnityEngine;

namespace Projectiles.UI
{
	public class UIAmmo : UIBehaviour
	{
		// PRIVATE MEMBERS

		[SerializeField]
		private TextMeshProUGUI _ammoValue;

		private int _lastValue = -1;

		// PUBLIC METHODS

		public void UpdateAmmo(int ammoCount)
		{
			if (ammoCount == _lastValue)
				return;

			_ammoValue.text = ammoCount.ToString();
			_lastValue = ammoCount;
		}
	}
}
