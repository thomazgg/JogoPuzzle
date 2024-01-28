using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Nav : MonoBehaviour
{
    [SerializeField] GameObject[] panels;
    [SerializeField] Button[] buttons;
    [SerializeField] Color activeColor;
    [SerializeField] Color inactiveColor;
    [SerializeField] Vector2 activeSize;
    [SerializeField] Vector2 inactiveSize;

    private GameObject initialPanel;

    private void Start()
    {
        initialPanel = panels[0];
        NavigationBarClick(initialPanel);
    }

    public void NavigationBarClick(GameObject activePanel)
    {
        foreach (GameObject panel in panels)
        {
            panel.SetActive(false);
        }

        activePanel.SetActive(true);

        foreach (Button button in buttons)
        {
            button.image.color = inactiveColor;
            button.GetComponent<RectTransform>().sizeDelta = inactiveSize;
        }

        int activeIndex = -1;
        for (int i = 0; i < panels.Length; i++)
        {
            if (panels[i].activeSelf)
            {
                activeIndex = i;
                break;
            }
        }

        if (activeIndex != -1)
        {
            buttons[activeIndex].image.color = new Color(activeColor.r, activeColor.g, activeColor.b, 250f / 255f);
            buttons[activeIndex].GetComponent<RectTransform>().sizeDelta = activeSize;
        }
    }
}
