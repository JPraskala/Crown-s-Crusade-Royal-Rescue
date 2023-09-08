using UnityEngine;

public class CheckComponent : MonoBehaviour
{
	CharacterController controller;
	
	void Start() 
	{
	   controller = GetComponent<CharacterController>();
	   
	   print(controller != null);
	}
}
