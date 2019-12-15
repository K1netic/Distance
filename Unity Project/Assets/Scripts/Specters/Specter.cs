using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Specter : MonoBehaviour
{
    [SerializeField] int specterIndex;
    [SerializeField] string associatedSkillName;
    [SerializeField] GameObject associatedExit;
    [SerializeField] GameObject otherSpecter;
    [SerializeField] GameObject disappearParticles;
    [SerializeField] GameObject interactionButton;
    [SerializeField] GameObject bubble;

    bool displayInteraction = false;
    GameObject player;

    [SerializeField] bool tutorialSpecter = false; 
    public bool testSucceed = false;
    bool interacted = false;

    void Start()
    {
        if (!tutorialSpecter)
            associatedExit.SetActive(false);
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

        if (testSucceed)
        {
            StartCoroutine(SpecterDisappearance());
        }
    }

    IEnumerator SpecterInteraction()
    {
        interactionButton.SetActive(false);
        // Animation transfert de compétences
        player.GetComponent<SkillsManagement>().ActivateSkill(associatedSkillName);
        yield return new WaitForSeconds(0.5f);
        bubble.SetActive(true);

        if (!tutorialSpecter)
        {
            associatedExit.SetActive(true);
            otherSpecter.SetActive(false);
        }
    }

    IEnumerator SpecterDisappearance()
    {
        // Disparition du fantôme
        PopParticle(disappearParticles);
        bubble.GetComponent<Animator>().SetBool("disappeared", true);
        gameObject.SetActive(false);
        yield return new WaitForSeconds(0.15f);
        bubble.SetActive(false);
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            displayInteraction = true;
            player = other.gameObject;
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
