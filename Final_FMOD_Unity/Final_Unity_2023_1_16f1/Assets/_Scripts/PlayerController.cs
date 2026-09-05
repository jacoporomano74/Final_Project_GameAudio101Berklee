using UnityEngine;
using System.Collections;
//using Studio.System.initialize;

public class PlayerController : MonoBehaviour
{

    // Declare your FMOD Sounds in this section

    public float speed;
    private int count = 0;
    public int totalPickups = 9;

    private GameObject playerFollow;

    Rigidbody rigidBody;                                //rigid body component


    FMODUnity.EventReference Music;                     //Declare FMOD Sound Event for Unity
    public string music = "event:/Music";               //Public string is the display property and type of variable we are declaring. ‘music’ is the name of the variable, and we also assign the path to the FMOD Event.
    FMOD.Studio.EventInstance musicEv;                  //cube event music

    FMODUnity.EventReference Rolling;                   //Declare FMOD Studio Event for Unity
    public string rolling = "event:/Rolling";			//declare the sound name and event path
    FMOD.Studio.EventInstance rollingEv;                //rolling event


    FMODUnity.EventReference cube_pickup;
    public string inputSound = "event:/cube_pickup";

    FMODUnity.EventReference Reverb;                    //Declaring snapshots
    public string reverbSnapshot = "snapshot:/ReverbRoom";
    private FMOD.Studio.EventInstance reverbSnapshotEv;


    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();

        // Setting up the references.
        playerFollow = GameObject.FindGameObjectWithTag("PlayerFollow");


        // Create FMOD event instances and get parameters in this section

        musicEv = FMODUnity.RuntimeManager.CreateInstance(music);


        //We use the event name that we specified up in the Public class section. Speed is how the parameter is named in FMOD Studio. We output that to rollingSpeedParam – so the parameter name that we supplied in the public class section. Then, we start the sound

        rollingEv = FMODUnity.RuntimeManager.CreateInstance(rolling);

        rollingEv.start();

        reverbSnapshotEv = FMODUnity.RuntimeManager.CreateInstance(reverbSnapshot);
    }

    void FixedUpdate()
    {
        //player movement with input axis
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        //apply impulse, clamp by max and apply velocity

        rigidBody.AddForce(movement * speed, ForceMode.Force);

        //calculate speed of ball rolling, assign that to the rolling event parameter

        //rollingSpeedParam.setParameterByName(Mathf.Max(Mathf.Abs(rigidBody.velocity.x), Mathf.Abs(rigidBody.velocity.z)) * 40.0f);
        rollingEv.setParameterByName("speed", Mathf.Max(Mathf.Abs(rigidBody.velocity.x), Mathf.Abs(rigidBody.velocity.z)) * 40.0f);
        Debug.Log(Mathf.Max(Mathf.Abs(rigidBody.velocity.x), Mathf.Abs(rigidBody.velocity.z)) * 40.0f);
    }

    void Update()
    {
        // Detect spacebar press

        if (Input.GetKeyDown("space"))
        {

            FMODUnity.RuntimeManager.PlayOneShot(inputSound);

        }
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Pickup"))
        {
            other.gameObject.SetActive(false);
            count++;
            Debug.Log("Pickup raccolti: " + count);

            if (count >= totalPickups)
            {
                musicEv.setParameterByName("Change", 2);
            }
        }
        if (other.gameObject.CompareTag ("losecube"))
        {
            other.gameObject.SetActive(false);
            musicEv.setParameterByName("Change", 3);
        }

        if (other.gameObject.CompareTag("ChangeCube"))
        {
            musicEv.setParameterByName("Change", 0);
        }

        if (other.gameObject.CompareTag("losecube"))
        {
            musicEv.setParameterByName("Change", 3);
        }

        if (other.gameObject.CompareTag("ReverbZone"))
        {
            reverbSnapshotEv.start();

            Debug.Log("reverb snapshot begin");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("ReverbZone"))
        {
            // When collision with the ReverbZone ends, turn off the Reverb Snapshot

            reverbSnapshotEv.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);

            Debug.Log("reverb snapshot stopped");
        }

        if (other.gameObject.CompareTag("Playcube"))
        {
            FMOD.Studio.PLAYBACK_STATE play_state;
            musicEv.getPlaybackState(out play_state);
            if (play_state != FMOD.Studio.PLAYBACK_STATE.PLAYING)
            {
                musicEv.setParameterByName("Change", 0);
                musicEv.start();
            }
        }

        if (other.gameObject.CompareTag("ChangeCube"))
        {
            musicEv.setParameterByName("Change", 1);
        }
    }
}