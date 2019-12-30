using UnityEngine;

public abstract class BaseGameUI : MonoBehaviour
{
    public abstract void RefreshUI();
    public void ShowUI(bool show)
    {
        if (gameObject.activeSelf != show)
            gameObject.SetActive(show);
    }
}
