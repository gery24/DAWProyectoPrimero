using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AnarAPantallaJoc : MonoBehaviour
{
    public void AnarAPantallaJugant()
    {
        DadesGlobals.ReiniciarPunts();
        SceneManager.LoadScene("PantallaJugant");
    }    
}
