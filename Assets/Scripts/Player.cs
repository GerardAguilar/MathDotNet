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

    public List<String> runePool;//current stock of runes
    List<String> runeYCache;//skill requirements
    List<String> runeUCache;
    List<String> runeICache;
    List<String> runeOCache;
    List<String> runePCache;

    SoundManager soundManager;
    NodeScript ns;

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

        runePool = new List<string>();
        runeYCache = new List<string>();
        runeUCache = new List<string>();
        runeICache = new List<string>();
        runeOCache = new List<string>();
        runePCache = new List<string>();

        //this is for charging
        //AddToRunePool(runePool, "^");
        //AddToRunePool(runePool, "+");

        //these are the skill activation requirements
        AddToRunePool(runeYCache, "^");
        AddToRunePool(runeYCache, "-");

        Debug.Log(IsAllOfCacheInPool(runePool, runeYCache));

        soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
    }

    private bool IsAllOfCacheInPool(List<string> pool, List<string> cache)
    {
        bool cacheInPool = true;
        for (int i = 0; i < cache.Count; i++) {
            if (!pool.Contains(cache[i])) {
                cacheInPool = false;
            }
        }
        return cacheInPool;
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
                soundManager.PlaySound(0);
                //anim.Play("SwordThrust");
                //arm.transform.Rotate(new Vector3(1f, 10f, 1f));
                //rb.MovePosition(transform.position + direction*10f);
                break;
            case 1:
                //arm.transform.localScale = new Vector3(1f, 1f, 2f);
                //direction = direction + Vector3.forward * sensitivity * 10f * Time.deltaTime;
                anim.Play("SwordThrust");
                soundManager.PlaySound(4);
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
                soundManager.PlaySound(3);
                break;
            case 3:
                //arm.transform.localScale = new Vector3(3f, 1f, 1f);
                //arm.transform.Rotate(new Vector3(1f, 1f, 180f));
                //arm.transform.localScale = new Vector3(2f, 2f, 8f);
                //arm.transform.localScale = new Vector3(2f, 2f, 8f);
                anim.Play("SwordSpecial");
                soundManager.PlaySound(3);
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
        if (Input.GetKeyDown(KeyCode.H) && Time.time > comboStart)
        {
            ns = parserScript.FindFirstNodeWithOnlyLeaves();
            if (ns != null)
            {
                if (ns.AreAllColorValuesZeroOrLessOfMyChildren())
                {
                    ns.Solve();
                }
            }
            comboStart = Time.time + comboNext;
            hbutton.Select();
            //Shoot(bulletPoolRed);
            //Punch(1);
            parserScript.UpdateLeaves(true, false, false, false);
        }
        else if (Input.GetKeyUp(KeyCode.H))
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
            ns = parserScript.FindFirstNodeWithOnlyLeaves();
            if (ns != null)
            {
                if (ns.AreAllColorValuesZeroOrLessOfMyChildren())
                {
                    ns.Solve();
                }
            }
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

    void ShootBlue()
    {
        if (Input.GetKey(KeyCode.K) && Time.time > nextFire)
        {
            ns = parserScript.FindFirstNodeWithOnlyLeaves();
            if (ns != null) {
                if (ns.AreAllColorValuesZeroOrLessOfMyChildren())
                {
                    ns.Solve();
                }
            }
            
            nextFire = Time.time + fireRate;
            kbutton.Select();
            Shoot(bulletPoolBlue);
            parserScript.UpdateLeaves(false, false, true, false);
            soundManager.PlaySound(2);
        }
        else if (Input.GetKeyUp(KeyCode.K))
        {
            EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(null);
        }
    }
    void ShootWhite()
    {
        if (Input.GetKeyDown(KeyCode.L) && Time.time > comboStart)
        {
            ns = parserScript.FindFirstNodeWithOnlyLeaves();
            if (ns != null)
            {
                if (ns.AreAllColorValuesZeroOrLessOfMyChildren())
                {
                    ns.Solve();
                }
            }
            comboStart = Time.time + comboNext;
            lbutton.Select();
            //Shoot(bulletPoolWhite);
            Punch(2);
            parserScript.UpdateLeaves(false, false, false, true);
            meleeing = true;
        }
        else if (Input.GetKeyUp(KeyCode.L))
        {
            EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(null);
            meleeing = false;
        }
    }
    void ShootBomb()
    {
        if (Input.GetKeyDown(KeyCode.Semicolon) && Time.time > comboStart)
        {
            ns = parserScript.FindFirstNodeWithOnlyLeaves();
            if (ns != null)
            {
                if (ns.AreAllColorValuesZeroOrLessOfMyChildren())
                {
                    ns.Solve();
                }
            }
            comboStart = Time.time + comboNext;
            semicolonbutton.Select();
            //Shoot(bulletPoolWhite);
            //parserScript.UpdateLeaves(false, false, false, true);
            Punch(1);
            meleeing = true;
        }
        else if (Input.GetKeyUp(KeyCode.Semicolon))
        {
            EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(null);
            meleeing = false;
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
        if (Input.GetKeyDown(KeyCode.Y) && IsAllOfCacheInPool(runePool, runeYCache))
        {
            Debug.Log("SelectExponent()");
            ybutton.Select();
            Punch(3);
            try
            {
                //NodeScript ns = parserScript.FindFirstNodeWithOnlyLeaves("^");
                //if (ns.AreAllColorValuesZeroOrLessOfMyChildren())
                //{
                //    ns.Solve();
                //}
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
        if (Input.GetKey(KeyCode.U) && IsAllOfCacheInPool(runePool, runeUCache))
        {
            ubutton.Select();
            try
            {

                //if (ns.AreAllColorValuesZeroOrLessOfMyChildren())
                //{
                //    ns.Solve();
                //}
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
        if (Input.GetKey(KeyCode.I) && IsAllOfCacheInPool(runePool, runeICache))
        {
            ibutton.Select();
            try
            {
                //NodeScript ns = parserScript.FindFirstNodeWithOnlyLeaves("*");
                //if (ns.AreAllColorValuesZeroOrLessOfMyChildren())
                //{
                //    ns.Solve();
                //}
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
        if (Input.GetKey(KeyCode.O) && IsAllOfCacheInPool(runePool, runeOCache))
        {
            obutton.Select();
            try
            {
                //    NodeScript ns = parserScript.FindFirstNodeWithOnlyLeaves("/");
                //    if (ns.AreAllColorValuesZeroOrLessOfMyChildren())
                //    {
                //        ns.Solve();
                //    }
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
        if (Input.GetKey(KeyCode.P) && IsAllOfCacheInPool(runePool, runePCache))
        {
            pbutton.Select();
            try
            {
                //NodeScript ns = parserScript.FindFirstNodeWithOnlyLeaves("+");
                //if (ns.AreAllColorValuesZeroOrLessOfMyChildren())
                //{
                //    ns.Solve();
                //}
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

    public void AddToRunePool(List<String> pool, String op) {
        pool.Add(op);
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

    //have to revamp how the parser gets solved (since we want to also consume operations?)
    //Keeping the tree visual with the operations in their normal node locations may be problematic
    
}
