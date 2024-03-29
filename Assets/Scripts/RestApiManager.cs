﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class RestApiManager : MonoBehaviour
{

    [SerializeField]
    private string URL;

    public string Username { get; set; }
    public string Token { get; set; }

    // Start is called before the first frame update
    void Awake()
    {
        initToken();
    }

    private void initToken()
    {
        Token = PlayerPrefs.GetString("token");
        Username = PlayerPrefs.GetString("username");

        if (Token == null || Token == "")
        {
            Debug.Log("No hay Token");
            UIManager.Instance.ShowLogin();
        }
        else
        {
            Debug.Log(Token);
            Debug.Log(Username);
            //Verficar token;
        }
    }

    public void Login()
    {
        JsonData data = new JsonData();

        data.username = GameObject.Find("InputUsername").GetComponent<InputField>().text;
        data.password = GameObject.Find("InputPassword").GetComponent<InputField>().text;

        string postData = JsonUtility.ToJson(data);

        StartCoroutine(LoginPost(postData));
    }

    public void Registro()
    {
        JsonData data = new JsonData();

        data.username = GameObject.Find("InputUsername").GetComponent<InputField>().text;
        data.password = GameObject.Find("InputPassword").GetComponent<InputField>().text;

        string postData = JsonUtility.ToJson(data);

        StartCoroutine(RegistroPost(postData));
    }

    IEnumerator LoginPost(string postData)
    {

        string url = URL + "/api/auth/login";

        UnityWebRequest www = UnityWebRequest.Put(url,postData);
        www.method = "POST";
        www.SetRequestHeader("content-type", "application/json");

        UIManager.Instance.ShowStaus("Conectando . . .");
        yield return www.SendWebRequest();

        if (www.isNetworkError)
        {
            Debug.Log("NETWORK ERROR :"+www.error);
            UIManager.Instance.ShowLogin();
        }
        else
        {
            // Show results as text
            Debug.Log(www.downloadHandler.text);

            if (www.responseCode == 200)
            {
                JsonData resData = JsonUtility.FromJson<JsonData>(www.downloadHandler.text);

                PlayerPrefs.SetString("token", resData.token);
                PlayerPrefs.SetString("username", resData.usuario.username);

                UIManager.Instance.ShowStaus("Autenticacion exitosa!");
            }
            else
            {
                string mensaje = "Status :" + www.responseCode;
                mensaje += "\ncontent-type:" + www.GetResponseHeader("content-type");
                mensaje += "\nError :" + www.error;
                Debug.Log(mensaje);

                UIManager.Instance.ShowLogin();
            }
            

        }
    }

    IEnumerator RegistroPost(string postData)
    {
        string url = URL + "/api/usuarios";

        UnityWebRequest www = UnityWebRequest.Put(url, postData);
        www.method = "POST";
        www.SetRequestHeader("content-type", "application/json");

        UIManager.Instance.ShowStaus("Procesando . . .");
        yield return www.SendWebRequest();

        if (www.isNetworkError)
        {
            Debug.Log("NETWORK ERROR :" + www.error);
            UIManager.Instance.ShowLogin();
        }
        else
        {
            // Show results as text
            Debug.Log(www.downloadHandler.text);

            if (www.responseCode == 200)
            {
                JsonData resData = JsonUtility.FromJson<JsonData>(www.downloadHandler.text);          

                UIManager.Instance.ShowStaus("Registro exitoso!"+ resData.usuario.username);

                StartCoroutine(LoginPost(postData));
            }
            else
            {
                string mensaje = "Status :" + www.responseCode;
                mensaje += "\ncontent-type:" + www.GetResponseHeader("content-type");
                mensaje += "\nError :" + www.error;
                Debug.Log(mensaje);

                UIManager.Instance.ShowLogin();
            }


        }
    }

    IEnumerator GetProfile()
    {
        string url = URL + "/api/usuarios/"+Username;

        UnityWebRequest www = UnityWebRequest.Get(url);
        //Como Agrego el token a la peticion?
        //El desarrollador del backend me dijo que lo colocara
        //en una cabecera "x-token"... pero como hago eso?
        // talvez algo como  www.SetRequestHeader("x-token",Token);


        UIManager.Instance.ShowStaus("Verificando . . .");
        yield return www.SendWebRequest();

        if (www.isNetworkError)
        {
            Debug.Log("NETWORK ERROR :" + www.error);
            UIManager.Instance.ShowLogin();
        }
        else
        {
            // Show results as text
            Debug.Log(www.downloadHandler.text);

            if (www.responseCode == 200)
            {
                //Token valido
                UIManager.Instance.ShowStaus("Token valido");
            }
            else
            {
                //Token no valido
                UIManager.Instance.ShowLogin();
                Debug.Log(www.error);
            }


        }
    }
}

[System.Serializable]
class JsonData
{
    public string username;
    public string password;
    public string msg;
    public UserData usuario;
    public string token;
}

[System.Serializable]
class UserData
{
    public int _id;
    public string username;
    public bool estado;
}
