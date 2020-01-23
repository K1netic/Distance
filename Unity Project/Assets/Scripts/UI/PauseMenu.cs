using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class PauseMenu : MonoBehaviour
{
	GameObject pauseMenu;
	GameObject itemSelected;

    GameObject player;

    void Awake()
    {
        pauseMenu = GameObject.Find("PauseScreen");
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Start()
    {
        pauseMenu.SetActive(false);
    }

    void Update()
    {
        if (Input.GetButtonDown("Pause") && !pauseMenu.activeSelf)
        {
            OpenPauseMenu ();
        }

        else if ((Input.GetButtonDown("Pause") || Input.GetButtonDown("Cancel")) && pauseMenu.activeSelf) 
        {
            ClosePauseMenu ();
        }
    }

    void OpenPauseMenu()
	{
		FreezePlayer ();
		CancelAllVibrations ();
		Time.timeScale = 0;
		pauseMenu.SetActive (true);
	}

	public void ClosePauseMenu()
	{
		Time.timeScale = 1;
		pauseMenu.SetActive (false);
		UnfreezePlayer ();
	}

    void FreezePlayer()
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

    void UnfreezePlayer()
	{
		//Activer les mouvements du joueur
        PlayerMovement.lockMovement = false;
        //Activer l'utilisation des skills du joueur
        foreach(string skill in SkillsManagement.skills)
        {
            player.GetComponent<SkillsManagement>().UnlockSkillUse(skill);
        }
	}

	void CancelAllVibrations()
	{
	    GamePad.SetVibration(0,0,0);
	}


}
