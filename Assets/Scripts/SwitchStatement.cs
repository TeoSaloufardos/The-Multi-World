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

    private void cheatCodes() //Υπάρχουν Cheat Codes που αναλογα το κουμπι κάνουν και τις κατάλληλες λειτουργιες. Παράδειγμα το L παει κατευθειαν τον παικτη στην επομενη πιστα, το C κανει το object να χανει τα colliders του που εχει και να το κανει να περναει μεσα απο objects και τελος το p κανει το object να μπορει να κανει collide χωρις ομως να το επηρεαζει στο να σκοτώνει. 
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
    }


    private void Start()// κατα την εναρξη απενεργοποιειται το inputfield και λαμβανονται τα AudioComponents.
    {
        audioSource = GetComponent<AudioSource>();
        inputField.interactable = false;
        inputField.gameObject.SetActive(false);
    }

    public void OnCollisionEnter(Collision collision)// Εδω είναι ο πυρηνας των μηχανισμων που αναλογα με το collide που κανει ο παικτης γινονται και οι καταλληλες εκτελεσεις των μεθοδων.
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

    public void StopMusicAndMovement()//Σταματαει την μουσικη (crash/push κτλ) και απενεργοποιειται η κινηση.
    {
        AudioSource audio = gameObject.GetComponent<AudioSource>();
        audio.Stop();
        Movement movementComponent = GetComponent<Movement>();
        movementComponent.enabled = false;
    }
    public void Death()// Με την εκτελεση του ο παικτης επαναφερεται στην αρχη της πιστας.
    {

        int currentStage = SceneManager.GetActiveScene().buildIndex;// λαμβανεται ο αριθμος του δεδομενου επιπεδου που ειναι ο παικτης
        SceneManager.LoadScene(currentStage);
    }

    public void NextStage()// Κατα την εκτελεση του ο παικτης παει στην επομενη πιστα ή στην αρχικη οθονη αναλογα εαν βρισκεται απο το 0 εως το 9 ή στο 10.
    {


        int currentStage = SceneManager.GetActiveScene().buildIndex;// λαμβανεται ο αριθμος του δεδομενου επιπεδου που ειναι ο παικτης
        int nextStage = currentStage + 1;

        if (nextStage == SceneManager.sceneCountInBuildSettings)// Λαμβάνεται ο αριθμος όλων των Scenes και ελεγχεται εαν το Stage που βρισκεται ο παικτης είναι το τελευταιο εαν ειναι τοτε το μεταφερει στην αρχη.
        {
            nextStage = 0;
        }

        SceneManager.LoadScene(nextStage);


    }

    public void PauseTheGameAndAsk()// Κατα την εκτελεση αυτης της μεθοδου εμφανιζεται ενα μηνυμα στον παικτη με την πραξη που καλειται να εκτελεσει.
    {
        isTransitioning = true;
        int currentStage = SceneManager.GetActiveScene().buildIndex;


        Rigidbody rigidbody = gameObject.GetComponent<Rigidbody>();
        int firstRandomNumber = currentStage; //Για την ληψη του πρωτου αριθμου (θεματικου αριθμου γίνεται φόρτωση της αριθμησης του επιπεδου, πχ το Scene1 έχει index 1 κτλ οπου στο Scene2(stage2) θα γινει πολλαπλασιασμος 2χ1 ή 2χ2 ή 2χ5 ή 2χ7 .... 2χ10.
        int secondRandomNumber = UnityEngine.Random.Range(1, 10); // παραγεται ενας τυχαιος αριθμος απο το 1 εως το 10
        if (currentStage != SceneManager.sceneCountInBuildSettings && !nextStage)//ετοιμαζεται το κειμενο και γινονται οι καταλληλες εκχωρησεις σε λογικες μεταβλητες για να αναλαβουν αλλοι μεθοδοι τις υπολοιπες λειτουργιες. Για παραδειγμα το nextStage οταν λαβει τιμη true δεν μπορει να ξανα εκτελεστει αυτη εδω η μεθοδος ενω με την μεταβλητη readyForAnswer δινεται η δυνατοτητα στο ελεγχο που εκτελει η μεθοδος getPlayersAnswer() να εφαρμοσει τις λειτουργιες της.
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
    public void getPlayersAnswer()// Με την εκτελεση της ελεγχεται η εισαγωγη του παικτη και εαν ειναι integer τοτε γινεται επιπλεον ελεγχος εαν ειναι ο αριθμος που ψαχνουμε εαν οχι του εμφανιζεται ενα μηνυμα λαθους με την σωστη απαντηση και προχωραει στην επομενη πιστα. Στην περιπτωση που δεν δωσει integer αριθμο του εμφανιζεται αντιστοιχο μηνυμα σφαλματος που τον προτρεπει να κανει εισαγωγη integer αριθμου.
    {
        if (!answerHasBeenReceived && readyForAnswer)
        {
            if (Input.GetKeyDown(KeyCode.Return))
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



