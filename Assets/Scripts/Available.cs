using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Available : MonoBehaviour {


	public int rechargeTime;
	public Text uiText;

	int remRecharge;
	
	//called when the button is first pressed
	public void StartRecharge()
	{
		remRecharge = rechargeTime;
		uiText.text = remRecharge.ToString();
		base.GetComponent<Button>().interactable = false;
		
	}
	
	//called after each turn while spell is recharging
	public void DecreaseRecharge()
	{
		remRecharge -= 1;
		uiText.text = remRecharge.ToString();

		if (remRecharge <= 0)
		{
			gameObject.GetComponent<SpellButton>().turnReady = true;
			uiText.text = "";
			base.GetComponent<Button>().interactable = true;
		}
		
	}

}
