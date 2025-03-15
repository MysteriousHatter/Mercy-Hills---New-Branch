using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Rendering.PostProcessing;


/// <summary>
/// This script is used for managing the player death, cutscenes, level specific gameObjects, and some virus bar(Health Bar) effect.
/// Some code may be refactored
/// Methods that might be included:
/// 1. A method to play tutorial level
/// 2. Instances of managing both the UI and Audio Manager for changing scenes or audio related instructions
/// 4. Might move status(health bar) related effects to status bar
/// </summary>
public class GameManager : MonoBehaviour
{


    private MonsterSpawner Spawners; //a reference to the spawner script instead of the GameObject
    private static GameManager _instance;
    [SerializeField] public bool lightsOff { get; set; }

    //public GameState currentState;
    //public static event Action<GameState> OnGameStateChanged;
    public PlayableDirector introCutscene;

    //Relating to Player's death
    public int numOfDeaths;
    Status status;
    public Transform respawnPoint;
    private Transform Player;

    //For Testing purposes
    [SerializeField] public bool enemyChecker;
    private AudioManager playVirusMusic;
    JumpScareSpawner spawner;

    //Cutscenes
    public PlayableDirector Caught;
    [SerializeField] public PlayableDirector restart;
    [SerializeField] public PlayableDirector gameOver;

    [SerializeField] PostProcessProfile m_profile;
    Vignette vignette;
    ColorGrading grading;


    [SerializeField] bool test;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null) { Debug.LogError("GameManager is Null!!!"); }
            return _instance;
        }



    }

    private void Awake()
    {
        Spawners = FindObjectOfType<MonsterSpawner>();
        status = FindObjectOfType<Status>();
        Player = GameObject.Find("Player(CM Prefab)").transform;
        playVirusMusic = FindObjectOfType<AudioManager>();

        m_profile.TryGetSettings(out vignette);
        m_profile.TryGetSettings(out grading);


        _instance = this;
        Caught.gameObject.SetActive(false);
        restart.gameObject.SetActive(false);
        gameOver.gameObject.SetActive(false);
        spawner = FindObjectOfType<JumpScareSpawner>();
        introCutscene.gameObject.SetActive(true);
        //winCutscene.SetActive(true);

    }
    private void FixedUpdate()
    {

        if (lightsOff == true)
        {
            Debug.Log("Play Music");
            ChangeMusic();
            StatusBarEffects();
        }
        else
        {
            ChangeMusic();
            StatusBarEffects();
        }


        ActivateDeactivateSpawner();
        SkipCutscene();

        if (Caught.time >= 1.66f)
        {
            Debug.Log("it's off");
            Caught.gameObject.SetActive(false);
            Caught.time = 0f;
            PlayerRestartOrDeath();
        }
    }


    public void StatusBarEffects()
    {
        vignette.color.value = numOfDeaths >= 3 && lightsOff ? new Color(255f / 255f, 74f / 255f, 47f / 255f, 255f / 255f) : Color.white;
        grading.colorFilter.value = numOfDeaths >= 3 && lightsOff ? new Color(255f / 255f, 0f / 255f, 0f / 255f) : Color.white;
        vignette.intensity.value = numOfDeaths >= 3 && lightsOff ? (((vignette.intensity.value * 2f) + numOfDeaths) / 2f) : vignette.intensity.value;

        if (!lightsOff) { spawner.JumpScareSpawnAwake(); }
        else if (lightsOff) { spawner.StopJumpScares(); }

    }


    public void ChangeMusic()
    {
        if (playVirusMusic != null)
        {
            switch (numOfDeaths)
            {
                case int n when ((!lightsOff)):
                    playVirusMusic.Stop("Background Music 3");
                    playVirusMusic.Stop("Background Music 4");
                    playVirusMusic.Stop("Background Music 5");
                    playVirusMusic.Stop("Background Music 2");
                    playVirusMusic.Play("Lights On Background");
                    playVirusMusic.Stop("Background Music 1");
                    break;
                case int n when ((n >= 0 && n < 1) && (lightsOff)):
                    Debug.Log("Play 1");
                    playVirusMusic.Stop("Background Music 3");
                    playVirusMusic.Stop("Background Music 4");
                    playVirusMusic.Stop("Background Music 5");
                    playVirusMusic.Stop("Background Music 2");
                    playVirusMusic.Play("Background Music 1");
                    playVirusMusic.Stop("Lights On Background");
                    break;
                case int n when ((n >= 1 && n < 2) && (lightsOff)):
                    Debug.Log("Play 2");
                    playVirusMusic.Stop("Background Music 3");
                    playVirusMusic.Stop("Background Music 4");
                    playVirusMusic.Stop("Background Music 5");
                    playVirusMusic.Stop("Background Music 1");
                    playVirusMusic.Play("Background Music 2");
                    playVirusMusic.Stop("Lights On Background");
                    break;
                case int n when ((n >= 2 && n < 3) && (lightsOff)):
                    Debug.Log("Play 3");
                    playVirusMusic.Stop("Background Music 2");
                    playVirusMusic.Stop("Background Music 4");
                    playVirusMusic.Stop("Background Music 5");
                    playVirusMusic.Stop("Background Music 1");
                    playVirusMusic.Play("Background Music 3");
                    playVirusMusic.Stop("Lights On Background");
                    break;
                case int n when ((n >= 3 && n < 4) && (lightsOff)):
                    Debug.Log("Play 4");
                    playVirusMusic.Stop("Background Music 2");
                    playVirusMusic.Stop("Background Music 3");
                    playVirusMusic.Stop("Background Music 5");
                    playVirusMusic.Stop("Background Music 1");
                    playVirusMusic.Play("Background Music 4");
                    playVirusMusic.Stop("Lights On Background");
                    break;
                case int n when ((n >= 4 && n < 5) && (lightsOff)):
                    Debug.Log("Play 5");
                    playVirusMusic.Stop("Background Music 2");
                    playVirusMusic.Stop("Background Music 3");
                    playVirusMusic.Stop("Background Music 4");
                    playVirusMusic.Stop("Background Music 1");
                    playVirusMusic.Play("Background Music 5");
                    playVirusMusic.Stop("Lights On Background");
                    break;

                default:
                    playVirusMusic.Stop("Background Music 2");
                    playVirusMusic.Stop("Background Music 3");
                    playVirusMusic.Stop("Background Music 4");
                    playVirusMusic.Stop("Background Music 1");
                    playVirusMusic.Stop("Background Music 5");
                    playVirusMusic.Stop("Lights On Background");
                    break;

            }
        }
    }

    //Plays the restart cutscene and transports player back to starting point
    private void PlayerRestartOrDeath()
    {
        Player.transform.position = respawnPoint.transform.position;
        if (Player == null) { Debug.Log("No Player"); }
        Debug.Log("Check to see null status for " + status.getValue() + "and " + status);
        if (numOfDeaths < 5)
        {
            restart.gameObject.SetActive(true);
            FindObjectOfType<AudioManager>().gameObject.SetActive(true);
        }

        //else if to see if a day task has the virus leakage on
        else
        {
            gameOver.gameObject.SetActive(true);
            numOfDeaths = 0;
        }
    }

    //Might move this method to a diffrent script
    private void ActivateDeactivateSpawner()
    {
        if (lightsOff)
        {
            Spawners.StartSpawners();
        }
        else
        {
            Spawners.StopSpawnWaves();
            foreach (GameObject o in GameObject.FindGameObjectsWithTag("Enemy"))
            {
                Destroy(o);
            }

        }
    }



    private void SkipCutscene()
    {
        if (Input.GetButtonDown("PauseButton"))
        {
            introCutscene.time = 46f;
            restart.time = 12.02f;
        }
    }
}

