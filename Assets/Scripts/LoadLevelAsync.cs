using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevelAsync : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Screen.SetResolution(1064, 371, false);
        Invoke(nameof(Load), 2F);
    }

    // Update is called once per frame
    void Load()
    {
        SceneManager.LoadSceneAsync(1);
    }
}