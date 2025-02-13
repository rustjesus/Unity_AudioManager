One script for Unity audio managment to RULE THEM ALL!
Add to your scene (or game canvas) and call the manager via FindAnyObjectByType (FindObjectByType if not Unity6+) then call the PlayEffectClipAtPoint or PlayMusicClipAtPoint.
NOTE: this script is for spacial sounds, if you want it to be 2D (not affected by distance) then set "blend" to 0
Example code:


    [SerializeField] private AudioClip shootClip;
    [Range(0, 2)] public float shootVol = 0.5f; // Maximum distance of the raycast
    [Range(0,2)] public float shootPitch = 1f; // Maximum distance of the raycast
    private AudioManager audioManager;
    private void Awake()
    {
        audioManager = FindAnyObjectByType<AudioManager>();
    }

    void Shoot()
    {
        if (audioManager != null && shootClip != null)
        {
            audioManager.PlayEffectClipAtPoint(shootClip, transform.position, shootVol, shootPitch);
        }
    }

