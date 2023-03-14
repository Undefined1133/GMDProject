using UnityEngine;
using UnityEngine.UI;

public class ManaBar : MonoBehaviour
{
public Slider slider;
	
	
	public void SetMaxMana(int mana)
	{
		slider.maxValue = mana;
		slider.value = mana;
	}

	public void SetMana(int mana)
	{ 
	Debug.Log("Mana to be set " +  mana + " slider max value " + slider.maxValue);
	  if(mana <= slider.maxValue){
   	  slider.value = mana;
	  }else
	  {
	  	slider.value = slider.maxValue;
	  }

	}
}
