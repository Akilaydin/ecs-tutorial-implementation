using TMPro;

using UnityEngine;
using UnityEngine.UI;

namespace Client.Views
{
	public class PopupView : MonoBehaviour
	{
		[field: SerializeField] public Button Button { get; private set; }

		[SerializeField] private TMP_Text _descriptionTmp;
		[SerializeField] private TMP_Text _buttonTmp;
		
		[SerializeField] private GameObject _root;

		public void SetDescription(string text)
		{
			_descriptionTmp.text = text;
		}

		public void SetButtonText(string text)
		{
			_buttonTmp.text = text;
		}

		public void SetActive(bool state)
		{
			_root.SetActive(state);
		}
	}
}
