using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarController : MonoBehaviour
{
    public GameObject MouseBox;
    public Animator thisAnimator;
    public UnityEngine.AI.NavMeshAgent thisNavMeshAgent;

    public Item ItemOnHead;
    public Item[] Inventory;
    public int currentItemIndex;
    //delte ItemInHand

    public enum MovementStateT { StandStill, walking }
    public MovementStateT currentState;

    private void Start()
    {
        thisNavMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();

        currentState = MovementStateT.StandStill;
    }

    // Update is called once per frame
    void Update()
    {
        Ray rayFromCameraToMouse;
        RaycastHit closestClickableGroundOnRay;

        //Make MouseBox follow mouse pointer
        rayFromCameraToMouse = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(rayFromCameraToMouse, out closestClickableGroundOnRay, Mathf.Infinity, LayerMask.GetMask("ClickableGround"));
        MouseBox.transform.position = closestClickableGroundOnRay.point;

        //Make MouseBox change size based on mouse button state
        if (Input.GetMouseButton(0))
            //When left button is down
            MouseBox.transform.localScale = new Vector3(10, 10, 10);
        else
            //When left button is up
            MouseBox.transform.localScale = new Vector3(5, 5, 5);

        //Tell Avatar's NavMeshAgent to go to MouseBox
        if (Input.GetMouseButtonDown(0))
            thisNavMeshAgent.SetDestination(MouseBox.transform.position);

        //Make animation speed match movement speed
        GetComponent<Animator>().SetFloat("WalkSpeed", .2f * thisNavMeshAgent.velocity.magnitude);

        if (Input.GetKeyDown(KeyCode.G))
        {
            //if already holding a item, do nothing
            if (Inventory[currentItemIndex] != null)
                Debug.Log("You can't get anything new, because your hands are full.");
            else
            {
                //look for an item.
                Collider[] overlappingItems;
                //Make sure you adjust vectors to fit the size of your avatar
                overlappingItems = Physics.OverlapBox(transform.position + 2 * Vector3.forward, 3 * Vector3.one, Quaternion.identity,
                    LayerMask.GetMask("Item"));

                //if no items found in front of you
                if (overlappingItems.Length == 0)
                    Debug.Log("There is no items in front of you.");
                else
                {
                    //else handle pick up of item
                    //if have an item in hand, drop it.
                    if (Inventory[currentItemIndex] != null)
                    {
                        Inventory[currentItemIndex].transform.SetParent(null);
                        Inventory[currentItemIndex] = null;
                    }
                    //then, pick up the first overlapping item
                    Inventory[currentItemIndex] = overlappingItems[0].GetComponent<Item>();
                    Inventory[currentItemIndex].transform.SetParent(gameObject.transform);
                    Inventory[currentItemIndex].transform.localPosition = new Vector3(0, 35, 1);
                    Debug.Log("You picked up a " + Inventory[currentItemIndex].name);
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            //if holding item, drop it.
            if (Inventory[currentItemIndex] != null)
            {
                //item will not stay in the air
                Inventory[currentItemIndex].transform.localPosition = new Vector3(0, 0, 1);

                Inventory[currentItemIndex].transform.SetParent(null);
                Debug.Log("You drooped the" + Inventory[currentItemIndex].name);
                Inventory[currentItemIndex] = null;
            }
            else
                Debug.Log("You can't drop an item, because you're not holding anything.");
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //if holding item, try to use it.
            if (Inventory[currentItemIndex] != null)
                Inventory[currentItemIndex].GetComponent<Item>().Use();
            else
                Debug.Log("You can't use an item, because you're not holding anything.");
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            //if holding item, try to wear it.
            if (Inventory[currentItemIndex] != null)
                Inventory[currentItemIndex].GetComponent<Item>().Wear();
            else
                Debug.Log("You can't wear an item, because you're not holdiing anything.");
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            //if holding item, try to wear it.
            if (Inventory[currentItemIndex] != null)
                Inventory[currentItemIndex].GetComponent<Item>().Eat();
            else
                Debug.Log("You can't eat an item, because you're not holding anything.");
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            //if holding an item, deactivate it.
            if (Inventory[currentItemIndex] != null)
                Inventory[currentItemIndex].gameObject.SetActive(false);

            //shift to next inventory slot.
            Debug.Log("You rearrange your inventory.");
            currentItemIndex++;
            if (currentItemIndex >= Inventory.Length)
                currentItemIndex = 0;

            //if there is an item in the slot,activate the item and place it in front of Avatar.
            if (Inventory[currentItemIndex] != null)
            {
                Inventory[currentItemIndex].gameObject.SetActive(true);
                Inventory[currentItemIndex].transform.localPosition = new Vector3(0, 25, 1);
                Debug.Log("You're now holding a" + Inventory[currentItemIndex].name);
            }
            else
                Debug.Log("You're now holding nothing.");
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            //if holding an item, deactivate it.
            if (Inventory[currentItemIndex] != null)
                Inventory[currentItemIndex].gameObject.SetActive(false);

            //shift to next inventory slot.
            Debug.Log("You rearrange your inventory.");
            currentItemIndex--;
            if (currentItemIndex < 0)
                currentItemIndex = Inventory.Length - 1;

            //if there is an item in the slot, activate the item and place it in front of Avatar.
            if (Inventory[currentItemIndex] != null)
            {
                Inventory[currentItemIndex].gameObject.SetActive(true);
                Inventory[currentItemIndex].transform.localPosition = new Vector3(0, 25, 1);
                Debug.Log("You are now holding a" + Inventory[currentItemIndex].name);
            }
            else
                Debug.Log("You are now holding nothing.");
        }

    }
    private int FindLocationOfFirst(System.Type targetItemType)
    {
        //it's also another good example of a linear search.

        for (int searchPosition = 0; searchPosition < Inventory.Length; searchPosition++)
        {
            //this uses some fancy stuff. You can look up GetType and IsAssignableFrom if you like.
            if (Inventory[searchPosition] != null && targetItemType.IsAssignableFrom(Inventory[searchPosition].GetType()))
                return searchPosition;
        }
        return -1;
    }
}
