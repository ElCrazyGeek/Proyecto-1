using UnityEngine;
using TMPro; //agregamos libreria de text mesh pro
public class GameManager : MonoBehaviour
{
    int score;
   public TextMeshProUGUI textoScore;
    void Start()
    {
        //Buscamos un objeto llamado score y su componente TMPro
        textoScore=GameObject.Find("Score").GetComponent<TextMeshProUGUI>();
    }
    public void sumarPuntos(){
        score++;
        textoScore.text=score.ToString();
    }

    void Update()
    {
        
    }
}
