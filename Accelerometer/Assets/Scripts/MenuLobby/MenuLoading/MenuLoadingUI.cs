using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuLoadingUI : MonoBehaviour
{
    public void HideCanvas() {
        gameObject.SetActive(false);
    }
    public void ShowCanvas() {
        gameObject.SetActive(true);
    }
}
