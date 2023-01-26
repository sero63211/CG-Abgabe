using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelExit : MonoBehaviour
{

    public string exitScreen;
    public float exitDelay = 1f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {

            GameManager.instance.levelEnding = true;
            SceneManager.LoadScene(exitScreen);
        }
    }

    private IEnumerator ExitLevel()
    {
        yield return new WaitForSeconds(exitDelay);
                

        SceneManager.LoadScene(exitScreen);
    }
}
