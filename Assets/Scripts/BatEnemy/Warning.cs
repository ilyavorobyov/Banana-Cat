using UnityEngine;

public class Warning : MonoBehaviour
{
    public void ShowWarningState(bool state)
    {
        if(state) gameObject.SetActive(true);
        else gameObject.SetActive(false);
    }    
}