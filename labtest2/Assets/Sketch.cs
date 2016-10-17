using UnityEngine;
using Pathfinding.Serialization.JsonFx; //make sure you include this using
using System;

public class Sketch : MonoBehaviour {
    public GameObject myPrefab;
    public GameObject the_cube;
    //public WaterPollutionReading[] water;
    public GameObject to_hide;
    public string jsonResponse;
    //public TextMesh textModel;
    // Put your URL here
    public string _WebsiteURL = "http://320lab.azurewebsites.net/tables/TreeSurvey?zumo-api-version=2.0.0"; //http://{Your Site Name}.azurewebsites.net/tables/{Your Table Name}?zumo-api-version=2.0.0


    void Start() {
        //Reguest.GET can be called passing in your ODATA url as a string in the form:
        //The response produce is a JSON string
        jsonResponse = Request.GET(_WebsiteURL);

        //Just in case something went wrong with the request we check the reponse and exit if there is no response.
        if (string.IsNullOrEmpty(jsonResponse))
        {
            return;
        }

        //We can now deserialize into an array of objects - in this case the class we created. The deserializer is smart enough to instantiate all the classes and populate the variables based on column name.
        TreeSurvey[] tree = JsonReader.Deserialize<TreeSurvey[]>(jsonResponse); //CHANGE

        int i = 0;
        int totalLocate = tree.Length;

        for (i = 0; i < totalLocate; i++)
        {
            //GameObject[] newCube = new;

            int x = tree[i].PositionX + i; //CHANGE
            int y = tree[i].PositionY; //CHANGE
            int z = tree[i].PositionZ + i; //CHANGE
            string the_text = tree[i].TreeID; //CHANGE
            string a_text = "Location:" + tree[i].Location + "\n" + "EcologicalValue: " + tree[i].EcologicalValue; //CHANGE

            GameObject newLocation = (GameObject)Instantiate(myPrefab, new Vector3(x, y, z), Quaternion.identity);
            newLocation.GetComponentInChildren<TextMesh>().text = tree[i].TreeID + tree[i].Location;
            newLocation.GetComponent<LocationScript>().Category = the_text;
            //GameObject newBoard = (GameObject)Instantiate(the_cube, new Vector3(x, y + 1, z), Quaternion.identity);
            //newBoard.GetComponentInChildren<TextMesh>().text = a_text;
            //newBoard.GetComponentInChildren<TextMesh>().color = Color.white;

            //newBoard.SetActive(false);
            //newLocation.GetComponentInChildren<TextMesh>().characterSize = 0;

            if (tree[i].EcologicalValue == "High") //CHANGE
            {
                newLocation.GetComponent<Renderer>().material.color = Color.green;
            }

            else if(tree[i].EcologicalValue == "Medium") //CHANGE
            {
                newLocation.GetComponent<Renderer>().material.color = Color.yellow;
            }
            else
            {
                newLocation.GetComponent<Renderer>().material.color = Color.red;
            }
            //textModel.gameObject.SetActive(true);
            //to_hide = GameObject.FindWithTag("board");
            //to_hide.gameObject.SetActive(false);

        }

        //textModel = GameObject.Find("3dtext").GetComponentInChildren<TextMesh>();
        //textModel.gameObject.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hitInfo = new RaycastHit();
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(ray, out hitInfo))
            {
                if (hitInfo.collider.tag == "location") 
                {
                    hitInfo.collider.gameObject.GetComponent<Renderer>().material.color = Color.grey;
                    int GIndex = Int32.Parse(hitInfo.collider.gameObject.GetComponent<LocationScript>().Category);
                    //to_hide.gameObject.SetActive(true);
                    //newBoard.SetActive(true);
                    //CHANGE
                    TreeSurvey[] tree = JsonReader.Deserialize<TreeSurvey[]>(jsonResponse);
                    GameObject newBoard = (GameObject)Instantiate(the_cube, new Vector3(hitInfo.point.x, hitInfo.point.y + 1.2f, hitInfo.point.z), Quaternion.identity);
                    newBoard.GetComponentInChildren<TextMesh>().text = "Location: " + tree[GIndex - 1].Location + "\n" + "EcologicalValue: " + tree[GIndex - 1].EcologicalValue + "\n" + "HistoricalSignificance: " + tree[GIndex - 1].HistoricalSignificance;
                    //CHANGE TEXT INFO

                }
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            if (Physics.Raycast(ray, out hitInfo))
            {
                if (hitInfo.collider.tag == "board")
                {
                    Destroy(hitInfo.collider.gameObject);
                }
            }
        }

    }
}
