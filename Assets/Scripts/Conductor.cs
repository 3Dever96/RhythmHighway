using UnityEngine;

public class Conductor : MonoBehaviour
{
    public static Conductor instance; // Allows other scripts to find this easily

    [Header("Audio Setup")]
    public AudioSource musicSource;
    public float songBpm = 120f;
    [Range(1,2)] public int correctiveSubdivider;
    public float firstBeatDelayBeats = 4f; // A 4-beat silent countdown before music starts

    [Header("Real-Time Tracking (Read Only)")]
    public float songPositionInSeconds;
    public float songPositionInBeats;

    private float secPerBeat;
    private float dspTimeSongStart;

    void Awake()
    {
        // Set up a singleton pattern so our spawner can find the beat safely
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        float standardPulse = 60f / songBpm;

        secPerBeat = standardPulse / correctiveSubdivider;

        // Schedule the song to start precisely after the countdown delay
        dspTimeSongStart = (float)AudioSettings.dspTime + (firstBeatDelayBeats * secPerBeat);
        musicSource.PlayScheduled(dspTimeSongStart);
    }

    void Update()
    {
        if (!musicSource.isPlaying && AudioSettings.dspTime < dspTimeSongStart) return;

        // Check the exact millisecond of the audio hardware clock
        songPositionInSeconds = (float)(AudioSettings.dspTime - dspTimeSongStart);

        // Convert the exact seconds into an accurate floating-point beat number
        songPositionInBeats = songPositionInSeconds / secPerBeat;
    }
}
