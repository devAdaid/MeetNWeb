using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

[System.Serializable]
public class NoticeData
{
    public int statusCode;
    public string responseMessage;
    public List<NoticeResponseData> responseData;
}

[System.Serializable]
public class NoticeResponseData
{
    public int notice_id;
    public string type;
    public string title;
    public string contents;
    public string filelink;
    public string post_at;
    public string finish_at;
    public string begin_at;
}

public class NoticeManager : MonoBehaviour
{
    public string cmsPath;
    public NoticeListUI noticeList;
    private NoticeData noticeData;

    void Start()
    {
        GetNoitice();
    }

    public void GetNoitice()
    {
        StartCoroutine("EmptyBodyUpload", ServerDefine.cmsServerRoot + "/notice");
    }

    public void OpenNotice()
    {
        noticeList.OpenUI();
    }

    IEnumerator EmptyBodyUpload(string path)
    {
        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();

        UnityWebRequest www = UnityWebRequest.Get(path);

        //AccountInfo.token = "Bearer eyJhbGciOiJIUzI1NiJ9.eyJzdWIiOiI0Iiwicm9sZXMiOiJST0xFX0FETUlOIiwiaWF0IjoxNTUxMDcxNzk5LCJleHAiOjE1NTEzMzA5OTl9.c29CwZMrgfTKly3Ha8JlqiGy9TYe18SuA9X9knET36A";
        www.SetRequestHeader("Authentiation", AccountInfo.token);

        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            //Debug.Log(www.downloadHandler.text);
            noticeData = JsonUtility.FromJson<NoticeData>(www.downloadHandler.text);

            for (int i = 0; i < noticeData.responseData.Count; i++)
            {
                noticeList.noticeResponseDatas.Add(noticeData.responseData[i]);
            }
            OpenNotice();
        }
    }
}
