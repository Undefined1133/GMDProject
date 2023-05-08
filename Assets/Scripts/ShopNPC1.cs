using UnityEngine;

public class ShopNPC1 : MonoBehaviour
{
	public GameObject interactUI;
	public GameObject shopUI;

	public void OnInteractClicked()
	{
		shopUI.SetActive(true);
	}
	public void OnShopCloseClicked()
	{
		shopUI.SetActive(false);
	}
	
	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			shopUI.SetActive(true);
		}
	}
	
	void OnTriggerStay(Collider other)
	{
		if(other.gameObject.CompareTag("Player") && shopUI.activeSelf)
		{
			interactUI.SetActive(false);
		}
	}
	void OnTriggerExit(Collider other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			shopUI.SetActive(false);
		}
	}
}
