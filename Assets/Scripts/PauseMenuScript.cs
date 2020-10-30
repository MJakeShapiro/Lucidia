using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PauseMenuScript : MonoBehaviour
{
    //Player myPlayer;

    // Start is called before the first frame update
    void Start()
    {
        //myPlayer = GameManager.Instance.GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.LogError("Escape!");
            Time.timeScale = 0;

            GameManager.Instance.player.DisableControls();

        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.LogError("Free!");
            Time.timeScale = 1;

            GameManager.Instance.player.EnableControls();
        }
    }
}
