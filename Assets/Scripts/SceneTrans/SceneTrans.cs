using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTrans : MonoBehaviour
{
	public static SceneTrans Instance;

	//씬의 이름
	public enum Location {MainGame, PlayerHome, Town, TitleScene, Tutorial, TutroialHome }
	public Location currentLocation;

	Transform playerPoint;

	private void Awake()
	{
		if (Instance != null && Instance != this)
		{
			Destroy(gameObject);
		}
		else
		{
			Instance = this;
		}

		//씬이 넘어가더라도 파괴되지 않는 정보들
		DontDestroyOnLoad(gameObject);

		SceneManager.sceneLoaded += OnLocationLoad;

		playerPoint = FindObjectOfType<Player>().transform;
	}

	//씬을 전환함
	public void SwitchLocation(Location locationToSwitch)
	{
		SceneManager.LoadScene(locationToSwitch.ToString());
	}

	public void OnLocationLoad(Scene scene, LoadSceneMode mode)
	{
		//The location the player is coming from when the scene loads
		Location oldLocation = currentLocation;

		//Get the new location by converting the string of our current scene into a Location enum value
		Location newLocation = (Location)Enum.Parse(typeof(Location), scene.name);

		//If the player is not coming from any new place, stop executing the function
		if (currentLocation == newLocation) return;

		//Find the start point
		Transform startPoint = LocationManager.Instance.GetPlayerStartingPosition(oldLocation);

		//Disable the player's CharacterController component
		CharacterController playerCharacter = playerPoint.GetComponent<CharacterController>();
		playerCharacter.enabled = false;

		//Change the player's position to the start point
		playerPoint.position = startPoint.position;
		playerPoint.rotation = startPoint.rotation;

		//Re-enable player character controller so he can move
		playerCharacter.enabled = true;

		//Save the current location that we just switched to
		currentLocation = newLocation;
	}

}
