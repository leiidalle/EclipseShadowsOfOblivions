using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using StarterAssets;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.InputSystem.LowLevel;
using Unity.VisualScripting;

public class ThirdPersonShooterController : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera aimVirtualCamera;         //Cámara de la mira
    [SerializeField] private float normalSensitivity;                           //Sensibilidad normal de la camara
    [SerializeField] private float aimSensitivity;                              //Sensibilidad de la camara mira
    [SerializeField] private LayerMask aimColliderLayerMask = new LayerMask();  //Mascara de colisión de la mira
    [SerializeField] private Transform debugTransform;                          //Posición de debug
    [SerializeField] private Image aimScale;                                    //Para ampliar la mira al apuntar
    [SerializeField] private Transform pfBulletProjectile;                          //prefab munición
    [SerializeField] private Transform spawnBulletPosition;                          //Posición del spawn de la munición

    private StarterAssetsInputs starterAssetsInputs;
    private ThirdPersonController thirdPersonController;
    private void Awake()
    {
        thirdPersonController = GetComponent<ThirdPersonController>();
        starterAssetsInputs = GetComponent<StarterAssetsInputs>();
    }


    private void Update()
    {
        //Vector que me guarda el vector posición de la camara
        Vector3 mouseWorldPosition = Vector3.zero;
        
        //creo un vector2d para saber donde está el centro de la pantalla
        Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);

        //Genero el rayo que sale hacia el centor de la pantalla
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, aimColliderLayerMask))
        {
            debugTransform.position = raycastHit.point;
            mouseWorldPosition = raycastHit.point;
        }

        //Código para activar y desactivar la cámara de apuntado y modificar la sensibilidad
        if (starterAssetsInputs.aim) {
            aimVirtualCamera.gameObject.SetActive(true);
            thirdPersonController.SetSensitivity(aimSensitivity); //modifica sensibilidad de apuntado
            thirdPersonController.SetRotateOnMove(false); //Para que no rote cuando estoy apuntando tambien el personaje

            Vector3 worldAimTarget = mouseWorldPosition;
            worldAimTarget.y = transform.position.y; //trackeo solo el eje Y porque quiero rotar solo de derecha a izq.
            Vector3 aimDirection = (worldAimTarget - transform.position).normalized;

            //Ahora roto el personaje de forma suave con lerp
            transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * 20f);

            aimScale.transform.localScale = new Vector3 (2, 2);

        }
        else {
            aimVirtualCamera.gameObject.SetActive(false);
            thirdPersonController.SetSensitivity(normalSensitivity);
            thirdPersonController.SetRotateOnMove(true);
            
            aimScale.transform.localScale = new Vector3(1, 1);
        }
        //Logica del instanciamiento de la bala
        if (starterAssetsInputs.shoot){
            Vector3 aimDir = (mouseWorldPosition - spawnBulletPosition.position).normalized;
            Instantiate(pfBulletProjectile, spawnBulletPosition.position, Quaternion.LookRotation(aimDir, Vector3.up));
            starterAssetsInputs.shoot = false;
        }
        


    }


}
