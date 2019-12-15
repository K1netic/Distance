﻿using System.Collections;
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

    [SerializeField] GameObject newSkillParticles;

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
                PopParticle(newSkillParticles, new Color(1,0,0,1));
                break;
            case "d_":
                rColor = characterSprite.color.r - colorAmountToChange;
                PopParticle(newSkillParticles, new Color(0,0,1,1));
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

    void PopParticle(GameObject particleToPop, Color newColor)
    {
        GameObject instantiated = Instantiate(particleToPop,new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, 0), new Quaternion(0,0,0,0));
        instantiated.transform.Rotate(new Vector3(0,0,90),Space.Self);
        instantiated.transform.localScale = new Vector3(1,1,1);
        // instantiated.GetComponent<ParticleSystem>().startColor = color;
        Destroy(instantiated, instantiated.GetComponent<ParticleSystem>().main.duration + instantiated.GetComponent<ParticleSystem>().main.startLifetime.constantMax);
    }
}
