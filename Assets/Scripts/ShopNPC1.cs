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
	
	void OnTriggerEnter()
	{
		Debug.Log("KSIUSHA :D");
		interactUI.SetActive(true);
	}
	
	void OnTriggerStay()
	{
		if(shopUI.activeSelf)
		{
			interactUI.SetActive(false);
		}
	}
	void OnTriggerExit()
	{
		interactUI.SetActive(false);
	}
}
