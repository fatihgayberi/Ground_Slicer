using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour
{
    [SerializeField] Slider loading;

    public void Load()
    {
    }

    IEnumerator StartLoading()
    {
        AsyncOperation async = SceneManager.LoadSceneAsync(1);

        while (!async.isDone)
        {
            loading.value = async.progress + 0.1f;
            
            yield return null;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(StartLoading());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
