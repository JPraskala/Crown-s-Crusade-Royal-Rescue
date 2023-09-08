using UnityEngine;

public class CheckComponent : MonoBehaviour
{
	CharacterController controller;

	void Start()
	{
		
		if (!TryGetComponent<CharacterController>(out controller))
		{
			print("No Controller");
		}
		else 
		{
			print("Controller");
		}
	}
}
