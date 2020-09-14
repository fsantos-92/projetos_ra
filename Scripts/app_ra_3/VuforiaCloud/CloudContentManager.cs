using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using App.Tools;
using TMPro;
using TriLib;
using App.Screens;


public class CloudContentManager : MonoBehaviour
{
    [Header("Url Content Objects")]
    [SerializeField]
    Image ImageContent;
    [SerializeField]
    GameObject ScrollImgContent;
    [SerializeField]
    GameObject VideoPlayerGameObject;
    [SerializeField]
    GameObject ModelParent;

    [Header("Screens")]
    [SerializeField]
    MainScreen mainScreen;

    [SerializeField]
    GameObject ScreenImageContent;
    [SerializeField]
    GameObject ScreenVideoContent;
    [SerializeField]
    GameObject ScreenModelContent;
   
    [Header("Title texts")]
    [SerializeField]
    TextMeshProUGUI TxtTitleImage;
    [SerializeField]
    TextMeshProUGUI TxtTitleVideo;
    [SerializeField]
    TextMeshProUGUI TxtTitle3dModel;

    [SerializeField]
    Slider sliderProgress;

    [Header("3D Model Loader")]
    [SerializeField]
    GameObject ModelLoader;

    void OnDisable()
    {
        StopAllCoroutines();
    }
    public void HandleTargetFinderResult(Vuforia.TargetFinder.CloudRecoSearchResult targetSearchResult)
    {
        if (ScreenImageContent.activeInHierarchy || ScreenModelContent.activeInHierarchy || ScreenVideoContent.activeInHierarchy)
            return;
        Debug.Log("<color=yellow>HandleTargetFinderResult(): " + targetSearchResult.TargetName + "</color>");
        Handheld.Vibrate();
        StartCoroutine(CheckUrl(targetSearchResult.MetaData));
    }

    IEnumerator CheckUrl(string url)
    {
        
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            // Request and wait for the desired page.

            sliderProgress.value = 0;
            this.mainScreen.ShowLoadScreen();
            webRequest.SendWebRequest();
            while (!webRequest.isDone)
            {
                //TxtDownloadProgress.text = "Progresso: " + (int)(webRequest.downloadProgress * 100f) + "%";
                sliderProgress.value = Mathf.Lerp(sliderProgress.value, webRequest.downloadProgress, Time.deltaTime * 3);

                yield return null;
            }

            if (webRequest.isNetworkError || webRequest.isHttpError)
            {
                Debug.Log("Erro no download url: " + url + "erro: " + webRequest.error);
                //this.ImgBoxError.SetActive(true);
                //yield return new WaitForSeconds(2f);
                this.mainScreen.HideLoadScreen();
            }
            else
            {
                //mainScreen.ShowLoadScreen();
                Debug.Log("Download concluido: " + url);
                yield return new WaitForSeconds(3f);
                string[] format = url.Split('/');
                string test = format[format.Length - 2];
                string title = format[format.Length - 1];

                if (test == "Imagem")
                {
                    this.TxtTitleImage.text = title;
                    this.PrepareImageContent(url);
                }
                if (test == "Video")
                {
                    this.TxtTitleVideo.text = title;
                    
                    this.PrepareVideoContent(url);
                }
                if (test == "3d")
                {
                    this.TxtTitle3dModel.text = title;
                    this.PrepareModelContent(url);
                }
                mainScreen.HideLoadScreen();
                mainScreen.Disable();
            }
        }
    }

    void PrepareVideoContent(string url)
    {
        this.ScreenVideoContent.SetActive(true);
        this.VideoPlayerGameObject.GetComponent<CustomVideoPlayer>()?.SetURL(url);
    }
    void PrepareImageContent(string url)
    {
        StartCoroutine(GetTexture(url));
    }
    void PrepareModelContent(string url)
    {
        //
        string[] a = url.Split('/');
        string[] b = a[a.Length - 1].Split('.');
        string extension = "." + b[b.Length - 1];

        this.ModelParent.GetComponent<Transform>().localScale = Vector3.one;
        this.ModelParent.GetComponent<Transform>().position = Vector3.zero;
        this.ModelParent.GetComponent<Transform>().localRotation = new Quaternion(0, 0, 0, 1);

        this.ModelLoader.GetComponent<AssetDownloader>().StartDownload(url, extension);

        this.ScreenModelContent.SetActive(true);
    }
    IEnumerator GetTexture(string url)
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
        yield return www.SendWebRequest();

        Texture2D myTexture = DownloadHandlerTexture.GetContent(www);
        if (myTexture != null)
        {
            this.ScrollImgContent.GetComponent<NewImageZoom>().ResetPosition();
            this.ImageContent.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width, Screen.width);
            this.ImageContent.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
            Rect rect = new Rect(0, 0, myTexture.width, myTexture.height);
            this.ImageContent.sprite = Sprite.Create(myTexture, rect, new Vector2(0.5f, 0.5f));
            this.ImageContent.preserveAspect = true;

            
        }
        else
        {
            //TODO implementar mensagem de erro
        }

        this.ScreenImageContent.SetActive(true);
    }
}
