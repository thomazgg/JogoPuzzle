using UnityEngine;
using UnityEngine.UI;

public class Switch : MonoBehaviour
{
    public GameObject switchActive;
    public GameObject switchInactive;

    private void Start()
    {
        SetSwitches(true);

        Button button = GetComponent<Button>();
        button.onClick.AddListener(ToggleSwitches);
    }

    private void SetSwitches(bool activeState)
    {
        switchActive.SetActive(activeState);
        switchInactive.SetActive(!activeState);
    }

    public void ToggleSwitches()
    {
        Debug.Log("Botão pressionado.");

        SetSwitches(!switchActive.activeSelf);
    }
}