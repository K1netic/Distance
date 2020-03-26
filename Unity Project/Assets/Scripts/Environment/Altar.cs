using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Altar : MonoBehaviour
{
    [SerializeField] GameObject interactionButton;
    [SerializeField] GameObject Parchment;

    // Dialogue management
    [SerializeField] TextAsset csv;
    [SerializeField] Text text;
    [SerializeField] string dialogueToLoadKey;
    string[,] dataArray;
    string fullText;
    bool textDisplayed = false;
    bool closeText = false;
    bool displayInteraction = false;
    bool interacted = false;
    bool textTyped = false;
    GameObject player;

    //Text displaying management
    float letterPause = 0.02f;
    float pauseBeforeNextSentence = 1f;
	string message;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        //Loading text
        dataArray = CSVReader.SplitCsvGrid(csv.text);
        fullText = CSVReader.GetTextWithKey(dataArray, dialogueToLoadKey); 
        text.text = fullText;
    }

    // Update is called once per frame
    void Update()
    {
        if (displayInteraction)
        {
            // Display text if the player presses the interact button
            if (Input.GetButtonDown("Interact") && !interacted)
            {
                StartCoroutine(TextActivating());
                interacted = true;
            }
        }

        if (textDisplayed)
        {
            // Close text if the player presses A / space
            if (Input.GetButtonDown("Jump"))
                closeText = true;
        }
    }

    IEnumerator TextActivating()
    {
        // Display parchment and text
        interactionButton.SetActive(false);
        BlockPlayerActions();
        Parchment.SetActive(true);
        textDisplayed = true;
        yield return new WaitUntil(() => closeText == true);
        // Hide parchment and text
        Parchment.SetActive(false);
        interacted = false;
        textDisplayed = false;
        closeText = false;
        UnblockPlayerActions();
    }

    // Block movement and player skills
    void BlockPlayerActions()
    {
        //Block movements
        PlayerMovement.lockMovement = true;
        //Block skills
        foreach(string skill in SkillsManagement.skills)
        {
            player.GetComponent<SkillsManagement>().LockSkillUse(skill);
        }
        player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }

    // Unblock movement and player skills
    void UnblockPlayerActions()
    {
        //Activate movements
        PlayerMovement.lockMovement = false;
        //Activate skills
        foreach(string skill in SkillsManagement.skills)
        {
            player.GetComponent<SkillsManagement>().UnlockSkillUse(skill);
        }
    }

    // Enable interaction with Altar if the player is nearby
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            displayInteraction = true;
            if (!interacted) interactionButton.SetActive(true);
        }
    }

    // Disable interaction with Altar if the player is too far
    void OnTriggerExit2D(Collider2D other)
    {
        interactionButton.SetActive(false);
        displayInteraction = false;
    }
}
