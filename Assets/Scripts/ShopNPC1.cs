using UnityEngine;

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

	}
	
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
			interactUI.SetActive(true);
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
			interactUI.SetActive(false);
		}
	}
}
