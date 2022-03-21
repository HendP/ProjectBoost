using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField]
    float levelLoadDelay = 2f;

    [SerializeField]
    AudioClip crashes;

    [SerializeField]
    AudioClip success;

    [SerializeField]
    ParticleSystem successParticles;

    [SerializeField]
    ParticleSystem failedParticles;

    AudioSource audioSource;

    bool isTransitioning = false;
    bool collisionDisabled = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        RespondToDebugKeys();
    }

    void RespondToDebugKeys()
    {
        if(Input.GetKeyDown(KeyCode.L))
        {
            LoadNextLevel();
        }
        else if(Input.GetKeyDown(KeyCode.C))
        {
            collisionDisabled = !collisionDisabled; // toggle collision
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (isTransitioning || collisionDisabled)
        {
            return;
        }

        switch (other.gameObject.tag)
        {
            case "Friendly":
                Debug.Log("This thing is friendly");
                break;
            case "Finish":
                Debug.Log("Congrats, yo, you finished!");
                StartSuccessSequence();
                break;
            case "Fuel":
                Debug.Log("You picked up fuel");
                break;
            default:
                StartCrashSequence();
                break;
        }
    }

    void AudioCrash()
    {
        audioSource.Stop();
        audioSource.PlayOneShot(crashes);
    }

    void AudioSuccess()
    {
        audioSource.Stop();
        audioSource.PlayOneShot(success);
    }

    void ReloadLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    void StartSuccessSequence()
    {
        isTransitioning = true;
        // todo add SFX upon crash
        // to add particle effect upon crash
        successParticles.Play();
        GetComponent<Movement>().enabled = false;
        AudioSuccess();
        Invoke("LoadNextLevel", levelLoadDelay);
    }

    void StartCrashSequence()
    {
        isTransitioning = true;
        // todo add SFX upon crash
        // to add particle effect upon crash
        failedParticles.Play();
        GetComponent<Movement>().enabled = false;
        AudioCrash();
        Invoke("ReloadLevel", levelLoadDelay);
    }

    void LoadNextLevel()
    {
        int nextLevel = SceneManager.GetActiveScene().buildIndex + 1;
        int maxLevel = SceneManager.sceneCountInBuildSettings;
        if (nextLevel == maxLevel)
        {
            nextLevel = 0;
        }
        SceneManager.LoadScene(nextLevel);
    }
}
