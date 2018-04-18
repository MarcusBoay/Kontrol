using System.Collections;
using UnityEngine;

namespace Kontrol
{
    public class RicochetBossController : MonoBehaviour
    {
        public GameObject sceneChanger;
        public GameObject flasher;

        public GameObject[] outerShooters;
        public GameObject[] innerShooters;
        public GameObject coreA;
        public GameObject coreB;
        private Rigidbody2D coreBRB2D;
        public GameObject ricochetBullet;
        public GameObject ricochetInvulBullet;
        public GameObject[] laser;
        public GameObject snipeLaser;
        public GameObject normalBullet;
        public GameObject spreadBullet;
        public GameObject ricochetSlowBullet;

        public Animator coreAAnim;
        public Animator coreBAnim;
        public Animator coreAShieldAnim;
        public Animator coreBShieldAnim;
        public Animator chargeUpAnim;

        private GameObject player;
        private Vector2 playerPosition;
        private AudioManager audioManager;

        [Header("General variables")]
        public float coreMoveTimeA;
        public float coreMoveTimeB;
        public Vector2 coreMoveSpeedA;
        public Vector2 coreMoveSpeedB;

        public Vector2 bodyMoveSpeed;

        public Vector2 bulletSpawnOffset;

        public _bossState[] phaseAAlgorithm;
        public _bossState[] phaseBAlgorithm;

        public Vector3 rotatedUnitVector;

        public string snipeLaserSoundName;
        public string ricochetSpreadSoundName;
        public string normalBulletSoundName;
        public string ricochetSlowBulletSoundName;
        public string ricochetInvulBulletSoundName;
        public string chargeUpSoundName;

        private float nextPhaseTime;
        private int phaseIndex;
        private float nextShoot1;
        private float nextShoot2;
        private float firstShot;
        private int currentShootQuantity;
        private int shooterIndex;

        private bool isFiringLaser = false;
        private bool moveBoss;

        [Header("Spawning variables")]
        public float spawnTime;

        [Header("Phase A1 variables")]
        public int maxShootQuantityA1;
        public int shotQuantityA1;
        public float deviationA1;
        public float shootIntervalA1;
        public float timeBeforeFirstShotA1;

        [Header("Phase A2 variables")]
        public int maxShootQuantityA2;
        public int shotQuantityA2;
        public float deviationA2;
        public float shootIntervalShortA2;
        public float shootIntervalLongA2;
        private int _shotQuantityA2;
        public Vector2 shootDirectionA2;
        private bool changeShootDirectionA2;
        private Quaternion _tempRotate;

        [Header("Phase A3 variables")]
        public int maxShootQuantityA3;
        public float shootIntervalSnipeA3;
        public float laserDurationA3;
        public float timeBeforeFirstLaserA3;
        public float timeBeforeFirstSnipeA3;

        [Header("Phase B initialization variables")]
        public float timeBeforeMove;
        public float rotateAccel;
        private bool rotateCoreB;
        private float rotateSpeed;

        [Header("Phase B1 variables")]
        public int maxShootQuantityB1;
        public int shotQuantityB1;
        public float deviationB1;
        public Vector2 shootDirectionB1;
        public float shootIntervalB1;
        public float timeBeforeFirstShotB1;

        [Header("Phase B2 variables")]
        public float phaseB2Time;
        public float shootIntervalSnipeB2;
        public float laserDurationB2;
        public float deviationB2;
        public float timeBeforeLaserB2;
        public float timeBeforeFirstShotB2;

        [Header("Phase B3 variables")]
        public float phaseB3Time;
        public float shootIntervalB3;
        public float deviationB3;
        public float timeBeforeFirstShotB3;


        [Header("Death variables")]
        public GameObject SmallExplosion;
        public GameObject MediumExplosion;
        public GameObject BigExplosion;
        public GameObject ExplosionArea;
        public string smallExplosionSoundName;
        public string mediumExplosionSoundName;
        public string bigExplosionSoundName;
        public float smallExplosionStartInterval;
        public float smallExplosionDecrement;

        public enum _bossState
        {
            SPAWNING,
            PHASE_A1,
            PHASE_A2,
            PHASE_A3,
            PHASE_B_INITIALIZE,
            PHASE_B1,
            PHASE_B2,
            PHASE_B3,
            DEAD
        }
        [Header("Current boss state")]
        public _bossState bossState;

        void Start()
        {
            //setting boss state when spawned
            bossState = _bossState.SPAWNING;
            //initialization
            coreBRB2D = coreB.GetComponent<Rigidbody2D>();
            nextPhaseTime = Time.time;
            currentShootQuantity = 0;
            phaseIndex = 0;
            nextShoot1 = -999;
            nextShoot2 = -999;
            firstShot = -999;
            changeShootDirectionA2 = true;
            moveBoss = false;

            audioManager = AudioManager.instance;
            flasher = GameObject.Find("Canvas").transform.Find("Flash").gameObject;

            if (audioManager == null)
            {
                Debug.LogError("AudioManager not found in the scene!!!");
            }
            try
            {
                player = GameObject.Find("Player").gameObject;
                playerPosition = new Vector2(player.transform.position.x, player.transform.position.y);
            }
            catch
            {
                //player is dead, do nothing
            }
            //set core states
            coreA.SetActive(true);
            coreB.SetActive(false);
        }

        void FixedUpdate()
        {
            //find player in case player is dead
            try
            {
                if (player == null)
                {
                    player = GameObject.Find("Player").gameObject;
                }
                playerPosition = new Vector2(player.transform.position.x, player.transform.position.y);
            }
            catch
            {
                //player is dead, do nothing
            }

            //add camera movement to boss
            if (moveBoss == true)
            {
                transform.position += new Vector3(Camera.main.GetComponent<CameraMovement>().moveSpeed.x, Camera.main.GetComponent<CameraMovement>().moveSpeed.y, 0);
            }

            //rotate core B
            if (rotateCoreB && coreB != null)
            {
                coreB.transform.parent.transform.Rotate(0, 0, rotateSpeed);
            }

            switch (bossState)
            {
                case (_bossState.SPAWNING):
                    if (Time.time > nextPhaseTime + spawnTime)
                    {
                        nextPhaseTime = Time.time;
                        //set boss state
                        bossState = phaseAAlgorithm[phaseIndex];
                        phaseIndex++;
                        //set core to be vulnerable
                        coreAShieldAnim.Play("BossCoreReveal");
                        coreA.transform.tag = "Enemy";
                        //change core phase
                        //core.GetComponent<RicochetCoreMovement>().enemyState = RicochetCoreMovement._enemyState.PHASE1;
                    }
                    break;

                case (_bossState.PHASE_A1):
                    //shooting algorithm
                    if (Time.time > nextShoot1 + shootIntervalA1 && Time.time > firstShot + timeBeforeFirstShotA1)
                    {
                        nextShoot1 = Time.time;
                        //increase shoot counter
                        currentShootQuantity++;

                        //find vector from enemy to player
                        Vector3 distToPlayer = new Vector3(playerPosition.x, playerPosition.y, transform.position.z) - outerShooters[shooterIndex].transform.position;
                        Vector3 spawnPosition = outerShooters[shooterIndex].transform.position + new Vector3(bulletSpawnOffset.x, bulletSpawnOffset.y, 0);
                        //shoot
                        ShootInDirection.ShootEqualSpread(ricochetBullet, shotQuantityA1, deviationA1, spawnPosition, distToPlayer, ricochetBullet.GetComponent<RicochetBulletMovement>().speed);
                        //play sound
                        audioManager.PlaySound(ricochetSpreadSoundName);
                        //increase shooter index to change where bullets are spawned
                        shooterIndex++;
                        if (shooterIndex >= outerShooters.Length)
                        {
                            shooterIndex = 0;
                        }
                    }

                    //phase changer
                    ChangePhaseOnBullet(maxShootQuantityA1, phaseAAlgorithm);

                    //check core hp for phase B
                    CheckCoreHP();
                    break;

                case (_bossState.PHASE_A2):
                    if (Time.time > nextShoot1 + shootIntervalShortA2 && Time.time > nextShoot2 + shootIntervalLongA2)
                    {
                        nextShoot1 = Time.time;

                        //change shoot direction after each 'wave' of shots
                        if (changeShootDirectionA2)
                        {
                            changeShootDirectionA2 = false;
                            _tempRotate = Quaternion.AngleAxis(Random.Range(-deviationA2, deviationA2), Vector3.forward);
                        }

                        //shoot a few times in mirrored directions for both shooters
                        Vector3 spawnPosition = outerShooters[0].transform.position + new Vector3(bulletSpawnOffset.x, bulletSpawnOffset.y, 0);
                        rotatedUnitVector = _tempRotate * shootDirectionA2.normalized;
                        ShootInDirection.Shoot(ricochetInvulBullet, 1, 0, spawnPosition, rotatedUnitVector.normalized, ricochetInvulBullet.GetComponent<RicochetBulletMovement>().speed, true);

                        spawnPosition = outerShooters[1].transform.position + new Vector3(bulletSpawnOffset.x, bulletSpawnOffset.y, 0);
                        rotatedUnitVector = Vector2.Reflect(rotatedUnitVector, Vector3.down);
                        ShootInDirection.Shoot(ricochetInvulBullet, 1, 0, spawnPosition, rotatedUnitVector.normalized, ricochetInvulBullet.GetComponent<RicochetBulletMovement>().speed, true);
                        //play sound
                        audioManager.PlaySound(ricochetInvulBulletSoundName);
                        // increase shoot quantity after each 'wave' of shots
                        _shotQuantityA2++;
                        if (_shotQuantityA2 >= shotQuantityA2)
                        {
                            nextShoot2 = Time.time;
                            _shotQuantityA2 = 0;
                            currentShootQuantity++;
                            changeShootDirectionA2 = true;
                        }
                    }

                    //phase changer
                    ChangePhaseOnBullet(maxShootQuantityA2, phaseAAlgorithm);

                    //check core hp for phase B
                    CheckCoreHP();
                    break;

                case (_bossState.PHASE_A3):
                    //laser shooter using Laser2D
                    if (!isFiringLaser && Time.time > nextShoot1 + timeBeforeFirstLaserA3)
                    {
                        isFiringLaser = true;
                        for (int i = 0; i < laser.Length; i++)
                        {
                            laser[i].GetComponent<LaserBeam>().rayDuration = laserDurationA3;
                            laser[i].GetComponent<LaserBeam>().canFire = true;
                        }
                        //play charge up sound
                        audioManager.PlaySound(chargeUpSoundName);
                    }

                    //snipe shooter
                    if (Time.time > nextShoot2 + shootIntervalSnipeA3 && Time.time > firstShot + timeBeforeFirstSnipeA3)
                    {
                        nextShoot2 = Time.time;

                        chargeUpAnim.Play("ChargeUp");
                        //snipe from core
                        StartCoroutine(ShootSnipeLaser());
                        //play charge up sound
                        audioManager.PlaySound(chargeUpSoundName);
                        //increasse shoot quantity counter
                        currentShootQuantity++;
                    }

                    //phase changer
                    ChangePhaseOnTime(laserDurationA3 + timeBeforeFirstLaserA3, phaseAAlgorithm);

                    //check core hp for phase B
                    CheckCoreHP();
                    break;

                case (_bossState.PHASE_B_INITIALIZE):
                    //accel core B rotation
                    rotateSpeed += rotateAccel;

                    if (Time.time > nextPhaseTime + timeBeforeMove)
                    {
                        //redirect camera
                        Camera.main.GetComponent<CameraMovement>().moveSpeed = new Vector2(-0.024f, 0f);
                        //set core to be vulnerable
                        coreBShieldAnim.Play("BossCoreReveal");
                        coreB.transform.tag = "Enemy";
                        //set boss state
                        bossState = phaseBAlgorithm[phaseIndex];
                        moveBoss = true;
                        phaseIndex++;
                    }
                    break;

                case (_bossState.PHASE_B1):
                    //shooting algorithm
                    if (Time.time > nextShoot1 + shootIntervalA1 && Time.time > firstShot + timeBeforeFirstShotB1)
                    {
                        nextShoot1 = Time.time;
                        //increase shoot counter
                        currentShootQuantity++;

                        if (shooterIndex == 0)
                        {
                            rotatedUnitVector = Quaternion.AngleAxis(Random.Range(-deviationB1, deviationB1), Vector3.forward) * shootDirectionB1.normalized;
                        }
                        else
                        {
                            rotatedUnitVector = Vector2.Reflect(rotatedUnitVector, Vector3.down);
                        }

                        //shoot at direction
                        Vector3 spawnPosition = outerShooters[shooterIndex].transform.position + new Vector3(bulletSpawnOffset.x, bulletSpawnOffset.y, 0);
                        ShootInDirection.ShootEqualSpread(ricochetBullet, shotQuantityB1, deviationB1, spawnPosition, rotatedUnitVector.normalized, ricochetBullet.GetComponent<RicochetBulletMovement>().speed);
                        //play sound
                        audioManager.PlaySound(ricochetSpreadSoundName);
                        //increase shooter index to change where bullets are spawned
                        shooterIndex++;
                        if (shooterIndex >= outerShooters.Length)
                        {
                            shooterIndex = 0;
                        }
                    }

                    //phase changer
                    ChangePhaseOnBullet(maxShootQuantityB1, phaseBAlgorithm);

                    //check if boss is dead
                    BossDead();
                    break;

                case (_bossState.PHASE_B2):
                    //laser shooter using Laser2D
                    if (!isFiringLaser && Time.time > nextShoot1 + timeBeforeLaserB2)
                    {
                        isFiringLaser = true;
                        for (int i = 0; i < laser.Length; i++)
                        {
                            laser[i].GetComponent<LaserBeam>().rayDuration = phaseB2Time;
                            laser[i].GetComponent<LaserBeam>().canFire = true;
                        }
                        //play charge up sound
                        audioManager.PlaySound(chargeUpSoundName);
                    }

                    //ricochet shooter
                    if (Time.time > nextShoot2 + shootIntervalSnipeB2 && Time.time > firstShot + timeBeforeFirstShotB2)
                    {
                        nextShoot2 = Time.time;
                        //shoot from core
                        Vector3 spawnPosition1 = coreB.transform.position;
                        Vector3 distToPlayer = new Vector3(playerPosition.x, playerPosition.y, transform.position.z) - coreB.transform.position;
                        ShootInDirection.Shoot(spreadBullet, 1, deviationB2, coreB.transform.position, distToPlayer, spreadBullet.GetComponent<RicochetBulletMovement>().speed, true);
                        //play sound
                        audioManager.PlaySound(ricochetSlowBulletSoundName);
                        //increase shoot quantity counter
                        currentShootQuantity++;
                    }

                    //phase changer
                    ChangePhaseOnTime(phaseB2Time + timeBeforeFirstShotB2, phaseBAlgorithm);

                    //check if boss is dead
                    BossDead();
                    break;

                case (_bossState.PHASE_B3):
                    //spread shooting
                    if (Time.time > nextShoot1 + shootIntervalB3 && Time.time > firstShot + timeBeforeFirstShotB3)
                    {
                        nextShoot1 = Time.time;
                        //shoot from core
                        Vector3 spawnPosition = coreB.transform.position;
                        ShootInDirection.Shoot(spreadBullet, 1, deviationB3, spawnPosition, Vector3.left, spreadBullet.GetComponent<RicochetBulletMovement>().speed, false);
                        //play sound
                        audioManager.PlaySound(ricochetSlowBulletSoundName);
                    }

                    //phase changer
                    ChangePhaseOnTime(phaseB3Time + timeBeforeFirstShotB3, phaseBAlgorithm);

                    //check if boss is dead
                    BossDead();
                    break;

                case (_bossState.DEAD):
                    break;
            }
        }

        IEnumerator ShootSnipeLaser()
        {
            yield return new WaitForSeconds(19f / 60f);
            //snipe from core
            Vector3 spawnPosition1 = coreA.transform.position;
            Vector3 distToPlayer = new Vector3(playerPosition.x, playerPosition.y, transform.position.z) - coreA.transform.position;

            ShootInDirection.Shoot(snipeLaser, 1, 0, coreA.transform.position, distToPlayer, snipeLaser.GetComponent<EnemyBullet2Movement>().moveSpeed, true);
            //play snipe sound
            audioManager.PlaySound(snipeLaserSoundName);
        }

        private void ChangePhaseOnBullet(int _maxShootQuantity, _bossState[] phaseAlgorithm)
        {
            if (currentShootQuantity >= _maxShootQuantity)
            {
                currentShootQuantity = 0;
                shooterIndex = 0;
                bossState = phaseAlgorithm[phaseIndex];
                phaseIndex++;
                if (phaseIndex >= phaseAlgorithm.Length)
                {
                    phaseIndex = 0;
                }
                nextShoot1 = Time.time;
                nextShoot2 = Time.time;
                firstShot = Time.time;
                nextPhaseTime = Time.time;
                isFiringLaser = false;
                for (int i = 0; i < laser.Length; i++)
                {
                    laser[i].GetComponent<LaserBeam>().canFire = false;
                    laser[i].GetComponent<LaserBeam>().laserOn = false;
                }
            }
        }

        private void ChangePhaseOnTime(float phaseTime, _bossState[] phaseAlgorithm)
        {
            if (Time.time > nextPhaseTime + phaseTime)
            {
                currentShootQuantity = 0;
                shooterIndex = 0;
                bossState = phaseAlgorithm[phaseIndex];
                phaseIndex++;
                if (phaseIndex >= phaseAlgorithm.Length)
                {
                    phaseIndex = 0;
                }
                nextShoot1 = Time.time;
                nextShoot2 = Time.time;
                firstShot = Time.time;
                nextPhaseTime = Time.time;
                isFiringLaser = false;
                for (int i = 0; i < laser.Length; i++)
                {
                    laser[i].GetComponent<LaserBeam>().canFire = false;
                    laser[i].GetComponent<LaserBeam>().laserOn = false;
                }
            }
        }

        private void CheckCoreHP()
        {
            //initialization for phase B
            if (coreA == null)
            {
                bossState = _bossState.PHASE_B_INITIALIZE;
                currentShootQuantity = 0;
                shooterIndex = 0;
                phaseIndex = 0;
                nextShoot1 = 0;
                nextShoot2 = 0;
                rotateCoreB = true;
                for (int i = 0; i < laser.Length; i++)
                {
                    laser[i].GetComponent<LaserBeam>().canFire = false;
                    laser[i].GetComponent<LaserBeam>().laserOn = false;
                    laser[i].GetComponent<LaserBeam>().CancelInvoke();
                }
                if (isFiringLaser)
                {
                    isFiringLaser = false;
                    for (int i = 0; i < laser.Length; i++)
                    {
                        laser[i].GetComponent<LaserBeam>().stopFiring();
                    }
                }

                nextPhaseTime = Time.time;

                //add explosions and stuff
                coreB.SetActive(true);
            }
        }

        private void BossDead()
        {
            if (coreB == null)
            {
                bossState = _bossState.DEAD;
                moveBoss = false;
                for (int i = 0; i < laser.Length; i++)
                {
                    laser[i].GetComponent<LaserBeam>().canFire = false;
                    laser[i].GetComponent<LaserBeam>().laserOn = false;
                    laser[i].GetComponent<LaserBeam>().CancelInvoke();
                }
                if (isFiringLaser)
                {
                    isFiringLaser = false;
                    for (int i = 0; i < laser.Length; i++)
                    {
                        laser[i].GetComponent<LaserBeam>().stopFiring();
                    }
                }
                Camera.main.GetComponent<CameraMovement>().moveSpeed = new Vector2(0, 0);

                //add explosions and stuff

                StartCoroutine(BossDie());
                StartCoroutine(RandomExplosions());
            }
        }

        IEnumerator BossDie()
        {
            yield return new WaitForSeconds(3);

            //explosions
            Transform EAT = ExplosionArea.transform;
            BoxCollider2D EABC2D = ExplosionArea.GetComponent<BoxCollider2D>();

            for (int i = 0; i < 12; i++)
            {
                Instantiate(BigExplosion, transform.position + new Vector3(Random.Range(-EABC2D.size.x / 2, EABC2D.size.x / 2), Random.Range(-EABC2D.size.y / 2, EABC2D.size.y / 2), 0), Quaternion.Euler(0, 0, Random.Range(0, 359)));
            }

            audioManager.PlaySound(bigExplosionSoundName);
            flasher.GetComponent<ScreenFadeWhite>().FadeOutWhite();
            Camera.main.GetComponent<ScreenShake>().Shake();

            Destroy(gameObject);
            //change to ship level
            Instantiate(sceneChanger, transform.position, transform.rotation);
            //stop boss music
            audioManager.StopMusic();
        }

        IEnumerator RandomExplosions()
        {
            Transform EAT = ExplosionArea.transform;
            BoxCollider2D EABC2D = ExplosionArea.GetComponent<BoxCollider2D>();

            while (true)
            {
                yield return new WaitForSeconds(smallExplosionStartInterval);

                Instantiate(SmallExplosion, transform.position + new Vector3(Random.Range(-EABC2D.size.x / 2, EABC2D.size.x / 2), Random.Range(-EABC2D.size.y / 2, EABC2D.size.y / 2), 0), Quaternion.Euler(0, 0, Random.Range(0, 359)));
                audioManager.PlaySound(smallExplosionSoundName);

                smallExplosionStartInterval -= smallExplosionDecrement;
                if (smallExplosionStartInterval <= 0.05)
                {
                    smallExplosionStartInterval = 0.05f;
                }
            }
        }
    }
}
