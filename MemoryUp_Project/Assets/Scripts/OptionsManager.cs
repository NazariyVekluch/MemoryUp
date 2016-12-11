using UnityEngine;
using UnityEngine.UI;

public class OptionsManager : MonoBehaviour {

    public Dropdown dropDown;

	void OnEnable () {
        switch (LevelManager.columnCount)
        {
            case 3:
                {
                    dropDown.value = 0;
                    break;
                }
            case 4:
                {
                    dropDown.value = 1;
                    break;
                }
            case 5:
                {
                    dropDown.value = 2;
                    break;
                }
            case 6:
                {
                    dropDown.value = 3;
                    break;
                }
            default:
                break;
        }
    }

    public void ChangeValueDropDown()
    {
        switch (dropDown.value)
        {
            case 0:
                {
                    LevelManager.columnCount = 3;
                    break;
                }
            case 1:
                {
                    LevelManager.columnCount = 4;
                    break;
                }
            case 2:
                {
                    LevelManager.columnCount = 5;
                    break;
                }
            case 3:
                {
                    LevelManager.columnCount = 6;
                    break;
                }
            default:
                break;
        }
    }
}
