using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Resetgame : MonoBehaviour
{

    public void Awake()
    {
        Time.timeScale=1.0f;//decongelamos el tiempo 
    }
    public void resetmalo(){
    
    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    
   }
}
