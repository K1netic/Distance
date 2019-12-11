using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillsManagement : MonoBehaviour
{
    SpriteRenderer characterSprite;

    // Jump scripts
    [SerializeField] Jump jumpScript;
    [SerializeField] WallJump wallJumpScript;
    // [SerializeField] DoubleJump doubleJumpScript;

    // Dash scripts
    [SerializeField] Dash dashScript;
    [SerializeField] HeavyDash heavyDashScript;
    // [SerializeField] Blink blinkScript;

    public static List<string> skills = new List<string>();
    float colorAmountToChange = 0.25f;

    void Start()
    {
        characterSprite = gameObject.GetComponent<SpriteRenderer>();
    }

    public void ActivateSkill(string skillName)
    {
        float rColor = 1f;
        float gColor = 0.5f;
        float bColor = 1f;
        string hashedSkillName = skillName.Substring(0,2);

        // Changement de couleur du perso en fonction du type de skill activé
        switch (hashedSkillName)
        {
            case "j_":
                bColor = characterSprite.color.b - colorAmountToChange;
                break;
            case "d_":
                rColor = characterSprite.color.r - colorAmountToChange;
                Debug.Log("ddddd");
                break;
            default:
                rColor = 1f;
                gColor = 0.5f;
                bColor = 1f;
            break;
        }

        // Activation du script correspondant au skill activé
        switch(skillName)
        {
            case "jump":
                rColor = 1;
                bColor = 0.5f;
                jumpScript.enabled = true;
                break;
            case "dash":
                rColor = 1f;
                bColor = 1f;
                dashScript.enabled = true;
                break;
            case "j_wallJump":
                wallJumpScript.enabled = true;
                break;
            // case "j_doubleJump":
            //     doubleJumpScript.enabled = true;
            //     break;
            case "d_heavyDash":
                heavyDashScript.enabled = true;
                break;
            // case "d_blink":
            //     blinkScript.enabled = true;
            //     break;
            // Ajouter les cas des compétences développées
            default:
                break;
        }

        characterSprite.color = new Color(rColor, gColor, bColor);
        skills.Add(skillName);
    }

    public void LockSkillUse(string skillName)
    {
        switch(skillName)
        {
            case "jump":
                jumpScript.enabled = false;
                break;
            case "dash":
                dashScript.enabled = false;
                break;
            case "j_wallJump":
                dashScript.enabled = false;
                break;
            // case "j_doubleJump":
            //     doubleJumpScript.enabled = false;
            //     break;
            case "d_heavyDash":
                heavyDashScript.enabled = false;
                break;
            // case "d_blink":
            //     blinkScript.enabled = false;
            //     break;
            default:
                break;
        }
    }

    public void UnlockSkillUse(string skillName)
    {
        switch(skillName)
        {
            case "jump":
                jumpScript.enabled = true;
                break;
            case "dash":
                dashScript.enabled = true;
                break;
            case "j_wallJump":
                dashScript.enabled = true;
                break;
            // case "j_doubleJump":
            //     doubleJumpScript.enabled = false;
            //     break;
            case "d_heavyDash":
                heavyDashScript.enabled = false;
                break;
            // case "d_blink":
            //     blinkScript.enabled = false;
            //     break;
            default:
                break;
        }
    }
}
