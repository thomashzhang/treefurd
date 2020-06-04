using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RocketTree : MonoBehaviour
{
    [SerializeField] float rotationSpeed = 200f;
    [SerializeField] float thrustSpeed = 2000f;
    [SerializeField] AudioClip winAudioClip;
    [SerializeField] AudioClip loseAudioClip;
    [SerializeField] AudioClip mainEngine;

    [SerializeField] ParticleSystem winParticles;
    [SerializeField] ParticleSystem loseParticles;
    [SerializeField] ParticleSystem mainEngineParticles;

    [SerializeField] float levelLoadDelay = 1f;

    Rigidbody rigidBody;
    AudioSource audioSource;
    State state = State.Alive;

    enum State { Alive, Dying, Trancending }
    // Start is called before the first frame update
    void Start()
    {
        rigidBody = gameObject.GetComponent<Rigidbody>();
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessInput();
    }

    private void ProcessInput()
    {
        if (state == State.Alive)
        {
            ProcessThrust();
            ProcessRotate();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (state != State.Alive)
        {
            return;
        }
        if (collision.gameObject.tag == "Finish")
        {
            StartSuccessSequence();
        }
        else if (collision.gameObject.tag == "Friendly")
        {

        }
        else
        {
            StartDeathSequence();
        }
    }

    private void StartDeathSequence()
    {
        state = State.Dying;
        audioSource.Stop();
        audioSource.PlayOneShot(loseAudioClip);
        loseParticles.Play();
        Invoke(nameof(ReloadCurrentLevel), levelLoadDelay);
    }

    private void StartSuccessSequence()
    {
        state = State.Trancending;
        audioSource.Stop();
        audioSource.PlayOneShot(winAudioClip);
        winParticles.Play();
        Invoke(nameof(LoadNextScene), levelLoadDelay);
    }

    private void ReloadCurrentLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private void ProcessRotate()
    {
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            transform.Rotate(-Vector3.forward * rotationSpeed * Time.deltaTime);
        }
    }

    private void ProcessThrust()
    {
        rigidBody.freezeRotation = true;
        if (Input.GetKey(KeyCode.Space))
        {
            rigidBody.AddRelativeForce(Vector3.up * thrustSpeed * Time.deltaTime);
            if (!audioSource.isPlaying)
            {
                audioSource.PlayOneShot(mainEngine);
            }
            mainEngineParticles.Play();
        }
        else
        {
            audioSource.Stop();
            mainEngineParticles.Stop();
        }
        rigidBody.freezeRotation = false;
    }
}
