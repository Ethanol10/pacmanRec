using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PacStudentController : MonoBehaviour
{
    private Tweener tweener;
    private Vector3 spawnPoint = new Vector3(1.25f, -1.25f, 0);
    private List<List<GameObject>> levelMapObjects;
    private List<List<GameObject>> surroundLMObjects;
    private LevelGenerator LevelGeneratorObj;
    private Teleporter TeleporterObj;
    private Vector2 gridPos = new Vector2(1, 1);
    public float delayAnim = 0.5f;
    private AudioSource movingAudio;
    public AudioClip eatingAudio;
    public AudioClip movingNoEating;

    [SerializeField]
    private GameObject deathSoundObj;
    private GameObject deathSound;
    public AudioClip firstDeathSound;
    public AudioClip secondDeathSound;
    private Animator playerAnimator;
    private ParticleSystem dustParticles;

    [SerializeField]
    private AudioSource wallAudioSource;
    
    public GameObject emptySquare;

    public Canvas HUD;
    public Text Score;
    public Text Lives;
    public Text gameTimer;
    private int playerScore;

    private int playerLives;

    private float playerTimer;

    private GhostHandler ghostHandler;
    public GameObject deathExplosionObj;
    private ParticleSystem deathExplosion;
    private PacmanStates playerState;

    private float deadTimer;
    public float deathWait;
    enum PacmanStates{
        ALIVE,
        DEAD
    }

    private float startTimer;
    private bool initialCountdownDone = false;
    private bool gameOver = false;
    public Sprite three;
    public Sprite two;
    public Sprite one;
    public Sprite go;
    public Sprite gameOverSprite;
    private Image countdown;
    private int pelletLeft;
    
    // Start is called before the first frame update
    void Start()
    {
        playerScore = 0;
        tweener = GetComponent<Tweener>();
        LevelGeneratorObj = GameObject.FindWithTag("levelGen").GetComponent<LevelGenerator>();
        TeleporterObj = GameObject.FindWithTag("levelGen").GetComponent<Teleporter>();
        levelMapObjects = LevelGeneratorObj.getLevelMapObjects();
        surroundLMObjects = LevelGeneratorObj.checkSurround((int)gridPos.x, (int)gridPos.y);
        movingAudio = GetComponent<AudioSource>();
        playerAnimator = GetComponent<Animator>();
        dustParticles = GetComponent<ParticleSystem>();
        ghostHandler = GameObject.FindWithTag("ghosthandler").GetComponent<GhostHandler>();
        Score = HUD.transform.GetChild(2).gameObject.GetComponent<Text>();
        Lives = HUD.transform.GetChild(5).gameObject.GetComponent<Text>();
        gameTimer = HUD.transform.GetChild(0).gameObject.GetComponent<Text>();
        Instantiate(deathExplosionObj, new Vector3(0,0,0), Quaternion.identity);
        deathExplosion = deathExplosionObj.GetComponent<ParticleSystem>();
        deathExplosion.Stop();
        playerState = PacmanStates.ALIVE;
        playerLives = 3;
        deadTimer = 0.0f;
        deathSound = Instantiate(deathSoundObj, new Vector3(0,0,0), Quaternion.identity);
        startTimer = 0.0f;
        countdown = HUD.transform.GetChild(6).GetComponent<Image>();
        playerTimer = 0.0f;
        pelletLeft = 0;


        bool done = false;
        while(!done){
            for(int i = 0; i < levelMapObjects.Count; i++){
                for(int j = 0; j < levelMapObjects[i].Count; j++){
                    if(levelMapObjects[i][j].tag == "innerwall" || levelMapObjects[i][j].tag == "outerwall" ){
                        wallAudioSource = levelMapObjects[i][j].GetComponent<AudioSource>();
                        done = true;
                    }
                    if(levelMapObjects[i][j].tag == "pellet" || levelMapObjects[i][j].tag == "powerpellet"){
                        pelletLeft++;
                    }
                }
            }
        }

    }

    // Update is called once per frame
    void Update()
    {   
        if(!initialCountdownDone){
            startTimer += Time.deltaTime;
            if(startTimer < 1){
                countdown.sprite = three;
            }   
            else if(startTimer < 2){
                countdown.sprite = two;
            }
            else if(startTimer < 3){
                countdown.sprite = one;
            }
            else if(startTimer < 4){
                countdown.sprite = go;
            }
            else if(startTimer > 4){
                startTimer = 0.0f;
                countdown.enabled = false;
                initialCountdownDone = true;
            }
        }

        if(initialCountdownDone && !gameOver && playerState != PacmanStates.DEAD){
            playerTimer += Time.deltaTime;
            updateSurround();
            if(playerState == PacmanStates.ALIVE){
                if(Input.GetKey("w")){
                    playerAnimator.enabled = true;
                    if(!dustParticles.isPlaying){
                        dustParticles.Play();
                    }
                    if(!tweener.tweenActive()){
                        // lastKey = 'w';
                        transform.rotation = Quaternion.Euler(0, 0, 90);
                        if(surroundLMObjects[0][1].tag == "pellet" || surroundLMObjects[0][1].tag == "powerpellet" || surroundLMObjects[0][1].tag == "empty"){
                            switchAudio(surroundLMObjects[0][1].tag);
                            tweener.AddTween(gameObject.transform, 
                                gameObject.transform.position, 
                                new Vector3(gameObject.transform.position.x,gameObject.transform.position.y + 1.25f, 0), 
                                delayAnim);
                            gridPos += new Vector2(-1, 0);
                            
                        }
                        else if(surroundLMObjects[0][1].tag == "innerwall" || surroundLMObjects[0][1].tag == "outerwall" || surroundLMObjects[0][1].tag == "outercorner" || surroundLMObjects[0][1].tag == "innercorner"){
                            if(!wallAudioSource.isPlaying){
                            wallAudioSource.Play();
                            }
                        }
                        else if(surroundLMObjects[0][1].tag == "teleport"){
                            gridPos = TeleporterObj.swapPosition(gameObject.transform);
                        }
                    }
                }
                else if(Input.GetKey("a")){
                    playerAnimator.enabled = true;
                    if(!dustParticles.isPlaying){
                        dustParticles.Play();
                    }
                    if(!tweener.tweenActive()){
                        //lastKey = 'a';
                        transform.rotation = Quaternion.Euler(0, 0, 180);
                        if(surroundLMObjects[1][0] == null){
                            //do Nothing
                        }
                        else if(surroundLMObjects[1][0].tag == "pellet" || surroundLMObjects[1][0].tag == "powerpellet" || surroundLMObjects[1][0].tag == "empty" || surroundLMObjects[1][0].tag == "teleport"){
                            if(surroundLMObjects[1][0].tag == "teleport"){
                                gridPos = TeleporterObj.swapPosition(gameObject.transform);
                            }
                            switchAudio(surroundLMObjects[1][0].tag);
                            tweener.AddTween(gameObject.transform, 
                                gameObject.transform.position, 
                                new Vector3(gameObject.transform.position.x - 1.25f ,gameObject.transform.position.y, 0), 
                                delayAnim);
                            if(!(gridPos.y - 1 < 0)){
                                gridPos += new Vector2(0, -1);     
                            }
                        }
                        else if(surroundLMObjects[1][0].tag == "innerwall" || surroundLMObjects[1][0].tag == "outerwall" || surroundLMObjects[1][0].tag == "outercorner" || surroundLMObjects[1][0].tag == "innercorner"){
                            if(!wallAudioSource.isPlaying){
                            wallAudioSource.Play();
                            }
                        }
                    }
                    
                }
                else if(Input.GetKey("s")){
                    playerAnimator.enabled = true;
                    if(!dustParticles.isPlaying){
                        dustParticles.Play();
                    }
                    if(!tweener.tweenActive()){
                        //lastKey = 's';
                        transform.rotation = Quaternion.Euler(0, 0, 270);
                        if(surroundLMObjects[2][1].tag == "pellet" || surroundLMObjects[2][1].tag == "powerpellet" || surroundLMObjects[2][1].tag == "empty"){
                            switchAudio(surroundLMObjects[2][1].tag);
                            tweener.AddTween(gameObject.transform, 
                                gameObject.transform.position, 
                                new Vector3(gameObject.transform.position.x,gameObject.transform.position.y - 1.25f, 0), 
                                delayAnim);
                            gridPos += new Vector2(1, 0);
                        }
                        else if(surroundLMObjects[2][1].tag == "innerwall" || surroundLMObjects[2][1].tag == "outerwall" || surroundLMObjects[2][1].tag == "outercorner" || surroundLMObjects[2][1].tag == "innercorner"){
                            if(!wallAudioSource.isPlaying){
                            wallAudioSource.Play();
                            }
                        }
                        else if(surroundLMObjects[2][1].tag == "teleport"){
                            gridPos = TeleporterObj.swapPosition(gameObject.transform);
                        }
                    }
                }
                else if(Input.GetKey("d")){
                    playerAnimator.enabled = true;
                    if(!dustParticles.isPlaying){
                        dustParticles.Play();
                    }
                    if(!tweener.tweenActive()){
                        //lastKey = 'd';
                        transform.rotation = Quaternion.Euler(0, 0, 0);
                        if(surroundLMObjects[1][2].tag == "pellet" || surroundLMObjects[1][2].tag == "powerpellet" || surroundLMObjects[1][2].tag == "empty"){
                            switchAudio(surroundLMObjects[1][2].tag);
                            tweener.AddTween(gameObject.transform, 
                                gameObject.transform.position, 
                                new Vector3(gameObject.transform.position.x + 1.25f ,gameObject.transform.position.y, 0), 
                                delayAnim);
                            gridPos += new Vector2(0, 1);
                        }
                        else if(surroundLMObjects[1][2].tag == "innerwall" || surroundLMObjects[1][2].tag == "outerwall" || surroundLMObjects[1][2].tag == "outercorner" || surroundLMObjects[1][2].tag == "innercorner"){
                            if(!wallAudioSource.isPlaying){
                            wallAudioSource.Play();
                            }
                        }
                        else if(surroundLMObjects[1][2].tag == "teleport"){
                            gridPos = TeleporterObj.swapPosition(gameObject.transform);
                        }
                    }
                }
                else{
                    if(!tweener.tweenActive()){
                        playerAnimator.enabled = false;
                        dustParticles.Stop();
                    }
                }
            }
            
            updateUI();
            if(checkForGameOver()){
                gameOver = true;
            }
        }
        if(playerState == PacmanStates.DEAD){
            deadTimer += Time.deltaTime;
            playerAnimator.enabled = true;
            if(deadTimer >= deathWait){
                if(playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("pacmanMoving")){
                    playerState = PacmanStates.ALIVE;
                    playerAnimator.SetBool("isDead", false);
                    gameObject.transform.position = spawnPoint;
                    gridPos = new Vector2(1,1);
                    deadTimer = 0;
                }
                if(dustParticles.isPlaying){
                    dustParticles.Stop();
                }
            }
        }
        if(gameOver){
            GameOverSequence();
        }
    }

    private void switchAudio(string type){
        if(!movingAudio.isPlaying){
            if(type == "pellet" || type == "powerpellet"){
                movingAudio.clip = eatingAudio;
            }
            else if(type == "empty"){
                movingAudio.clip = movingNoEating;
            }
            
            movingAudio.Play();
        }
    }

    private bool checkForGameOver(){
        if(pelletLeft == 0){
            return true;
        }
        if(playerLives < 0){
            return true;
        }
        return false;
    }

    public void GameOverSequence(){
        startTimer += Time.deltaTime;
        countdown.sprite = gameOverSprite;
        countdown.enabled = true;
        if(playerScore > PlayerPrefs.GetInt("score1", 0)){
            PlayerPrefs.SetInt("score1", playerScore);
            PlayerPrefs.SetFloat("timer1", playerTimer);
        }
        else if(playerScore == PlayerPrefs.GetInt("score1", 0) && ( playerTimer < PlayerPrefs.GetFloat("timer1", 9999999.0f))){
            PlayerPrefs.SetInt("score1", playerScore);
            PlayerPrefs.SetFloat("timer1", playerTimer);
        }
        if(startTimer > 3){
            SceneManager.LoadScene(0);
        }
    }

    private void updateSurround(){
        surroundLMObjects = LevelGeneratorObj.checkSurround((int)gridPos.x, (int)gridPos.y);
        // for (int row = 0; row < surroundLMObjects.Count; row++){
        //     for(int col = 0; col < surroundLMObjects[row].Count; col++){
        //         Debug.Log("X:Y :" + gridPos.x + ":" + gridPos.y + " tag: " + surroundLMObjects[row][col].tag);
        //     }
        // }
        // for (int row = 0; row < levelMapObjects.Count; row++){
        //     for(int col = 0; col < levelMapObjects[row].Count; col++){
        //         //Debug.Log("X:Y :" + gridPos.x + ":" + gridPos.y + " tag: " + surroundLMObjects[row][col].tag);
        //         Debug.Log("row:col: " + row + ":" + col + " lmo: " + levelMapObjects[row][col].tag );
        //     }
        // }
    }

    void OnTriggerEnter(Collider other){
        if(other.gameObject.tag == "pellet" || other.gameObject.tag == "powerpellet"){
            if(other.gameObject.tag == "powerpellet"){
                ghostHandler.setGhostState("scared");
            }
            switchAudio("pellet");
            playerScore += 10; 
            levelMapObjects[(int)gridPos.x][(int)gridPos.y] = Instantiate(emptySquare, other.gameObject.transform.position, Quaternion.identity);
            Destroy(other.gameObject);
            pelletLeft--;
        }
        if(other.gameObject.tag == "cherry"){
            playerScore += 100;
        }
        if(other.gameObject.tag == "ghost"){
            if(other.gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("scaredGhost") || other.gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("recoveringGhost")){
                playerScore += 300;
                other.gameObject.GetComponent<Animator>().SetBool("isDead", true);
                List<Ghost> ghosts = ghostHandler.GetGhosts();
                for(int i = 0; i < ghosts.Count; i++){
                    if(ghosts[i].getGhostShape().GetInstanceID() == other.gameObject.GetInstanceID()){
                        ghosts[i].setGhostState("dead");
                    }
                }
            }
            else if(other.gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("ghostMoving")){
                Debug.Log("ping");
                playerLives -= 1;
                playerAnimator.SetBool("isDead", true);
                playerState = PacmanStates.DEAD;
            }
        }
    }

    private void updateUI(){
        TimeSpan time = TimeSpan.FromSeconds(playerTimer);

        //here backslash is must to tell that colon is
        //not the part of format, it just a character that we want in output
        string str = time.ToString(@"mm\:ss\:ff");
        Score.text = "Score: " + playerScore;
        Lives.text = "X " + playerLives;
        gameTimer.text = "TIME: " + str;
    }

    public void playFirstDeathAudio(){
        AudioSource dead = deathSound.GetComponent<AudioSource>();
        dead.clip = firstDeathSound;
        dead.Play();
        if(!deathExplosion.isPlaying){
            deathExplosion.Play();
        }
    }

    public void playSecondDeathAudio(){
        AudioSource dead = deathSound.GetComponent<AudioSource>();
        dead.clip = secondDeathSound;
        dead.Play();
    }
    public void LoadStartMenu() {
        SceneManager.LoadSceneAsync(0);
    }

    public Vector2 getGridPos(){
        return gridPos;
    }
}   

