using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{

    [SerializeField]  private float _laserSpeed=8f;
    private bool isEnemyLaser=false;
    private float _ybound=7.3f;

    void Update()
    {
        Trajectory(isEnemyLaser?-1:1);
    }

    void Trajectory(int direction)
    {
        transform.Translate(new Vector3(0,direction*1,0) * _laserSpeed * Time.deltaTime);
        if(direction*transform.position.y > _ybound)
        {
            if(transform.parent != null)
                Destroy(transform.parent.gameObject);
            Destroy(gameObject);
        }
    }

    public void EnemyLaser()
    {
        isEnemyLaser=true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag=="Player" && isEnemyLaser)
        {
            Player player = other.transform.GetComponent<Player>();
            if(player != null)
                player.Damage(true);
            Destroy(gameObject);
        }
    }
}
