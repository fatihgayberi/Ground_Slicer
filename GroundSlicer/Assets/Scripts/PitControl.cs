using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PitControl : MonoBehaviour
{
    // Start is called before the first frame update
    TrailRenderer trail;
    List<GameObject> cubes;
    
    [SerializeField]
    GameObject PitTrigger;

    bool cycle = false;

    void Start()
    {
        trail = GetComponent<TrailRenderer>();
        cubes = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    List<List<GameObject>> SortingBoundariesCubes()
    {
        List < List < GameObject >> BoundariesCubes = new List<List<GameObject>>();

      
        List<GameObject> firstGameObjects = new List<GameObject>();
        firstGameObjects.Add(cubes[0]);
        BoundariesCubes.Add(firstGameObjects);
        

        int j;
        bool isAdded;
        int nearestIndex;
        float minDistance;

        for (int i = 1; i < cubes.Count; ++i)
        {
            isAdded = false;
            nearestIndex = 100;//büyük bir değer ile ilklendirildi
            minDistance = 100;//büyük bir değer ile ilklendirildi

            for(j = 0; j < BoundariesCubes.Count; ++j)
            {
                if(cubes[i].transform.position.z == BoundariesCubes[j][0].transform.position.z)
                {
                    for (int k = 0; k < BoundariesCubes[j].Count ;++k)
                    {
                        if(cubes[i].transform.position.x <BoundariesCubes[j][k].transform.position.x)
                        {
                            BoundariesCubes[j].Insert(k, cubes[i]);
                            isAdded = true;
                            break;
                        }

                    }
                    if (!isAdded)
                    {
                        BoundariesCubes[j].Add(cubes[i]);
                        isAdded = true;
                    }
                    break;
                }

                //eğer bu kübün yüksekliğinde satır yoksa en yakınının indisini bul
                else if(minDistance > Mathf.Abs (cubes[i].transform.position.z - BoundariesCubes[j][0].transform.position.z))
                { 
                    minDistance = Mathf.Abs(cubes[i].transform.position.z - BoundariesCubes[j][0].transform.position.z);
                    nearestIndex = j;
                } 
        
            }

            if (!isAdded) { // elimizdeki satırlarla aynı yükseklikte değilse yeni satır olarak ekle
                                
                if (cubes[i].transform.position.z < BoundariesCubes[nearestIndex][0].transform.position.z)//en yakının üstüne ekle
                {
                    List<GameObject> gameObjects = new List<GameObject>();
                    gameObjects.Add(cubes[i]);
                    BoundariesCubes.Insert(nearestIndex, gameObjects);
                }
                else // en yakının altına ekle
                {
                    List<GameObject> gameObjects = new List<GameObject>();
                    gameObjects.Add(cubes[i]);
                    if( (nearestIndex+1) >= (cubes.Count-1)) // en son satırsa sonuna ekle
                        BoundariesCubes.Add(gameObjects);

                    else // en son satır değilse araya ekle 
                        BoundariesCubes.Insert(nearestIndex + 1, gameObjects);
                }
            }
          
        }

        return BoundariesCubes;
    }

    void AddRigidBody()
    {
        List<List<GameObject>> BoundariesCubes = SortingBoundariesCubes();

        for (int i = 0; i < BoundariesCubes.Count; ++i)
        {
            int size = BoundariesCubes[i].Count;

            Vector3 pos = (BoundariesCubes[i][0].transform.position + BoundariesCubes[i][size - 1].transform.position) / 2;

            GameObject newPitTrigger = Instantiate(PitTrigger, pos, Quaternion.identity);
            float x = Mathf.Abs(BoundariesCubes[i][0].transform.position.x - BoundariesCubes[i][size - 1].transform.position.x);

            newPitTrigger.transform.localScale = new Vector3((x-2) * 0.9f, 0.9f, 0.9f);
           
        }

        while (cubes.Count>0)
        {
            if (cubes[0].GetComponent<Rigidbody>() == null)
            {
                cubes[0].transform.localScale *= 0.95f;
                cubes[0].AddComponent<Rigidbody>();
            }

            Destroy(cubes[0], 2f);
            cubes.RemoveAt(0);
        }
    }
   
    private void OnTriggerEnter(Collider other)
    {
        GameObject otherObject = other.transform.gameObject;

        if (!otherObject.CompareTag("Deformable"))
            return;

        if (cubes.Count == 0)
        {
            cubes.Add(otherObject);
            StartCoroutine(RemoveFromList(otherObject, trail.time));
        }
        else if(cubes[cubes.Count -1] != otherObject)
        {
            bool isOnList = false;

            for(int i=0; i<cubes.Count; ++i)
            {
                if(cubes[i] == otherObject)
                {
                    isOnList = true;

                    if (!LineCheck() && !SideBySideCheck() && !IsClose(i))
                    {
                        for (int j = 0; j < i; ++j)
                            cubes.RemoveAt(0);

                        AddRigidBody();
                    }
                   
                    break;
                }

            }

            if (!isOnList)
            {
                cubes.Add(otherObject);
                StartCoroutine(RemoveFromList(otherObject, trail.time));

            }
        }
    }

    IEnumerator RemoveFromList(GameObject gameObject, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        cubes.Remove(gameObject);
    }



    float Slope(float X0, float Z0, float X1, float Z1)
    {
        float m;

        if (X0 == X1)
            m = 0;

        else if (Z0 == Z1)
            m = 1;

        else
        { 
            m = (Z1 - Z0) / (X1 - X0);
        }

        return m;
    }

    bool LineCheck()
    {
        if (cubes[0] != null)
        {
            float X0 = cubes[0].transform.position.x;
            float Z0 = cubes[0].transform.position.z;
    
            float X1 = cubes[cubes.Count-1].transform.position.x;
            float Z1 = cubes[cubes.Count-1].transform.position.z;
            
            float m = Slope(X0,Z0,X1,Z1);
    
            for(int i = 1;i<cubes.Count-1; ++i)
            {
                if (cubes[i] != null)
                {
                    X1 = cubes[i].transform.position.x;
                    Z1 = cubes[i].transform.position.z;

                    if (m != Slope(X0, Z0, X1, Z1))
                    {
                        return false;
                    }
                }
            }
        }
            return true;
    }


    bool IsSideBySide(float X0, float Z0, float X1, float Z1)
    {
        if(Mathf.Abs(X1-X0)<3 || Mathf.Abs(Z1-Z0)<3)
            return true;

        return false;
    }

    bool SideBySideCheck()
    {
        float X0 = cubes[0].transform.position.x;
        float Z0 = cubes[0].transform.position.z;

        float X1 = cubes[cubes.Count - 1].transform.position.x;
        float Z1 = cubes[cubes.Count - 1].transform.position.z;


        for (int i = 1; i < cubes.Count - 1; ++i)
        {
            X1 = cubes[i].transform.position.x;
            Z1 = cubes[i].transform.position.z;

            if (!IsSideBySide(X0, Z0, X1, Z1))
            {
                return false;
            }

        }
        return true;
    }

    bool IsClose(int i)
    {
        if (cubes.Count > 8 && (cubes.Count - 1 - i) > 8)
            return false;
        return true;
    }

}
