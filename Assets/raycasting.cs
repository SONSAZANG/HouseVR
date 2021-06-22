using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.Video;

public class raycasting : MonoBehaviour {


    FadeController Fader;
    public Light BathLight;
    public VideoPlayer TVvideo;
    public BoxCollider BedWall;
    public BoxCollider BedBedOn;
    public BoxCollider BedTable;
    public BoxCollider BathSink;
    public BoxCollider BathWashStand;
    public BoxCollider BathLightButton;
    public BoxCollider Chair;
    public BoxCollider Cooker;
    public BoxCollider Sofa;
    public BoxCollider TV;
    public GameObject[] Tel;
    public Transform[] pos;
    private bool IsPlaying = false;
    private bool RayCheckBool = false;
    private float TimeElapsed;
    private bool IsStartCheck = false;
    private bool IsInHouse = false;
    private bool IsMainCheck = false;
    public GameObject CamOffset;
    public Animator FirstDoor;
    public Animator BedDoor;
    public Animator BathDoor;
    public ParticleSystem Sink;
    public ParticleSystem gas;
    public Vector3 POSV;
    public Image Reticle;

    // Start is called before the first frame update
    void Start()
    {
        Fader = FindObjectOfType<FadeController>();
    }

    // Update is called once per frame
    void Update()
    {
        Raycast();

    }
    // 이동할때 페이드 부분 구현
    void Raycast()
    {
        RaycastHit hit;
        Vector3 forward = transform.TransformDirection(Vector3.forward * 1000);
        if (Physics.Raycast(transform.position, forward, out hit))
        {
            if (RayCheckBool == true)
            {
                return;
            }
            else if (TimeElapsed < 2)
            {

                TimeElapsed += Time.deltaTime;
                Reticle.fillAmount = TimeElapsed / 2f;
            }
            else if (TimeElapsed >= 2)
            {
                TimeElapsed = 0;
                Reticle.fillAmount = 0;
                RayCheckBool = true;
                Movement(hit);
                return;
            } // switch 호출
        }
        else
        {
            TimeElapsed -= Time.deltaTime + 3f;
            if (TimeElapsed < 0)
            {
                TimeElapsed = 0;
            }
            Reticle.fillAmount = TimeElapsed * 0;
        }
        Debug.DrawRay(transform.position, forward, Color.red);
    }
    void MovePoint(Vector3 ves3)
    {
        InStartFade();
        MoveStart(ves3);
        OutStartFade();
    }

    void PosiCheck()
    {
        if (IsStartCheck == true)
        {
            Tel[0].transform.position = new Vector3(-4.38f, -3.36f, -0.934f);
            Tel[1].transform.position = new Vector3(-4.38f, 0.33f, 2.05f);
        }
        else if (IsInHouse == true)
        {
            Tel[1].transform.position = new Vector3(-4.38f, -3.36f, 2.05f);
            Tel[2].transform.position = new Vector3(0.8f, 0.33f, 8.34f);
        }
        else if (IsMainCheck == true)
        {
            Tel[1].transform.position = new Vector3(-4.38f, 0.33f, 2.05f);
            Tel[2].transform.position = new Vector3(0.8f, -3.36f, 8.34f);
            Tel[3].transform.position = new Vector3(-2.92f, 0.33f, 11.11f);
            Tel[4].transform.position = new Vector3(3.4f, 0.33f, 10.77f);
            Tel[5].transform.position = new Vector3(1.46f, 0.33f, 5.77f);
            Tel[6].transform.position = new Vector3(-1.51f, 0.33f, 0.56f);
        }
        else if (IsMainCheck == false)
        {
            Tel[1].transform.position = new Vector3(-4.38f, -3.36f, 2.05f);
            Tel[2].transform.position = new Vector3(0.8f, 0.33f, 8.34f);
            Tel[3].transform.position = new Vector3(-2.92f, -3.36f, 11.11f);
            Tel[4].transform.position = new Vector3(3.4f, -3.36f, 10.77f);
            Tel[5].transform.position = new Vector3(1.46f, -3.36f, 5.77f);
            Tel[6].transform.position = new Vector3(-1.51f, -3.36f, 0.56f);
        }
    }

    void Movement(RaycastHit hit)
    {
        // tel on : 0.33f, tel off : -3.36f
        string hitName = hit.transform.tag;
        switch (hitName)
        {
            case "start":
                IsStartCheck = true;
                FirstDoor.Play("firstDoor");
                PosiCheck();
                break;

            case "inHouse":
                IsStartCheck = false;
                IsMainCheck = false;
                IsInHouse = true;
                FirstDoor.Play("firstDoorClose");
                PosiCheck();
                POSV = pos[0].position;
                MovePoint(POSV);
                break;

            case "main":
                IsInHouse = false;
                IsMainCheck = true;
                PosiCheck();
                gas.Stop();
                TVvideo.Stop();
                POSV = pos[1].position;
                MovePoint(POSV);
                break;

            case "kitchenMain":
                IsMainCheck = false;
                PosiCheck();
                POSV = pos[3].position;
                MovePoint(POSV);
                Cooker.enabled = true;
                Chair.enabled = true;
                break;

            case "Cooker":
                Cooker.enabled = false;
                gas.Play();
                break;

            case "Chair":
                Chair.enabled = false;
                POSV = pos[7].position;
                Tel[3].transform.position = new Vector3(-2.92f, 0.33f, 11.11f);
                MovePoint(POSV);
                break;



            case "livingMain":
                IsMainCheck = false;
                PosiCheck();
                POSV = pos[2].position;
                MovePoint(POSV);
                Sofa.enabled = true;
                break;

            case "Sofa":
                Sofa.enabled = false;
                TV.enabled = true;
                Tel[4].transform.position = new Vector3(3.4f, 0.33f, 10.77f);
                POSV = pos[6].position;
                MovePoint(POSV);
                break;

            case "TV":
                TV.enabled = false;
                TVvideo.Play();
                break;

            case "bedMain":
                IsMainCheck = false;
                BedDoor.Play("bedDoorOpen");
                PosiCheck();
                POSV = pos[4].position;
                MovePoint(POSV);
                BedWall.enabled = true;
                break;

            case "BedWall":
                BedWall.enabled = false;
                BedBedOn.enabled = true;
                Tel[5].transform.position = new Vector3(1.46f, 0.33f, 5.77f);
                POSV = pos[9].position;
                MovePoint(POSV);
                break;

            case "BedBedOn":
                BedBedOn.enabled = false;
                BedTable.enabled = true;
                POSV = pos[10].position;
                MovePoint(POSV);
                break;

            case "BedTable":
                BedTable.enabled = false;
                POSV = pos[11].position;
                MovePoint(POSV);
                break;

            case "BathMain":
                IsMainCheck = false;
                PosiCheck();
                BathDoor.Play("bathDoorOpen");
                POSV = pos[5].position;
                MovePoint(POSV);
                BathLightButton.enabled = true;
                break;

            case "BathLightButton":
                BathLightButton.enabled = false;
                BathWashStand.enabled = true;
                BathLight.range = 20;
                break;

            case "BathWashStand":
                BathWashStand.enabled = false;
                POSV = pos[8].position;
                Tel[6].transform.position = new Vector3(-1.51f, 0.33f, 0.56f);
                MovePoint(POSV);
                BathSink.enabled = true;
                break;

            case "BathSink":
                Sink.Play();
                break;
        }
        RayCheckBool = false;
    }
    public void OutStartFade()
    {
        if (IsPlaying == true)
        {
            return;
        }

        IsPlaying = true;
        Fader.StartCoroutine(Fader.FadeOut());
        IsPlaying = false;
        Debug.Log("OutFade");
    }
    public void InStartFade()
    {
        if (IsPlaying == true)
        {
            return;
        }

        IsPlaying = true;
        Fader.StartCoroutine(Fader.FadeIn());
        IsPlaying = false;
        Debug.Log("StartFade");
    }
    public void MoveStart(Vector3 vcs3)
    {
        if (IsPlaying == true)
        {
            return;
        }

        IsPlaying = true;
        StartCoroutine(Move(vcs3));
        Debug.Log("Move");
    }
    IEnumerator Move(Vector3 vc3)
    {
        CamOffset.transform.position = vc3;
        IsPlaying = false;
        yield return null;
    }

}
