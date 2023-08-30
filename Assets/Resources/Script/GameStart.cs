using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStart : MonoBehaviour
{
    void Update()
    {
        ClickStart();
    }

    void ClickStart()
    {
        if (Input.GetMouseButtonDown(0))
            SceneManager.LoadScene("GameScene");
    }
}
