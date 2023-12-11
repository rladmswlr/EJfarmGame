using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public Button loadGameButton;

    public void NewGame()
	{
        StartCoroutine(LoadGameAsync(SceneTrans.Location.MainGame, null));
	}

    public void Tutorial()
	{
        StartCoroutine(LoadGameAsync(SceneTrans.Location.Tutorial, null));
    }

    public void ContinueGame()
	{
        StartCoroutine(LoadGameAsync(SceneTrans.Location.MainGame, LoadGame));
	}

    public void QuitGame()
	{
        Application.Quit();
	}

    void LoadGame()
	{
        if(GameState.Instance == null)
		{
            Debug.LogError("Cannot find Game State Manager!");
            return;
		}

        GameState.Instance.LoadSave();
	}

    IEnumerator LoadGameAsync(SceneTrans.Location scene, Action onFirstFrameLoad)
	{
        AsyncOperation asyncload = SceneManager.LoadSceneAsync(scene.ToString());

        DontDestroyOnLoad(gameObject);
        while(!asyncload.isDone)
		{
            yield return null;
            Debug.Log("Loading");
		}

        Debug.Log("Loaded!");

        yield return new WaitForEndOfFrame();

        onFirstFrameLoad?.Invoke();

        Destroy(gameObject);
	}

    // Start is called before the first frame update
    void Start()
    {
        loadGameButton.interactable = SaveManager.HasSave();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
