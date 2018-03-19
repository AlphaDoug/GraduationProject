using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace FallDown{
public class ClockEffect : FallDownEffect {

	[SerializeField]
	private Button button;

	[SerializeField]
	private GameObject GameWinUI;
	public override void FallDownEvent()
	{
		Debug.Log("通关!");
		button.onClick.AddListener(()=>{GameWinUI.SetActive(true);});
		//button.onClick
	}
	
}
}
