﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class KnifeManager : NetworkBehaviour
{
    [SerializeField] private GameObject knifePrefab;
    [SerializeField] private GameObject knifeSpawnPos;
    [SerializeField] private float throwForce;
    private Text currentKunaiCounter;

    public int maxAmmo;

    private int currentAmmo;
    public int CurrentAmmo { get { return currentAmmo; } set { currentAmmo = value; } }

    private bool startTimer = false;
    private float timer = 0;

    private void Start()
    {
        currentAmmo = maxAmmo;
        currentKunaiCounter = GameObject.FindGameObjectWithTag("KunaiCounter").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer)
            return;

        if ((Input.GetButtonDown("Fire1") || Input.GetAxisRaw("Right Trigger") != 0) && !startTimer)
        {
            CmdThrow(knifeSpawnPos.transform.position, knifeSpawnPos.transform.rotation);
            startTimer = true;
        }

        if (startTimer)
        {
            timer += Time.deltaTime;
            if (timer > 0.5f)
            {
                timer = 0;
                startTimer = false;
            }
        }

        currentKunaiCounter.text = "Current Kunai Count: " + currentAmmo;
    }

    [Command]
    void CmdThrow(Vector3 position, Quaternion rotation)
    {
        if (currentAmmo > 0)
        {
            GameObject knife = Instantiate(knifePrefab, position, rotation);
            knife.transform.rotation = Quaternion.LookRotation(knife.transform.right, knife.transform.up);
            knife.GetComponent<Rigidbody>().velocity = -knife.transform.right * throwForce;
            currentAmmo--;
            NetworkServer.Spawn(knife.gameObject);
        }

    }
}
