using UnityEngine;
using UnityEngine.UI;

public class ExpBar : MonoBehaviour
{
	
public Slider slider;
	
	
	public void SetMaxExp(float exp)
	{
		slider.maxValue = exp;
	}

	public void SetExp(float exp)
	{ 
	Debug.Log("Exp to be set " +  exp + " slider max value " + slider.maxValue);
	  if(exp <= slider.maxValue){
		  slider.value = exp;
	  }else
	  {
	  	slider.value = 0;
	  }
	}
}
