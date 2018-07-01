using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Player : MonoBehaviour {
    #region Attributes
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
    //NodeScript ns;

    Boolean stage1 = false;
    Boolean stage2 = false;
    Boolean stage3 = false;
    float comboTime;
    float stage1InputDelay = .5f;
    float stage2InputDelay = .5f;
    float stage3InputDelay = .5f;

    #endregion

    #region Awake()
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

        SetupBulletPool(bulletPoolRed, "BulletRed", 1);
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

        //runePools are an old implementation of skill activation
        //the purpose is to keep track of triggered ops to activate something
        //we use runePool to gather realtime triggered ops, and the runeCache are the specific ops that need to be activated in order to change the skill
        //runePool = new List<string>();
        //runeYCache = new List<string>();
        //runeUCache = new List<string>();
        //runeICache = new List<string>();
        //runeOCache = new List<string>();
        //runePCache = new List<string>();

        ////this is for charging
        ////AddToRunePool(runePool, "^");
        ////AddToRunePool(runePool, "+");

        ////these are the skill activation requirements
        //AddToRunePool(runeYCache, "^");       
        //AddToRunePool(runeYCache, "-");

        //Debug.Log(IsAllOfCacheInPool(runePool, runeYCache));

        soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
    }
    #endregion

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

    #region Update()
    void Update () {

        MoveWithMovePosition();
        Shooting();
        Specials();
        StartCombo();
        ResetComboTime();
    }
    #endregion

    #region OnTriggerEnter()
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            //rb.AddExplosionForce(1000f, transform.position, 10f, 10f, ForceMode.Impulse);
        }
        else if (other.gameObject.CompareTag("AttributeShard")) {
            AttributeShardScript attributeShardScript = other.gameObject.GetComponent<AttributeShardScript>();
            parserScript.UpdateLeaves(attributeShardScript.r, attributeShardScript.g, attributeShardScript.b, attributeShardScript.w);
            other.gameObject.SetActive(false);
        }
    }
    #endregion

    void Specials() {
        SelectSpecial1();
        SelectSpecial2();
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
        if (!meleeing) {
            direction = Vector3.zero;

            #region MovementInputs
            if (Input.GetKey(KeyCode.W))
            {
                moving = true;
                comboCount = 0;
                direction = direction + Vector3.forward * sensitivity * Time.deltaTime;
                faceDirection = direction;
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
            if (Input.GetKey(KeyCode.S))
            {
                moving = true;
                comboCount = 0;
                direction = direction + Vector3.back * sensitivity * Time.deltaTime;
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
            #endregion

            rb.MovePosition(transform.position + direction);
        }
    }
    
    void ResetComboTime()
    {
        if (Time.time > comboStart + comboNext)
        {
            comboStart = Time.time;
            comboCount = 0;
        }
    }

    void Punch(int type) {
        switch (type) {
            case 0:
                meleeing = true;
                anim.Play("SwordSwing");
                soundManager.PlaySound(0);
                Debug.Log("Punch(0)");
                break;
            case 1:
                meleeing = true;
                anim.Play("SwordThrust");
                soundManager.PlaySound(4);
                Debug.Log("Punch(1)");
                break;
            case 2:
                meleeing = true;
                anim.Play("SwordPound");
                soundManager.PlaySound(3);
                Debug.Log("Punch(2)");
                break;
            case 3:
                anim.Play("SwordSpecial");
                soundManager.PlaySound(3);
                Debug.Log("Punch(3)");
                break;
            default:
                break;
        }
    }

    //should be 6 basic types with two arguments
    //how do we deal with compound attributes up the line?
    //make it so that it makes sense
    //

    //when you press y,u,i,o.p,[
    //these correspond to different operators
    //the event would then consume what's on the ember machina (provided that it's been charged)
    //and trigger power ups
    void OperatorEffect(int type) {
        switch (type)
        {
            case 0:
                
                break;
            case 1:

            case 2:

            case 3:

            default:
                break;
        }
    }

    void RetractPunch() {
        arm.transform.localScale = new Vector3(.35f, 1f, 1f);
    }

    void Shooting() {
        ShootRed();
        //ShootGreen();
        //ShootBlue();
        //ShootWhite();
        //ShootBomb();
        
        SelectSubtract();
        SelectMultiply();
        SelectDivide();
        SelectExponent();
        SelectAdd();
    }



    void ShootRed() 
    {
        //if keydown
        //shoot

        if (Input.GetKeyDown(KeyCode.H) && Time.time > comboStart)
        {
            comboStart = Time.time + comboNext;
            hbutton.Select();
            Shoot(bulletPoolRed);
            //Punch(1);
            //parserScript.UpdateLeaves(true, false, false, false);
        }
        else if (Input.GetKeyUp(KeyCode.H))
        {
            EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(null);
        }
    }

    void StartCombo() {
        if (Input.GetKeyDown(KeyCode.J))
        {
            jbutton.Select();
            if (stage3)
            {
                Punch(2);
                stage3 = false;
            }
            if (stage2)
            {
                Punch(1);
                SetStage3();
            }
            if (stage1)
            {
                comboTime = Time.time;
                Punch(0);
                SetStage2();
            }
            if (Time.time > comboTime + 10f || (stage1 == false && stage2 == false && stage3 == false))
            {
                SetStage1();
            }
        }
        else if (Input.GetKeyUp(KeyCode.J)) {
            meleeing = false;
        }
    }

    void SetStage1() {
        stage1 = true;
        stage2 = false;
        stage3 = false;
    }
    void SetStage2() {
        stage1 = false;
        stage2 = true;
        stage3 = false;
    }
    void SetStage3() {
        stage1 = false;
        stage2 = false;
        stage3 = true;
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
    void SelectSpecial1()
    {
        //if (Input.GetKey(KeyCode.P) && IsAllOfCacheInPool(runePool, runePCache))
        if (Input.GetKey(KeyCode.K))
        {
            kbutton.Select();
            try
            {
                //NodeScript ns = parserScript.FindFirstNodeWithOnlyLeaves("^");
                //if (ns.AreAllColorValuesZeroOrLessOfMyChildren())
                //{
                //    ns.Solve();
                //}
                Shoot(bulletPoolMultiply);
            }
            catch (Exception e)
            {
                Debug.Log("No + with just leaves found. Error: " + e);
            }
        }
        else if (Input.GetKeyUp(KeyCode.K))
        {
            EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(null);
        }
    }
    
    void SelectSpecial2()
    {
        //if (Input.GetKey(KeyCode.U) && IsAllOfCacheInPool(runePool, runeUCache))
        if (Input.GetKey(KeyCode.L))
        {
            lbutton.Select();
            try
            {
                //NodeScript ns = parserScript.FindFirstNodeWithOnlyLeaves("^");
                //if (ns.AreAllColorValuesZeroOrLessOfMyChildren())
                //{
                //    ns.Solve();
                //}
                Shoot(bulletPoolGreen);
            }
            catch (Exception e)
            {
                Debug.Log("No - with just leaves found. Error: " + e);
            }
        }
        else if (Input.GetKeyUp(KeyCode.L))
        {
            EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(null);
        }
    }

    void SelectAdd() {
        if (Input.GetKey(KeyCode.Y))
        {
            ybutton.Select();
            try
            {
                NodeScript ns = parserScript.FindFirstNodeWithOnlyLeaves("+");
                if (ns != null) {
                    if (ns.AreAllColorValuesZeroOrLessOfMyChildren())
                    {
                        Debug.Log("ns.Solve()");
                        ns.Solve();
                    }
                }

            }
            catch (Exception e)
            {
                Debug.Log("No + with just leaves found. Error: " + e);
            }
        }
        else if (Input.GetKeyUp(KeyCode.Y))
        {
            EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(null);
        }
    }
    void SelectSubtract() {
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
        //if (Input.GetKey(KeyCode.I) && IsAllOfCacheInPool(runePool, runeICache))
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
                //Shoot(bulletPoolGreen);
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
        //if (Input.GetKey(KeyCode.O) && IsAllOfCacheInPool(runePool, runeOCache))
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
                //Shoot(bulletPoolWhite);
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

    

    void SelectExponent()
    {
        //if (Input.GetKeyDown(KeyCode.Y) && IsAllOfCacheInPool(runePool, runeYCache))
        if (Input.GetKeyDown(KeyCode.P))
        {
            pbutton.Select();
            try
            {
                NodeScript ns = parserScript.FindFirstNodeWithOnlyLeaves("^");
                if (ns.AreAllColorValuesZeroOrLessOfMyChildren())
                {
                    ns.Solve();
                }
            }
            catch (Exception e)
            {
                Debug.Log("No ^ with just leaves found. Error: " + e);
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
        if (!pool.Contains(op))
        {
            pool.Add(op);
        }        
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
