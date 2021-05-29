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
   Light spotLight;

   GameManager gm;
    
   void Start()
   {
       characterController = GetComponent<CharacterController>();
       playerCamera = GameObject.Find("Main Camera");
       spotLight = GameObject.Find("Spot Light").GetComponent<Light>();
       cameraRotation = 0.0f;
       gm = GameManager.GetInstance();

   }

   void Update()
   {
       if (gm.gameState != GameManager.GameState.GAME) { return; }

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

       if (Input.GetKey("f") && (Time.realtimeSinceStartup - previousFlashPress > 0.5f))
       {
           spotLight.enabled = !spotLight.enabled;
           previousFlashPress = Time.realtimeSinceStartup;
       } 

        _sprintFactor = 1.0f;
       if (Input.GetKey("space"))
       {
           _sprintFactor = 2.0f;
           gm.enemyObject.transform.position = new Vector3(0, 11, 0);
           gm.enemyObject.SetActive(true);
       } 

       if(Input.GetKeyDown(KeyCode.Escape) && gm.gameState == GameManager.GameState.GAME)
       {
           gm.ChangeState(GameManager.GameState.PAUSE);
       }
   }

   void LateUpdate()
   {
       RaycastHit hit;
       Debug.DrawRay(playerCamera.transform.position, transform.forward*10.0f, Color.magenta);
       if(Physics.Raycast(playerCamera.transform.position, transform.forward, out hit, 100.0f))
       {
           Debug.Log(hit.collider.name);
       }
       
   }
}
