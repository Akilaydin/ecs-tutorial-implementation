﻿using TMPro;

using UnityEngine;

namespace Client.Views
{
	public class CounterView : MonoBehaviour
	{
		[SerializeField] private TMP_Text _text;

		public void SetText(string text)
		{
			_text.text = text;
		}
	}
}
