using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * Repas
 * 
 * Que hem vist:
 *      - crear objectes a l'ascena.
 *      - crear EmptyObject(per exemple per fer el GeneradorNumeros).
 *      - Prefabs (per crear objectes quan el joc esta en execusio).
 *          - per crear-los: l'objecta que ja teniem creat l'arroseguem a la carpeta prefabs.
 *          - per crear un prefab a l'escena en execucio: metode Instantiate(variablePrefab).
 *              - variablePrefab: variable de tipus GameObject.
 *      - trobar posici� objecta actual: transform.position
 *      - trobar marges pantalla: Camera.main.ViewportToWorldPoint().
 *      - [SerilizeFiled]: per fer que una variable private de la classe es mostri a l'editor de Unity.
 *      - Utilitzar una imatge/sprite com si fos mes d'una (contenint subimatges)
 *          - seleccionem l'esprite.
 *          - en l'opcio Sprite Mode canviem de single a multiple, i cliquem boto Apply
 *          - fem servir les opcions del boto sprite editor
 *      - Destruir objecte actual: Destroy(gameObject).
 *      - cridar un metode al cap de x segons: Invoke("NomMetode", xf).
 *      - cridar un metode al cap de x segons i cada y segons: InvokeRepeting("NomMetode", xf, yf).
 *      - com aturar un InvokeRepeating: CancelInvoke("NomMetode").
 *      - detectar objecte toca a un altre:
 *          - afagir als objectes que volem que es toquin, els components: BoxXollider2D i Rigibody2D.
 *          - en Boxcollider2D: activar checkbox IsTrigger.
 *          - en RigidBody2D: gravitiScale posar-ho a 0
 */


public class NauJugador : MonoBehaviour
{
    private float _vel;

    Vector2 minPantalla, maxPantalla;

    [SerializeField] private GameObject prefabProjectil;
    [SerializeField] private GameObject prefabExplosio;

    [SerializeField] private TMPro.TextMeshProUGUI componentTextVides;

    private int videsNau;
    
    // Start is called before the first frame update
    void Start()
    {
        _vel = 15;
        minPantalla = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
        maxPantalla = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));

        float meitatMidaImatgeX = GetComponent<SpriteRenderer>().sprite.bounds.size.x * transform.localScale.x / 2;
        float meitatMidaImatgeY = GetComponent<SpriteRenderer>().sprite.bounds.size.y * transform.localScale.y / 2;

        minPantalla.x = minPantalla.x + meitatMidaImatgeX;
        maxPantalla.x = maxPantalla.x - meitatMidaImatgeX;

        //Esto es lo mismo de arriba puesto de otra manera

        minPantalla.y += meitatMidaImatgeY;
        maxPantalla.y -= meitatMidaImatgeY;

        videsNau = 3;
    }

    // Update is called once per frame
    void Update()
    {
        MoureNau();
        DisparaProjectil();
    }

    private void DisparaProjectil()
    {
        if (Input.GetKeyDown("space"))
        {
            GameObject projectil = Instantiate(prefabProjectil);
            projectil.transform.position = transform.position;
        }
    }

    private void MoureNau()
    {
        float direccioIndicadaX = Input.GetAxisRaw("Horizontal");
        float direccioIndicadaY = Input.GetAxisRaw("Vertical");
        Debug.Log("X: " + direccioIndicadaX + "Y: " + direccioIndicadaY);
        Vector2 direccioIndicada = new Vector2(direccioIndicadaX, direccioIndicadaY).normalized;

        Vector2 novaPos = transform.position;//transform.position: pos actual de la nau
        novaPos = novaPos + direccioIndicada * _vel * Time.deltaTime;

        novaPos.x = Mathf.Clamp(novaPos.x, minPantalla.x, maxPantalla.x);
        novaPos.y = Mathf.Clamp(novaPos.y, minPantalla.y, maxPantalla.y);

        transform.position = novaPos;
    }

    private void OnTriggerEnter2D(Collider2D objectaTocat)
    {
        if(objectaTocat.tag == "Numero")
        {
            videsNau--;
            componentTextVides.text = "Vides: " + videsNau.ToString();

            if(videsNau < 0)
            {
                GameObject explosio = Instantiate(prefabExplosio);
                explosio.transform.position = transform.position;

                SceneManager.LoadScene("PantallaResultats");

                Destroy(gameObject);
            }
        }   
    }

}