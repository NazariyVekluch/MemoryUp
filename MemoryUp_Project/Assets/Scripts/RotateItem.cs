using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RotateItem : MonoBehaviour {

    public Color obverse;
    public Color reverse;

    public static event Action<RotateItem> CheckForMatches;

    [HideInInspector]
    public bool openedItem = false;

    RectTransform rectTransform;
    float speed = 10;

    void Start()
    {
        rectTransform = gameObject.GetComponent<RectTransform>();

        StartCoroutine(StartRotate());  
    }

    public void LevelManager_action1()
    {
        StartCoroutine(Rotate1());
    }

    public void Click_Rotate()
    {
        if (LevelManager.allowedClick && !openedItem)
        {
            LevelManager.allowedClick = false;
            openedItem = true;

            StartCoroutine(Rotate2());

            if (CheckForMatches != null)
            {
                CheckForMatches(this);
            }
        }
    }

    IEnumerator Rotate1()
    {
        float acumulate = 0;

        while (rectTransform.rotation != Quaternion.Euler(0, 90, 0))
        {
            acumulate += speed * Time.deltaTime;

            rectTransform.rotation = Quaternion.Lerp(Quaternion.identity, Quaternion.Euler(0, 90, 0), acumulate);

            yield return null;
        }

        ChangesReverse();

        acumulate = 0;

        while (rectTransform.rotation != Quaternion.Euler(0, 0, 0))
        {
            acumulate += speed * Time.deltaTime;

            rectTransform.rotation = Quaternion.Lerp(Quaternion.Euler(0, 90, 0), Quaternion.identity, acumulate);

            yield return null;
        }

        LevelManager.allowedClick = true;
        openedItem = false;
    }

    IEnumerator Rotate2()
    {
        float acumulate = 0;

        while (rectTransform.rotation != Quaternion.Euler(0, 90, 0))
        {
            acumulate += speed * Time.deltaTime;

            rectTransform.rotation = Quaternion.Lerp(Quaternion.identity, Quaternion.Euler(0, 90, 0), acumulate);

            yield return null;
        }

        ChangesObverse();

        acumulate = 0;

        while (rectTransform.rotation != Quaternion.Euler(0, 0, 0))
        {
            acumulate += speed * Time.deltaTime;

            rectTransform.rotation = Quaternion.Lerp(Quaternion.Euler(0, 90, 0), Quaternion.identity, acumulate);

            yield return null;
        }
    }

    IEnumerator RotateSTR()
    {
        float acumulate = 0;

        while (rectTransform.rotation != Quaternion.Euler(0, 90, 0))
        {
            acumulate += speed * Time.deltaTime;

            rectTransform.rotation = Quaternion.Lerp(Quaternion.identity, Quaternion.Euler(0, 90, 0), acumulate);

            yield return null;
        }

        ChangesObverse();

        acumulate = 0;

        while (rectTransform.rotation != Quaternion.Euler(0, 0, 0))
        {
            acumulate += speed * Time.deltaTime;

            rectTransform.rotation = Quaternion.Lerp(Quaternion.Euler(0, 90, 0), Quaternion.identity, acumulate);

            yield return null;
        }

        // wait
        yield return new WaitForSeconds(1.5f);

        acumulate = 0;

        while (rectTransform.rotation != Quaternion.Euler(0, 90, 0))
        {
            acumulate += speed * Time.deltaTime;

            rectTransform.rotation = Quaternion.Lerp(Quaternion.identity, Quaternion.Euler(0, 90, 0), acumulate);

            yield return null;
        }

        ChangesReverse();

        acumulate = 0;

        while (rectTransform.rotation != Quaternion.Euler(0, 0, 0))
        {
            acumulate += speed * Time.deltaTime;

            rectTransform.rotation = Quaternion.Lerp(Quaternion.Euler(0, 90, 0), Quaternion.identity, acumulate);

            yield return null;
        }

        AfterStart();
    }

    IEnumerator StartRotate()
    {
        yield return new WaitForSeconds(0.5f);

        StartCoroutine(RotateSTR());
    }

    void ChangesReverse()
    {
        gameObject.GetComponent<Image>().color = reverse;
        gameObject.transform.GetChild(0).gameObject.SetActive(false);
    }

    void ChangesObverse()
    {
        gameObject.GetComponent<Image>().color = obverse;
        gameObject.transform.GetChild(0).gameObject.SetActive(true);
    }

    void AfterStart()
    {
        LevelManager.allowedClick = true;
        LevelManager.startTime = DateTime.Now;
        LevelManager.goTimer = true;
    }
}
