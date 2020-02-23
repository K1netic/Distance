using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Specter : MonoBehaviour
{
    [SerializeField] int specterIndex;
    [SerializeField] string associatedSkillName;
    [SerializeField] GameObject associatedExit;
    [SerializeField] GameObject otherSpecter;
    Specter otherSpecterScript;
    SpriteRenderer otherSpecterSprite;
    [SerializeField] GameObject disappearParticles;
    [SerializeField] GameObject interactionButton;
    [SerializeField] GameObject bubble;
    [SerializeField] GameObject[] bubbleButtons;

    bool displayInteraction = false;
    GameObject player;

    [SerializeField] bool tutorialSpecter = false; 
    public bool testSucceed = false;
    bool closeTest = false;
    bool interacted = false;
    [FMODUnity.EventRef] public string inputsoundSpecterTalk;
    [FMODUnity.EventRef] public string inputsoundSpecterDisappear;
    float distanceWithPlayer;
    public bool calculateDistance = false;
    float distanceThreshold = 20f;

    // Dialogue management
    [SerializeField] TextAsset csv;
    [SerializeField] Text text;
    [SerializeField] string dialogueToLoadKey;
    string[,] dataArray;
    string fullText;
    string[] textLines;
    int lineIndex = 0;
    bool dialogueOver = false;
    bool dialogueStarted = false;

    CinematicBars cinematicBars;

    void Start()
    {
        cinematicBars = FindObjectOfType<CinematicBars>();
        if (bubbleButtons != null) 
        {
            foreach(GameObject button in bubbleButtons)
            {
                button.SetActive(false);
            }
        }

        if (!tutorialSpecter)
        {
            associatedExit.SetActive(false);
            otherSpecterScript = otherSpecter.GetComponent<Specter>();
            otherSpecterSprite = otherSpecter.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();
        }
        player = GameObject.FindGameObjectWithTag("Player");

        //Loading text
        dataArray = CSVReader.SplitCsvGrid(csv.text);
        fullText = CSVReader.GetTextWithKey(dataArray, dialogueToLoadKey); 
        textLines = CSVReader.SplitCsvLine(fullText);
        text.text = textLines[lineIndex];
    }

    void Update()
    {
        if (displayInteraction)
        {
            if (Input.GetButtonDown("Interact") && !interacted)
            {
                StartCoroutine(SpecterInteraction());
                interacted = true;
            }
        }

        if (testSucceed && !closeTest)
        {
            // StartCoroutine(SpecterDisappearance());
            transform.position = new Vector3(0,500,0);
            closeTest = true;
        }

        if (!tutorialSpecter && calculateDistance)
        {
            distanceWithPlayer = Vector2.Distance(player.transform.position, gameObject.transform.position);
            if (distanceWithPlayer <= distanceThreshold)
            {
                // otherSpecterSprite.color = new Color(otherSpecterSprite.color.r, otherSpecterSprite.color.g, otherSpecterSprite.color.b, distanceWithPlayer/distanceThreshold);
                Color newColor = otherSpecterSprite.color;
                newColor.r = distanceWithPlayer/distanceThreshold;
                newColor.g = distanceWithPlayer/distanceThreshold;
                newColor.b = distanceWithPlayer/distanceThreshold;
                newColor.a = distanceWithPlayer/distanceThreshold;
                otherSpecterSprite.color = newColor;
            }
        }

        if (dialogueStarted)
        {
            DisplayLineByLine();
        }
    }

    IEnumerator SpecterInteraction()
    {
        interactionButton.SetActive(false);
        BlockPlayerActions();
        cinematicBars.Show(150, 0.3f);
        bubble.SetActive(true);
        dialogueStarted = true;
        yield return new WaitUntil(() => dialogueOver == true);
        cinematicBars.Hide(0.3f);

        // Animation transfert de compétences
        player.GetComponent<SkillsManagement>().ActivateSkill(associatedSkillName);
        yield return new WaitForSeconds(0.5f);
        UnblockPlayerActions();
        if (bubbleButtons != null) 
        {
            foreach(GameObject button in bubbleButtons)
            {
                button.SetActive(true);
            }
        }

        if (!tutorialSpecter)
        {
            associatedExit.SetActive(true);
            otherSpecter.SetActive(false);
            FMODUnity.RuntimeManager.PlayOneShot(inputsoundSpecterDisappear);
        }
    }

    // The following code works in Editor but no on the build for mystical reasons

    // IEnumerator SpecterDisappearance()
    // {
    //     FMODUnity.RuntimeManager.PlayOneShot(inputsoundSpecterDisappear);
    //     // Disparition du fantôme
    //     PopParticle(disappearParticles);
    //     // bubble.GetComponent<Animator>().SetBool("disappeared", true);
    //     // gameObject.GetComponent<Animator>().SetBool("disappeared", true);

    //     yield return new WaitForSeconds(0.15f); //0.15f
    //     bubble.SetActive(false);
    //     gameObject.GetComponent<SpriteRenderer>().enabled = false;
    //     yield return new WaitForSeconds(0.85f);
    //     // gameObject.SetActive(false);
    // }

    void DisplayLineByLine()
    {
        if (Input.GetButtonDown("Jump"))
        {
            FMODUnity.RuntimeManager.PlayOneShot(inputsoundSpecterTalk);
            // Display next line of dialogue
            if (lineIndex < textLines.Length - 1)
            {
                lineIndex ++;
                text.text = textLines[lineIndex];
            }

            // If there are no more dialogue lines, erase all text
            else
            {
                text.text = "";
                dialogueOver = true;
                dialogueStarted = false;
            }
        }
    }

    void BlockPlayerActions()
    {
        //Bloquer les mouvements du joueur 
        PlayerMovement.lockMovement = true;
        //Bloquer l'utilisation de compétences
        foreach(string skill in SkillsManagement.skills)
        {
            player.GetComponent<SkillsManagement>().LockSkillUse(skill);
        }
        player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }

    void UnblockPlayerActions()
    {
        //Activer les mouvements du joueur
        PlayerMovement.lockMovement = false;
        //Activer l'utilisation des skills du joueur
        foreach(string skill in SkillsManagement.skills)
        {
            player.GetComponent<SkillsManagement>().UnlockSkillUse(skill);
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            displayInteraction = true;
            // player = other.gameObject;
            if (!interacted) interactionButton.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        interactionButton.SetActive(false);
    }

    void PopParticle(GameObject particleToPop)
    {
        GameObject instantiated = Instantiate(particleToPop,new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, 0), new Quaternion(0,0,0,0));
        instantiated.transform.Rotate(new Vector3(0,0,0),Space.Self);
        // instantiated.transform.parent = gameObject.transform;
        instantiated.transform.localScale = new Vector3(1,1,1);
        Destroy(instantiated, instantiated.GetComponent<ParticleSystem>().main.duration + instantiated.GetComponent<ParticleSystem>().main.startLifetime.constantMax);
    }
}
