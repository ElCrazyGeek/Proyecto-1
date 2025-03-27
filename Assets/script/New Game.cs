using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewGame : MonoBehaviour
{
 
public void botonInicio(){
    Debug.Log("NEW GAME");
    SceneManager.LoadScene("Examen Breakout");

    //para cargar escenas que no son la de default usamos 
    //la buil profile y desde ahi le damos en add open scenes 
}


}
