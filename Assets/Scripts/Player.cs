using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{	
	[SerializeField] private float _speed = 5f;
    [SerializeField] private GameObject _laserPrefab;
    [SerializeField] private GameObject _tripleShotPrefab;
    [SerializeField] private GameObject _shield;
    [SerializeField] private float _fireRate=0.2f;
    [SerializeField] private int _lives=3;
    [SerializeField] private float _powerUpTime=10f;
    [SerializeField] private float _speedMult=2f;
    [SerializeField] private int _score=0;
    [SerializeField] private AudioClip _laserSound;
    [SerializeField] private AudioClip _explosionSound;
    [SerializeField] private GameObject [] _engines; //left=0, right=0
    private bool _tripleShotState=false;
    private bool _shieldState=false;
    private float _canFire=-1;
    private bool[] _damageState;
    private SpawnManager sm;
    private UIManager uiManager;
    private PostProcessing pp;
    private AudioSource _audioSource;
    private string _playerName;
    private int _scene;
    private Animator animator;
    
    void Start()
    {
       Initialize();
       InitializeCheck();
    }

    void InitializeCheck()
    {
        if(sm==null) Debug.Log("Spawn Manager not present");
        if(uiManager==null) Debug.Log("UI Manager not present");
        if(_audioSource == null) Debug.Log("Audio Source not present");
        if(animator==null) Debug.Log("Animator not found");
    }

    void Initialize()
    {
       _scene = SceneManager.GetActiveScene().buildIndex;
        if(_scene==1)
            transform.position = new Vector3(0,0,0);
        animator = GetComponent<Animator>();
        _audioSource = gameObject.GetComponent<AudioSource>();
        _damageState = new bool[2];
        sm = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        pp = GameObject.Find("Post_Processing_Volume").GetComponent<PostProcessing>();
        uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _shield.SetActive(false);
        _playerName = gameObject.transform.name;
    }

    void Update()
    {
    	
        if(_playerName == "Player_1")
        {
            CalculateMovement();    
            if(Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire && _lives>0)
                Shoot();
        }
        else
        {
            CalculateMovement(false);
            if(Input.GetKeyDown(KeyCode.RightShift) && Time.time > _canFire && _lives>0)
                Shoot();
        }
        
    }

    void CalculateMovement(bool one=true)
    {
        float horizontalInput = (_scene==1)?    //Single player?
                                    (Input.GetAxis("Horizontal")): 
                                    (one)?                         //Which player?
                                        ((Input.GetKey(KeyCode.D))?1:(Input.GetKey(KeyCode.A))?-1:0):
                                        ((Input.GetKey(KeyCode.RightArrow))?1:(Input.GetKey(KeyCode.LeftArrow))?-1:0);
    
        float verticalInput = (_scene==1)?  //Single player?
                                (Input.GetAxis("Vertical")):
                                (one)?                          //Which player?
                                    ((Input.GetKey(KeyCode.W))?1:Input.GetKey(KeyCode.S)?-1:0):
                                    ((Input.GetKey(KeyCode.UpArrow))?1:(Input.GetKey(KeyCode.DownArrow))?-1:0);
        if(horizontalInput == 0 || _lives==0)    
        {
            animator.SetTrigger("OnPlayerStraight");
            animator.SetFloat("OnPlayerTurn",0);
        }
        else if(horizontalInput !=0 && _lives!=0)    
        {
            animator.SetFloat("OnPlayerTurn",horizontalInput);
            animator.ResetTrigger("OnPlayerStraight");
        }
        transform.Translate(new Vector3(horizontalInput, verticalInput, 0) * _speed * Time.deltaTime);
        if(transform.position.x > 10)
        	transform.position = new Vector3(-10, transform.position.y, transform.position.z);
        else if(transform.position.x < -10)
        	transform.position = new Vector3(10, transform.position.y, transform.position.z);
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 0), 0);
    }

    void Shoot()
    {
        _canFire = Time.time + _fireRate;
        _audioSource.clip = _laserSound;
        _audioSource.Play();
        if(!_tripleShotState)
        {
            GameObject laser = Instantiate(_laserPrefab, transform.position + new Vector3(0,1,0), Quaternion.identity);
            laser.tag=_playerName + "_Laser";
        }    
        else
        {
            GameObject laser = Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
            for(int i=0; i<laser.transform.childCount; i++)
            {
                laser.transform.GetChild(i).tag=_playerName + "_Laser";                
            }
        }           
    }

    void ShowPlayerDamage()
    {
        int choose = Random.Range(0,2);
        if(!_damageState[choose])
        {
            _damageState[choose] = true;
            _engines[choose].SetActive(true);
        }
        else
            _engines[choose==0?choose+1:choose-1].SetActive(true);

    }

    public void Damage(bool enemyLaser=false)
    {
        if(!_shieldState)
        {
            _lives--;
            _lives=_lives<0?0:_lives;
            ShowPlayerDamage();
            uiManager.SetLives(_playerName,_lives);
            if(enemyLaser)
            {
                _audioSource.clip = _explosionSound;
                _audioSource.Play();
            }
            if(_lives == 0)
            {
                sm.OnPlayerDeath();
                foreach(Transform child in transform)
                    child.gameObject.SetActive(false);
                _speed=0;
                animator.SetTrigger("OnPlayerDeath");
                Destroy(gameObject,2.3f);
            }
            else
            {
                _score-=5;
                uiManager.SetScore(_playerName,_score);
            }
        }
        else
        {
            _shieldState=false;
            _shield.SetActive(_shieldState);
        }
    }

    public void ActivatePowerUp(int powerUpID)
    {
        StartCoroutine(Activate(powerUpID));
    }

    IEnumerator Activate(int powerUpID)
    {
        if(powerUpID == 0)
        {
            _tripleShotState=true;
            yield return new WaitForSeconds(_powerUpTime);
            _tripleShotState=false;
        }
        else if(powerUpID == 1)
        {
            _speed *=_speedMult;
            SetBoost(true);
            yield return new WaitForSeconds(_powerUpTime);
            SetBoost(false);
            _speed /=_speedMult;
        }
        else if(_lives>0)
        {
            _shieldState=true;
            _shield.SetActive(_shieldState);
        }
         
    }

    public void AddScore()
    {
        _score+=10;
        uiManager.SetScore(_playerName, _score);
    }

    void SetBoost(bool val)
    {
        pp.IsBoost(val);
    }
}
