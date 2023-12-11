using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoTitle : MonoBehaviour
{
    public void GTitle()
	{
        SceneManager.LoadScene("MainGame");
        SceneTrans.Instance.currentLocation = 0;
	}

    // Start is called before the first frame update
    void Start()
    {
        
    }

}
