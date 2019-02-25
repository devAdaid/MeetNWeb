using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoticeListUI : MonoBehaviour
{
    public NoticeUI noticeUI;
    public List<NoticeBtn> noticeBtns;
    public List<NoticeResponseData> noticeResponseDatas;

    public void OpenUI()
    {
        gameObject.SetActive(true);

        for (int i = 0; i < noticeResponseDatas.Count; i++)
        {
            noticeBtns[i].gameObject.SetActive(true);
            noticeBtns[i].SetTitle(noticeResponseDatas[i].title);
        }
        for (int i = noticeResponseDatas.Count; i < noticeBtns.Count; i++)
        {
            noticeBtns[i].gameObject.SetActive(false);
        }
    }

    public void SelectBtn(int idx)
    {
        SetNoticeUI(idx);
        noticeUI.gameObject.SetActive(true);
    }

    public void SetNoticeUI(int idx)
    {
        NoticeResponseData data = noticeResponseDatas[idx];
        noticeUI.idText.text = "ID: " + data.notice_id;
        noticeUI.titleText.text = data.title;
        noticeUI.contentText.text = data.contents;

        DateTime postTime = DateTime.Parse(data.post_at);
        noticeUI.postTimeText.text = string.Format("게시일: {0:yyyy.MM.dd}", postTime);

        DateTime beginTime = DateTime.Parse(data.begin_at);
        DateTime endTime = DateTime.Parse(data.finish_at);
        noticeUI.durationText.text = string.Format("기간: {0:yyyy.MM.dd}\n~{1:yyyy.MM.dd}", beginTime, endTime);
    }
}
