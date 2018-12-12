using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//A tracer minigame; when a spell button is pressed, waypoints will be displayed in some orientation
//the player will trace the waypoints as accurately and quickly as possible, and the number traced
//will determine the accuracy of the spell

public class TracerGM : MonoBehaviour {

    public GameObject player; //reference to the player who's playing
    public Spell spell; //reference to the spell being traced
    public GameMaster gameMaster; //

    //Some variables taken from https://www.youtube.com/watch?v=jx5U1ZaZ080
    public GameObject cursorAsset; //original asset for the cursor
    public GameObject waypointAsset; //original asset for the waypoint
    public GameObject traceAsset; //original asset for the trace (cursor paint)

    GameObject cursor; //reference to the current cursor object
    GameObject previousCursor; //reference to the previous cursor object

    static int numWaypoints = 15; //number of waypoints 
    GameObject[] waypoints = new GameObject[numWaypoints]; //array holding the waypoints

    float[] x = new float[numWaypoints]; //contains the world x positions for the waypoints
    float[] y = new float[numWaypoints]; //contains the world y positions for the waypoints

    ArrayList traceList = new ArrayList(); //contains the traced "breadcrumbs"

    int[] color_progress = new int[numWaypoints]; //keeps track of the intermediate lerped colors of the waypoints (used for gradienting color change)

    Color[] starting_colors = new Color[numWaypoints]; //holds the initial colors of the waypoints

    bool isGameOver = false; //whether the game is over (either time runs out or all waypoints have been traced)

    int currentWaypointIndex = 0; //the current traceable waypoint as an index in the waypoint list

    float timeLimit = 8.0f; //time in seconds to trace the waypoints
    float startTime; //time when the tracer is initialized
    float endTime; //time when the game ends (when time limit is confirmed to have been exceeded, or all waypoints have been confirmed to have been traced)

    float timeElapsed;

    bool timedOut = false; //indicates whether the game ended by time out

    float accuracy; //holds the proportion that the player has hit so far

	// Use this for initialization
	void Start () {
        //Referred to https://docs.microsoft.com/en-us/dotnet/api/system.object.gettype?view=netframework-4.7.2
        //and https://stackoverflow.com/questions/38043471/how-to-check-if-class-is-an-instance-of-a-type-given-as-a-variable-in-c
        //and https://www.google.com/search?ei=rlYNXJW3B7K9ggfZmJWADw&q=unity+check+object+type&oq=unity+check+object+type&gs_l=psy-ab.3..0.7845.9412..9533...0.0..0.218.613.5j0j1......0....1..gws-wiz.......0i71j0i22i30.OIWb1Osp8Fo
        string spellName = spell.GetType().ToString(); //get the spell name of the button pressed to trigger the tracer

        for (int j = 0; j < numWaypoints; j++) //initialize all color_progress to 0 (indicating no waypoints have begun changing color)
        {
            color_progress[j] = 0;
        }

        startTime = Time.time; //save the start time

    //use various parametric functions to approximate the spell runes
    //these functions were generated with the help of WolframAlpha and 
    //http://jwilson.coe.uga.edu/EMAT6680Fa2013/Thurston/Write-Ups/Write-up%2011/Polar_Petals.html

        if (spellName == "CurseSpell")
        {
            //Curse (skull)
            for (int i = 0; i < numWaypoints - 4; i++)
            {
                x[i + 2] = 1.3f * Mathf.Sin((1.5f * Mathf.PI * (i - 5)) /(numWaypoints - 2)) + 10;
                y[i + 2] = 1.3f * Mathf.Cos((1.5f * Mathf.PI * (i - 5)) / (numWaypoints - 2)) + 3.7f;
            }

            x[numWaypoints - 2] = 3 * Mathf.Sin((2 * Mathf.PI * ((numWaypoints - 5) - 5)) / (numWaypoints - 2)) + 9;
            y[numWaypoints - 2] = 3 * Mathf.Cos((2 * Mathf.PI * ((numWaypoints - 5) - 5)) / (numWaypoints - 2)) + 4 + 1.2f;

            x[numWaypoints - 1] = 3 * Mathf.Sin((2 * Mathf.PI * ((numWaypoints - 5) - 5)) / (numWaypoints - 2)) + 9;
            y[numWaypoints - 1] = 3 * Mathf.Cos((2 * Mathf.PI * ((numWaypoints - 5) - 5)) / (numWaypoints - 2)) + 4 + 0.6f;

            x[1] = 3 * Mathf.Sin((2 * Mathf.PI * ((5 - numWaypoints) + 5)) / (numWaypoints - 2)) + 11;
            y[1] = 3 * Mathf.Cos((2 * Mathf.PI * ((5 - numWaypoints) + 5)) / (numWaypoints - 2)) + 4 + 1.2f;

            x[0] = 3 * Mathf.Sin((2 * Mathf.PI * ((5 - numWaypoints) + 5)) / (numWaypoints - 2)) + 11;
            y[0] = 3 * Mathf.Cos((2 * Mathf.PI * ((5 - numWaypoints) + 5)) / (numWaypoints - 2)) + 4 + 0.6f;


        } else if (spellName == "WallSpell")
        {
            //Wall
            for (int i = 0; i < (numWaypoints / 3); i++)
            {
                x[i] = -2 + 10;
                y[i] = 2 + 0.5f * i;
            }

            for (int i = (numWaypoints / 3); i < ((2 * numWaypoints) / 3); i++)
            {
                x[i] = (4.0f / 6.0f) + ((4.0f/6.0f) * (i - (numWaypoints / 3))) + 8;
                y[i] = 4;
            }

            for (int i = ((2 * numWaypoints) / 3); i < numWaypoints; i++)
            {
                x[i] = 2 + 10;
                y[i] = 4 - (0.5f * (i - ((2 * numWaypoints) / 3)));
            }
        } else if (spellName == "HexSpell")
        {
            //Hex (chicken)
            for (int i = 0; i < numWaypoints - 4; i++)
            {
                x[i] = 1.3f * Mathf.Sin((2 * Mathf.PI * (i - 6)) / (numWaypoints)) + 9.2f;
                y[i] = 1.3f * Mathf.Cos((2 * Mathf.PI * (i - 6)) / (numWaypoints)) + 1 + 2.5f;
            }

            x[numWaypoints - 4] = 1.3f * Mathf.Sin((2 * Mathf.PI * ((numWaypoints - 5) - 6)) / (numWaypoints - 2)) + 0.5f + 9.2f;
            y[numWaypoints - 4] = 1.3f * Mathf.Cos((2 * Mathf.PI * ((numWaypoints - 5) - 6)) / (numWaypoints - 2)) + 1 + .1f + 2.2f;

            x[numWaypoints - 3] = 1.3f * Mathf.Sin((2 * Mathf.PI * ((numWaypoints - 5) - 6)) / (numWaypoints - 2)) + 1.2f + 9.2f;
            y[numWaypoints - 3] = 1.3f * Mathf.Cos((2 * Mathf.PI * ((numWaypoints - 5) - 6)) / (numWaypoints - 2)) + 1 - .3f + 2.2f;

            x[numWaypoints - 2] = 1.3f * Mathf.Sin((2 * Mathf.PI * ((numWaypoints - 5) - 6)) / (numWaypoints - 2)) + 0.8f + 9.2f;
            y[numWaypoints - 2] = 1.3f * Mathf.Cos((2 * Mathf.PI * ((numWaypoints - 5) - 6)) / (numWaypoints - 2)) + 1 - .7f + 2.2f;

            x[numWaypoints - 1] = 1.3f * Mathf.Sin((2 * Mathf.PI * ((numWaypoints - 5) - 6)) / (numWaypoints - 2)) + 0.4f + 9.2f;
            y[numWaypoints - 1] = 1.3f * Mathf.Cos((2 * Mathf.PI * ((numWaypoints - 5) - 6)) / (numWaypoints - 2)) + 1 - 1.1f + 2.2f;

        } else if (spellName == "ProjectileSpell")
        {
            //Fireball/projectile
            for (int i = 0; i < ((numWaypoints - 1) / 2 + 1); i++)
            {
                float theta = i * (Mathf.PI / 3) / ((numWaypoints - 1) / 2 + 2);
                x[i] = 2.5f * Mathf.Cos(theta) * (1 + Mathf.Cos(3 * theta)) + 7.5f;
                y[i] = 2.5f * Mathf.Sin(theta) * (1 + Mathf.Cos(3 * theta)) + 3;
            }

            x[(numWaypoints - 1) / 2 + 1] = 7.5f;
            y[(numWaypoints - 1) / 2 + 1] = 3;

            for (int i = ((numWaypoints - 1) / 2 + 2); i < numWaypoints; i++)
            {
                float theta = (i - ((numWaypoints - 1) / 2 + 2) + 2) * (Mathf.PI / 3) / ((numWaypoints - 1) / 2 + 2);
                x[numWaypoints + ((numWaypoints - 1) / 2 + 2) - i - 1] = 2.5f * Mathf.Cos(theta) * (1 + Mathf.Cos(3 * theta)) + 7.5f;
                y[numWaypoints + ((numWaypoints - 1) / 2 + 2) - i - 1] = -1 * (2.5f * Mathf.Sin(theta) * (1 + Mathf.Cos(3 * theta))) + 3;
            }
        } else if (spellName == "WindSpell")
        {
            //Wind (tornado)
            for (int i = 0; i < numWaypoints; i++)
            {
                x[i] = (2 * Mathf.PI / numWaypoints) * i + 7;
                y[i] = 1.4f * Mathf.Sin((2 * Mathf.PI / numWaypoints) * i) + 3.1f;
            }
        } else
        {
            for (int i = 0; i < numWaypoints; i++)
            {
                x[i] = 7.2f + 0.4f * i;
                y[i] = 0.35f * Mathf.Pow((7.2f + 0.4f * i - 10), 2) + 1.7f;
            }
        }

        // Initialize the waypoints at the prescribed positions
        for (int i = 0; i < numWaypoints; i++)
        {
            Vector2 waypointPos = new Vector2(x[i], y[i]);
            //and save references to the waypoints
            waypoints[i] = Instantiate(waypointAsset, waypointPos, waypointAsset.transform.rotation);

			if (player.GetComponent<Player>().playerID == 1)
			{
				waypoints[i].transform.position = new Vector3(waypoints[i].transform.position.x, waypoints[i].transform.position.y, 24.8f);
			}

            //have the waypoints be in increasingly green shades (lighter green waypoints need to be traced before the darker green ones)
            float colorGradient = 1.0f / (numWaypoints);

            //Referred to:
            //https://docs.unity3d.com/ScriptReference/Color.html
            Color c = new Color(0, (1 - i * colorGradient), 0);
            waypoints[i].GetComponent<Renderer>().material.color = c;
            starting_colors[i] = c;

            //Referred to https://answers.unity.com/questions/577187/increase-the-radius-of-unitys-primitive-sphere.html
            //and https://forum.unity.com/threads/changing-a-primitives-width-and-height.21402/
            //and https://www.google.com/search?q=unity+change+primitive+size&ie=utf-8&oe=utf-8&client=firefox-b-1
            waypoints[i].transform.localScale = new Vector2(2.5f, 2.5f); //set size of the waypoints
        }

    }
	
	// Update is called once per frame
	void Update () {
        //Referred to https://docs.unity3d.com/ScriptReference/Time-time.html
        //https://gamedev.stackexchange.com/questions/110914/check-elapsed-time-in-unity
        float currentTime = Time.time; //the current time in the frame
        timeElapsed = currentTime - startTime; //the time elapsed since the start

        accuracy = Mathf.RoundToInt((float)currentWaypointIndex / (float)numWaypoints * 100); //calculate the accuracy
        
        //if the game isn't over and the elapsed time exceeds the limit
        if (!isGameOver && (timeElapsed >= timeLimit))
        {
            isGameOver = true; //mark the game as over
            endTime = Time.time; //store the game end time
            timedOut = true; //mark that the game ended through timeout
        }

        //else if the game isn't over (and elapsed time hasn't exceeded the limit)
        if (!isGameOver)
        {
            //incrementally change color of any waypoints that haven't completely turned purple
            for (int k = 0; k < numWaypoints; k++)
            {
                if (color_progress[k] > 0 && color_progress[k] < 25)
                {
                    //Referred to https://forum.unity.com/threads/making-object-gradually-change-color.119233/
                    waypoints[k].GetComponent<Renderer>().material.SetColor("_Color", Color.Lerp(starting_colors[k], Color.magenta, color_progress[k] / 25.0f));
                    color_progress[k]++;
                }

            }

            //Some basic code for the tracing mechanism was taken from here (including mouse control methods and the idea to draw a trace
            //by instantiating an asset repeatedly, as well as screen to world transformations):
            //https://www.youtube.com/watch?v=jx5U1ZaZ080
            //If left mouse button is clicked
            if (Input.GetMouseButton(0))
            {
                //Referred to this to fix a bug: https://answers.unity.com/questions/331558/screentoworldpoint-not-working.html
                //(didn't add a z coordinate to mousePos; I tried several possibilites to found one that worked decently well)

                //Get mouse position in screen space
                Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 4.9f);

                //Referred to these to fix a bug: 
                //https://stackoverflow.com/questions/52242441/camera-main-null-reference-exception
                //https://stackoverflow.com/questions/53417161/getting-object-reference-error-when-camera-main-screentoworldpoint-is-used
                //https://answers.unity.com/questions/15801/finding-cameras.html
                //https://docs.unity3d.com/ScriptReference/Camera-main.html
                //https://answers.unity.com/questions/854362/im-getting-a-nullreferenceexception-on-cameramain.html
                //https://www.google.com/search?ei=neoMXNCaHe3n_QaAuabwBg&q=unity+object+reference+not+set+to+an+instance+of+an+object+camera.main&oq=unity+object+reference+not+set+to+an+instance+of+an+object+camera.main&gs_l=psy-ab.3...3356.3956..4128...0.0..0.79.340.5......0....1..gws-wiz.......0i71j0i30.k4coUnYoMjo
                //https://forum.unity.com/threads/object-reference-not-set-to-an-instance-of-an-object-c.226430/
                //https://forum.unity.com/threads/object-reference-not-set-to-an-instance-of-an-object.484530/
                //https://www.google.com/search?ei=KecMXJ-wBIWc_Qbfm7eAAw&q=unity+object+reference+not+set+to+an+instance+of+an+object&oq=unity+object+refere&gs_l=psy-ab.1.0.0l9.764995.770173..771651...11.0..0.141.2175.26j2....2..0....1..gws-wiz.......0i71j0i67j0i131j0i10j0i22i30j33i160j0i13.4hQ2Ah3gLrI
                //Get mouse position in world space
                Vector3 mouseWorldPos = player.GetComponent<Player>().playerCamera.GetComponent<Camera>().ScreenToWorldPoint(mousePos);

                //check if the mouse position in world space is within a certain distance to the waypoint position in world space
                if (distance(mouseWorldPos, waypoints[currentWaypointIndex].transform.position) < 0.27)
                {
                    //start changing the waypoint's color
                    color_progress[currentWaypointIndex]++;

                    //move the current waypoint to the next one in the sequence
                    currentWaypointIndex++;

                    //if all waypoints have been traced
                    if (currentWaypointIndex >= numWaypoints)
                    {
                        isGameOver = true; //mark game over
                        endTime = Time.time; //store the time
                    }

                }


                //add a "breadcrumb" for each cursor location
                GameObject trace = Instantiate(traceAsset, mouseWorldPos, traceAsset.transform.rotation);

                trace.GetComponent<Renderer>().material.SetColor("_Color", Color.cyan);
                trace.transform.localScale = new Vector2(0.3f, 0.3f);

                //save those breadcrumbs in a list (to destroy later)
                traceList.Add(trace);

                //referred to code from this for instantiation/destruction of GameObjects:
                //https://www.youtube.com/watch?v=DBfgutOzstc
                //destroy the previous cursor (at the location in the previous frame)
                if (previousCursor != null)
                {
                    Destroy(previousCursor);
                }

                //initialize a new cursor
                cursor = Instantiate(cursorAsset, mouseWorldPos, cursorAsset.transform.rotation);
                cursor.GetComponent<Renderer>().material.SetColor("_Color", Color.cyan);
                cursor.transform.localScale = new Vector2(1.2f, 1.2f);

                previousCursor = cursor;

            }
        } else //game is over
        { 
            if (!timedOut) //if game is over and game has not ended through timeout, ensure all waypoints are purple
            {
                for (int k = 0; k < numWaypoints; k++)
                {
                    //Referred to //https://answers.unity.com/questions/353015/how-to-instantiate-a-prefab-and-change-its-color.html
                    //and https://www.youtube.com/watch?v=Qh64cpCxk54
                    waypoints[k].GetComponent<Renderer>().material.SetColor("_Color", Color.magenta);
                }
            }
            
            //pause for 2 seconds before clearing the screen
            if (currentTime - endTime > 2)
            {
                Destroy(cursor); //clear cursor

                for (int p = 0; p < numWaypoints; p++) //clear the waypoints
                {
                    Destroy(waypoints[p]);
                }

                for (int q = 0; q < traceList.Count; q++) //clear the trace list
                {
                    Destroy((GameObject)traceList[q]);
                }

                player.GetComponent<Player>().status.accuracy = accuracy; //modify player's accuracy

                //instantiate an aimer to begin shooting the spell
                //Code written by Matt Russiello, Albert James, and others
                Aimer aimer = Instantiate(Resources.Load("Aimer") as GameObject).GetComponent<Aimer>();
                aimer.cam = player.GetComponent<Player>().playerCamera.GetComponent<Camera>();
                aimer.spell = spell;
                aimer.gameMaster = gameMaster;
                aimer.player = player.GetComponent<Player>();
                aimer.targetList = player.GetComponent<TargetList>();
                player.GetComponent<TargetList>().CmdClear();
                aimer.startFollowing = true;
                //

                //re-enable spell buttons
                gameMaster.spellButtonsEnabled = true;

                //Referred to this: https://answers.unity.com/questions/149715/making-an-object-destory-itself.html
                //destroy the tracer GM
                Destroy(gameObject);
            }
        }    
    }

    //referred to: 
    //https://answers.unity.com/questions/232471/display-a-block-of-text-to-the-screen.html
    //https://forum.unity.com/threads/displaying-text-that-isnt-a-ui.439126/
    //https://www.youtube.com/watch?v=DpO5ESeJDpA
    //https://answers.unity.com/questions/1386574/how-to-display-health-on-screen-as-text.html
    //https://forum.unity.com/threads/how-to-output-text-to-the-screen.46606/
    //https://forum.unity.com/threads/how-to-display-a-variable-value-on-screen.282725/
    //https://unity3d.com/learn/tutorials/projects/roll-ball-tutorial/displaying-score-and-text
    //https://docs.unity3d.com/ScriptReference/GUI.Label.html
    //https://forum.unity.com/threads/displaying-a-variable-on-the-screen-c.214206/
    //https://forum.unity.com/threads/how-to-display-a-text-on-the-screen.335413/
    //https://forum.unity.com/threads/how-to-simply-print-a-variable-on-screen.42968/
    private void OnGUI()
    {
        GUI.Label(new Rect(0, 0, 100, 100), accuracy.ToString() + "%"); //print current accuracy

        timeElapsed = Time.time - startTime;


        float timeLeft = timeLimit - timeElapsed;

        //Reference to:
        //https://stackoverflow.com/questions/13171994/why-i-cant-call-math-rounddouble-int-overload
        //https://stackoverflow.com/questions/164926/how-do-i-round-a-decimal-value-to-2-decimal-places-for-output-on-a-page
        //https://forum.unity.com/threads/how-to-round-a-float-to-2-decimal-places.361504/
        //https://stackoverflow.com/questions/38871691/rounding-up-to-2-decimal-places-c-sharp
        timeLeft = (float) System.Math.Round(timeLeft, 1);

        if (isGameOver)
        {
            timeLeft = 0;
        }

        if (timeLeft > 0)
        {
            GUI.Label(new Rect(0, 15, 100, 100), timeLeft.ToString() + "s left");
        } else
        {
            GUI.Label(new Rect(0, 15, 100, 100), "Tracing ended");
        }
        
    }

    //Calculates x-y Euclidean distance between two vectors
    float distance(Vector3 v1, Vector3 v2)
    {
        return Mathf.Sqrt(Mathf.Pow((v2.x - v1.x), 2) + Mathf.Pow((v2.y - v1.y), 2));
    }
}

//Citations used to create credits scene:
//https://docs.unity3d.com/Manual/animeditor-CreatingANewAnimationClip.html
//https://www.youtube.com/watch?v=AJy-FHb6_Jk
//https://www.youtube.com/watch?v=cj6hwCjiVZE (entire production process taken from this video)

//Other sources used:
//https://answers.unity.com/questions/494263/for-some-reason-i-cannot-drag-my-prefabs-from-the.html
//https://answers.unity.com/questions/268427/activating-a-prefab-in-scene.html
//https://forum.unity.com/threads/cant-drag-objects-in-the-scene-view.210529/
//https://answers.unity.com/questions/30057/what-is-the-difference-between-grey-and-blue-prefa.html
//https://forum.unity.com/threads/enable-disable-gui-components.24641/
//https://www.reddit.com/r/Unity3D/comments/2b4k88/what_exactly_is_world_space_and_screen_space/
//https://docs.unity3d.com/ScriptReference/Camera.WorldToScreenPoint.html
//https://forum.unity.com/threads/how-to-open-unity-project-from-project-folder-where-file-is.433407/
//https://forum.unity.com/threads/unable-to-build-my-projects-in-2018-2-0b7-after-receiving-fatal-build-error.535810/
//https://stackoverflow.com/questions/3110254/how-to-retrieve-object-from-arraylist-in-c-sharp
//https://docs.microsoft.com/en-us/dotnet/api/system.collections.arraylist?view=netframework-4.7.2
//https://stackoverflow.com/questions/50341668/how-to-indent-unindent-a-block-of-lines
//https://stackoverflow.com/questions/3081916/convert-int-to-string
//https://stackoverflow.com/questions/51916153/passing-parameters-on-instantiation-in-unity
//https://www.google.com/search?q=unity+start+with+parameters&sa=X&ved=2ahUKEwjf5NTJq5PfAhXhnuAKHeUxAR4Q1QIoAXoECAYQAg&biw=1707&bih=804
//https://docs.unity3d.com/ScriptReference/MonoBehaviour.Start.html
//https://stackoverflow.com/questions/42179789/unity-pass-parameters-on-instantiation
//https://answers.unity.com/questions/254003/instantiate-gameobject-with-parameters.html
//https://answers.unity.com/questions/666696/passing-a-variable-to-start-c.html
//https://answers.unity.com/questions/526058/addcomponent-passing-variable-before-startawake.html
//https://www.google.com/search?q=unity+set+parameters+before+start&oq=unity+set+parameters+before+start&aqs=chrome..69i57.7213j0j7&sourceid=chrome&ie=UTF-8
//https://answers.unity.com/questions/160610/passing-values-into-base-constructor-with-monobeha.html
//https://docs.microsoft.com/en-us/dotnet/api/system.collections.arraylist?view=netframework-4.7.2
//https://docs.unity3d.com/ScriptReference/WaitForSeconds.html
//https://stackoverflow.com/questions/41096570/display-1-no-cameras-rendering
//https://docs.unity3d.com/Manual/GameView.html
//https://answers.unity.com/questions/531852/how-do-you-make-your-game-play-fullscreen.html
//https://docs.unity3d.com/ScriptReference/GameObject.GetComponent.html
//https://docs.unity3d.com/Manual/Components.html
//https://docs.unity3d.com/ScriptReference/Resources.Load.html
//https://www.google.com/search?q=unity+zoom+in&ie=utf-8&oe=utf-8&client=firefox-b-1-ab
//https://www.google.com/search?q=c%23+round+to+integer&ie=utf-8&oe=utf-8&client=firefox-b-1
//https://stackoverflow.com/questions/661028/how-can-i-divide-two-integers-to-get-a-double
//https://docs.unity3d.com/ScriptReference/SceneManagement.LoadSceneMode.Additive.html
//https://forum.unity.com/threads/is-it-possible-to-play-a-scene-from-another-scene.505006/
//https://www.google.com/search?q=unity+embed+scene+in+another+scene&ie=utf-8&oe=utf-8&client=firefox-b-1
//https://forum.unity.com/threads/how-can-i-make-the-whole-screen-flash-briefly.103527/
//https://answers.unity.com/questions/897157/make-whole-screen-white-flash-bang-effect-in-unity.html
//https://stackoverflow.com/questions/29717718/can-i-mix-2d-and-3d-scenes-in-game
//https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/preprocessor-directives/preprocessor-elif
//https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/if-else
//https://docs.unity3d.com/ScriptReference/Object.html
//https://www.google.com/search?q=unity+2d+prefabs&ie=utf-8&oe=utf-8&client=firefox-b-1
//https://answers.unity.com/questions/783178/sprite-not-visible.html
//https://answers.unity.com/questions/1057863/help-sprite-showing-up-in-scene-but-not-in-game.html
//https://www.google.com/search?q=unity+sprites+not+rendering&ie=utf-8&oe=utf-8&client=firefox-b-1
//https://forum.unity.com/threads/draw-a-simple-rectangle-filled-with-a-color.116348/
//https://unity3d.com/learn/tutorials/topics/2d-game-creation/sprite-type
//https://www.reddit.com/r/Unity2D/comments/28l1h1/most_painless_way_to_make_2d_objects/
//https://answers.unity.com/questions/899189/is-there-any-way-of-creating-primitive-2d-shapes-i.html
//https://answers.unity.com/questions/521984/how-do-you-draw-2d-circles-and-primitives.html
//https://forum.unity.com/threads/creating-basic-2d-shapes-in-unity3d-4-3.213328/
//https://forum.unity.com/threads/quad-color-not-changing-even-though-material-does.344955/
//https://answers.unity.com/questions/1308564/instantiate-vs-instantiate-as-gameobject.html
//https://answers.unity.com/questions/237806/instantiate-not-returning-anything.html
//https://docs.unity3d.com/ScriptReference/Object.Instantiate.html
//https://forum.unity.com/threads/free-draw-2d-shapes.499558/
//https://gamedev.stackexchange.com/questions/106625/how-to-correctly-draw-a-2d-polygon-in-unity
//https://www.youtube.com/watch?v=3E8yKvUyBko
//https://answers.unity.com/questions/1411572/how-do-i-draw-simple-shapes.html
//https://docs.unity3d.com/ScriptReference/EditorGUI.DrawRect.html
//https://answers.unity.com/questions/37752/how-to-render-a-colored-2d-rectangle.html
//https://forum.unity.com/threads/fastest-way-to-draw-a-square.403850/
//https://forum.unity.com/threads/cant-drag-object-into-inspector.429064/
//https://answers.unity.com/questions/658928/how-to-destroy-an-instantiated-prefab-in-c.html
//https://www.youtube.com/watch?v=_kukBTAa4xg
//https://medium.com/@hyperparticle/draw-2d-physics-shapes-in-unity3d-2e0ec634381c
//https://forum.unity.com/threads/change-the-color-of-a-primitive-specifically-a-quad.505967/
//https://www.youtube.com/results?search_query=unity+draw+rectangle
//https://answers.unity.com/questions/756313/how-to-change-the-capsules-height-without-deformin.html
//https://docs.unity3d.com/Manual/InstantiatingPrefabs.html
//https://docs.unity3d.com/Manual/AssetStore.html
//https://docs.unity3d.com/Manual/class-Transform.html