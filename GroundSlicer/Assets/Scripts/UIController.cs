using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] GameObject splashPanel;
    [SerializeField] GameObject player;
    [SerializeField] GameObject holdToControlPanel;
    float timer;

    // Start is called before the first frame update
    void Start()
    {
        SplashControl();
    }

    private void Update()
    {
        HoldToControlPanel();
    }

    void SplashControl()
    {
        if (PlayerPrefs.GetInt("FirstOpen") == 0)
        {
            player.SetActive(false);
            splashPanel.SetActive(true);
        }
    }

    void HoldToControlPanel()
    {
        if (PlayerPrefs.GetInt("FirstOpen") == 1)
        {
            holdToControlPanel.SetActive(true);

            timer += Time.deltaTime;
            if (timer >= 2.5)
            {
                holdToControlPanel.SetActive(false);
            }
        }
    }
}
