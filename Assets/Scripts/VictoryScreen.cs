using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class VictoryScreen : MonoBehaviour
{
    public string mainMenuScene;
    public float timeBetweenShowing = 1f;
    public GameObject textBox, returnButton;

    public Image blackScreen;
    public float fadeSpeed = 2f;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ShowVictoryScreen());

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // Update is called once per frame
    void Update()
    {
        blackScreen.color = new Color(blackScreen.color.r, blackScreen.color.g, blackScreen.color.b, Mathf.MoveTowards(blackScreen.color.a, 0, fadeSpeed * Time.deltaTime));

    }

    public void MainMenu()
    {
        SceneManager.LoadScene(mainMenuScene);
    }
    public IEnumerator ShowVictoryScreen()
    {
        yield return new WaitForSeconds(timeBetweenShowing);
        textBox.SetActive(true);
        yield return new WaitForSeconds(timeBetweenShowing);
        returnButton.SetActive(true);
    }
}
