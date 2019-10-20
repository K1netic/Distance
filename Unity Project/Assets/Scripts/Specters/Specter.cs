using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Specter : MonoBehaviour
{
    [SerializeField] int specterIndex;
    [SerializeField] string associatedSkillName;

    bool displayInteraction = false;
    GameObject player;

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
}
