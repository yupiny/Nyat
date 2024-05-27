using UnityEngine;

public class Count : MonoBehaviour
{
    public int sparrowCount;

    public void ClearText(bool value)
    {
        if (sparrowCount <= 0)
            UIManager.Instance.GameClearTextUpdate(true);
    }

   private void Start ()
   {
      
   }
}