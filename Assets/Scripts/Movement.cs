using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [SerializeField] InputAction thrust;
    [SerializeField] InputAction rotation;
    [SerializeField] AudioClip mainEngine;
    [SerializeField] float thrustStrength = 1000f;
    [SerializeField] float rotationStrength = 200f;
    [SerializeField] ParticleSystem mainThruster;
    [SerializeField] ParticleSystem leftThruster;
    [SerializeField] ParticleSystem rightThruster;


    Rigidbody rb;
    AudioSource audioSource;
    bool collisionCheck;

    void OnEnable() 
    {
        thrust.Enable();
        rotation.Enable();
    }

    private void Start() 
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    void OnCollisionEnter(Collision collision)
    {
        collisionCheck = true;
    }

    void OnCollisionExit(Collision other) 
    {
        collisionCheck = false;
    }

    private void FixedUpdate() 
    {
        ThrustAction();
        if(collisionCheck)
        {
            TorqueRotateAction();
        }
        else if(collisionCheck == false)
        {
            TransformRotateAction();
        }       
    }

    private void ThrustAction()
    {
        if(thrust.IsPressed() == true)
        {
            rb.AddRelativeForce(Vector3.up * thrustStrength * Time.fixedDeltaTime);            
            if(!audioSource.isPlaying)
            {
                audioSource.PlayOneShot(mainEngine);
            }
            if(!mainThruster.isPlaying)
            {
                mainThruster.Play();    
            }            
        }
        else
        {
            mainThruster.Stop();
            audioSource.Stop();
        } 
    }

    private void TransformRotateAction()
    {
        rb.angularVelocity = new Vector3(0,0,0);
        float rotationInput = rotation.ReadValue<float>();
        if(rotationInput < 0)
        {
            rightThruster.Play();
            transform.Rotate(Vector3.forward * rotationStrength * Time.fixedDeltaTime);
        }
        else if(rotationInput > 0)
        {
            leftThruster.Play();
            transform.Rotate(Vector3.back * rotationStrength * Time.fixedDeltaTime);
        }
        else
        {
        leftThruster.Stop(); 
        rightThruster.Stop();
        }        
    }
    private void TorqueRotateAction()
    {
        float rotationInput = rotation.ReadValue<float>();
        if(rotationInput < 0)
        {
            rightThruster.Play();
            rb.AddTorque(Vector3.forward * rotationStrength * Time.fixedDeltaTime * 4f);
        }
        else if(rotationInput > 0)
        {
            leftThruster.Play();
            rb.AddTorque(Vector3.back * rotationStrength * Time.fixedDeltaTime * 4f);
        }
        else
        {
        leftThruster.Stop(); 
        rightThruster.Stop();
        }
    }
}