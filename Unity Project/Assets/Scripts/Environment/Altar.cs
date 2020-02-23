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

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        //Loading text
        dataArray = CSVReader.SplitCsvGrid(csv.text);
        fullText = CSVReader.GetTextWithKey(dataArray, dialogueToLoadKey); 
        text.text = fullText;
        // text.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        if (displayInteraction)
        {
            if (Input.GetButtonDown("Interact") && !interacted)
            {
                StartCoroutine(TextActivating());
                interacted = true;
            }
        }

        if (textDisplayed)
        {
            // if(!textTyped)
            // {
            //     StartCoroutine(TypeText());
            //     textTyped = true;
            // }
            if (Input.GetButtonDown("Jump"))
                closeText = true;
        }
    }

    IEnumerator TextActivating()
    {
        interactionButton.SetActive(false);
        BlockPlayerActions();
        // zoom camera / add black lines
        Parchment.SetActive(true);
        textDisplayed = true;
        yield return new WaitUntil(() => closeText == true);
        Parchment.SetActive(false);
        interacted = false;
        textDisplayed = false;
        closeText = false;
        // textTyped = false;
        UnblockPlayerActions();
    }

    // IEnumerator TypeText () 
    // {
	// 	foreach (char letter in fullText.ToCharArray()) 
    //     {
    //         if (letter == '.') yield return new WaitForSeconds(pauseBeforeNextSentence);
	// 		text.text += letter;
	// 		yield return new WaitForSeconds (letterPause);
	// 	}
	// }

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
        displayInteraction = false;
    }
}
