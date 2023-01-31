using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using ElRaccoone.Tweens;

public class WorldSpaceExample : MonoBehaviour
{
    public TMP_Text infoBox;
    public GameObject UIElement;
    public GameObject UIElement2;

    public GameObject WorldSprite;
    public GameObject WorldObject;

    //reference to the object currently dragged
    public GameObject objectDragged;


    // Start is called before the first frame update
    void Start()
    {
        //just move the object back and forth
        WorldObject.TweenPosition(new Vector3(-4, 2, 3), 3f).SetEaseCubicInOut().SetPingPong().SetInfinite();
        WorldObject.TweenRotationY(180f, 1f).SetFrom(0).SetPingPong().SetEaseCubicInOut().SetInfinite();
    }

    // Update is called once per frame
    void Update()
    {

        //convert the mouse position into world point
        Vector2 worldPoint = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));

        string info = "";
        info += "Screen pixels: " + Camera.main.pixelWidth + ":" + Camera.main.pixelHeight + "<br>";

        info += "Input.mousePosition: " + Input.mousePosition + "<br>";

        info += "World position: " + worldPoint.ToString("F3") + "<br>";

        infoBox.text = info;


        
        //Scenario: I have an object in the world space (out of the canvas)
        //and I want to overimpose a UI to it
        //I'm not using the position directly because I want to offset it a bit and stay on the canvas plane
        UIElement2.transform.position = new Vector2(WorldObject.transform.position.x, WorldObject.transform.position.y + 2);


        if(objectDragged != null)
        {
            //a UI element can be moved through the (world) position
            //which is different from the anchored position shown in the rect transform
            //anchored position is more useful when determining the layout of an interface
            objectDragged.transform.position = worldPoint;
        }


        //to detect clicks in the world space you need to raycast
        
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;

            //convert the mouse position to a ray based on the current camera
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            //if it hit a collider get it
            if (Physics.Raycast(ray, out hit))
            {
                //from the hit object you can access the collider and the game object
                //and check tags, names, or object references
                if(hit.collider.gameObject == WorldObject)
                {
                    print("Cube hit!");
                    
                    WorldObject.TweenLocalScale(new Vector3(2, 2, 2), 0.3f).SetEaseCircIn();
                    WorldObject.TweenLocalScale(new Vector3(1, 1, 1), 0.3f).SetEaseCircOut().SetDelay(0.3f);

                }

                //the red circle has a box collider attached so it's detected by the same kind of raycast
                if (hit.collider.gameObject.name == "Sprite3D")
                {
                    print("Sprite 3d hit!");
                    SpriteRenderer sr = hit.collider.gameObject.GetComponent<SpriteRenderer>();
                    
                    sr.color = Color.white;
                    sr.TweenCancelAll();
                    sr.TweenSpriteRendererColor(new Color(0.870f, 0.33f, 0.29f), 0.5f).SetEaseCircOut();
                }
            }
        }

        //Remember the 2D physics system is separate from the 3d on so I have to use a 2D raycast to get the circle
        //the sprite needs a collider! It is not automatically added
        //the rest is basically the same
        if (Input.GetMouseButtonDown(0))
        {
            //Get the mouse position on the screen and send a raycast into the game world from that position.
            Vector2 worldPt = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            //a slightly different way to use raycast, the result is returned without using the "out"
            RaycastHit2D hit = Physics2D.Raycast(worldPt, Vector2.zero);

            if (hit.collider != null)
            {
                if (hit.collider.gameObject == WorldSprite)
                {
                    print("2D Sprite hit!");

                    WorldSprite.TweenLocalScale(new Vector3(0.5f, 0.5f, 0.5f), 0.3f).SetEaseCircIn();
                    WorldSprite.TweenLocalScale(new Vector3(0.2f, 0.2f, 0.2f), 0.3f).SetEaseCircOut().SetDelay(0.3f);
                }
            }
        }



    }

    //these functions are called by the event trigger component set up in the inspector
    //which uses the same event system of buttons and ui but it's more flexible

    //drag and drop functionality without using drag events
    public void PointerDown(GameObject obj) {

        
        //nothing is being dragged
        if (objectDragged == null)
        {
            //then the current clicked thing is the currently dragged one
            //the actual position is changed in the Update
            objectDragged = obj;
        }
    }

    //when pointer goes up reset the dragged object
    public void PointerUp()
    {
        objectDragged = null;
    }

}
