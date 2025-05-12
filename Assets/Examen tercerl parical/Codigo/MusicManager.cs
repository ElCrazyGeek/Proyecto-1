using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    [SerializeField] private AudioClip pacificMusic;
    [SerializeField] private AudioClip alertMusic;
    [SerializeField] private float fadeDuration = 1f; // Duración del fade en segundos
    private AudioSource musicSource1;
    private AudioSource musicSource2;
    private bool isPlayingSource1;
    private bool isInAlertState;
    private bool isFading;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Crear y configurar dos AudioSource
        musicSource1 = gameObject.AddComponent<AudioSource>();
        musicSource2 = gameObject.AddComponent<AudioSource>();
        ConfigureAudioSource(musicSource1);
        ConfigureAudioSource(musicSource2);
        isPlayingSource1 = true;
    }

    private void ConfigureAudioSource(AudioSource source)
    {
        source.loop = true;
        source.playOnAwake = false;
        source.volume = 0f; // Iniciar con volumen 0
    }

    void Start()
    {
        if (pacificMusic == null || alertMusic == null)
        {
            Debug.LogWarning("Faltan clips de música en MusicManager");
            return;
        }

        // Iniciar la música según el estado del juego
        SyncMusicWithGameState();
    }

    public void PlayPacificMusic()
    {
        if (!isInAlertState && !isFading)
        {
            // Iniciar música pacífica si no está sonando
            if (musicSource1.clip != pacificMusic && musicSource2.clip != pacificMusic)
            {
                musicSource1.clip = pacificMusic;
                musicSource1.volume = 1f;
                musicSource1.Play();
                musicSource2.Stop();
                musicSource2.clip = null;
                musicSource2.volume = 0f;
                isPlayingSource1 = true;
                Debug.Log("Iniciando música pacífica directamente");
            }
            return;
        }

        if (isFading || musicSource1.clip == pacificMusic || musicSource2.clip == pacificMusic)
        {
            return;
        }

        isInAlertState = false;
        StartCoroutine(FadeMusic(pacificMusic));
        Debug.Log("Transición a música pacífica");
    }

    public void PlayAlertMusic()
    {
        if (isInAlertState && !isFading)
        {
            // Iniciar música de alerta si no está sonando
            if (musicSource1.clip != alertMusic && musicSource2.clip != alertMusic)
            {
                musicSource1.clip = alertMusic;
                musicSource1.volume = 1f;
                musicSource1.Play();
                musicSource2.Stop();
                musicSource2.clip = null;
                musicSource2.volume = 0f;
                isPlayingSource1 = true;
                Debug.Log("Iniciando música de alerta directamente");
            }
            return;
        }

        if (isFading || musicSource1.clip == alertMusic || musicSource2.clip == alertMusic)
        {
            return;
        }

        isInAlertState = true;
        StartCoroutine(FadeMusic(alertMusic));
        Debug.Log("Transición a música de alerta");
    }

    private IEnumerator FadeMusic(AudioClip newClip)
    {
        if (isFading)
        {
            yield break; // Evitar múltiples fades
        }

        isFading = true;

        AudioSource currentSource = isPlayingSource1 ? musicSource1 : musicSource2;
        AudioSource nextSource = isPlayingSource1 ? musicSource2 : musicSource1;

        // Configurar el nuevo AudioSource
        nextSource.clip = newClip;
        nextSource.volume = 0f;
        nextSource.Play();

        // Fade out del source actual y fade in del nuevo
        float timer = 0f;
        float startVolumeCurrent = currentSource.volume;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float t = timer / fadeDuration;
            currentSource.volume = Mathf.Lerp(startVolumeCurrent, 0f, t);
            nextSource.volume = Mathf.Lerp(0f, 1f, t);
            yield return null;
        }

        // Detener el source anterior
        currentSource.Stop();
        currentSource.clip = null;
        currentSource.volume = 0f;
        nextSource.volume = 1f;
        isPlayingSource1 = !isPlayingSource1;
        isFading = false;
    }

    public void SyncMusicWithGameState()
    {
        if (GameManagerETP.Instance == null)
        {
            Debug.LogWarning("GameManagerETP no encontrado, iniciando música pacífica por defecto");
            PlayPacificMusic();
            return;
        }

        // Detener cualquier música previa para evitar solapamientos
        musicSource1.Stop();
        musicSource2.Stop();
        musicSource1.clip = null;
        musicSource2.clip = null;
        musicSource1.volume = 0f;
        musicSource2.volume = 0f;

        if (GameManagerETP.Instance.GetEnemiesInAlertOrThreat() > 0)
        {
            isInAlertState = true;
            PlayAlertMusic();
        }
        else
        {
            isInAlertState = false;
            PlayPacificMusic();
        }
        Debug.Log($"Sincronizando música, enemigos en Alerta/Amenaza: {GameManagerETP.Instance.GetEnemiesInAlertOrThreat()}");
    }
}