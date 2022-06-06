using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField] private float _rotateSpeed=10f;
    private SpawnManager sm;
    private AudioSource _audioSource;
    
    void Start()
    {
        sm = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _audioSource = gameObject.GetComponent<AudioSource>();
        if(sm == null) Debug.Log("No Spawn Manager");
        if(_audioSource == null) Debug.Log("No Audio Source");
    }

    void Update()
    {
        transform.Rotate(Vector3.forward * _rotateSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Animator animator = GetComponent<Animator>();
        if(animator == null) Debug.Log("Animator not found");
        if((other.tag).Contains("Laser"))
        {
            Destroy(other.gameObject);
            sm.StartGame();
            _audioSource.Play();
            animator.SetTrigger("OnAsteroidDeath");
            _rotateSpeed =0;
            Destroy(gameObject,2.8f);
        }
    }
}
