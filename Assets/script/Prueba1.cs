using UnityEngine;

public class Prueba1 : MonoBehaviour
{
    public int vidas;
    public float score;
    public string nombrePlayer;
    public bool vivo;
    public Transform checkpoint;
    public SpriteRenderer skin;


    // Start se ejecuta 1 sola vez al dar play 
    void Start()
    {
        score = 0;
        vivo = true;
        Debug.Log(nombrePlayer);
        transform.position = checkpoint.position;
    }

    // Update is called once per frame
    void Update()
    {
        score++;
    }
}
