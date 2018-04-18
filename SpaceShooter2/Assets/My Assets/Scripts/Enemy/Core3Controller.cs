using System.Collections;
using UnityEngine;

namespace Kontrol
{
    public class Core3Controller : MonoBehaviour
    {
        public GameObject sceneChanger;
        public GameObject flasher;

        public GameObject[] core;
        public GameObject[] coreShield;
        public GameObject[] blade;
        public GameObject[] shooters;

        public Animator[] coreAnim;
        public Animator[] coreShieldAnim;
        public Animator[] bladeAnim;
        public Animator[] shooterAnim;
        private Animator bossBodyAnim;

        public GameObject laser;
        public GameObject normalBullet;
        public GameObject accelBullet;

        public GameObject ShootRecoil;

        private GameObject player;
        private Vector2 playerPosition;

        private AudioManager audioManager;

        [Header("General variables")]
        public _bossState[] phaseAlgorithm;

        public Vector2 shooterBulletSpawnOffset;
        public Vector2 bladeShootSpawnOffset;
        public Vector2 bossMovement;
        private Vector3 initialBossPosition;
        private Vector2 _bossMovement;

        public string introSoundName;
        public string normalBulletSoundName;
        public string spreadBulletSoundName;
        public string laserSoundName;
        public string accelBulletSoundName;
        public string bladeOpenSoundName;
        public string bladeCloseSoundName;

        private float initialCoreTime;
        private float nextPhaseTime;
        private int phaseIndex;
        private float nextShoot1;
        private float nextShoot2;
        private float firstShot;
        private int currentShootQuantity;
        private int shooterIndex;
        private float bladeTime;

        private bool moveBoss;
        private bool isCoreVulnerable;

        [Header("Spawning variables")]
        public float spawnTime;
        public float coreInvulTime;

        [Header("Phase 1 variables")]
        public int maxShootQuantity1;
        public int shotQuantity1;
        public float bulletSpeed1;
        public float deviation1;
        public float shootInterval1;
        public float timeBeforeFirstShot1;

        [Header("Phase 2 variables")]
        public int maxShootQuantity2;
        public float bulletSpeed2;
        public float deviation2;
        public float shootInterval2;
        private int _shotQuantity2;
        public float timeBeforeFirstShot2;
        public float timeBeforeBladesOpen;
        private bool isBladeOpen;

        [Header("Phase 3 variables")]
        public GameObject LaserSpawnArea;
        public int maxShootQuantity3;
        public float shootIntervalAccel3;
        public float shootIntervalLaser3;
        public float timeBeforeFirstLaser3;
        public float timeBeforeFirstAccel3;
        public float accelBulletSpeed;
        public float sinSpeed;
        public float sinAmplitude;
        private float sinCounter;

        [Header("Death variables")]
        public GameObject SmallExplosion;
        public GameObject MediumExplosion;
        public GameObject BigExplosion;
        public GameObject TopBladeExplosionArea;
        public GameObject BottomBladeExplosionArea;
        public string smallExplosionSoundName;
        public string mediumExplosionSoundName;
        public string bigExplosionSoundName;
        public float smallExplosionStartInterval;
        public float smallExplosionDecrement;
        public float smallExplosionRadius;

        public enum _bossState
        {
            SPAWNING,
            PHASE_1,
            PHASE_2,
            PHASE_3,
            DEAD,
            DEBUG_DEAD
        }

        [Header("Current boss state")]
        public _bossState bossState;

        void Start()
        {
            //setting boss state when spawned
            bossState = _bossState.SPAWNING;
            nextPhaseTime = Time.time;
            initialCoreTime = Time.time;
            currentShootQuantity = 0;
            phaseIndex = 0;
            nextShoot1 = -999;
            nextShoot2 = -999;
            firstShot = -999;
            sinCounter = 0;
            moveBoss = false;
            isCoreVulnerable = false;
            _bossMovement = bossMovement;
            isBladeOpen = false;

            //disable colliders during spawning
            gameObject.GetComponent<PolygonCollider2D>().enabled = false;
            for (int i = 0; i < core.Length; i++)
            {
                core[i].GetComponent<PolygonCollider2D>().enabled = false;
            }
            for (int i = 0; i < blade.Length; i++)
            {
                blade[i].GetComponent<PolygonCollider2D>().enabled = false;
            }

            //animators
            bossBodyAnim = GetComponent<Animator>();
            for (int i = 0; i < core.Length; i++)
            {
                coreAnim[i] = core[i].GetComponent<Animator>();
                coreShieldAnim[i] = coreShield[i].GetComponent<Animator>();
            }
            for (int i = 0; i < blade.Length; i++)
            {
                bladeAnim[i] = blade[i].GetComponent<Animator>();
                shooterAnim[i] = shooters[i].GetComponent<Animator>();
            }

            bossBodyAnim.Play("IntroBossAnim");
            for (int i = 0; i < coreAnim.Length; i++)
            {
                coreAnim[i].Play("UmletCoreIntro");
                coreShieldAnim[i].Play("UmletCoreShieldIntro");
            }
            bladeAnim[0].Play("UmletBladeTopIntro");
            bladeAnim[1].Play("UmletBladeBottomIntro");
            shooterAnim[0].Play("UmletShooterTopIntro");
            shooterAnim[1].Play("UmletShooterBottomIntro");

            audioManager = AudioManager.instance;
            if (audioManager == null)
            {
                Debug.LogError("AudioManager not found in the scene!!!");
            }

            flasher = GameObject.Find("Canvas").transform.Find("Flash").gameObject;

            audioManager.PlaySound(introSoundName);

            try
            {
                player = GameObject.Find("Player").gameObject;
                playerPosition = new Vector2(player.transform.position.x, player.transform.position.y);
            }
            catch
            {
                //player is dead, do nothing
            }

            //make core invulnerable
            for (int i = 0; i < core.Length; i++)
            {
                core[i].transform.tag = "InvulEnemy";
            }
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

            //move boss
            //float sinMovement = sinAmplitude * Easing.Sinusoidal.InOut(sinCounter);
            if (moveBoss == true)
            {
                transform.position = new Vector3(_bossMovement.x + transform.position.x, _bossMovement.y + transform.position.y, 0);
                //change y direction
                if (transform.position.y >= 3.5f + Camera.main.transform.position.y || transform.position.y >= 1.0f + playerPosition.y)
                {
                    _bossMovement.y = -bossMovement.y * Random.Range(0.7f, 1f);
                }
                else if (transform.position.y <= -3.5f + Camera.main.transform.position.y || transform.position.y <= -1.0f + playerPosition.y)
                {
                    _bossMovement.y = bossMovement.y * Random.Range(0.7f, 1f);
                }

                //change x direction
                if (transform.position.x >= 5f + Camera.main.transform.position.x)
                {
                    _bossMovement.x = -bossMovement.x * Random.Range(0.7f, 1f);
                }
                else if (transform.position.x <= 3f + Camera.main.transform.position.x)
                {
                    _bossMovement.x = bossMovement.x * Random.Range(0.7f, 1f);
                }
            }

            //make cores vulnerable
            if (Time.time > initialCoreTime + coreInvulTime && !isCoreVulnerable)
            {
                isCoreVulnerable = true;
                for (int i = 0; i < coreShieldAnim.Length; i++)
                {
                    coreShieldAnim[i].enabled = true;
                    core[i].transform.tag = "Enemy";
                }
            }

            switch (bossState)
            {
                case (_bossState.SPAWNING):
                    //to change boss phase
                    if (Time.time > nextPhaseTime + spawnTime)
                    {
                        nextPhaseTime = Time.time;
                        //set boss state
                        bossState = phaseAlgorithm[phaseIndex];
                        phaseIndex++;
                        //let boss move in vertical direction
                        moveBoss = true;
                        initialBossPosition = transform.position;

                        bossBodyAnim.enabled = false;
                        for (int i = 0; i < coreShieldAnim.Length; i++)
                        {
                            coreShieldAnim[i].Play("BossCoreReveal");
                        }

                        //enable colliders after spawning
                        gameObject.GetComponent<PolygonCollider2D>().enabled = true;
                        for (int i = 0; i < core.Length; i++)
                        {
                            core[i].GetComponent<PolygonCollider2D>().enabled = true;
                        }
                        for (int i = 0; i < blade.Length; i++)
                        {
                            blade[i].GetComponent<PolygonCollider2D>().enabled = true;
                        }
                    }
                    break;

                case (_bossState.PHASE_1):
                    //shooting algorithm
                    if (Time.time > nextShoot1 + shootInterval1 && Time.time > firstShot + timeBeforeFirstShot1)
                    {
                        //check if shooterIndex is overflowing before shooting
                        if (shooterIndex >= core.Length)
                        {
                            shooterIndex = 0;
                        }
                        //if current selected core is dead, move to next core
                        int coreCursor = 0;
                        while (core[shooterIndex] == null && coreCursor < core.Length)
                        {
                            shooterIndex++;
                            //check if shooterIndex is overflowing before shooting
                            if (shooterIndex >= core.Length)
                            {
                                shooterIndex = 0;
                            }
                            //increase core cursor index
                            ++coreCursor;
                        }
                        nextShoot1 = Time.time;
                        //increase shoot counter for phase check and change
                        currentShootQuantity++;

                        //spread shot from cores to player
                        if (core[shooterIndex] != null)
                        {
                            Vector3 distToPlayer = new Vector3(playerPosition.x, playerPosition.y, transform.position.z) - core[shooterIndex].transform.position;
                            ShootInDirection.ShootEqualSpread(normalBullet, shotQuantity1, deviation1,
                                core[shooterIndex].transform.position, distToPlayer, bulletSpeed1);
                            audioManager.PlaySound(spreadBulletSoundName);

                            //increase shooter index to change where bullets are spawned
                            shooterIndex++;
                        }
                        else
                        {
                            Debug.Log("Core is dead");
                        }
                    }

                    //phase changer
                    ChangePhaseOnBullet(maxShootQuantity1, phaseAlgorithm);

                    //check if all cores are still alive
                    BossDead();
                    break;

                case (_bossState.PHASE_2):

                    if (!isBladeOpen && Time.time > timeBeforeBladesOpen + bladeTime)
                    {
                        isBladeOpen = true;
                        bladeAnim[0].Play("UmletBladeTopOpen");
                        bladeAnim[1].Play("UmletBladeBottomOpen");
                        audioManager.PlaySound(bladeOpenSoundName);
                    }

                    //shooting algorithm
                    if (Time.time > nextShoot1 + shootInterval2 && Time.time > firstShot + timeBeforeFirstShot2)
                    {
                        nextShoot1 = Time.time;

                        //shoot once in general direction of player for both shooters
                        for (int i = 0; i < shooters.Length; i++)
                        {
                            Vector3 spawnPosition = shooters[i].transform.position + new Vector3(shooterBulletSpawnOffset.x, shooterBulletSpawnOffset.y, 0);
                            //find vector from enemy to player
                            Vector3 distToPlayer = new Vector3(playerPosition.x, playerPosition.y, transform.position.z) - shooters[shooterIndex].transform.position;
                            ShootInDirection.Shoot(normalBullet, 1, deviation2, spawnPosition, distToPlayer, bulletSpeed2, false);
                            Instantiate(ShootRecoil, spawnPosition, Quaternion.identity);
                        }
                        shooterAnim[0].Play("UmletShooterTopRecoil");
                        shooterAnim[1].Play("UmletShooterBottomRecoil");

                        //play sound
                        audioManager.PlaySound(normalBulletSoundName);
                        //increase shoot quantity
                        currentShootQuantity++;
                    }

                    //phase changer
                    if (ChangePhaseOnBullet(maxShootQuantity2, phaseAlgorithm))
                    {
                        isBladeOpen = false;
                        bladeAnim[0].Play("UmletBladeTopClose");
                        bladeAnim[1].Play("UmletBladeBottomClose");
                        audioManager.PlaySound(bladeCloseSoundName);
                    }

                    //check if all cores are still alive
                    BossDead();
                    break;

                case (_bossState.PHASE_3):
                    //movement
                    //sinCounter += sinSpeed;

                    //laser shooter at middle core
                    if (Time.time > nextShoot1 + shootIntervalLaser3 && Time.time > firstShot + timeBeforeFirstLaser3)
                    {
                        nextShoot1 = Time.time;

                        Vector3 spawnPosition = LaserSpawnArea.transform.position;
                        ShootInDirection.Shoot(laser, 1, 0, spawnPosition, Vector2.left, laser.GetComponent<LaserMovement2>().moveSpeed, false);


                        //play sound
                        audioManager.PlaySound(laserSoundName);
                    }

                    //accel shooter from outer cores
                    if (Time.time > nextShoot2 + shootIntervalAccel3 && Time.time > firstShot + timeBeforeFirstAccel3)
                    {
                        nextShoot2 = Time.time;
                        //accel from core
                        int _shooterIndex = 0;
                        while (_shooterIndex == 0 || _shooterIndex == 2)
                        {
                            if (core[_shooterIndex] != null)
                            {
                                Vector3 spawnPosition = core[_shooterIndex].transform.position;
                                Vector3 distToPlayer = new Vector3(playerPosition.x, playerPosition.y, transform.position.z) - core[_shooterIndex].transform.position;
                                //shoot accel bullet from core to player
                                ShootInDirection.Shoot(accelBullet, 1, 0, spawnPosition, distToPlayer.normalized, accelBulletSpeed, true);
                            }
                            //increase shooter index
                            _shooterIndex += 2;
                        }
                        //play sound
                        audioManager.PlaySound(accelBulletSoundName);
                        //increase shoot quantity counter
                        currentShootQuantity++;
                    }

                    //phase changer
                    ChangePhaseOnBullet(maxShootQuantity3, phaseAlgorithm);
                    //if (ChangePhaseOnBullet(maxShootQuantity3, phaseAlgorithm) && Mathf.Abs(sinMovement) >= 0.99)
                    //{
                    //    sinCounter = 0;
                    //}

                    //check if all cores are still alive
                    BossDead();
                    break;

                case (_bossState.DEAD):
                    break;

                case (_bossState.DEBUG_DEAD):
                    bossState = _bossState.DEAD;
                    moveBoss = false;
                    StartCoroutine(BossDie());
                    StartCoroutine(RandomExplosions());
                    break;
            }
        }

        private bool ChangePhaseOnBullet(int _maxShootQuantity, _bossState[] phaseAlgorithm)
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
                bladeTime = Time.time;
                return true;
            }
            else
            {
                return false;
            }
        }

        private void BossDead()
        {
            if (core[0] == null && core[1] == null && core[2] == null)
            {
                bossState = _bossState.DEAD;
                moveBoss = false;
                StartCoroutine(BossDie());
                StartCoroutine(RandomExplosions());
            }
        }

        IEnumerator BossDie()
        {
            if (bladeAnim[0].GetCurrentAnimatorStateInfo(0).IsName("UmletBladeTopOpen"))
            {
                bladeAnim[0].Play("UmletBladeTopClose");
            }
            if (bladeAnim[1].GetCurrentAnimatorStateInfo(0).IsName("UmletBladeBottomOpen"))
            {
                bladeAnim[1].Play("UmletBladeBottomClose");
            }
            blade[0].GetComponent<PolygonCollider2D>().enabled = false;
            blade[1].GetComponent<PolygonCollider2D>().enabled = false;
            yield return new WaitForSeconds(1);

            Instantiate(MediumExplosion, TopBladeExplosionArea.transform.position, Quaternion.Euler(0, 0, Random.Range(0, 359)));
            audioManager.PlaySound(mediumExplosionSoundName);
            bladeAnim[0].Play("UmletBladeTopDeath");
            yield return new WaitForSeconds(0.5f);
            Instantiate(MediumExplosion, BottomBladeExplosionArea.transform.position, Quaternion.Euler(0, 0, Random.Range(0, 359)));
            audioManager.PlaySound(mediumExplosionSoundName);
            bladeAnim[1].Play("UmletBladeBottomDeath");
            yield return new WaitForSeconds(2f);
            for (int i = 0; i < 12; i++)
            {
                Instantiate(BigExplosion, transform.position + Random.insideUnitSphere * smallExplosionRadius * 2, Quaternion.Euler(0, 0, Random.Range(0, 359)));
            }
            audioManager.PlaySound(bigExplosionSoundName);
            flasher.GetComponent<ScreenFadeWhite>().FadeOutWhite();
            Camera.main.GetComponent<ScreenShake>().Shake();

            Destroy(gameObject);
            Instantiate(sceneChanger, transform.position, transform.rotation);
            audioManager.StopMusic();
        }

        IEnumerator RandomExplosions()
        {
            while (true)
            {
                yield return new WaitForSeconds(smallExplosionStartInterval);

                Instantiate(SmallExplosion, transform.position + Random.insideUnitSphere * smallExplosionRadius, Quaternion.Euler(0, 0, Random.Range(0, 359)));
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