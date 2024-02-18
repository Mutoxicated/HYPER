using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Message : MonoBehaviour
{
    [SerializeField] private Feed feed;
    [SerializeField] private Image image;
    [SerializeField] private TMP_Text message;

    private float lifetime = 10f;//seconds
    private float time = 0f;

    private Vector3 alteredPos;

    private Vector3 initScale;
    private Vector3 alteredScale;

    private Color alteredColor;
    private float alpha;
    private float preDeterminedAlpha;

    private int index;
    private float lerpSpeed = 5f;
    private float scaleT;

    private void Start(){
        initScale = transform.localScale;
        alteredScale = initScale;
        alteredScale.x = 0f;
        transform.localScale = alteredScale;
    }

    private void OnEnable(){
        time = 0f;
    }

    private void Update(){
        scaleT = Mathf.InverseLerp(0f,0.1f,time);
        scaleT = Mathf.Clamp01(scaleT);
        alteredScale = initScale;
        alteredScale.x *= scaleT;
        transform.localScale = alteredScale;
        time += Time.deltaTime;
        alpha = Mathf.InverseLerp(lifetime,0f,time);

        if (preDeterminedAlpha*alpha <= 0.001f){
            gameObject.SetActive(false);
            feed.RetrieveMessage(this);
            return;
        }

        alteredColor = image.color;
        alteredColor.a = preDeterminedAlpha*alpha;
        image.color = Color.Lerp(image.color, alteredColor, Time.deltaTime*lerpSpeed);

        alteredColor = message.color;
        alteredColor.a = preDeterminedAlpha*alpha;
        message.color = Color.Lerp(message.color, alteredColor, Time.deltaTime*lerpSpeed);

        transform.localPosition = Vector3.Lerp(transform.localPosition,alteredPos,Time.deltaTime*lerpSpeed);
    }

    private void UpdatePos(){
        alteredPos = feed.GetBasePosition();
        alteredPos.y += Feed.space*index;
    }

    private void UpdatePredeterminedAlpha(){
        preDeterminedAlpha = Mathf.InverseLerp(Feed.maxIndex,0,index);
    }

    public void PopMessage(string text, int index){
        message.text = text;
        this.index = index;
        UpdatePos();
        UpdatePredeterminedAlpha();
        gameObject.SetActive(true);
    }

    public void IncrementIndex(){
        index++;
        UpdatePos();
        UpdatePredeterminedAlpha();
    }
}
