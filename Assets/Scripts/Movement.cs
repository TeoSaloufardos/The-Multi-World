using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    
    Rigidbody myRigidBody;
    AudioSource myAudioSource;
    [SerializeField] float speed = 100.0f;
    [SerializeField] float rotationSpeed = 100.0f;
    [SerializeField] AudioClip audioClip;
    [SerializeField] ParticleSystem movingParticle;
    [SerializeField] ParticleSystem rightPushParticle;
    [SerializeField] ParticleSystem leftPushParticle;
   

    void Start()
    {
        myRigidBody = GetComponent<Rigidbody>();
        myAudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessThrust();
        ProcessRotation();
    }

    private void ProcessThrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            myRigidBody.AddRelativeForce(Vector3.up * speed * Time.deltaTime );
            if (!myAudioSource.isPlaying)
            {
                movingParticle.Play();
                myAudioSource.PlayOneShot(audioClip);
            }
        }
        else
        {
            movingParticle.Stop();
            myAudioSource.Stop();
        }
        
    }

    private void ProcessRotation()
    {
        if (Input.GetKey(KeyCode.A))
        {
            rotateTheObject(rotationSpeed);
            if (!rightPushParticle.isPlaying)
            {
                rightPushParticle.Play();
            }

        }
        else if (Input.GetKey(KeyCode.D))
        {
            rotateTheObject(-rotationSpeed);
            if (!leftPushParticle.isPlaying)
            {
                leftPushParticle.Play();
            }
        }
        else
        { 
            rightPushParticle.Stop();
            leftPushParticle.Stop();
        }
    }

    private void rotateTheObject(float rotation)
    {
        myRigidBody.freezeRotation = true;
        transform.Rotate(Vector3.forward * rotation * Time.deltaTime);
        myRigidBody.freezeRotation = false;

    }

}
