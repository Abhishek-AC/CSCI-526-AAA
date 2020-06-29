using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public Transform currentCube;
    public Transform clickedCube;
    public List<Transform> finalPath;
    public float walkingSpeed;
    private float timePerUnitMove;
    private Sequence s;


    // Start is called before the first frame update
    void Start()
    {
        Screen.orientation = ScreenOrientation.PortraitUpsideDown;
        RayCastDown();
        timePerUnitMove = 1f / walkingSpeed;
    }
    // Update is called once per frame
    void Update()
    {
        //find player current block
        RayCastDown();
        Debug.Log(currentCube.name);
        //camera raycast to find the clicked block ref:https://docs.unity3d.com/ScriptReference/Physics.Raycast.html
        if (Input.GetMouseButtonDown(0))
        {
            Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit mouseHit;
            if (Physics.Raycast(mouseRay, out mouseHit))
            {
                if (mouseHit.transform.GetComponent<Walkable>() != null)
                {
                    Clear();
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
    //method to generate path from current cube to clicked cube, storing it to finalPath
    private void BuildPath()
    {
        FindPath();
        Transform cube = clickedCube;
        while (cube != currentCube)
        {
            finalPath.Add(cube);
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
    }
    public void KillMovement()
    {
        //kill player movement
        s.Kill();
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
        Vector3 offset = new Vector3(0, 0.0f, 0);
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
            s.Append(transform.DOMove(finalPath[i].GetComponent<Walkable>().GetWalkPoint() + offset, timePerUnitMove).SetEase(Ease.Linear));
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
        }
        if (other.gameObject.CompareTag("star"))
        {
            Debug.Log("Collision");
            other.gameObject.SetActive(false);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +1);
        }
    }
}
