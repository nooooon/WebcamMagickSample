using System.Collections;
using UnityEngine;
using System.IO;
using ImageMagick;

public class WebcamCapture : MonoBehaviour {

    private WebCamTexture webcamTex;
    private WebCamDevice userCameraDevice;
    private ImageMagickProcess imageMagickProcess;

    private const int WIDTH = 1920;
    private const int HEIGHT = 1080;
    private const int FPS = 30;

    public enum FileType {
        jpg,
        png
    }
    [SerializeField] FileType fileType = FileType.jpg;


    IEnumerator Start() {

        string cameraName = "";
    
        WebCamDevice[] devices = WebCamTexture.devices;

        yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);

        if(!Application.HasUserAuthorization(UserAuthorization.WebCam)) {
            Debug.LogFormat("No Authorizatio");
            yield break;
        }
        
        if(devices.Length == 0) {
            Debug.LogFormat("No Device");
            yield break;
        }else {
            for (int i = 0; i < devices.Length; i++) {
                Debug.Log(devices[i].name);
                if(devices[i].name == "HD Pro Webcam C920") {
                    userCameraDevice = devices[i];
                    cameraName = devices[i].name;
                }
            }
        }

        if(cameraName == null) {
            userCameraDevice = devices[1];
            cameraName = devices[1].name;
        }

        webcamTex = new WebCamTexture(userCameraDevice.name, WIDTH, HEIGHT, FPS);

        Renderer _renderer = GetComponent<Renderer>();
        _renderer.material.mainTexture = webcamTex;

        imageMagickProcess = new ImageMagickProcess();

        webcamTex.Play();
    }


    void Update() {

        if(Input.GetKeyDown(KeyCode.Space)) {
            if(webcamTex != null) {
                SaveCapture(webcamTex.GetPixels(), Application.dataPath + "/../capture");
                Debug.Log(fileType);
            }
        }
    }

    private void SaveCapture(Color[] pixels, string savePath) {
        Texture2D texture = new Texture2D(WIDTH, HEIGHT, TextureFormat.ARGB32, false);

        texture.SetPixels(pixels);
        texture.Apply();

        byte[] img;
        switch (fileType) {

            case FileType.jpg:
                img = ImageConversion.EncodeToJPG(texture, 80);
                break;

            case FileType.png:
                img = texture.EncodeToPNG();
                break;

            default:
                img = texture.EncodeToPNG();
                break;
        }

        MagickImage convertImg = imageMagickProcess.Comvert(img, Application.dataPath + "/Images/cover.png");
        imageMagickProcess.Save(convertImg, savePath, System.Enum.GetName(typeof(FileType), fileType));
        
        // base64 encoding
        Debug.Log(convertImg.ToBase64());
        //Debug.Log(System.Convert.ToBase64String(img));
        
        Destroy(texture);

        //File.WriteAllBytes(savePath + "." + fileType, img);

    }

}
