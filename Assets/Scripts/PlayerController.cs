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
    private bool onEnter = true;
    // public GameObject rotationGear;
    private int levelPassed, sceneIndex;
    
    // whether the player is allowed to move or not
    public bool CanMove { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        Screen.orientation = ScreenOrientation.PortraitUpsideDown;
        RayCastDown();
        timePerUnitMove = 1f / walkingSpeed;
        clickSecondsCount = clickTimeInterval;
        CanMove = true;
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
            if (clickSecondsCount < clickTimeInterval || !CanMove)
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
                    GetComponent<AudioSource>().Play(); //SFX
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
                if (onEnter)
                {
                    transform.position = playerHit.transform.GetComponent<Walkable>().GetWalkPoint();
                    onEnter = false;
                }
            }
        }
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
    
    private void BfsFindPath()
    {
        bool findTarget = false;
        HashSet<Transform> cubeCovered = new HashSet<Transform>();
        Queue<Transform> queue = new Queue<Transform>();
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
        }
    }
    
    //method to generate path from current cube to clicked cube, storing it to finalPath
    private void BuildPath()
    {
        BfsFindPath();
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
        s.Kill();
        finalPath.Clear();
        anim.SetBool("isWalking", false);
        GetComponent<AudioSource>().Stop(); //SFX 
    }
    
    public void KillMovement()
    {
        //kill player movement
        s.Kill();
        anim.SetBool("isWalking", false);
        GetComponent<AudioSource>().Stop(); //SFX
    }
    
    public bool OnMove() => s.IsActive() && !s.IsComplete();

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
            }
            Vector3 newForward = relativeDirection.magnitude < 0.1f ? transform.forward : CalculatePlayerDirection(relativeDirection);
            // if the player is walking on a stair, his direction should not change
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
            // this check if there is a gap between two cubes in scene view(not game view) , if so, player need to 
            // first move to the the edge of current cube
            // second transform its position to the edge position of the cube you are moving to
            // lastly move to the walkpoint of the you are moving to
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
            Debug.Log("Crystal Collision");

            //SFX
            //AudioSource audio = other.gameObject.GetComponent<AudioSource>();
            //audio.Play();
            GameObject.Find("collectable_sfx").GetComponent<AudioSource>().Play();  //SFX

            //Destroy Collectible after audio is played
            Destroy(other.gameObject, 0.3f);

            Debug.Log("rotation gear now visible");

            if (GameObject.Find("RotationGear"))
            {
                GameObject.Find("RotationGear").transform.localScale = new Vector3(1, 1, 1);
            }

            if (GameObject.Find("RotationGear_Key"))
            {
                GameObject.Find("RotationGear_Key").transform.localScale = new Vector3(1, 1, 1);
                GameObject.Find("Rotate_Key").GetComponent<RotationManagerLevelThreeKey>().ActivateAnimation();

            }
        }
        if (other.gameObject.CompareTag("star"))
        {
            Debug.Log("Destination Colectable Collision");
            // other.gameObject.SetActive(false);

            //SFX++
            AudioSource audio = other.gameObject.GetComponent<AudioSource>();
            audio.Play();

            StartCoroutine(LoadNewScene(other));
        }

        if (other.gameObject.CompareTag("Key_collectable"))
        {
            Debug.Log("Key Collision");
            //other.gameObject.SetActive(false);
            //AudioSource audio = other.gameObject.GetComponent<AudioSource>();
            //audio.Play();
            GameObject.Find("key_sfx").GetComponent<AudioSource>().Play();  //SFX

            Destroy(other.gameObject, 0.4f);

            Debug.Log("rotation gear now visible");
            if (GameObject.Find("RotationGear_Destination"))
                GameObject.Find("RotationGear_Destination").transform.localScale = new Vector3(1, 1, 1);
            GameObject.Find("Rotate_Destination").GetComponent<RotationManagerLevelThreeDest>().ActivateAnimation();
        }
    }

    //SFX++
    IEnumerator LoadNewScene(Collider other)
    {
        // initialize the current level
        var currentLevel = transform.parent;
        if (currentLevel != null) currentLevel.GetComponent<LevelManager>().ResetLevel(); 
        
        yield return new WaitForSeconds(0.4f);
        Destroy(other.gameObject);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    
    //SFX--
    public Vector3 CalculatePlayerDirection(Vector3 input)
    {
        Vector3[] directions = { new Vector3(1, 0, 0), new Vector3(-1, 0, 0), new Vector3(0, 0, 1), new Vector3(0, 0, -1) };
        //determine where the player is facing by determine the quadrants the direction is

        if (input.x < 0 && input.z > 0)
            return directions[2];
        else if (input.x < 0 && input.z < 0)
            return directions[1];
        else if (input.x > 0 && input.z < 0)
            return directions[3];
        else if (input.x > 0 && input.z > 0)
            return directions[0];
        else
            return input;
    }
}