using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField] private float _powerUpSpeed=3f;
    [SerializeField] private AudioClip _audioClip;
    private int _powerUpID; //0=Triple Shot, 1=Speed, 2=Shield

    void Update()
    {
        transform.Translate(Vector3.down * _powerUpSpeed * Time.deltaTime);
        if(transform.position.y < -4.1f)
            Destroy(gameObject);
    }

    int GetPowerUpID()
    {
        if(gameObject.tag == "TripleShotPowerUp")
            return 0;
        else if(gameObject.tag == "SpeedPowerUp")
            return 1;
        else return 2;
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            _powerUpID = GetPowerUpID();
            AudioSource.PlayClipAtPoint(_audioClip, Camera.main.transform.position);//new Vector3(transform.position.x, transform.position.y, -10.0f), 100);
            other.GetComponent<Player>().ActivatePowerUp(_powerUpID);
            Destroy(gameObject);
        }
    }
}
