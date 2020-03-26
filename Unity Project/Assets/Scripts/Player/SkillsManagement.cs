using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

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
    [FMODUnity.EventRef]
    public string inputsound;

    float rColor = 1f;
    float gColor = 1f;
    float bColor = 1f;

    void Start()
    {
        characterSprite = gameObject.GetComponent<SpriteRenderer>();
    }

    public void ActivateSkill(string skillName)
    {

        string hashedSkillName = skillName.Substring(0,2);

        // Character color change depending on type of skill activated
        switch (hashedSkillName)
        {
            case "j_":
                rColor = characterSprite.color.r - colorAmountToChange;
                // Greenish for jump related skills
                StartCoroutine(PopNewSkillParticles(new Color(0.5f,1,0,1)));
                break;
            case "d_":
                gColor = characterSprite.color.g - colorAmountToChange;
                // Orange for dash related skills
                StartCoroutine(PopNewSkillParticles(new Color(1,0.5f,0,1)));
                break;
            default:
                rColor = 1f;
                gColor = 1f;
                bColor = 1f;
            break;
        }

        // Script activation depending on the skill activated
        switch(skillName)
        {
            case "jump":
                // Color set when the player gains the jump ability
                rColor = 0.5f;
                gColor = 1;
                bColor = 0;
                jumpScript.enabled = true;
                StartCoroutine(PopNewSkillParticles(new Color(0.5f,1,0,1)));
                break;
            case "dash":
                // Color set when the player gains the dash ability
                rColor = 1f;
                gColor = 1f;
                bColor = 0;
                dashScript.enabled = true;
                StartCoroutine(PopNewSkillParticles(new Color(1,0.5f,0,1)));
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
            default:
                break;
        }

        // Adding the newly gained script to the player script list
        skills.Add(skillName);
        // Skill gain management (sound, animations, particles...)
        FMODUnity.RuntimeManager.PlayOneShot(inputsound);
        characterSprite.color = new Color(rColor, gColor, bColor);
        PlayerMovement.lockMovement = true;
        gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
        gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        StartCoroutine(CancelVibration (Vibrations.PlayVibration("NewSkillGain")));
        StartCoroutine(UnlockMove());
    }

    // Lock skill usage to avoid player from using them in non-appropriate sections
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
                heavyDashScript.enabled = true;
                break;
            // case "d_blink":
            //     blinkScript.enabled = false;
            //     break;
            default:
                break;
        }
    }

    IEnumerator PopNewSkillParticles(Color color)
    {
        PopParticle(newSkillParticles.transform.GetChild(0).gameObject, color);
        yield return new WaitForSeconds(0.4f);
        PopParticle(newSkillParticles.transform.GetChild(1).gameObject, color);
    }

    void PopParticle(GameObject particleToPop, Color newColor)
    {
        GameObject instantiated = Instantiate(particleToPop,new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, 0), new Quaternion(0,0,0,0));
        instantiated.transform.Rotate(new Vector3(0,0,90),Space.Self);
        instantiated.transform.localScale = new Vector3(1,1,1);
        instantiated.GetComponent<ParticleSystem>().startColor = newColor;
        Destroy(instantiated, instantiated.GetComponent<ParticleSystem>().main.duration + instantiated.GetComponent<ParticleSystem>().main.startLifetime.constantMax);
    }

    public IEnumerator CancelVibration(float delay)
	{
		yield return new WaitForSeconds (delay);
		GamePad.SetVibration(0,0,0);
        
	}

    IEnumerator UnlockMove()
    {
        yield return new WaitForSeconds(0.4f);
        PlayerMovement.lockMovement = false;
        gameObject.GetComponent<Rigidbody2D>().isKinematic = false;
    }
}
