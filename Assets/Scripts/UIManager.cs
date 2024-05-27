using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject gameOverText;
    [SerializeField]
    private GameObject gameClearText;

    private static UIManager m_instance;
    public static UIManager Instance
    {
        get
        {
            if(m_instance == null)
                m_instance = GameObject.FindObjectOfType<UIManager>();

            return m_instance;
        }
    }

    private void Awake()
    {
        if (m_instance)
            Destroy(gameObject);
    }

    public void GameOverTextUpdate(bool value)
    {
        gameOverText.SetActive(value);
    }

    public void GameClearTextUpdate(bool value)
    {
        gameClearText.SetActive(value);
    }
}