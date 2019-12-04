using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCanvas : MonoBehaviour
{
    public GameObject clockPrefab;
    public GameObject buttonPrefab;

    private GameObject clock;
    private GameObject button;
    private GameObject bullCount;

    private void Start() {
        Screen.SetResolution(1440, 100, true);

        bullCount = GameObject.Find("BullCount");
    }

    public void newClock(float seconds) {
        if (!clock) {
            clock = GameObject.Instantiate(clockPrefab);
            ClockController clockConfig = clock.GetComponent<ClockController>();
            clockConfig.delay = seconds;
            clockConfig.fire();
            clock.transform.SetParent(gameObject.transform, false);
        }
    }

    public void newButton(float seconds, char reactionkey) {
        if (!button) {
            button = GameObject.Instantiate(buttonPrefab);

            ButtonController buttonConfig = button.GetComponent<ButtonController>();
            buttonConfig.delay = seconds;
            buttonConfig.fire();

            button.transform.Find("Text").gameObject.GetComponent<UnityEngine.UI.Text>().text = reactionkey.ToString().ToUpper();
            button.transform.SetParent(gameObject.transform, false);
        }
    }

    public void killClock() {
        if(clock) Destroy(clock);
    }

    public void killButton() {
        if(button) Destroy(button);
    }

    public void clearScreen() {
        if (clock) Destroy(clock);
        if (button) Destroy(button);
    }

    public void setBullCount(int num) {
        if (bullCount) bullCount.GetComponent<UnityEngine.UI.Text>().text = num.ToString();
    }
}
