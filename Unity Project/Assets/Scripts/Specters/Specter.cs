using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    bool displayInteraction = false;
    GameObject player;

    [SerializeField] bool tutorialSpecter = false; 
    public bool testSucceed = false;
    bool closeTest = false;
    bool interacted = false;
    [FMODUnity.EventRef]
    public string inputsoundSpecterTalk;
    [FMODUnity.EventRef]
    public string inputsoundSpecterDisappear;
    float distanceWithPlayer;
    public bool calculateDistance = false;
    float distanceThreshold = 20f;

    void Awake()
    {
        
    }
    void Start()
    {
        if (!tutorialSpecter)
        {
            associatedExit.SetActive(false);
            otherSpecterScript = otherSpecter.GetComponent<Specter>();
            otherSpecterSprite = otherSpecter.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();
        }
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
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
            StartCoroutine(SpecterDisappearance());
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
    }

    IEnumerator SpecterInteraction()
    {
        FMODUnity.RuntimeManager.PlayOneShot(inputsoundSpecterTalk);
        interactionButton.SetActive(false);
        // Animation transfert de compétences
        player.GetComponent<SkillsManagement>().ActivateSkill(associatedSkillName);
        yield return new WaitForSeconds(0.5f);
        bubble.SetActive(true);

        if (!tutorialSpecter)
        {
            associatedExit.SetActive(true);
            otherSpecter.SetActive(false);
            FMODUnity.RuntimeManager.PlayOneShot(inputsoundSpecterDisappear);
        }
    }

    IEnumerator SpecterDisappearance()
    {
        FMODUnity.RuntimeManager.PlayOneShot(inputsoundSpecterDisappear);
        // Disparition du fantôme
        PopParticle(disappearParticles);
        bubble.GetComponent<Animator>().SetBool("disappeared", true);
        gameObject.GetComponent<Animator>().SetBool("disappeared", true);
        yield return new WaitForSeconds(0.15f);
        bubble.SetActive(false);
        yield return new WaitForSeconds(0.85f);
        gameObject.SetActive(false);
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
