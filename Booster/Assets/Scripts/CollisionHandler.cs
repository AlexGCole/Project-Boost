using UnityEngine;
using UnityEngine.SceneManagement;
public class CollisionHandler : MonoBehaviour
{
    float levelDelay = 1f;

    bool isTransitioning = false;
    bool collisionsDisabled = false;
    AudioSource audioSource;

    [SerializeField] AudioClip crash;
    [SerializeField] AudioClip success;

    [SerializeField] ParticleSystem crashParticles;
    [SerializeField] ParticleSystem successParticles;



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
        if (Input.GetKey(KeyCode.L))
        {
            NextLevel();
        }
        else if (Input.GetKey(KeyCode.C))
        {
            collisionsDisabled = !collisionsDisabled;
        }

    }

    void OnCollisionEnter(Collision other)
    {

        if (isTransitioning || collisionsDisabled) { return; }

        switch (other.gameObject.tag)
        {
            case "Friendly":
                Debug.Log("This is freindly");
                break;
            case "Finish":
                StartSuccessSequence();
                break;
            default:
                StartCrashSequence();
                break;


        }

    }

    void StartCrashSequence()
    {
        audioSource.Stop();

        audioSource.PlayOneShot(crash);
        crashParticles.Play();

        isTransitioning = true;
        //todo add particle effect upon crash
        stopControls();
        Invoke("ReloadLevel", levelDelay);
    }

    void StartSuccessSequence()
    {
        audioSource.Stop();

        audioSource.PlayOneShot(success);
        successParticles.Play();

        isTransitioning = true;
        stopControls();
        Invoke("NextLevel", levelDelay);
    }

    void stopControls()
    {
        GetComponent<Movement>().enabled = false;
    }

    void NextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }
        SceneManager.LoadScene(nextSceneIndex);


    }
    void ReloadLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }
}
