using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private bool isInMenu = true;

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    public void LoadScene(int id)
    {
        if (id == 0)
            isInMenu = true;
        else
            isInMenu = false;

        SceneManager.LoadScene(id);
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isInMenu)
            {
                Application.Quit();
            }
            else
            {
                LoadScene(0);
            }
        }
    }
}
