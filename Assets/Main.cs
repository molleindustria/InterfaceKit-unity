using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

//game brain to handle all the interactions in a centralized way
public class Main : MonoBehaviour
{
    

    //the prefab to create a "card" dynamically
    public Interactable cardPrefab;

    //the prefab for a generic UI item
    public GameObject itemPrefab;

    //keeping a list of all the interactables
    //lists are like arrays with variable lenght
    public List<Interactable> interactables;

    public float snapDistance = 100f;

    //keeping a reference of the canvas for instantiation
    public Canvas canvas;
    public Transform spawnArea;

    public Image pie;

    public bool charging = false;

    // Start is called before the first frame update
    void Start()
    {

        //keeping an up to date list of all the interactables
        UpdateInteractableList();
    }

    // Update is called once per frame
    void Update()
    {

        //charging is a public variable changed from the "button" InteractableD events
        if(charging)
        {
            pie.fillAmount += Time.deltaTime / 2;
        }
        else
        {
            pie.fillAmount -= Time.deltaTime / 3;
        }

    }



    /////All the events called by the interactables

    //pointer down
    public void InteractableDown(Interactable inter, PointerEventData eventData)
    {
        if (inter.draggable)
        {
            //if the object is meant to be picked up 
            //bring it to the bottom of the hierarchy thus above all the other objects in the container
            inter.transform.SetSiblingIndex(inter.transform.parent.childCount - 1);
        }
    }

    //pointer up
    public void InteractableUp(Interactable inter, PointerEventData eventData)
    {

    }

    public void InteractableBeginDrag(Interactable inter, PointerEventData eventData)
    {

    }

    //every frame it's dragged
    public void InteractableDrag(Interactable inter, PointerEventData eventData)
    {

        if (inter.draggable)
        {
            inter.transform.position = Input.mousePosition;


            //look for all interactables
            foreach (Interactable i in interactables)
            {
                //if its not the one being dragged and it's snappable check the snapping
                if (i != inter && i.snappable)
                {
                    float distance = Vector2.Distance(inter.transform.position, i.transform.position);

                    if (distance < snapDistance)
                    {
                        inter.transform.position = i.transform.position;
                    }
                }
            }
        }
    }

    //dragged object is dropped
    public void InteractableEndDrag(Interactable inter, PointerEventData eventData)
    {
        //look for all interactables
        foreach (Interactable i in interactables)
        {
            //if its not the one being dragged check the overlap
            //since the interaction may result in the destruction of object on the list 
            //I make sure they are not null to avoid game breaking null errors
            if (i != inter && i != null && inter != null)
            {
                if (AreOverlapping(inter.gameObject, i.gameObject))
                {
                    Interaction(inter, i);

                }
            }
        }

        //since the interaction may have change the cards in the scene I get a new list
        UpdateInteractableList();
    }

    //pointer roll over
    public void InteractableEnter(Interactable inter, PointerEventData eventData)
    {
    }

    //pointer rolls out
    public void InteractableExit(Interactable inter, PointerEventData eventData)
    {

    }

    //example of object to object interaction
    public void Interaction(Interactable a, Interactable b)
    {
        print(a.name + " is interacting with " + b.name);

        //a has been dropped on b do something
        //there are many ways to differentiate object, through tags, public properties, subclasses
        //to keep it simple and generic I'll use the name as identifier

        if ((a.name == "Red" && b.name == "Green") || (b.name == "Red" && a.name == "Green"))
        {

            //create a new "card"
            Interactable newInteractable = CreateCard("Blue");
            
            newInteractable.transform.position = a.transform.position;

            //"combine" them by destroying them and creating a new one
            //remember to destroy the gameObject not the script
            Destroy(a.gameObject);
            Destroy(b.gameObject);

        }

    }


    


    //called from "button"
    public void CreateItem()
    {
        GameObject newItem = Instantiate(itemPrefab);

        //I spawn it in an area with a layout element so it will be automatically positioned
        newItem.transform.SetParent(spawnArea.transform);
    }

    //called from "button"
    public void CreateCardFromButton(string type)
    {
        Interactable card = CreateCard(type);
        
        //if I want to position an object on canvas I would use anchoredPosition
        //which is the position display on the rectTransform inspector 
        card.GetComponent<RectTransform>().anchoredPosition = new Vector2(-710, 407);
        
    }

    public void ChargeOn()
    {
        charging = true;
    }

    public void ChargeOff()
    {
        charging = false;
    }



    public Interactable CreateCard(string type)
    {

        //create a new "card"
        Interactable newInteractable = Instantiate(cardPrefab);

        //parent it to the canvas or it will be invisible
        newInteractable.transform.SetParent(canvas.transform);

        //differentiate it by changing color and name
        //initialize it
        newInteractable.name = type;

        //color depends on type
        if (type == "Blue")
        {
            //color32 is the notation 0-255 of the color picker as opposed to 0-1
            newInteractable.GetComponent<Image>().color = new Color32(18, 162, 202, 255);
        }
        if(type == "Yellow")
        {
            //color32 is the notation 0-255 of the color picker as opposed to 0-1
            newInteractable.GetComponent<Image>().color = new Color32(251, 211, 81, 255);
        }
        
        
        newInteractable.draggable = true;
        newInteractable.changesColor = false;

        //add it to the list
        UpdateInteractableList();

        return newInteractable;
    }

    public void UpdateInteractableList()
    {
        //FindObjectsOfType returns an array which has fixed length
        Interactable[] interArray = GameObject.FindObjectsOfType<Interactable>();

        //so I create a global list populated with the interactables on the scene
        interactables = new List<Interactable>(interArray);
    }

    //check if two UI elements (have rectTransform) bounding boxes are overlapping
    public bool AreOverlapping(GameObject a, GameObject b)
    {
        RectTransform rectTransform1 = a.GetComponent<RectTransform>();
        RectTransform rectTransform2 = b.GetComponent<RectTransform>();
        bool over = false;

        if (rectTransform1 != null && rectTransform2 != null)
        {
            Vector3[] corners = new Vector3[4];
            rectTransform1.GetWorldCorners(corners);
            Rect rec1 = new Rect(corners[0].x, corners[0].y, corners[2].x - corners[0].x, corners[2].y - corners[0].y);

            rectTransform2.GetWorldCorners(corners);
            Rect rec2 = new Rect(corners[0].x, corners[0].y, corners[2].x - corners[0].x, corners[2].y - corners[0].y);

            if (rec1.Overlaps(rec2))
            {
                over = true;
                Debug.Log("rectTransform1 is Overlapping rectTransform2!");
            }
        }

        return over;
    }
}
