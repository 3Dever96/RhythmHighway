using UnityEngine;

public class RhythmSpawner : MonoBehaviour
{
    [Header("Setup")]
    public GameObject dartPrefab;
    public Transform spawnPoint;

    [Header("Timing")]
    public float dartTravelTimeInSeconds = 2.0f;

    [Header("Level Timeline (Type Target Beats Here)")]
    [Tooltip("The exact beats where a dart must hit the player.")]
    public HitBeat[] targetHitBeats;

    private int nextBeatIndex = 0;
    private float travelTimeInBeats;

    void Start()
    {
        // Convert the physical travel seconds into its equivalent musical beat duration
        float secPerBeat = 60f / Conductor.instance.songBpm;
        travelTimeInBeats = dartTravelTimeInSeconds / secPerBeat;
    }

    void Update()
    {
        // Check if we still have obstacles left to spawn in our timeline array
        if (nextBeatIndex < targetHitBeats.Length)
        {
            // The polling check: Target Beat minus the flight time
            float spawnTriggerBeat = targetHitBeats[nextBeatIndex].beat - travelTimeInBeats;

            // Ask the conductor if the audio clock has met or crossed the threshold
            if (Conductor.instance.songPositionInBeats >= spawnTriggerBeat)
            {
                SpawnTrackedDart(targetHitBeats[nextBeatIndex].height);
                nextBeatIndex++; // Advance to monitoring the next beat on our list
            }
        }
    }

    void SpawnTrackedDart(int height)
    {
        // Instantiate the dart at our spawner's location
        GameObject currentDart = Instantiate(dartPrefab, spawnPoint.position + Vector3.up * height, Quaternion.LookRotation(-Vector3.right));
    }
}

[System.Serializable]
public class HitBeat
{
    public float beat;
    [Range(0, 2)] public int height;
}
