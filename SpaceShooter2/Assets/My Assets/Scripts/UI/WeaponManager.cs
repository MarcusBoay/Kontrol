using UnityEngine;
using UnityEngine.UI;

namespace Kontrol
{
    public class WeaponManager : MonoBehaviour
    {
        public static WeaponManager instance;

        public GameObject HUDWeapon1;
        public GameObject HUDWeapon2;
        public GameObject HUDWeapon3;
        private GameObject player;

        private enum _currentWeapon
        {
            WEAPON1,
            WEAPON2,
            WEAPON3,
        }
        private _currentWeapon currentWeapon;

        private void Awake()
        {
            if (instance != null)
            {
                if (instance != this)
                {
                    Destroy(gameObject);
                }
            }
            else
            {
                instance = this;
                DontDestroyOnLoad(this);
            }
        }

        private void Start()
        {
            try
            {
                player = GameObject.Find("Player").gameObject;
            }
            catch
            { }
            //setting start weapon
            currentWeapon = _currentWeapon.WEAPON1;
            HUDWeapon1.GetComponent<Image>().color = new Color(1, 1, 1, 1f);
            HUDWeapon2.GetComponent<Image>().color = new Color(1, 1, 1, 0.2f);
            HUDWeapon3.GetComponent<Image>().color = new Color(1, 1, 1, 0.2f);
            try
            {
                HUDWeapon1 = GameObject.Find("Canvas").transform.Find("Weapon Panel BG").transform.Find("Weapon Panel").transform.Find("HUD 1").gameObject;
                HUDWeapon2 = GameObject.Find("Canvas").transform.Find("Weapon Panel BG").transform.Find("Weapon Panel").transform.Find("HUD 2").gameObject;
                HUDWeapon3 = GameObject.Find("Canvas").transform.Find("Weapon Panel BG").transform.Find("Weapon Panel").transform.Find("HUD 3").gameObject;
            }
            catch
            {
            }
        }

        void Update()
        {
            try
            {
                player = GameObject.Find("Player").gameObject;
                HUDWeapon1 = GameObject.Find("Canvas").transform.Find("Weapon Panel BG").transform.Find("Weapon Panel").transform.Find("HUD 1").gameObject;
                HUDWeapon2 = GameObject.Find("Canvas").transform.Find("Weapon Panel BG").transform.Find("Weapon Panel").transform.Find("HUD 2").gameObject;
                HUDWeapon3 = GameObject.Find("Canvas").transform.Find("Weapon Panel BG").transform.Find("Weapon Panel").transform.Find("HUD 3").gameObject;
            }
            catch
            { }
            try
            {
                CheckWeapon();
                CheckUpgrade();
                UpdateUI();
            }
            catch
            { }
            if (GameStateMachine.myGameState == GameStateMachine.GameState.GAMEOVER)
            {
                //Destroy(this.gameObject);
            }
        }

        public void InitializePlayerWeapon()
        {
            currentWeapon = _currentWeapon.WEAPON1;
            HUDWeapon1 = GameObject.Find("Canvas").transform.Find("Weapon Panel BG").transform.Find("Weapon Panel").transform.Find("HUD 1").gameObject;
            HUDWeapon2 = GameObject.Find("Canvas").transform.Find("Weapon Panel BG").transform.Find("Weapon Panel").transform.Find("HUD 2").gameObject;
            HUDWeapon3 = GameObject.Find("Canvas").transform.Find("Weapon Panel BG").transform.Find("Weapon Panel").transform.Find("HUD 3").gameObject;
            HUDWeapon1.GetComponent<Image>().color = new Color(1, 1, 1, 1f);
            HUDWeapon2.GetComponent<Image>().color = new Color(1, 1, 1, 0.2f);
            HUDWeapon3.GetComponent<Image>().color = new Color(1, 1, 1, 0.2f);
        }

        //use this function when player dies
        public void TakeAwayWeapons()
        {
            for (int i = 0; i < WeaponUnlocks.unlocks.Length; i++)
            {
                if (i != 0 && WeaponUnlocks.unlocks[i] >= 1)
                    WeaponUnlocks.unlocks[i] -= 1;
                else if (i == 0 && WeaponUnlocks.unlocks[i] == 2)
                {
                    WeaponUnlocks.unlocks[i] -= 1;
                }
            }
        }

        private void CheckUpgrade()
        {
            if (currentWeapon == _currentWeapon.WEAPON1)
            {
                if (WeaponUnlocks.unlocks[0] == 1)
                    GameObject.Find("Player").gameObject.GetComponent<PlayerController>().myBulletType = PlayerController.BulletType.NORMAL1;
                else if (WeaponUnlocks.unlocks[0] == 2)
                    GameObject.Find("Player").gameObject.GetComponent<PlayerController>().myBulletType = PlayerController.BulletType.NORMAL2;
            }
            else if (currentWeapon == _currentWeapon.WEAPON2)
            {
                if (WeaponUnlocks.unlocks[1] == 1)
                    GameObject.Find("Player").gameObject.GetComponent<PlayerController>().myBulletType = PlayerController.BulletType.RICOCHET1;
                else if (WeaponUnlocks.unlocks[1] == 2)
                    GameObject.Find("Player").gameObject.GetComponent<PlayerController>().myBulletType = PlayerController.BulletType.RICOCHET2;
            }
            else if (currentWeapon == _currentWeapon.WEAPON3)
            {
                if (WeaponUnlocks.unlocks[2] == 1)
                    GameObject.Find("Player").gameObject.GetComponent<PlayerController>().myBulletType = PlayerController.BulletType.PENETRATE1;
                else if (WeaponUnlocks.unlocks[2] == 2)
                    GameObject.Find("Player").gameObject.GetComponent<PlayerController>().myBulletType = PlayerController.BulletType.PENETRATE2;
            }
        }

        private void CheckWeapon()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1) && currentWeapon != _currentWeapon.WEAPON1 && WeaponUnlocks.unlocks[0] != 0)
            {
                currentWeapon = _currentWeapon.WEAPON1;
                HUDWeapon1.GetComponent<Image>().color = new Color(1, 1, 1, 1f);
                HUDWeapon2.GetComponent<Image>().color = new Color(1, 1, 1, 0.2f);
                HUDWeapon3.GetComponent<Image>().color = new Color(1, 1, 1, 0.2f);
                if (WeaponUnlocks.unlocks[0] == 1)
                    player.GetComponent<PlayerController>().myBulletType = PlayerController.BulletType.NORMAL1;
                else if (WeaponUnlocks.unlocks[0] == 2)
                    player.GetComponent<PlayerController>().myBulletType = PlayerController.BulletType.NORMAL2;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2) && currentWeapon != _currentWeapon.WEAPON2 && WeaponUnlocks.unlocks[1] != 0)
            {
                currentWeapon = _currentWeapon.WEAPON2;
                HUDWeapon1.GetComponent<Image>().color = new Color(1, 1, 1, 0.2f);
                HUDWeapon2.GetComponent<Image>().color = new Color(1, 1, 1, 1f);
                HUDWeapon3.GetComponent<Image>().color = new Color(1, 1, 1, 0.2f);
                if (WeaponUnlocks.unlocks[0] == 1)
                    player.GetComponent<PlayerController>().myBulletType = PlayerController.BulletType.RICOCHET1;
                else if (WeaponUnlocks.unlocks[0] == 2)
                    player.GetComponent<PlayerController>().myBulletType = PlayerController.BulletType.RICOCHET2;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3) && currentWeapon != _currentWeapon.WEAPON3 && WeaponUnlocks.unlocks[2] != 0)
            {
                currentWeapon = _currentWeapon.WEAPON3;
                HUDWeapon1.GetComponent<Image>().color = new Color(1, 1, 1, 0.2f);
                HUDWeapon2.GetComponent<Image>().color = new Color(1, 1, 1, 0.2f);
                HUDWeapon3.GetComponent<Image>().color = new Color(1, 1, 1, 1f);
                if (WeaponUnlocks.unlocks[0] == 1)
                    player.GetComponent<PlayerController>().myBulletType = PlayerController.BulletType.PENETRATE1;
                else if (WeaponUnlocks.unlocks[0] == 2)
                    player.GetComponent<PlayerController>().myBulletType = PlayerController.BulletType.PENETRATE2;
            }
        }

        private void UpdateUI()
        {
            if (WeaponUnlocks.unlocks[0] >= 1)
            {
                HUDWeapon1.SetActive(true);
            }
            else
            {
                HUDWeapon1.SetActive(false);
            }
            if (WeaponUnlocks.unlocks[1] >= 1)
            {
                HUDWeapon2.SetActive(true);
            }
            else
            {
                HUDWeapon2.SetActive(false);
            }
            if (WeaponUnlocks.unlocks[2] >= 1)
            {
                HUDWeapon3.SetActive(true);
            }
            else
            {
                HUDWeapon3.SetActive(false);
            }
        }
    }
}
