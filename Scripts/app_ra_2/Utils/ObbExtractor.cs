using System.IO;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
 https://developer.vuforia.com/forum/android/load-dataset-android-split-binary-obb

    Quando há o split de binario para Android é necessário rodar esta cena antes do splash
 */

public class ObbExtractor : MonoBehaviour
{

    void Start()
    {
        StartCoroutine(ExtractObbDatasets());
    }

    private IEnumerator ExtractObbDatasets()
    {
        string[] filesInOBB = { "FTD_imagem_imersiva_MP.dat", "FTD_imagem_imersiva_MP.xml", "FTD_imagem_imersiva.dat", "FTD_imagem_imersiva.xml" };
        foreach (var filename in filesInOBB)
        {
            string uri = Application.streamingAssetsPath + "/Vuforia/" + filename;

            string outputFilePath = Application.persistentDataPath + "/Vuforia/" + filename;
            if (!Directory.Exists(Path.GetDirectoryName(outputFilePath)))
                Directory.CreateDirectory(Path.GetDirectoryName(outputFilePath));

            var www = new WWW(uri);
            yield return www;

            Save(www, outputFilePath);
            yield return new WaitForEndOfFrame();
        }

        // When done extracting the datasets, Start Vuforia AR scene
        SceneManager.LoadScene("Loader");
    }

    private void Save(WWW w, string outputPath)
    {
        File.WriteAllBytes(outputPath, w.bytes);

        // Verify that the File has been actually stored
        if (File.Exists(outputPath))
        {
            Debug.Log("File successfully saved at: " + outputPath);
        }
        else
        {
            Debug.LogError("Failure!! - File does not exist at: " + outputPath);
        }
    }
}
