using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Player : MonoBehaviour {
    Rigidbody rb;
    public int sensitivity;
    public int rotationSpeed;
    public Vector3 rot;
    Vector3 direction;
    public Vector3 faceDirection;
    GameObject arm;
    List<GameObject> bulletPool;
    List<GameObject> bulletPoolAdd;
    List<GameObject> bulletPoolSubtract;
    List<GameObject> bulletPoolMultiply;
    List<GameObject> bulletPoolDivide;
    List<GameObject> bulletPoolExponent;

    List<GameObject> bulletPoolRed;
    List<GameObject> bulletPoolGreen;
    List<GameObject> bulletPoolBlue;
    List<GameObject> bulletPoolWhite;

    int bulletMaxCount;
    GameObject nozzle;
    int bulletForce;
    float nextFire;
    float fireRate;
    Button hbutton;
    Button jbutton;
    Button kbutton;
    Button lbutton;
    Button semicolonbutton;
    Button ybutton;
    Button ubutton;
    Button ibutton;
    Button obutton;
    Button pbutton;
    
    GameObject manager;
    public Parser parserScript;

    bool moving;
    bool meleeing;
    bool shooting;
    public int comboCount;
    public float comboStart;
    public float comboNext;
    private int maxCombo;

    Animator anim;

    public List<String> runePool;
    List<String> runeYCache;
    List<String> runeUCache;
    List<String> runeICache;
    List<String> runeOCache;
    List<String> runePCache;

    // Use this for initialization
    void Awake () {
        parserScript = manager.GetComponent<Parser>();
        rb = GetComponent<Rigidbody>();
        nozzle = GameObject.Find("Nozzle");

        bulletPoolAdd = new List<GameObject>();
        bulletPoolSubtract = new List<GameObject>();
        bulletPoolMultiply = new List<GameObject>();
        bulletPoolDivide = new List<GameObject>();
        bulletPoolExponent = new List<GameObject>();

        bulletPoolRed = new List<GameObject>();
        bulletPoolGreen = new List<GameObject>();
        bulletPoolBlue = new List<GameObject>();
        bulletPoolWhite = new List<GameObject>();

        SetupBulletPool(bulletPoolAdd, "BulletAdd", 10);
        SetupBulletPool(bulletPoolSubtract, "BulletSubtract", 10);
        SetupBulletPool(bulletPoolMultiply, "BulletMultiply", 10);
        SetupBulletPool(bulletPoolDivide, "BulletDivide", 10);
        SetupBulletPool(bulletPoolExponent, "BulletExponent", 10);

        SetupBulletPool(bulletPoolRed, "BulletRed", 10);
        SetupBulletPool(bulletPoolGreen, "BulletGreen", 10);
        SetupBulletPool(bulletPoolBlue, "BulletBlue", 10);
        SetupBulletPool(bulletPoolWhite, "BulletWhite", 10);

        Transform tempTrans;
        for (int i = 0; i < transform.childCount; i++) {
            tempTrans = transform.GetChild(i);
            if (tempTrans.gameObject.name.Equals("Arm")) {
                arm = tempTrans.gameObject;
            }
        }

        fireRate = .1f;
        bulletForce = 20;
        //sensitivity = 10;

        manager = GameObject.Find("Manager");

        hbutton = GameObject.Find("HButton").GetComponent<Button>();
        jbutton = GameObject.Find("JButton").GetComponent<Button>();
        kbutton = GameObject.Find("KButton").GetComponent<Button>();
        lbutton = GameObject.Find("LButton").GetComponent<Button>();
        semicolonbutton = GameObject.Find("SemiColonButton").GetComponent<Button>();

        ybutton = GameObject.Find("YButton").GetComponent<Button>();
        ubutton = GameObject.Find("UButton").GetComponent<Button>();
        ibutton = GameObject.Find("IButton").GetComponent<Button>();
        obutton = GameObject.Find("OButton").GetComponent<Button>();
        pbutton = GameObject.Find("PButton").GetComponent<Button>();

        comboStart = 0f;
        comboNext = .25f;
        maxCombo = 3;

        anim = gameObject.GetComponent<Animator>();

        AddToRuneCache(runeYCache, "^");
        AddToRuneCache(runeYCache, "+");
    }
	
	// Update is called once per frame
	void Update () {
        //MoveWithAddForce();
        //MoveWithTranslate();
        if (!meleeing) {
            MoveWithMovePosition();
        }
        Attacks();
        Shooting();

        //Reset timing
        if (Time.time > comboStart + comboNext)
        {
            comboStart = Time.time;
            comboCount = 0;
            //Debug.Log("MeleeCombo(): Reset");
            
        }

    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy")) {
            rb.AddExplosionForce(1000f, transform.position, 10f, 10f, ForceMode.Impulse);
        }
    }


    public void UpdateParserScript() {
        manager = GameObject.Find("Manager");
        parserScript = manager.GetComponent<Parser>();
    }

    void SetupBulletPool(List<GameObject> pool, string graphic, int bulletCount) {
        for (int i = 0; i < bulletCount; i++)
        {
            GameObject temp = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/"+graphic));
            temp.transform.localPosition = Vector3.zero;
            temp.SetActive(false);
            pool.Add(temp);
        }
    }

    void MoveWithMovePosition()
    {
        direction = Vector3.zero;
        
        if (Input.GetKey(KeyCode.W))
        {
            moving = true;
            comboCount = 0;
            direction = direction + Vector3.forward * sensitivity * Time.deltaTime;
            faceDirection = direction;
            transform.LookAt(transform.position + rb.velocity * sensitivity * Time.deltaTime*100);
        }
        if (Input.GetKey(KeyCode.S))
        {
            moving = true;
            comboCount = 0;
            direction = direction + Vector3.back * sensitivity * Time.deltaTime;
            faceDirection = direction;
            //transform.LookAt(faceDirection);
            transform.LookAt(transform.position + rb.velocity * sensitivity * Time.deltaTime * 100);
        }
        if (Input.GetKey(KeyCode.A))
        {
            moving = true;
            comboCount = 0;
            direction = direction + Vector3.left * sensitivity * Time.deltaTime;
            faceDirection = direction;
            //transform.LookAt(faceDirection);
            transform.LookAt(transform.position + rb.velocity * sensitivity * Time.deltaTime * 100);
        }
        if (Input.GetKey(KeyCode.D))
        {
            moving = true;
            comboCount = 0;
            direction = direction + Vector3.right * sensitivity * Time.deltaTime;
            faceDirection = direction;
            //transform.LookAt(faceDirection);
            transform.LookAt(transform.position + rb.velocity * sensitivity * Time.deltaTime * 100);
        }
        rb.MovePosition(transform.position + direction);
    }

    void Attacks() {
        //if (Input.GetKeyDown(KeyCode.X)) {
        //    Punch();            
        //}
        //if (Input.GetKeyUp(KeyCode.X)) {
        //    RetractPunch();
        //}
        //MeleeCombo();


    }

    void Punch(int type) {
        switch (type) {
            case 0:
                //arm.transform.localScale = new Vector3(3f, 1f, 1f);                
                anim.Play("SwordSwing");
                //anim.Play("SwordThrust");
                //arm.transform.Rotate(new Vector3(1f, 10f, 1f));
                //rb.MovePosition(transform.position + direction*10f);
                break;
            case 1:
                //arm.transform.localScale = new Vector3(1f, 1f, 2f);
                //direction = direction + Vector3.forward * sensitivity * 10f * Time.deltaTime;
                anim.Play("SwordThrust");
                //Debug.Log("ThrustSword");
                //rb.MovePosition(transform.position + faceDirection * 10f);

                //transform.SetPositionAndRotation(transform.position + direction * 10f, transform.rotation);
                //arm.transform.Rotate(new Vector3(10f, 1f, 1f));
                break;
            case 2:
                //arm.transform.localScale = new Vector3(3f, 1f, 1f);
                //arm.transform.Rotate(new Vector3(1f, 1f, 180f));
                //arm.transform.localScale = new Vector3(2f, 2f, 8f);
                //arm.transform.localScale = new Vector3(2f, 2f, 8f);
                anim.Play("SwordPound");
                break;
            default:
                break;
        }
        
    }

    void RetractPunch() {
        arm.transform.localScale = new Vector3(.35f, 1f, 1f);
    }

    //void Punch2() {
    //    Debug.Log("Punch2()");
    //    Quaternion neededRotation = Quaternion.Euler(rot);
    //    arm.transform.localScale = new Vector3(1f, 1f, 1f);
    //    //arm.transform.rotation = Quaternion.RotateTowards(arm.transform.rotation, neededRotation, Time.deltaTime);
    //    //arm.transform.Rotate(rot);
    //    arm.transform.rotation = Quaternion.Euler(Vector3.Lerp(arm.transform.rotation.eulerAngles, rot, Time.deltaTime * rotationSpeed));
    //    Debug.Log(arm.transform.rotation);
    //}

    //void RetractPunch2() {
    //    arm.transform.localScale = new Vector3(.35f, 1f, 1f);
    //}

    //void Kick() {
    //    arm.transform.localScale = new Vector3(1f, 1f, 1f);

    //}

    void Shooting() {
        //ShootAdd();
        //ShootSubtract();
        //ShootMultiply();
        //ShootDivide();
        //ShootExponent();
        ShootRed();
        //ShootGreen();
        ShootBlue();
        ShootWhite();
        ShootBomb();
        SelectAdd();
        SelectSubtract();
        SelectMultiply();
        SelectDivide();
        SelectExponent();
        StartCombo();
    }

    void ShootRed() 
    {
        if (Input.GetKey(KeyCode.Semicolon) && Time.time > comboStart)
        {
            comboStart = Time.time + comboNext;
            hbutton.Select();
            //Shoot(bulletPoolRed);
            Punch(1);
            parserScript.UpdateLeaves(true, false, false, false);
        }
        else if (Input.GetKeyUp(KeyCode.Semicolon))
        {
            EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(null);
        }
    }
    //void ShootGreen()
    void StartCombo()
    {
        //need a combo structure here
        if (Input.GetKeyDown(KeyCode.J) && Time.time > comboStart)
        {
            meleeing = true;
            comboStart = Time.time + comboNext;
            jbutton.Select();
            //Shoot(bulletPoolGreen);
            //Punch();
            //MeleeCombo();
            Punch(0);
            parserScript.UpdateLeaves(false, true, false, false);
        }
        else if (Input.GetKeyUp(KeyCode.J))
        {
            meleeing = false;
            RetractPunch();
            EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(null);
        }
    }

    void MeleeCombo() {
        if (comboCount == 0)
        {            
            Debug.Log("MeleeCombo(): " + comboCount);
            comboStart = Time.time;
            Punch(comboCount);
            comboCount++;
        }
        else if (comboCount == maxCombo-1) {
            Punch(comboCount);
            Debug.Log("MeleeCombo(): Max Combo");
            comboCount = 0;
        }
        else if (comboCount >= 1)
        {
            //if (Time.time <= comboStart + comboNext)
            //{
            
            //    comboCount++;
            //    comboStart = Time.time;
            //    Punch(comboCount);
            //}
            //else
            //{

            //comboCount = 0;
            Punch(comboCount);
            Debug.Log("MeleeCombo(): " + comboCount);
            comboCount++;
            //}
        }

    }

    //void ComboStart() {
    //    comboStart = Time.time;
        
    //}

    void ShootBlue()
    {
        if (Input.GetKey(KeyCode.K) && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            kbutton.Select();
            Shoot(bulletPoolBlue);
            parserScript.UpdateLeaves(false, false, true, false);
        }
        else if (Input.GetKeyUp(KeyCode.K))
        {
            EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(null);
        }
    }
    void ShootWhite()
    {
        if (Input.GetKey(KeyCode.L) && Time.time > comboStart)
        {
            comboStart = Time.time + comboNext;
            lbutton.Select();
            //Shoot(bulletPoolWhite);
            Punch(2);
            parserScript.UpdateLeaves(false, false, false, true);
        }
        else if (Input.GetKeyUp(KeyCode.L))
        {
            EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(null);
        }
    }
    void ShootBomb()
    {
        if (Input.GetKey(KeyCode.Semicolon) && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            semicolonbutton.Select();
            //Shoot(bulletPoolWhite);
            //parserScript.UpdateLeaves(false, false, false, true);
        }
        else if (Input.GetKeyUp(KeyCode.L))
        {
            EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(null);
        }
    }

    void Shoot(List<GameObject> pool) {
        for (int i = pool.Count - 1; i >= 0; i--)
        {
            if (!pool[i].activeSelf)
            {
                pool[i].SetActive(true);
                //Debug.Log("Shoot()");
                BulletScript bs = pool[i].GetComponent<BulletScript>();
                bs.SetDirection(faceDirection); ;
                bs.SetInitialPosition(transform.position);
                bs.SetRotation(transform.rotation);
                i = 0;
            }
        }
    }

    void SelectExponent()
    {
        if (Input.GetKey(KeyCode.Y))
        {
            ybutton.Select();
            try
            {
                NodeScript ns = parserScript.FindFirstNodeWithOnlyLeaves("^");
                if (ns.AreAllColorValuesZeroOrLessOfMyChildren())
                {
                    ns.Solve();
                }
            }
            catch (Exception e) {
                Debug.Log("No ^ with just leaves found. Error: " + e);
            }

        }
        else if (Input.GetKeyUp(KeyCode.Y)) {
            EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(null);
        }
    }

    void SelectSubtract()
    {
        if (Input.GetKey(KeyCode.U))
        {
            ubutton.Select();
            try
            {
                NodeScript ns = parserScript.FindFirstNodeWithOnlyLeaves("-");
                if (ns.AreAllColorValuesZeroOrLessOfMyChildren())
                {
                    ns.Solve();
                }
            }
            catch (Exception e)
            {
                Debug.Log("No - with just leaves found. Error: " + e);
            }
        }
        else if (Input.GetKeyUp(KeyCode.U))
        {
            EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(null);
        }
    }

    void SelectMultiply()
    {
        if (Input.GetKey(KeyCode.I))
        {
            ibutton.Select();
            try
            {
                NodeScript ns = parserScript.FindFirstNodeWithOnlyLeaves("*");
                if (ns.AreAllColorValuesZeroOrLessOfMyChildren())
                {
                    ns.Solve();
                }
            }
            catch (Exception e)
            {
                Debug.Log("No * with just leaves found. Error: " + e);
            }
        }
        else if (Input.GetKeyUp(KeyCode.I))
        {
            EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(null);
        }
    }

    void SelectDivide()
    {
        if (Input.GetKey(KeyCode.O))
        {
            obutton.Select();
            try
            {
                NodeScript ns = parserScript.FindFirstNodeWithOnlyLeaves("/");
                if (ns.AreAllColorValuesZeroOrLessOfMyChildren())
                {
                    ns.Solve();
                }
            }
            catch (Exception e)
            {
                Debug.Log("No / with just leaves found. Error: " + e);
            }
        }
        else if (Input.GetKeyUp(KeyCode.O))
        {
            EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(null);
        }
    }

    void SelectAdd()
    {
        if (Input.GetKey(KeyCode.P))
        {
            pbutton.Select();
            try
            {
                NodeScript ns = parserScript.FindFirstNodeWithOnlyLeaves("+");
                if (ns.AreAllColorValuesZeroOrLessOfMyChildren())
                {
                    ns.Solve();
                }
            }
            catch (Exception e)
            {
                Debug.Log("No + with just leaves found. Error: " + e);
            }
        }
        else if (Input.GetKeyUp(KeyCode.P))
        {
            EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(null);
        }
    }

    public void AddToRunePool(String op) {
        runePool.Add(op);
    }

    public Boolean CheckIfInRunePool(String op) {
        return runePool.Contains(op);
    }

    public void AddToRuneCache(List<String> cache, String op) {
        cache.Add(op);
    }

    public Boolean CheckIfAllRunesInCacheIsInRunePool(List<String> cache) {
        Boolean runesAreAllPresent = true;
        for (int i = 0; i < cache.Count; i++) {
            if (!runePool.Contains(cache[i])){
                runesAreAllPresent = false;
                break;
            }
            runesAreAllPresent = true;
        }
        return runesAreAllPresent;
    }

    //have to revamp how the parser gets solved (since we want to also consume operations?
}
