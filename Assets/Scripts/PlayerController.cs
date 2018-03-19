﻿using UnityEngine;
namespace Player{


 public enum  MeshType
 {
     circle,
     rectangle
 }   
public class PlayerController : MonoBehaviour {

	// Use this for initialization

    public static void CloseControll()
    {
        PlayerController player= ((PlayerController)FindObjectOfType(typeof( PlayerController)));
        player.enabled=false;
        player.transform.GetComponentInChildren<CameraController>().enabled=false;

    }
    public static void EnableControll()
    {
          PlayerController player= ((PlayerController)FindObjectOfType(typeof( PlayerController)));
        player.enabled=true;
          player.transform.GetComponentInChildren<CameraController>().enabled=true;
    }
    
    public  Mesh[] meshs;


    public MeshType CurrentMesh
    {
        set
        {
            //Debug.Log((int)value);
            transform.Find("Body").Find("Mesh").GetComponent<SkinnedMeshRenderer>().sharedMesh=meshs[(int)value];
            currentMesh=value;
        }
        get
        {
            return currentMesh;
        }
    }

    private MeshType currentMesh=MeshType.circle;
    public void ChangeMesh(MeshType mesh)
    {
        
        if( CurrentMesh!=mesh)CurrentMesh=mesh;
        else CurrentMesh=MeshType.circle;
    }
	public PlayerState state;

   // [SerializeField]
    public Animator animator;

	// private Rigidbody _rigidbody;
    [HideInInspector]
    public Transform _transform;

     [HideInInspector]
    public Transform sizeTransform;
    public float MoveSpeed = 15;
    public float RotateSpeed = 40;
    public float LowJumpForce = 10;
    public float HighJumpForce = 20;
    public float JumpCriticalTime = 1;
    public float DownRayLength;
    [HideInInspector]
    public Vector3 TolerantSize;
    [HideInInspector]
    public float TolerantPosY;
    public float MinSizeX, MaxSizeX;

    public int sizeStep;
    public float SizeScrollSpeed = 10;
    [HideInInspector]
    public bool canChangeSize;



    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
          _transform = transform;
        sizeTransform=transform.Find("Body");
        TolerantSize = sizeTransform.localScale;
      //  ChangeMesh(MeshType.rectangle);
    }
    void Start()
    {
        state=new NormalState(this);
        state.Enter();
      
       // _rigidbody = GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void FixedUpdate()
    {

		state.Update();
        if (canChangeSize)
        { 
            ControlPlayerSize();
        }

    }


	public void SwitchState(PlayerState state)
	{

			this.state.Exit();
			this.state=state;
			this.state.Enter();
	}
        void ControlPlayerSize()
        {
            Vector3 size =  sizeTransform.localScale;
            Vector3 newSize = size + Vector3.one * Input.GetAxisRaw("Mouse ScrollWheel") * SizeScrollSpeed * Time.deltaTime;




            //Debug.Log(newSize );
            if (size != newSize && newSize.x <= MaxSizeX && newSize.x >= MinSizeX)
            {
                sizeStep=(int)((newSize.x-MinSizeX)/ (MaxSizeX-MinSizeX)*10);

                Vector3 pos =  sizeTransform.localPosition;
                float sizeY0 =  sizeTransform.localScale.y;
                 sizeTransform.localScale = newSize;
                float sizeY1 =  sizeTransform.localScale.y;
                //DownRayLength += sizeY1 - sizeY0;
                 sizeTransform.localPosition = pos + new Vector3(0, sizeY1 - sizeY0, 0);
            }
            

        }


    }
}