using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public Transform currentCube;
    public Transform clickedCube;
    public List<Transform> finalPath;
    public Animator anim;
    public float walkingSpeed;
    public Camera cam;
    private float timePerUnitMove;
    public float clickTimeInterval = 0.5f;
    private float clickSecondsCount;
    private Sequence s;
    // public GameObject rotationGear;


    // Start is called before the first frame update
    void Start()
    {
        Screen.orientation = ScreenOrientation.PortraitUpsideDown;
        RayCastDown();
        timePerUnitMove = 1f / walkingSpeed;
        clickSecondsCount = clickTimeInterval;
        // rotationGear = GameObject.Find("RotationGear");
    }
    // Update is called once per frame
    void Update()
    {
        //find player current block
        RayCastDown();
        clickSecondsCount += Time.deltaTime;
        //camera raycast to find the clicked block ref:https://docs.unity3d.com/ScriptReference/Physics.Raycast.html
        if (Input.GetMouseButtonDown(0))
        {
            if (clickSecondsCount < clickTimeInterval)
                return;
            clickSecondsCount = 0;
            Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit mouseHit;
            if (Physics.Raycast(mouseRay, out mouseHit))
            {
                if (mouseHit.transform.GetComponent<Walkable>() != null)
                {
                    Clear();
                    anim.SetBool("isWalking", true);
                    clickedCube = mouseHit.transform;
                    BuildPath();
                }
            }
        }
    }
    private void RayCastDown()
    {
        Ray playerRay = new Ray(transform.position, -transform.up);
        RaycastHit playerHit;
        if (Physics.Raycast(playerRay, out playerHit))
        {

            if (playerHit.transform.GetComponent<Walkable>() != null)
            {
                currentCube = playerHit.transform;
            }
        }
    }
    //Use djikstra algorithm to find the path from the current cube to clicked cube
    private void FindPath()
    {
        List<Transform> nextCubes = new List<Transform>();
        List<Transform> pastCubes = new List<Transform>();
        foreach (GamePath path in currentCube.GetComponent<Walkable>().possiblePath)
        {
            if (path.active)
            {
                nextCubes.Add(path.target);
                path.target.GetComponent<Walkable>().previousBlock = currentCube;
            }
        }
        pastCubes.Add(currentCube);
        ExploreCube(nextCubes, pastCubes);
    }
    //supporting method to findPath function
    private void ExploreCube(List<Transform> nextCubes, List<Transform> visitedCubes)
    {
        Transform current = nextCubes.First();
        nextCubes.Remove(current);
        if (current == clickedCube)
        {
            return;
        }
        foreach (GamePath path in current.GetComponent<Walkable>().possiblePath)
        {
            if (!visitedCubes.Contains(path.target) && path.active)
            {
                nextCubes.Add(path.target);
                path.target.GetComponent<Walkable>().previousBlock = current;
            }
        }
        visitedCubes.Add(current);
        if (nextCubes.Any())
        {
            ExploreCube(nextCubes, visitedCubes);
        }
    }
    private void BFSFindPath()
    {
        bool findTarget = false;
        HashSet<Transform> cubeCovered = new HashSet<Transform>();
        Queue < Transform > queue = new Queue<Transform>();
        queue.Enqueue(currentCube);
        while (queue.Count > 0)
        {
            Transform element = queue.Dequeue();
            if (cubeCovered.Contains(element))
                continue;
            else
                cubeCovered.Add(element);
            foreach (GamePath path in element.GetComponent<Walkable>().possiblePath)
            {
                if (!cubeCovered.Contains(path.target) && path.active)
                {
                    queue.Enqueue(path.target);
                    path.target.GetComponent<Walkable>().previousBlock = element;
                }
                if (path.target == clickedCube)
                    findTarget = true;
            }
            if (findTarget)
                break;
            //Debug.Log("in");
        }
    }
    //method to generate path from current cube to clicked cube, storing it to finalPath
    private void BuildPath()
    {
        BFSFindPath();
        Transform cube = clickedCube;
        while (cube != currentCube)
        {
            finalPath.Add(cube);
            Debug.Log(cube.transform.position);
            if (cube.GetComponent<Walkable>().previousBlock != null)
                cube = cube.GetComponent<Walkable>().previousBlock;
            else
            {
                return;
            }
        }
        FollowPath();
    }
    //remove all path in finalPath
    private void Clear()
    {
        Walkable[] components = GameObject.FindObjectsOfType<Walkable>();
        foreach (Walkable w in components)
        {
            w.previousBlock = null;
        }
        finalPath.Clear();
        anim.SetBool("isWalking", false);
    }
    public void KillMovement()
    {
        //kill player movement
        s.Kill();
        anim.SetBool("isWalking", false);
    }
    public bool onMove()
    {
        //check if the player is currently moving
        if (s.IsActive() && !s.IsComplete())
        {
            return true;
        }
        return false;
    }
    //method to generate player movement, use api called dotween ref: http://dotween.demigiant.com/documentation.php#creatingTweener
    private void FollowPath()
    {
        bool skipNext = false;
        //offset to move player up a little bit in y direction
        Vector3 offset = new Vector3(0, 0.05f, 0);
        s = DOTween.Sequence();

        for (int i = finalPath.Count - 1; i >= 0; i--)
        {
            if (skipNext)
            {
                skipNext = false;
                continue;
            }
            Vector3 temp = finalPath[i].GetComponent<Walkable>().GetWalkPoint();
            if (!finalPath[i].GetComponent<Walkable>().canWalkOnThisBlock)
            {
                Debug.Log("|| " + finalPath[i].name);
                Debug.Log("SHOULD BREAK");
                break;
            }
            //check the correct direction where the player is supposed to face
            Vector3 relativeDirection;
            if (i < finalPath.Count - 1)
            {
                //relativeDirection = ((finalPath[i].GetComponent<Walkable>().GetWalkPoint() + offset) - (finalPath[i + 1].GetComponent<Walkable>().GetWalkPoint() + offset)).normalized;
                Vector3 cubeNext = cam.WorldToScreenPoint(finalPath[i].GetComponent<Walkable>().GetWalkPoint() + offset);
                Vector3 cubeThis = cam.WorldToScreenPoint(finalPath[i + 1].GetComponent<Walkable>().GetWalkPoint() + offset);
                Vector3 cubeN = new Vector3(cubeNext.x, 0, cubeNext.y);
                Vector3 cubeT = new Vector3(cubeThis.x, 0, cubeThis.y);
                relativeDirection = (cubeN - cubeT).normalized;
            }
            else
            {
                Vector3 cubeNext = cam.WorldToScreenPoint(finalPath[i].GetComponent<Walkable>().GetWalkPoint() + offset);
                Vector3 cubeThis = cam.WorldToScreenPoint(transform.position);
                Vector3 cubeN = new Vector3(cubeNext.x, 0, cubeNext.y);
                Vector3 cubeT = new Vector3(cubeThis.x, 0, cubeThis.y);
                relativeDirection = (cubeN - cubeT).normalized;
                //relativeDirection = ((finalPath[i].GetComponent<Walkable>().GetWalkPoint() + offset) - transform.position).normalized;
            }
            //Debug.Log("Relative direction is" + relativeDirection);
            Vector3 newForward = relativeDirection.magnitude < 0.1f ? transform.forward : CalculatePlayerDirection(relativeDirection);
            //Debug.Log("calculated direction is" + newForward);
            //if the player is walking on a stair, his direction should not change
            Tween t;
            if ((finalPath[i].GetComponent<Walkable>().isStair) || (i < finalPath.Count - 1 && finalPath[i + 1].GetComponent<Walkable>().isStair))
            {
                t = transform.DOMove(finalPath[i].GetComponent<Walkable>().GetWalkPoint() + offset, timePerUnitMove).SetEase(Ease.Linear);
            }
            else
            {
                t = transform.DOMove(finalPath[i].GetComponent<Walkable>().GetWalkPoint() + offset, timePerUnitMove).SetEase(Ease.Linear).OnStart(() => transform.forward = newForward);
            }
            s.Append(t);
            //this check if there is a gap between two cubes in scene view(not game view) , if so, player need to 
            // first move to the the edge of current cube
            // second transform its position to the edge position of the cube you are moving to
            //lastly move to the walkpoint of the you are moving to
            // the whole process take 1 unit time
            if (finalPath[i].GetComponent<Walkable>().edgeValue != Vector3.zero && i > 0 && finalPath[i - 1].GetComponent<Walkable>().edgeValue != Vector3.zero)
            {
                Vector3 newPos = finalPath[i - 1].GetComponent<Walkable>().edgeValue + offset;
                Tween tween = transform.DOMove(finalPath[i].GetComponent<Walkable>().edgeValue + offset, timePerUnitMove / 2)
                    .SetEase(Ease.Linear).OnComplete(() => transform.position = newPos);
                s.Append(tween);
                s.Append(transform.DOMove(finalPath[i - 1].GetComponent<Walkable>().GetWalkPoint() + offset, timePerUnitMove / 2).SetEase(Ease.Linear));
                skipNext = true;
            }
        }
        s.AppendCallback(() => Clear());
    }


    //Eventhandler for collecting collectibles.
    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter");
        if (other.gameObject.CompareTag("crystal"))
        {
            Debug.Log("Collision");
            other.gameObject.SetActive(false);

            Debug.Log("rotation gear now visible");
            // rotationGear.gameObject.SetActive(true);
            GameObject.Find ("RotationGear").transform.localScale = new Vector3(1, 1, 1);
        }
        if (other.gameObject.CompareTag("star"))
        {
            Debug.Log("Collision");
            other.gameObject.SetActive(false);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
    public Vector3 CalculatePlayerDirection(Vector3 input)
    {
        Vector3[] directions = { new Vector3(1, 0, 0), new Vector3(-1, 0, 0), new Vector3(0, 0, 1), new Vector3(0, 0, -1) };
        //determine where the player is facing by determine the quadrants the direction is

        if (input.x < 0 && input.z > 0)
        {
            return directions[2];
        }
        else if (input.x < 0 && input.z < 0)
        {
            return directions[1];
        }
        else if (input.x > 0 && input.z < 0)
        {
            return directions[3];
        }
        else if (input.x > 0 && input.z > 0)
        {
            return directions[0];
        }
        else
        {
            return input;
        }
    }
}
