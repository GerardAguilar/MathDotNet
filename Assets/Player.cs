using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    int bulletMaxCount;
    GameObject nozzle;
    int bulletForce;
    float nextFire;
    float fireRate;

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

        SetupBulletPool(bulletPoolAdd, 10);
        SetupBulletPool(bulletPoolSubtract, 10);
        SetupBulletPool(bulletPoolMultiply, 10);
        SetupBulletPool(bulletPoolDivide, 10);
        SetupBulletPool(bulletPoolExponent, 10);

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

    void SetupBulletPool(List<GameObject> pool, int bulletCount) {
        for (int i = 0; i < bulletCount; i++)
        {
            GameObject temp = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Bullet"));
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
        ShootAdd();
        ShootSubtract();
        ShootMultiply();
        ShootDivide();
        ShootExponent();

    }

    void ShootAdd() {

        if (Input.GetKey(KeyCode.J) && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            Shoot(bulletPoolExponent, "+");
        }
    }
    void ShootSubtract()
    {

        if (Input.GetKey(KeyCode.K) && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            Shoot(bulletPoolExponent, "-");
        }
    }
    void ShootMultiply()
    {

        if (Input.GetKey(KeyCode.L) && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            Shoot(bulletPoolMultiply, "*");
        }
    }
    void ShootDivide()
    {

        if (Input.GetKey(KeyCode.I) && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            Shoot(bulletPoolDivide, "/");
        }
    }
    void ShootExponent()
    {

        if (Input.GetKey(KeyCode.H) && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            Shoot(bulletPoolExponent, "^");
        }
    }

    void Shoot(List<GameObject> pool, string op) {
        bool found = false;
        for (int i = pool.Count - 1; i >= 0; i--)
        {
            if (!pool[i].activeSelf)
            {
                pool[i].SetActive(true);
                //Debug.Log("Shoot()");
                BulletScript bs = pool[i].GetComponent<BulletScript>();
                bs.SetDirection(faceDirection); ;
                found = true;
                i = 0;
                //break;
            }
        }
        if (found)
        {
            //press a + button
            try
            {
                parserScript.FindFirstNodeWithOnlyLeaves(op).Solve();
            }
            catch (Exception e)
            {
                Debug.Log("No " + op + " with just leaves found. Error: " + e);
            }
        }
    }


}
