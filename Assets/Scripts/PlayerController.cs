using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
   float _baseSpeed = 10.0f;
   float _gravidade = 9.8f;
   float _sprintFactor = 1.0f;

   CharacterController characterController;

   //Referência usada para a câmera filha do jogador
   GameObject playerCamera;
   //Utilizada para poder travar a rotação no angulo que quisermos.
   float cameraRotation;
   float previousFlashPress = 0.0f;
   float sprintPress = 0.0f;
   float slenderReset = 0.0f;
   Light spotLight;
   Vector3 enemyDirection;

   [SerializeField]
   private AudioClip _captureClip = null;
   private AudioSource _source = null;


   GameManager gm;
    
   void Start()
   {
       characterController = GetComponent<CharacterController>();
       playerCamera = GameObject.Find("Main Camera");
       spotLight = GameObject.Find("Spot Light").GetComponent<Light>();
       spotLight.enabled = false;
       cameraRotation = 0.0f;
       gm = GameManager.GetInstance();
       _source = GetComponent<AudioSource>();
       if (_source == null)
       {
           Debug.Log("Audio Source is NULL");
       }
       else
       {
           _source.clip = _captureClip;
       }  

   }

   void Update()
   {
       if (gm.gameState != GameManager.GameState.GAME) { return; }

       if (gm.playerFirstUpdate) 
       { 
           characterController.transform.position = new Vector3(0, 1, 0);
           // characterController.transform.rotation = new Quaternion(0, 0, 0);
           gm.playerFirstUpdate = false;
           return; 
       }

       float x = Input.GetAxis("Horizontal");
       float z = Input.GetAxis("Vertical");
       //Verificando se é preciso aplicar a gravidade
       float y = 0;
       if(!characterController.isGrounded){
           y = -_gravidade;
       }

       //Tratando movimentação do mouse
       float mouse_dX = Input.GetAxis("Mouse X")*5;
       float mouse_dY = Input.GetAxis("Mouse Y")*10;

       //Tratando a rotação da câmera
       cameraRotation += mouse_dY;
       Mathf.Clamp(cameraRotation, -75.0f, 75.0f);

       Vector3 direction = transform.right * x + transform.up * y + transform.forward * z;

       characterController.Move(direction * _baseSpeed * _sprintFactor * Time.deltaTime);
       transform.Rotate(Vector3.up, mouse_dX);
       playerCamera.transform.localRotation = Quaternion.Euler(cameraRotation, 0.0f, 0.0f);

       if (gm.enemyObject.activeInHierarchy) {
           enemyDirection = characterController.transform.position - gm.enemyObject.transform.position;
           if (enemyDirection.magnitude < 3.0f) {
               gm.ChangeState(GameManager.GameState.ENDGAME);
           } else if (enemyDirection.magnitude > 20.0f) {
               gm.enemyObject.SetActive(false);
           } else {
               gm.enemyObject.transform.position += enemyDirection.normalized * Time.deltaTime * 12.0f;
           }
       }

       if (Input.GetKey("f") && (Time.realtimeSinceStartup - previousFlashPress > 0.5f))
       {
           spotLight.enabled = !spotLight.enabled;
           previousFlashPress = Time.realtimeSinceStartup;
           slenderReset = Time.realtimeSinceStartup;
       } 

        _sprintFactor = 1.0f;
       if (Input.GetKey("space"))
       {
           _sprintFactor = 2.0f;
           sprintPress = Time.realtimeSinceStartup;
           if (Time.realtimeSinceStartup - slenderReset > 1.0f) {
               slenderReset = Time.realtimeSinceStartup;
               if (spotLight.enabled && !gm.enemyObject.activeInHierarchy) { 
                   gm.enemyObject.transform.position = characterController.transform.position + direction * 20.0f;
                   gm.enemyObject.SetActive(true);
               }
           }
       } else if (Time.realtimeSinceStartup - sprintPress > 0.5f) {
           slenderReset = Time.realtimeSinceStartup;
       } 

       if(Input.GetKeyDown(KeyCode.Escape) && gm.gameState == GameManager.GameState.GAME)
       {
           gm.ChangeState(GameManager.GameState.PAUSE);
       }

       /* if (gm.GetPontos() == 3)
           {
               gm.enemyObject.transform.position = characterController.transform.position + direction * 25.0f;
               gm.enemyObject.SetActive(true);
           }
        */   
   }

   void LateUpdate()
   {
       RaycastHit hit;
       Debug.DrawRay(playerCamera.transform.position, transform.forward*10.0f, Color.magenta);
       if(Physics.Raycast(playerCamera.transform.position, transform.forward, out hit, 1.0f))
       {
           string objectName = hit.collider.name;
           if (objectName == "Carta-1") 
           {
               gm.Carta1Captured = true;
               gm.Carta1Object.SetActive(false);
               _source.Play();
           }
           if (objectName == "Carta-2") 
           {
               gm.Carta2Captured = true;
               gm.Carta2Object.SetActive(false);
               _source.Play();
           }
           if (objectName == "Carta-3") 
           {
               gm.Carta3Captured = true;
               gm.Carta3Object.SetActive(false);
               _source.Play();
           }
           if (objectName == "Carta-4") 
           {
               gm.Carta4Captured = true;
               gm.Carta4Object.SetActive(false);
               _source.Play();
           }
           if (objectName == "Carta-5") 
           {
               gm.Carta5Captured = true;
               gm.Carta5Object.SetActive(false);
               _source.Play();
           }

           if (gm.GetPontos() == 5)
           {
               gm.ChangeState(GameManager.GameState.ENDGAME);
           }


           Debug.Log("Hit: " + hit.collider.name);
       }
       
   }
}
