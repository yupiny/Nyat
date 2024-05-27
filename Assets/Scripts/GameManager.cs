using UnityEngine;

public class GameManager : MonoBehaviour
{
    private int sparrowCount;
    //public int SparrowCount
    //{
    //    get => sparrowCount;
    //    set {
    //        sparrowCount += value;
    //        if (sparrowCount <= 0)
    //        {
    //            UIManager.Instance.GameClearTextUpdate(true);
    //        }
    //    }
    //}
    private static GameManager m_instance;
    public static GameManager Instance
    {
        get
        {
            if (m_instance == null)
                m_instance = GameObject.FindObjectOfType<GameManager>();

            return m_instance;
        }
    }

    public void SetSparrow(int value)
    {
        sparrowCount = sparrowCount + value;
        if(sparrowCount <= 0)
        {
            UIManager.Instance.GameClearTextUpdate(true);
        }
    }
    public int GetSparrow()
    {
        return sparrowCount;
    }

    private void Start ()
   {
      
   }
}