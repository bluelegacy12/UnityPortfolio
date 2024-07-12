using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Assist : MonoBehaviour
{
    public TMPro.TMP_Dropdown myDrop;
    public TMPro.TextMeshProUGUI assistText;
    public void AssistSelector()
    {
        if (myDrop.value == 0)
        {
            ThirdPersonMovement.thirdPersonMovement.assist = true;
            assistText.text = "Auto-target mouse when dashing (easy mode)";
        }
        else
        {
            ThirdPersonMovement.thirdPersonMovement.assist = false;
            assistText.text = "Accuracy is key!    (expert mode)";
        }
        
    }
}
