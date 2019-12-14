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

    bool displayInteraction = false;
    GameObject player;

    [SerializeField] bool tutorialSpecter = false; 

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
            // Afficher explication bouton à appuyer
            if (Input.GetButtonDown("Interact"))
            {
                // Afficher bulles de dialogue
                // Animation transfert de compétences
                player.GetComponent<SkillsManagement>().ActivateSkill(associatedSkillName);
                // Disparition du fantôme
                PopParticle(disappearParticles);
                if (!tutorialSpecter)
                {
                    associatedExit.SetActive(true);
                    otherSpecter.SetActive(false);
                }
                gameObject.SetActive(false);
            }
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            displayInteraction = true;
            player = other.gameObject;
        }
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
