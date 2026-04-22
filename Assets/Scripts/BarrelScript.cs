using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class BarrelScript : MonoBehaviour
{
    // referencing scripts.
    public LevelManager levelManagerScript;

    // referencing game objects.
    public Transform player;

    // projectile intervals.
    private float projectileInterval;
    private float startWaitPeriod = 5f;
    private int randomProjectileIntervalMin = 1;
    private float randomProjectileIntervalMax = 8f;

    // projectile - fireball.
    [SerializeField] private GameObject fireBall;
    private float fireBallSpeed = 10f;
    private float fireBallOffset = 15f;

    // projectile - laser.
    [SerializeField] private GameObject laserBarrel;
    private float laserSpeed = 1.5f;

    // projectile - homing missile.
    [SerializeField] private GameObject homingMissile;
    private float homingMissileSpeed = 10;

    // projectile percentage calculators.
    public int projectileT1 = 100;             //will decrease fireBallPercentage and increase laserPercentage or the opposite for both.
    public int projectileT2 = 100;             //will decrease laserPercentage and increase the homingMissilePercentage or the opposite for both.

    //----------------------------------------------!Debug---------------------------------------------
    // [SerializeField] private Text percentageText;   //referencing text component.
    // public int fireBallPercentage = 100;            //first tertile
    // public int laserPercentage = 0;                 //second tertile
    // public int homingMissilePercentage = 0;         //third tertile
    //-----------------------------------------------Debug!--------------------------------------------

    void Start(){
        projectileInterval = startWaitPeriod + Random.Range(randomProjectileIntervalMin, randomProjectileIntervalMax);
    }
    void Update(){
        // shoots unless game over.
        if (levelManagerScript.gameOver == false){
            if (Time.deltaTime > projectileInterval){
                Shoot();
                projectileInterval = Time.deltaTime + Random.Range(randomProjectileIntervalMin, randomProjectileIntervalMax);
            }else{
                projectileInterval -= Time.deltaTime;
            }
        }
        //----------------------------------------------!Debug---------------------------------------------
        // showing the percentage and the speed of the projectiles.
        // percentageText.text = (
        //     "PHASE: " + levelManagerScript.phase + "\n\n" +
        //
        //     "Max Shooting Interval: " + randomProjectileIntervalMax + "\n\n" + 
        //
        //     "PERCENTAGE\n" +
        //     "Fire Ball: " + fireBallPercentage + "\n" +
        //     "Laser: " + laserPercentage +  "\n" +
        //     "Homing Missile: " + homingMissilePercentage + "\n\n" +
        //
        //     "SPEED\n" +
        //     "Fire Ball: " + fireBallSpeed + "\n" +
        //     "Laser: " + laserSpeed + "\n" +
        //     "Homing Missile: " + homingMissileSpeed
        // );
        //-----------------------------------------------Debug!--------------------------------------------
    }

    public void UpdatePhase(){
        if(levelManagerScript.phase <= 5) {         // F: 100% L: 0% H: 0%
            // does nothing
        }else if(levelManagerScript.phase <= 10){   // F :100% ~ 90% L: 0% ~ 10% H: 0%
            projectileT1 -= (10 / 5);
        }else if(levelManagerScript.phase <= 15){   // F :90% ~ 80% L: 10% ~ 15% H: 5%
            projectileT1 -= (10 / 5);
            projectileT2 -= (5 / 5);
        }else if(levelManagerScript.phase <= 20){   // F :80% ~ 70% L: 15% ~ 20% H: 5% ~ 10%
            projectileT1 -= (10 / 5);
            projectileT2 -= (5 / 5);
        }else if(levelManagerScript.phase <= 25){   // F :70% ~ 65% L: 20% H: 10% ~ 15%
            projectileT1 -= (5 / 5);
            projectileT2 -= (5 / 5); 
        }else{
            Debug.LogError("Error, Wave number exceeds the limit");
        }

        // changing the speed of the projectiles
        randomProjectileIntervalMax -= 4f/25f;          // 8 ~ 4
        fireBallSpeed += 10f/25f;                       // 10 ~ 20
        laserSpeed -= 0.5f/25f;                         // 1.5 ~ 1.0
        homingMissileSpeed += 4f/25f;                   // 4 ~ 8

        //----------------------------------------------!Debug---------------------------------------------
        // update the projectile percentage 
        // fireBallPercentage = projectileT1 - 0;
        // laserPercentage = projectileT2 - projectileT1;
        // homingMissilePercentage = 100 - projectileT2;
        //-----------------------------------------------Debug!--------------------------------------------
    }

    void Shoot(){
        int randomRate = Random.Range(1, 101);
        if (randomRate <= projectileT1) {
            ProjectileFireBall();
            //ProjectileHomingMissile();
            //ProjectileLaser();
        }else if(randomRate <= projectileT2) {
            ProjectileLaser();
        }else{
            ProjectileHomingMissile();
        }
    }

    private void ProjectileFireBall(){
        // setting the angle of the fire ball with a random offset from the player.
        Vector3 randomOffset = new Vector3(Random.Range(-fireBallOffset, fireBallOffset), Random.Range(-fireBallOffset, fireBallOffset), player.position.z);
        Vector3 randomPlayerPosition = player.position + randomOffset;
        Vector3 difference = randomPlayerPosition - transform.position;
        float angle = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        transform.eulerAngles = Vector3.forward * angle;

        // setting the direction of the fire ball.
        float distance = difference.magnitude;
        Vector2 direction = difference / distance;
        direction.Normalize();

        // creating the fire ball with the calculated values.
        GameObject projectile = Instantiate(fireBall);
        projectile.transform.position = transform.position;
        projectile.transform.rotation = Quaternion.Euler(0f, 0f, angle);
        projectile.GetComponent<Rigidbody2D>().velocity = direction * fireBallSpeed;
    }

    private void ProjectileLaser(){
        // creating the laser barrel
        GameObject projectile = Instantiate(laserBarrel);
        projectile.GetComponent<LaserBarrel>().laserIntervalTime = laserSpeed;
        projectile.transform.position = transform.position;
    }

    private void ProjectileHomingMissile(){
        // setting the angle of the homing missile.
        Vector3 difference = player.position - transform.position;
        float angle = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        transform.eulerAngles = Vector3.forward * angle;

        // setting the direction of the homing missile.
        float distance = difference.magnitude;
        Vector2 direction = difference / distance;
        direction.Normalize();

        // creating the homing missile with the calculated values.
        GameObject projectile = Instantiate(homingMissile);
        projectile.transform.position = transform.position;
        projectile.transform.rotation = Quaternion.Euler(0f, 0f, angle);
        projectile.GetComponent<Rigidbody2D>().velocity = direction * homingMissileSpeed;
    }
}