using UnityEngine;

public class DragToMove : MonoBehaviour
{
    public GameObject moveGroup;
    private Vector3 mOffset;
    public float moveRangeMax;
    public float moveRangeMin;
    private float mZCoord;
    private float playerOffset;
    private bool isPlayerOn = false;
    public GameObject player;
    public MoveManager manager;
    private void OnMouseDown()
    {
        //set offset for mousepoint on the world space
        mZCoord = Camera.main.WorldToScreenPoint(transform.position).z;
        mOffset = transform.position - GetMousePosWorld();
        //check if player on top of the moving cube group, if so set the isplayer on true;
        Transform playerTranform = GameObject.Find("Player").transform;
        Ray playerRay = new Ray(playerTranform.position, -transform.up);
        RaycastHit playerHit;
        if (Physics.Raycast(playerRay, out playerHit))
            if (playerHit.transform.tag == "moveCube")
            {
                isPlayerOn = true;
                playerOffset = playerTranform.position.x - moveGroup.transform.position.x;
            }
            else
                isPlayerOn = false;
    }
    //function to get the world positio of mouse
    private Vector3 GetMousePosWorld()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = mZCoord;
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }
    
    //functions when mouse the draging the moving cube
    private void OnMouseDrag()
    {
        GameObject.Find("move").GetComponent<AudioSource>().Play();   //SFX
        manager.StopAnimation();
        //if the player is on and the player is moving, dont move moving block
        if (isPlayerOn)
        {
            Debug.Log("PLAYER IS ON");
            if (player.transform.GetComponent<PlayerController>().OnMove())
            {
                Debug.Log("PLAYER IS ON");
                return;
            }
        }
        
        //get the updated mouse position
        Vector3 newPos = GetMousePosWorld() + mOffset;
        //check if the moving block would exceed the limit range
        if (newPos.x < moveRangeMin)
            newPos.x = moveRangeMin;
        else if (newPos.x > moveRangeMax)
            newPos.x = moveRangeMax;
        
        //move the handle and the moving cube group
        moveGroup.transform.position = new Vector3(newPos.x, moveGroup.transform.position.y, moveGroup.transform.position.z);
        transform.position = new Vector3(newPos.x, transform.position.y, transform.position.z);
        //if the player is on the moving cube group, move the player also
        if (isPlayerOn)
            player.transform.position = new Vector3(newPos.x + playerOffset, player.transform.position.y, player.transform.position.z);
        //disable player movement when currently cube moving group is moving
        else
            player.transform.GetComponent<PlayerController>().KillMovement();
    }
}
