using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] float sceneDelay = 2f;
    [SerializeField] AudioClip deathSfx;
    [SerializeField] AudioClip successSfx;
    [SerializeField] ParticleSystem successParticle;
    [SerializeField] ParticleSystem deathPartickle;
    
    Rigidbody rb;
    AudioSource audioSource;
    float timer = 0.5f;
    float objectRotationRate;

    bool isControllable = true;
    bool isCollidable = true;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.sleepThreshold = 0f;
        }
        audioSource = GetComponent<AudioSource>();
    }

    void Update() 
    {
        DebugKeys();
    }

    void DebugKeys()
    {
        if(Keyboard.current.lKey.wasPressedThisFrame)
        {
            LoadNextLevel();
        }
        else if(Keyboard.current.oKey.wasPressedThisFrame)
        {
            isCollidable = !isCollidable;
        }
    }
    
    void OnCollisionStay(Collision other)
    {
        if (!isControllable || !isCollidable) {return;}      
        OnCollisionSwitches(other);
    }

    void OnCollisionSwitches(Collision other)
    {
        objectRotationRate = GetComponent<Transform>().localRotation.eulerAngles.z;
        if (objectRotationRate > -1 && objectRotationRate < 1)
        {
            objectRotationRate = 0;
        }
        switch (other.gameObject.tag)
        {
            case "Friendly":
                break;
            case "Finish":
                timer -= Time.deltaTime;
                if (Math.Abs(objectRotationRate) == 0)
                {
                    if (timer < 0)
                    {
                        StartSuccessSequence();
                    }
                }
                else
                {
                    timer = 0.5f;
                }
                break;
            case "Fuel":
                break;
            default:
                StartCrashSequence();
                break;
        }
    }

    void StartSuccessSequence()
    {
        isControllable = false;
        audioSource.Stop();
        successParticle.Play();
        audioSource.PlayOneShot(successSfx);  
        GetComponent<Movement>().enabled = false;     
        Invoke("LoadNextLevel", sceneDelay);
    }

    void StartCrashSequence()
    {
        isControllable = false;
        audioSource.Stop();
        deathPartickle.Play();
        audioSource.PlayOneShot(deathSfx);
        rb.angularVelocity = new Vector3(1,1,1);
        GetComponent<Movement>().enabled = false;
        Invoke("ReloadLevel", sceneDelay);        
    }

    void ReloadLevel()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentScene);
    }

    void LoadNextLevel()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        int nextScene = currentScene + 1;
        if(nextScene == SceneManager.sceneCountInBuildSettings)
        {
            nextScene = 0;
        }           
        SceneManager.LoadScene(nextScene);
    }
}
