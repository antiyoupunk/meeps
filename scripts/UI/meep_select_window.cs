using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class meep_select_window : MonoBehaviour {
    GameObject[] meeps;

    public GameObject meepButton;
    private Transform tempTransform;
    private int buttonOffset = -5;
    public RectTransform contentWindow;
    private GameObject forObject;

	public void populateMeeps (GameObject callingObject)
    {
        forObject = callingObject;
        meeps = GameObject.FindGameObjectsWithTag("Meeps");
        foreach(GameObject meep in meeps)
        {
            GameObject newMeepButton = Instantiate(meepButton, new Vector2(0, 0), Quaternion.identity) as GameObject;
            newMeepButton.transform.SetParent(contentWindow.transform);
            newMeepButton.GetComponent<RectTransform>().anchoredPosition = new Vector3(5, buttonOffset);
            newMeepButton.GetComponent<button_texts>().button_name.text = meep.GetComponent<meep_stats>().meepName;
            newMeepButton.GetComponent<button_texts>().myMeep = meep;
            buttonOffset -= 105;
        }
        contentWindow.sizeDelta = new Vector2(contentWindow.sizeDelta.x, Mathf.Abs(buttonOffset));
        contentWindow.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);

    }

    public void selectMeep(GameObject button)
    {
        Destroy(button.GetComponent<button_texts>().myMeep);
    }
	
}
