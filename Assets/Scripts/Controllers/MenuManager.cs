using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class MenuManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI titleImage;
    [SerializeField] GameObject startObj;
    [SerializeField] GameObject continueObj;
    [SerializeField] GameObject quitObj;

    [SerializeField] float alpha;
    [SerializeField] float duration1;
    [SerializeField] float duration2;
    [SerializeField] float waitTime;

    private void Start()
    {
        StartCoroutine(MenuFade());
    }

    IEnumerator MenuFade()
    {
        titleImage.DOFade(alpha, duration1);

        yield return new WaitForSeconds(waitTime);

        startObj.GetComponentInChildren<TextMeshProUGUI>().DOFade(alpha, duration2);
        Image[] images1 = startObj.GetComponentsInChildren<Image>();
        foreach (Image i in images1)
        {
            i.DOFade(alpha, duration2);
        }

        continueObj.GetComponentInChildren<TextMeshProUGUI>().DOFade(alpha, duration2);
        Image[] images2 = continueObj.GetComponentsInChildren<Image>();
        foreach (Image i in images2)
        {
            i.DOFade(alpha, duration2);
        }

        quitObj.GetComponentInChildren<TextMeshProUGUI>().DOFade(alpha, duration2);
        Image[] images3 = quitObj.GetComponentsInChildren<Image>();
        foreach (Image i in images3)
        {
            i.DOFade(alpha, duration2);
        }
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("LevelSelect");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
