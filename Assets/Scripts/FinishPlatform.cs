using UnityEngine;

public class FinishPlatform : MonoBehaviour
{
    public GameObject finishCanvas;
    private bool finished = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !finished)
        {
            FinishGame();
        }
    }

    void FinishGame()
    {
        finished = true;

        finishCanvas.SetActive(true);

        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}