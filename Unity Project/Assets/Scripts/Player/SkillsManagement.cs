using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillsManagement : MonoBehaviour
{
    SpriteRenderer characterSprite;
    [SerializeField] Dash dashScript;
    [SerializeField] Jump jumpScript;
    public static List<string> skills = new List<string>();
    [SerializeField] float colorAmountToChange = 100;

    void Start()
    {
        characterSprite = gameObject.GetComponent<SpriteRenderer>();
    }

    public void ActivateSkill(string skillName)
    {
        float rColor = 255;
        float gColor = 0;
        float bColor = 255;
        string hashedSkillName = skillName.Substring(0,2);

        // Changement de couleur du perso en fonction du type de skill activé
        switch (hashedSkillName)
        {
            case "j_":
                bColor = characterSprite.color.b - colorAmountToChange;
                break;
            case "d_":
                rColor = characterSprite.color.r - colorAmountToChange;
                break;
            default:
                rColor = 255;
                gColor = 0;
                bColor = 255;
            break;
        }

        // Activation du script correspondant au skill activé
        switch(skillName)
        {
            case "jump":
                rColor = 255;
                bColor = 0;
                jumpScript.enabled = true;
                break;
            case "dash":
                rColor = 255;
                bColor = 255;
                dashScript.enabled = true;
                break;
            // Ajouter les cas des compétences développées
            default:
                break;
        }

        characterSprite.color = new Color(rColor, gColor, bColor);
        skills.Add(skillName);
    }

    public void DeactivateSkill(string skillName)
    {
        switch(skillName)
        {
            case "jump":
                jumpScript.enabled = false;
                break;
            case "dash":
                dashScript.enabled = false;
                break;
            // Ajouter les cas des compétences développées
            default:
                break;
        }
    }
}
