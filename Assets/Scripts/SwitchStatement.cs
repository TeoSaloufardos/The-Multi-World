using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchStatement : MonoBehaviour
{
    [SerializeField] float delay = 1.5f;
    [SerializeField] AudioClip crashSound;
    [SerializeField] AudioClip successLanding;

    [SerializeField] ParticleSystem crashParticle;
    [SerializeField] ParticleSystem successParticle;


    AudioSource audioSource;
    [SerializeField] TMP_Text message;
    [SerializeField] TMP_Text warningMessage;
    [SerializeField] TMP_InputField inputField;

    int playersAnswer = -1;
    int correctAnswer;
    bool isTransitioning = false;
    bool cHasBeenPressed = false;
    bool collideWithoutCrash = false;
    bool nextStage = false;
    bool answerHasBeenReceived = false;
    bool readyForAnswer = false;



    void Update()
    {
        cheatCodes();
        getPlayersAnswer();
    }

    private void cheatCodes()
    {
        if (Input.GetKey(KeyCode.L))
        {
            NextStage();
        }
        else if (Input.GetKey(KeyCode.C))
        {
            BoxCollider boxCollider = GetComponent<BoxCollider>();
            CapsuleCollider capsuleCollider = GetComponent<CapsuleCollider>();
            if (cHasBeenPressed == false)
            {
                boxCollider.enabled = false;
                capsuleCollider.enabled = false;
                cHasBeenPressed = true;
            }
            else if (cHasBeenPressed == true)
            {
                boxCollider.enabled = true;
                capsuleCollider.enabled = true;
                cHasBeenPressed = false;
            }
        }
        else if (Input.GetKey(KeyCode.P))
        {
            collideWithoutCrash = !collideWithoutCrash;
        }
        else if (Input.GetKey(KeyCode.M))
        {
            Time.timeScale = 0f;
        }
    }


    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        inputField.interactable = false;
        inputField.gameObject.SetActive(false);
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (isTransitioning || collideWithoutCrash) { return; }

        switch (collision.gameObject.tag)
        {
            case ("Obstacle"):
                StopMusicAndMovement();
                isTransitioning = true;
                Invoke("Death", delay);
                audioSource.PlayOneShot(crashSound);
                crashParticle.Play();
                break;
            case ("Finish"):
                PauseTheGameAndAsk();
                break;
            default:

                break;
        }
    }

    public void StopMusicAndMovement()
    {
        AudioSource audio = gameObject.GetComponent<AudioSource>();
        audio.Stop();
        Movement movementComponent = GetComponent<Movement>();
        movementComponent.enabled = false;
    }
    public void Death()
    {

        int currentStage = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentStage);
    }

    public void NextStage()
    {


        int currentStage = SceneManager.GetActiveScene().buildIndex;
        int nextStage = currentStage + 1;

        if (nextStage == SceneManager.sceneCountInBuildSettings)
        {
            nextStage = 0;
        }

        SceneManager.LoadScene(nextStage);


    }

    public void PauseTheGameAndAsk()
    {
        isTransitioning = true;
        int currentStage = SceneManager.GetActiveScene().buildIndex;


        Rigidbody rigidbody = gameObject.GetComponent<Rigidbody>();
        int firstRandomNumber = currentStage;
        int secondRandomNumber = UnityEngine.Random.Range(1, 10);
        if (currentStage != SceneManager.sceneCountInBuildSettings && !nextStage)
        {
            rigidbody.constraints = RigidbodyConstraints.FreezeAll;
            Debug.Log(firstRandomNumber + " * " + secondRandomNumber);
            message.text = ("Stage 1\r\n\r\nType the correct answer\r\nand then press ENTER\r\n\r\n" + firstRandomNumber.ToString() + " x " + secondRandomNumber.ToString() + " = ?");
            nextStage = true;

            correctAnswer = firstRandomNumber * secondRandomNumber;
            inputField.interactable = true;
            inputField.gameObject.SetActive(true);
            readyForAnswer = true;
        }

        audioSource.PlayOneShot(successLanding);
        successParticle.Play();

    }
    public void getPlayersAnswer()
    {
        if (!answerHasBeenReceived && readyForAnswer)
        {
            if (Input.GetKeyDown(KeyCode.Return))//να βαλω τον ελεγχο στο update οχι εδω.
            {
                string playersInput = inputField.text;
                if (int.TryParse(playersInput, out playersAnswer))
                {
                    if (correctAnswer == playersAnswer)
                    {
                        successParticle.Play();
                        audioSource.PlayOneShot(successLanding);
                        Invoke("NextStage", 2f);
                        
                    }
                    else
                    {
                        warningMessage.text = "Your answer is wrong. The correct answer is: " + correctAnswer;
                        Invoke("NextStage", 5f);
                        inputField.interactable = false;
                        inputField.gameObject.SetActive(false);
                    }

                }
                else
                {
                    warningMessage.text = "Please insert ONLY integer numbers";
                }
            }
        }
    }




}



