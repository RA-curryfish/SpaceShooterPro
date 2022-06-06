using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _enemySpeed=3f;
    [SerializeField] private GameObject _enemyLaser;
    private GameObject[] _player;
    List<Player> player_script;
    private bool stopLaser = false; //to prevent adding points on shooting dead enemies in exploding sequence
    private AudioSource _audioSource;
    private bool _isCoop=false;
    private bool _stopShoot=false;
    
    void Start()
    {
        Initialize();
        InitializeCheck();
        StartCoroutine(Shoot());
    }

    void Initialize()
    {
        _player=GameObject.FindGameObjectsWithTag("Player");
        player_script = new List<Player>();
        if(_player.Length == 2) _isCoop = true;
        _audioSource = gameObject.GetComponent<AudioSource>();
    }
    void InitializeCheck()
    {
        if(_player !=null) 
            for(int i=0;i<_player.Length;i++)
                player_script.Add(_player[i].GetComponent<Player>());
        if(_audioSource == null) Debug.Log("Audio Source not found");
    }

    void Update()
    {
        CalculateMovement();
    }

    void CalculateMovement()
    {
        transform.Translate(Vector3.down * _enemySpeed * Time.deltaTime);
        if(transform.position.y < -4.1f)
            transform.position = new Vector3(Random.Range(-9f,9f),6.3f,0);
    }

    IEnumerator Shoot()
    {
        while(!_stopShoot)
        {
            GameObject EnemyLaser = Instantiate(_enemyLaser,transform.position, Quaternion.identity);
            EnemyLaser.transform.GetChild(0).GetComponent<Laser>().EnemyLaser();
            EnemyLaser.transform.GetChild(1).GetComponent<Laser>().EnemyLaser();
            EnemyLaser.transform.parent = gameObject.transform.parent.parent;
            yield return new WaitForSeconds(Random.Range(3,10));
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Animator animator = GetComponent<Animator>();
        if(animator==null) Debug.Log("Animator not found");
        if(other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();
            if(player != null)  player.Damage();
            stopLaser = true;
            if(gameObject != null)  _audioSource.Play();
            animator.SetTrigger("OnEnemyDeath");
            _enemySpeed =0;
            _stopShoot=true;
            Destroy(gameObject, 2.8f);
        }
        else if((other.tag).Contains("Laser"))
        {
            if(!stopLaser)
            {
                _audioSource.Play();
                if(_player[0]!=null && (other.tag).Contains(_player[0].transform.name))
                    player_script[0].AddScore();
                else if(_isCoop && _player[1]!=null && (other.tag).Contains(_player[1].transform.name))
                    player_script[1].AddScore();
            }
            stopLaser = true;
            Destroy(other.gameObject);
            animator.SetTrigger("OnEnemyDeath");
            _enemySpeed=0;
            _stopShoot=true;
            Destroy(gameObject, 2.8f);
        }
    }
}
