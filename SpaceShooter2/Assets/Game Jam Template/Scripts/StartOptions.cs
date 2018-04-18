using Kontrol;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

public class StartOptions : MonoBehaviour
{



    public int sceneToStart = 1;                                        //Index number in build settings of scene to load if changeScenes is true
    public int scene2 = 2;
    public int scene3 = 3;
    public bool changeScenes;                                           //If true, load a new scene when Start is pressed, if false, fade out UI and continue in single scene
    public bool changeMusicOnStart;                                     //Choose whether to continue playing menu music or start a new music clip


    [HideInInspector] public bool inMainMenu = true;                    //If true, pause button disabled in main menu (Cancel in input manager, default escape key)
    [HideInInspector] public Animator animColorFade;                    //Reference to animator which will fade to and from black when starting game.
    [HideInInspector] public Animator animMenuAlpha;                    //Reference to animator that will fade out alpha of MenuPanel canvas group
    public AnimationClip fadeColorAnimationClip;        //Animation clip fading to color (black default) when changing scenes
    [HideInInspector] public AnimationClip fadeAlphaAnimationClip;      //Animation clip fading out UI elements alpha


    private PlayMusic playMusic;                                        //Reference to PlayMusic script
    private float fastFadeIn = .01f;                                    //Very short fade time (10 milliseconds) to start playing music immediately without a click/glitch
    private ShowPanels showPanels;                                      //Reference to ShowPanels script on UI GameObject, to show and hide panels
    private AudioManager audioManager;

    void Awake()
    {
        //Get a reference to ShowPanels attached to UI object
        showPanels = GetComponent<ShowPanels>();

        //Get a reference to PlayMusic attached to UI object
        playMusic = GetComponent<PlayMusic>();

        //play main menu music
        playMusic.PlayLevelMusic();
    }

    private void LateUpdate()
    {
        if (audioManager == null)
        {
            audioManager = AudioManager.instance;
            Assert.IsNotNull(audioManager);
        }
    }

    public void StartButtonClicked()
    {
        //If changeMusicOnStart is true, fade out volume of music group of AudioMixer by calling FadeDown function of PlayMusic, using length of fadeColorAnimationClip as time. 
        //To change fade time, change length of animation "FadeToColor"
        if (changeMusicOnStart)
        {
            playMusic.FadeDown(fadeColorAnimationClip.length);
        }

        //If changeScenes is true, start fading and change scenes halfway through animation when screen is blocked by FadeImage
        if (changeScenes)
        {
            //Use invoke to delay calling of LoadDelayed by half the length of fadeColorAnimationClip
            Invoke("LoadDelayed", fadeColorAnimationClip.length * .5f);

            //Set the trigger of Animator animColorFade to start transition to the FadeToOpaque state.
            animColorFade.SetTrigger("fade");
        }

        //If changeScenes is false, call StartGameInScene
        else
        {
            //Call the StartGameInScene function to start game without loading a new scene.
            StartGameInScene();
        }

    }

    public void Level2ButtonClicked()
    {//If changeMusicOnStart is true, fade out volume of music group of AudioMixer by calling FadeDown function of PlayMusic, using length of fadeColorAnimationClip as time. 
        //To change fade time, change length of animation "FadeToColor"
        if (changeMusicOnStart)
        {
            playMusic.FadeDown(fadeColorAnimationClip.length);
        }

        //If changeScenes is true, start fading and change scenes halfway through animation when screen is blocked by FadeImage
        if (changeScenes)
        {
            //Use invoke to delay calling of LoadDelayed by half the length of fadeColorAnimationClip
            Invoke("LoadDelayedNextScene2", fadeColorAnimationClip.length * .5f);

            //Set the trigger of Animator animColorFade to start transition to the FadeToOpaque state.
            animColorFade.SetTrigger("fade");
        }

        //If changeScenes is false, call StartGameInScene
        else
        {
            //Call the StartGameInScene function to start game without loading a new scene.
            StartGameInScene();
        }

    }

    public void Level3ButtonClicked()
    {//If changeMusicOnStart is true, fade out volume of music group of AudioMixer by calling FadeDown function of PlayMusic, using length of fadeColorAnimationClip as time. 
        //To change fade time, change length of animation "FadeToColor"
        if (changeMusicOnStart)
        {
            playMusic.FadeDown(fadeColorAnimationClip.length);
        }

        //If changeScenes is true, start fading and change scenes halfway through animation when screen is blocked by FadeImage
        if (changeScenes)
        {
            //Use invoke to delay calling of LoadDelayed by half the length of fadeColorAnimationClip
            Invoke("LoadDelayedNextScene3", fadeColorAnimationClip.length * .5f);

            //Set the trigger of Animator animColorFade to start transition to the FadeToOpaque state.
            animColorFade.SetTrigger("fade");
        }

        //If changeScenes is false, call StartGameInScene
        else
        {
            //Call the StartGameInScene function to start game without loading a new scene.
            StartGameInScene();
        }

    }
    //public void GoToScene2()
    //{
    //    StartCoroutine(_GoToScene2());
    //}

    //IEnumerator _GoToScene2()
    //{
    //    yield return new WaitForSeconds(3);
    //    //If changeScenes is true, start fading and change scenes halfway through animation when screen is blocked by FadeImage
    //    if (changeScenes)
    //    {
    //        //Use invoke to delay calling of LoadDelayed by half the length of fadeColorAnimationClip
    //        Invoke("LoadDelayedScene2", fadeColorAnimationClip.length * .5f);

    //        //Set the trigger of Animator animColorFade to start transition to the FadeToOpaque state.
    //        animColorFade.SetTrigger("fade");
    //    }
    //}

    void OnEnable()
    {
        SceneManager.sceneLoaded += SceneWasLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= SceneWasLoaded;
    }

    //Once the level has loaded, check if we want to call PlayLevelMusic
    void SceneWasLoaded(Scene scene, LoadSceneMode mode)
    {
        //if changeMusicOnStart is true, call the PlayLevelMusic function of playMusic
        if (changeMusicOnStart)
        {
            //playMusic.PlayLevelMusic ();
            //audioManager.PlayMusic("Prototype Music");
        }
    }

    public void LoadDelayedNextScene2()
    {
        //Pause button now works if escape is pressed since we are no longer in Main menu.
        inMainMenu = false;

        //Hide the main menu UI element
        showPanels.HideMenu();

        //change game state to playing
        GameStateMachine.myGameState = GameStateMachine.GameState.PLAYING;

        //Load the selected scene, by scene index number in build settings
        SceneManager.LoadScene(scene2);
    }

    public void LoadDelayedNextScene3()
    {
        //Pause button now works if escape is pressed since we are no longer in Main menu.
        inMainMenu = false;

        //Hide the main menu UI element
        showPanels.HideMenu();

        //change game state to playing
        GameStateMachine.myGameState = GameStateMachine.GameState.PLAYING;

        //Load the selected scene, by scene index number in build settings
        SceneManager.LoadScene(scene3);
    }

    public void LoadDelayed()
    {
        //Pause button now works if escape is pressed since we are no longer in Main menu.
        inMainMenu = false;

        //Hide the main menu UI element
        showPanels.HideMenu();

        //change game state to playing
        GameStateMachine.myGameState = GameStateMachine.GameState.PLAYING;

        //Load the selected scene, by scene index number in build settings
        SceneManager.LoadScene(sceneToStart);
    }

    public void HideDelayed()
    {
        //Hide the main menu UI element after fading out menu for start game in scene
        showPanels.HideMenu();
    }

    public void StartGameInScene()
    {
        //Pause button now works if escape is pressed since we are no longer in Main menu.
        inMainMenu = false;

        //If changeMusicOnStart is true, fade out volume of music group of AudioMixer by calling FadeDown function of PlayMusic, using length of fadeColorAnimationClip as time. 
        //To change fade time, change length of animation "FadeToColor"
        if (changeMusicOnStart)
        {
            //Wait until game has started, then play new music
            //Invoke ("PlayNewMusic", fadeAlphaAnimationClip.length);
            //to start the intro music
            //audioManager.PlayMusic("Prototype Music");
        }
        //Set trigger for animator to start animation fading out Menu UI
        animMenuAlpha.SetTrigger("fade");
        Invoke("HideDelayed", fadeAlphaAnimationClip.length);
        Debug.Log("Game started in same scene! Put your game starting stuff here.");
    }


    public void PlayNewMusic()
    {
        //Fade up music nearly instantly without a click 
        playMusic.FadeUp(fastFadeIn);
        //Play music clip assigned to mainMusic in PlayMusic script
        playMusic.PlaySelectedMusic(1);
    }
}
