using System;
using System.Collections.Generic;
using Test_Simulation.Locations;
using UnityEngine;
using UnityEngine.Serialization;
using Tree = Test_Simulation.Locations.Tree;

namespace Test_Simulation.Managers
{
    public class LocationManager : MonoBehaviour
    {
        public static LocationManager Instance { get; private set; }
        
        public HuntingPoint[] hunterLocations;
        public StoneOre stoneOre;
        public IronOre[] ironMineLocations;
        public Fruit[] gathererLocations;
        public List<Tree> treeLocations;
        public Kitchen cookLocations;
        public Storage foodStorage;
        public Storage stoneStorage;
        public Storage ironStorage;
        public Storage woodStorage;
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
 
        public Transform GetHunterLocation(Transform currentPosition)
        {
            return GetClosestLocation(hunterLocations, currentPosition.position);
        }
        
        public Transform GetGathererLocation(Transform currentPosition)
        {
            return GetClosestLocation(gathererLocations, currentPosition.position);
        }
        
        public Tree GetTreeLocation(Transform currentPosition)
        {
            return GetClosestTree(treeLocations.ToArray(), currentPosition.position);
        }
        
        
        public Transform GetClosestLocation(Location[] locations, Vector3 currentPosition)
        {
            Transform closestLocation = null;
            float closestDistance = Mathf.Infinity;
            foreach (var location in locations)
            {
                float distance = Vector3.Distance(location.transform.position, currentPosition);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestLocation = location.transform;
                }
            }

            return closestLocation;
        }
        
        public Transform GetStorageLocation(WorkLocation workLocation)
        {
            switch (workLocation)
            {
                case WorkLocation.Kitchen:
                    return foodStorage.transform;
                    break;
                case WorkLocation.HuntingPoint:
                    return foodStorage.transform;
                    break;
                case WorkLocation.IronMine:
                    return ironStorage.transform;
                    break;
                case WorkLocation.StoneMine:
                    return stoneStorage.transform;
                    break;
                case WorkLocation.Tree:
                    return woodStorage.transform;
                    break;
                case WorkLocation.GatheringPoint:
                    return foodStorage.transform;
                    break;
                case WorkLocation.None:
                    return null;
                    break;
            }

            return null;
        }

        public Tree GetClosestTree(Tree[] locations, Vector3 currentPosition)
        {
            Tree closestLocation = null;
            float closestDistance = Mathf.Infinity;
            foreach (var location in locations)
            {
                float distance = Vector3.Distance(location.transform.position, currentPosition);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestLocation = location;
                }
            }

            return closestLocation;
        }
        
        public void RemoveTree(Tree tree)
        {
            treeLocations.Remove(tree);
        }
        
    }


}