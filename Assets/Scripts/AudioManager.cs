using System.Runtime.CompilerServices;
using UnityEngine;

[System.Serializable]
public class Sound {
    public string name;

    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume = 0.7f;
    [Range(0.5f, 1.5f)]
    public float pitch = 1.0f;

    [Range(0f, 0.5f)]
    public float volMULT = 0.1f;
    [Range(0f, 0.5f)]
    public float pitMULT = 0.1f;

    public bool loop = false;

    private AudioSource source;

    public bool isMusic = false;

    public void SetSource (AudioSource _source)
    {
        source = _source;
        source.clip = clip;
        source.loop = loop;
    }

    public void Play()
    {
        source.volume = volume * (1 + Random.Range(-volMULT / 2f, volMULT /2f));
        source.pitch = pitch * (1 + Random.Range(-pitMULT / 2f, pitMULT / 2f));
        source.Play();
    }

    public void Stop()
    {
        source.Stop();
    }
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField]
    Sound[] sounds;

    Sound[] music;

    private int music_count = 0;

    public int music_selection = 0;

    public bool killed_by_enemy = false;

    public bool has_jumped = false;

    public bool is_walking = false;

    public DialogueTrigger thing;

    void Awake()
    {
        if (instance != null)
        {
            if (instance != this)
            {
                Destroy(this.gameObject);
            }
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
    }

    void Start()
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            GameObject _go = new GameObject("Sound_" + i + "_" + sounds[i].name);
            _go.transform.SetParent(this.transform);
            sounds[i].SetSource (_go.AddComponent<AudioSource>());

            if (sounds[i].isMusic)
            {
                music_count++;
            }
        }

        music = new Sound[music_count];
        music_count = 0;

        for (int i = 0; i < sounds.Length; i++)
        {

            if (sounds[i].isMusic)
            {
                music[music_count] = sounds[i];
                music_count++;
            }
        }

        PlaySound(music[music_selection].name);

        thing.TriggerDialogue();
    }

    public void PlaySound(string _name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].name == _name)
            {
                sounds[i].Play();
                return;
            }
        }

        // No sound found

        Debug.LogWarning("AudioManager: Sound not found: " + _name);
    }

    public void StopSound(string _name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].name == _name)
            {
                sounds[i].Stop();
                return;
            }
        }

        // No sound found

        Debug.LogWarning("AudioManager: Sound not found: " + _name);
    }

    public void NextTrack()
    {
        StopSound(music[music_selection].name);

        if (music_selection < music_count-1)
        {
            music_selection++;
        }
        else
        {
            music_selection = 0;
        }

        PlaySound(music[music_selection].name);
    }
}
