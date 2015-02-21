#pragma strict

var walkSpeed: float = 7; // regular speed 
var crchSpeed: float = 3; // crouching speed 
var runSpeed:  float = 20; // run speed
var walkAudio: AudioClip; // walking audio
var runAudio:  AudioClip; //running audio
var walkAudioSpeed : float = 0.4;
var runAudioSpeed : float = 0.2;
var isWalking : boolean = false;
var isRunning : boolean = false;

private var walkAudioTimer : float = 0.0;
private var runAudioTimer : float = 0.0;
private var chMotor: CharacterMotor; 
private var ch: CharacterController;
private var tr: Transform; //myself
private var height: float; // initial height

function Start()
{ 
	chMotor = GetComponent(CharacterMotor); 
	tr = transform;
	ch = GetComponent(CharacterController); 
	height = ch.height; 
}

function Update()
{
 var h = height;
 var speed = walkSpeed;
 
if ( ch.isGrounded )
 {
         PlayFootsteps();
 }
else
 {
    walkAudioTimer = 0.0;
    runAudioTimer = 0.0;
  }
     
 if (ch.isGrounded && Input.GetKey("left shift") || Input.GetKey("right shift"))
 {
     speed = runSpeed;
 }
 if (Input.GetKey("c"))
 { 
 	 // press C to crouch
     h = 0.5 * height;
     speed = crchSpeed; // slow down when crouching
 }
 
 chMotor.movement.maxForwardSpeed = speed; // set max speed
 var lastHeight = ch.height; // crouch/stand up smoothly 
 ch.height = Mathf.Lerp(ch.height, h, 5*Time.deltaTime);
 tr.position.y += (ch.height-lastHeight)/2; // fix vertical position
 
 }
  function PlayFootsteps() 
 {
     if ( Input.GetAxis( "Horizontal" ) || Input.GetAxis( "Vertical" ) )
     {
        if ( Input.GetKey( "left shift" ) || Input.GetKey( "right shift" ) )
        {
          // Running
          isWalking = false;
          isRunning = true;
        }
        else
        {
          // Walking
          isWalking = true;
          isRunning = false;
        }
     }
     else
     {
        // Stopped
        isWalking = false;
        isRunning = false;
     }
     
     // Play Audio
     if ( isWalking )
     {
        if ( audio.clip != walkAudio )
        {
          audio.Stop();
          audio.clip = walkAudio;
        }
 
        //if ( !audio.isPlaying )
        if ( walkAudioTimer > walkAudioSpeed )
        {
          audio.Stop();
          audio.Play();
          walkAudioTimer = 0.0;
        }
     }
     else if ( isRunning )
     {
        if ( audio.clip != runAudio )
        {
          audio.Stop();
          audio.clip = runAudio;
        }
 
        //if ( !audio.isPlaying )
        if ( runAudioTimer > runAudioSpeed )
        {
          audio.Stop();
          audio.Play();
          runAudioTimer = 0.0;
        }
     }
     else
     {
        audio.Stop();
     }
     
     // increment timers
     walkAudioTimer += Time.deltaTime;
     runAudioTimer += Time.deltaTime;    
 }