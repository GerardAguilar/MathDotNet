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
    Vector3 faceDirection;
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
    }
	
	// Update is called once per frame
	void Update () {
        //MoveWithAddForce();
        //MoveWithTranslate();
        MoveWithMovePosition();
        Attacks();
        Shooting();
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

    void MoveWithAddForce() {
        if (Input.GetKeyDown(KeyCode.W)) {
            rb.AddForce(Vector3.forward * sensitivity);
        }
        if (Input.GetKeyDown(KeyCode.S)){
            rb.AddForce(Vector3.back * sensitivity);
        }
        if (Input.GetKeyDown(KeyCode.A)) {
            rb.AddForce(Vector3.left * sensitivity);
        }
        if (Input.GetKeyDown(KeyCode.D)) {
            rb.AddForce(Vector3.right * sensitivity);
        }
    }

    void MoveWithTranslate() {
        if (Input.GetKeyDown(KeyCode.W))
        {
            transform.Translate(Vector3.forward * sensitivity);            
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            transform.Translate(Vector3.back * sensitivity);
            
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            transform.Translate(Vector3.left * sensitivity);
            
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            transform.Translate(Vector3.right * sensitivity);
            
        }
    }

    void MoveWithMovePosition()
    {
        direction = Vector3.zero;

        if (Input.GetKey(KeyCode.W))
        {
            direction = direction + Vector3.forward * sensitivity * Time.deltaTime;
            faceDirection = direction;
            transform.LookAt(transform.position + rb.velocity * sensitivity * Time.deltaTime*100);
        }
        if (Input.GetKey(KeyCode.S))
        {
            direction = direction + Vector3.back * sensitivity * Time.deltaTime;
            faceDirection = direction;
            //transform.LookAt(faceDirection);
            transform.LookAt(transform.position + rb.velocity * sensitivity * Time.deltaTime * 100);
        }
        if (Input.GetKey(KeyCode.A))
        {
            direction = direction + Vector3.left * sensitivity * Time.deltaTime;
            faceDirection = direction;
            //transform.LookAt(faceDirection);
            transform.LookAt(transform.position + rb.velocity * sensitivity * Time.deltaTime * 100);
        }
        if (Input.GetKey(KeyCode.D))
        {
            direction = direction + Vector3.right * sensitivity * Time.deltaTime;
            faceDirection = direction;
            //transform.LookAt(faceDirection);
            transform.LookAt(transform.position + rb.velocity * sensitivity * Time.deltaTime * 100);
        }        
        rb.MovePosition(transform.position + direction);
    }

    void Attacks() {
        if (Input.GetKeyDown(KeyCode.X)) {
            Punch();            
        }
        if (Input.GetKeyUp(KeyCode.X)) {
            RetractPunch();
        }
        //if (Input.GetKeyDown(KeyCode.Z)) {
        //    Punch2();
        //}
        //if (Input.GetKeyUp(KeyCode.Z))
        //{
        //    RetractPunch2();
        //}

    }

    void Punch() {
        arm.transform.localScale = new Vector3(1f, 1f, 1f);
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
        ShootGreen();
        ShootBlue();
        ShootWhite();
        ShootBomb();
        SelectAdd();
        SelectSubtract();
        SelectMultiply();
        SelectDivide();
        SelectExponent();
    }

    void ShootRed() 
    {
        if (Input.GetKey(KeyCode.Semicolon) && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            hbutton.Select();
            Shoot(bulletPoolRed);
            parserScript.UpdateLeaves(true, false, false, false);
        }
        else if (Input.GetKeyUp(KeyCode.Semicolon))
        {
            EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(null);
        }
    }
    void ShootGreen()
    {
        if (Input.GetKey(KeyCode.J) && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            jbutton.Select();
            Shoot(bulletPoolGreen);
            parserScript.UpdateLeaves(false, true, false, false);
        }
        else if (Input.GetKeyUp(KeyCode.J))
        {
            EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(null);
        }
    }
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
        if (Input.GetKey(KeyCode.L) && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            lbutton.Select();
            Shoot(bulletPoolWhite);
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


}
