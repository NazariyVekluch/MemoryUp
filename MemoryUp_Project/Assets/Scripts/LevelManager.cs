using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

public class LevelManager : MonoBehaviour
{
    public GameObject prefabItem;
    public GridLayoutGroup gridLayoutGroup;
    public GameObject WinPanel;
    public GameObject TopMenuPanel;
    public Text timerText;
    public Text timerTextResult;
    public Text tableBestResults;

    public static int columnCount = 3;

    public List<string> baseItems = new List<string>();
    private List<string> tempPairItems = new List<string>();

    List<RotateItem> rotateItems = new List<RotateItem>();
    List<string> rotateItemsName = new List<string>();

    public static bool allowedClick = false;

    int colectedWinItems = 0;

    TimeSpan timer;
    public static DateTime startTime;
    public static bool goTimer = false;

    Vector2 sizeItem;

    void OnEnable()
    {
        RotateItem.CheckForMatches += RotateItem_CheckForMatches1;

        sizeItem = DefineItemSize();

        GenerateItem();

        GeneratePairItems();

        colectedWinItems = 0;

        timerText.text = string.Format("{0:D2}:{1:D2} s", 0, 0);

        goTimer = false;

        allowedClick = false;
    }

    void OnDisable()
    {
        RotateItem.CheckForMatches -= RotateItem_CheckForMatches1;

        ClearBord();
    }

    void FixedUpdate()
    {
        if (goTimer)
        {
            timer = DateTime.Now - startTime;

            if (timer.Minutes >= 59 && timer.Seconds >= 59)
            {
                goTimer = false;
                CheckWin();
                return;
            }

            timerText.text = string.Format("{0:D2}:{1:D2} s", timer.Minutes, timer.Seconds);
        }
    }

    private void RotateItem_CheckForMatches1(RotateItem obj)
    {
        if (!rotateItems.Contains(obj))
        {
            rotateItems.Add(obj);

            if (rotateItems.Count == 2)
            {
                if (rotateItemsName.Contains(obj.name))
                {
                    Invoke("RemovePair", 0.5f);
                }
                else
                {
                    Invoke("ReverseRotate", 0.5f);
                }
            }
            else if (rotateItems.Count == 1)
            {
                allowedClick = true;
            }

            rotateItemsName.Add(obj.name);
        }
    }

    void ReverseRotate()
    {
        foreach (var item in rotateItems)
        {
            item.LevelManager_action1();
        }

        rotateItems.Clear();
        rotateItemsName.Clear();
    }

    void RemovePair()
    {
        foreach (var item in rotateItems)
        {
            RemoveItem(item.gameObject);
        }

        rotateItems.Clear();
        rotateItemsName.Clear();

        CheckWin();

        allowedClick = true;
    }

    void GenerateItem()
    {
        gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        gridLayoutGroup.constraintCount = columnCount;
        gridLayoutGroup.cellSize = sizeItem;

        for (int i = 0; i < columnCount * columnCount; i++)
        {
            Instantiate(prefabItem).transform.SetParent(gridLayoutGroup.transform, false);
        }

        if (columnCount % 2 == 1)
        {
            gridLayoutGroup.transform.GetChild((columnCount * columnCount - 1) / 2).GetComponent<RotateItem>().enabled = false;
            gridLayoutGroup.transform.GetChild((columnCount * columnCount - 1) / 2).GetComponent<Image>().enabled = false;
            gridLayoutGroup.transform.GetChild((columnCount * columnCount - 1) / 2).GetChild(0).gameObject.SetActive(false);
        }
    }

    void GeneratePairItems()
    {
        // mix base items
        Mix(baseItems);

        tempPairItems.Clear();
        int p = 0;
        // need pair count of items, check for pair
        int needCountItems = ((columnCount * columnCount) % 2 == 1) ? columnCount * columnCount - 1 : columnCount * columnCount;

        // take radom pair
        for (int i = 1; i <= needCountItems; i++)
        {
            if (i % 2 == 0)
            {
                p = i / 2;
            }
            else if (i % 2 == 1)
            {
                p = (i + 1) / 2;
            }
            tempPairItems.Add(baseItems[p - 1]);
        }

        // mix pair
        Mix(tempPairItems);

        // output pair
        for (int i = 0; i < columnCount * columnCount; i++)
        {
            if (needCountItems != columnCount * columnCount && i == ((columnCount * columnCount - 1) / 2))
            {
                // jump center item for odd matrix
                continue;
            }

            gridLayoutGroup.transform.GetChild(i).GetChild(0).GetComponent<Text>().text = tempPairItems[0];
            gridLayoutGroup.transform.GetChild(i).name = tempPairItems[0];

            tempPairItems.Remove(tempPairItems[0]);
        }
    }

    void Mix(List<string> array)
    {
        for (int t = 0; t < array.Count; t++)
        {
            string tmp = array[t];
            int r = UnityEngine.Random.Range(t, array.Count);
            array[t] = array[r];
            array[r] = tmp;
        }
    }

    void RemoveItem(GameObject obj)
    {
        obj.transform.GetChild(0).gameObject.SetActive(false);
        obj.GetComponent<Image>().enabled = false;
    }

    void ClearBord()
    {
        for (int i = 0; i < gridLayoutGroup.transform.childCount; i++)
        {
            Destroy(gridLayoutGroup.transform.GetChild(i).gameObject);
        }

        rotateItems.Clear();
        rotateItemsName.Clear();
    }

    void CheckWin()
    {
        colectedWinItems++;

        switch (columnCount)
        {
            case 3:
                {
                    WinAction(4); // 4 - max count of pairs
                    break;
                }
            case 4:
                {
                    WinAction(8);
                    break;
                }
            case 5:
                {
                    WinAction(12);
                    break;
                }
            case 6:
                {
                    WinAction(18);
                    break;
                }
            default:
                {
                    colectedWinItems = 0;
                    break;
                }
        }
    }

    void WinAction(int countPairs)
    {
        if (colectedWinItems == countPairs)
        {
            WinPanel.SetActive(true);
            TopMenuPanel.SetActive(false);
            gameObject.SetActive(false);

            timerTextResult.text = string.Format("Time: {0:D2}:{1:D2} s", timer.Minutes, timer.Seconds);

            if (goTimer == false)
            {
                timerTextResult.text = "Time out";
            }

            GenerateTableBestTime();
        }
    }

    void GenerateTableBestTime()
    {
        switch (columnCount)
        {
            case 3:
                {
                    SaveAchievement(3);
                    GetSavedTime(3);
                    break;
                }

            case 4:
                {
                    SaveAchievement(4);
                    GetSavedTime(4);
                    break;
                }

            case 5:
                {
                    SaveAchievement(5);
                    GetSavedTime(5);
                    break;
                }

            case 6:
                {
                    SaveAchievement(6);
                    GetSavedTime(6);
                    break;
                }
            default:
                break;
        }
    }

    void GetSavedTime(int columns)
    {
        tableBestResults.text = string.Empty;
        TimeSpan ts;
        for (int i = 1; i <= 3; i++)
        {
            ts = TimeSpan.FromSeconds(PlayerPrefs.GetInt("time " + columns + i, 0));
            tableBestResults.text += string.Format("{0}. {1:D2}:{2:D2} s\n", i, ts.Minutes, ts.Seconds);
        }
    }

    void SaveAchievement(int columns)
    {
        List<int> bestResults = new List<int>();

        for (int i = 1; i <= 3; i++)
        {
            bestResults.Add(PlayerPrefs.GetInt("time " + columns + i, 0));
        }

        bestResults.Add((int)timer.TotalSeconds);

        bestResults.Sort();

        int j = 1;

        for (int i = 1; i <= bestResults.Count; i++)
        {
            if (bestResults[i - 1] == 0)
            {
                continue;
            }

            if (j <= 3)
            {
                PlayerPrefs.SetInt("time " + columns + j, bestResults[i - 1]);
                j++;
            }
        }
    }

    Vector2 DefineItemSize()
    {
        return new Vector2(GetComponent<RectTransform>().rect.width / (columnCount + 1), GetComponent<RectTransform>().rect.height / (columnCount + 1));
    }
}

