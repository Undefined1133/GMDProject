using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ShopNPC1 : MonoBehaviour
{
	public GameObject interactUI;
	public GameObject shopUI;
	// Start is called before the first frame update
	void Start()
	{
		
	}

	// Update is called once per frame
	void Update()
	{
		if(shopUI.activeSelf)
		{
			interactUI.SetActive(false);
		}
	}
	
	public void OnInteractClicked()
	{
		shopUI.SetActive(true);
	}
	public void OnShopCloseClicked()
	{
		shopUI.SetActive(false);
	}
	
	void OnTriggerEnter()
	{
		interactUI.SetActive(true);
	}
	void OnTriggerExit()
	{
		interactUI.SetActive(false);
	}
}
