using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
//remember to import this
using ElRaccoone.Tweens;

//Some example based on the unity tweens library
//which works well on ui elements
//https://github.com/jeffreylanters/unity-tweens


//easing functions cheat sheet
//https://easings.net/

public class Tweens : MonoBehaviour
{
    public Image yellowBar;

    void Start()
    {
        
    }

    void Update()
    {
        

    }
    
    public void BumpOn(GameObject go)
    {
        //increase the scale with Quad Out easing
        go.TweenCancelAll();
        go.TweenLocalScale(new Vector3(1.5f, 1.5f, 1f), 0.2f).SetEaseQuadOut();
    }

    public void BumpOff(GameObject go)
    {
        //set scale to 1 with Quad In easing
        go.TweenCancelAll();
        go.TweenLocalScale(new Vector3(1f, 1f, 1f), 0.2f).SetEaseQuadIn();
    }

    public void SquashStretch(GameObject go)
    {
        go.TweenCancelAll();
        
        go.TweenLocalScale(new Vector3(1.5f, 0.5f, 1f), 0.2f).SetEaseCubicOut();

        //Call the second tween with a delay equal to the duration of the first
        go.TweenLocalScale(new Vector3(1f, 1f, 1f), 0.2f).SetEaseBackIn().SetDelay(0.2f);

    }

    //this function will always apply to the yellow bar but the fill value can be passed as argument (set from inspector)
    public void FillBar(float value)
    {
        yellowBar.TweenCancelAll();

        //instead of a fixed duration I make it dependent on the amount I have to fill
        float time = Mathf.Abs(yellowBar.fillAmount-value) * 2;

        yellowBar.TweenImageFillAmount(value, time).SetEaseSineInOut();
        
    }

    public void MoveToPositionSine(GameObject go)
    {
        go.TweenAnchoredPositionX(-47, 2).SetEaseSineInOut();
    }

    public void MoveToPositionQuad(GameObject go)
    {
        go.TweenAnchoredPositionX(-47, 2).SetEaseCubicInOut();

    }

    

    public void ComplexMovement(GameObject go)
    {
        go.TweenCancelAll();

        go.TweenAnchoredPosition(new Vector2(490, -190), 1f).SetEaseBounceOut().SetOvershooting(0.2f);
        
        go.TweenGraphicColor(Color.blue, 0.2f).SetEaseCircInOut().SetPingPong().SetFrom(Color.red);

        go.TweenLocalRotationZ(45f, 0.1f).SetEaseCircInOut();

    }

    public void CoroutineExample(GameObject go)
    {
        //coroutines need to be "started" not called like functions
        StartCoroutine(TweenSequence(go));

        
    }

    private IEnumerator TweenSequence(GameObject go)
    {
        //make it non clickable
        go.GetComponent<Image>().raycastTarget = false;
        go.TweenCancelAll();

        //yield and .yield() waits for this operation to end before moving to the next
        yield return go.TweenLocalRotationZ(180, 1f).SetEaseQuadIn().Yield();

        //do nothing for 1 second
        yield return new WaitForSeconds(1);
        
        //anchored position is just for rect transforms so I access that
        RectTransform rt = go.GetComponent<RectTransform>();

        yield return rt.TweenAnchoredPosition(new Vector2(495, -176f), 2f).SetEaseExpoInOut().Yield();

        //this one won't yield so it starts at the same time as the one below
        go.TweenGraphicColor(Color.white, 3f).SetEaseCircInOut();

        yield return go.TweenLocalScale(new Vector3(2,2,2), 1f).SetEaseSineInOut().Yield();
        yield return go.TweenLocalScale(new Vector3(1, 1, 1), 2f).SetEaseSineInOut().Yield();
        

    }
}
