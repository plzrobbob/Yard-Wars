using UnityEngine;

public class MenuAudioManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] AudioClip confirm;
    [SerializeField] AudioClip cancel;
    [SerializeField] AudioClip select;

    private AudioSource source;

    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    public void PlaySoundOnce(AudioClip clip)
    {
        Debug.Log("Playing Clip");
        source.PlayOneShot(clip);
    }

    public void Confirm()
    {
        source.PlayOneShot(confirm);
    }

    public void Cancel()
    {
        source.PlayOneShot(cancel);
    }

    public void Select()
    {
        source.PlayOneShot(select);
    }
}
