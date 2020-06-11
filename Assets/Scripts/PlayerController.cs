using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Transform currentCube;
    public Transform clickedCube;
    public List<Transform> finalPath;
    public float walkingSpeed;
    private bool walking = false;
    // Start is called before the first frame update
    void Start()
    {
        RayCastDown();
    }

    // Update is called once per frame
    void Update()
    {
        RayCastDown();
        //camera raycast
        if (Input.GetMouseButtonDown(0))
        {
            Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit mouseHit;
            if(Physics.Raycast(mouseRay, out mouseHit))
            {
                if(mouseHit.transform.GetComponent<Walkable>()!= null)
                {
                    Clear();
                    clickedCube = mouseHit.transform;
                    BuildPath();
                }
            }
        }
        MovePlayer();
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
    private void FindPath()
    {
        List<Transform> nextCubes = new List<Transform>();
        List<Transform> pastCubes = new List<Transform>();
        foreach(GamePath path in currentCube.GetComponent<Walkable>().possiblePath)
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
    private void ExploreCube(List<Transform> nextCubes, List<Transform> visitedCubes)
    {
        Transform current = nextCubes.First();
        nextCubes.Remove(current);
        if(current == clickedCube)
        {
            return;
        }
        foreach (GamePath path in current.GetComponent<Walkable>().possiblePath)
        {
            if(!visitedCubes.Contains(path.target) && path.active)
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
    private void BuildPath()
    {
        FindPath();
        Transform cube = clickedCube;
        while(cube != currentCube)
        {
            finalPath.Add(cube);
            if (cube.GetComponent<Walkable>().previousBlock != null)
                cube = cube.GetComponent<Walkable>().previousBlock;
            else
                return;
        }
    }
    private void Clear()
    {
        foreach(Transform t in finalPath)
        {
            t.GetComponent<Walkable>().previousBlock = null;
        }
        finalPath.Clear();
    }
    private void MovePlayer()
    {
        if(targetBlock = cu)
    }
}
