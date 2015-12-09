using UnityEngine;
using System.Collections;

public enum CivAction { MoveTo, TakeSelfie, UseTwitter, UseTumblr, Wait };
public enum ActionFlag { None, ActionComplete};

public class BeCivilian : MonoBehaviour {

    public Material[] materials;

    private int ActionIndex;
    private CivilianAction[] QueuedBehaviours;
    private Vector3 oldPosition;
    private bool initializeAction = true;

    public bool HideWaypoints = false;
    public GameObject Player;

    public PopupController mediaPopUp;
    private BillboardScript mediaIcon;
    private bool iconShown = false;
    public float TravelSpeed;
    private Animator animator;

    //Behaviour Warnings:
    private GameObject indicator;
    private Renderer indicatorRenderer;
    private Texture circleIndicator;
    private Texture arcIndicator;

    // Behaviour Specific:
    // Wait
    float elapsedSeconds = 0f;
    // Selfie
    public bool EndedSelfie = false;
    public bool TakingSelfie = false;
    // Twitter
    private bool caughtPlayer = false;
    private Vector3 playerLastFramePosition = new Vector3();
    //public MeshRenderer phone;

    public GameObject TSAAgentRunner;

    public Vector3 SpawnAgentLocation = new Vector3(0, 0, 0);

	// Use this for initialization
	void Start ()
    {
        indicator = GameObject.CreatePrimitive(PrimitiveType.Plane);
        indicator.transform.parent = transform;
        indicator.transform.position = transform.position + new Vector3(0,0.01f,0);
        indicatorRenderer = indicator.GetComponent<Renderer>();
        arcIndicator = Resources.Load("arc") as Texture;
        circleIndicator = Resources.Load("circle") as Texture;
        indicatorRenderer.material = Resources.Load("Hazmat") as Material;
        indicatorRenderer.material.mainTexture = arcIndicator;
        indicator.SetActive(false);

        GetComponentInChildren<SkinnedMeshRenderer>().material = materials[Random.Range(0,materials.Length-1)];

        animator = GetComponentInChildren<Animator>();
        QueuedBehaviours = GetComponentsInChildren<CivilianAction>();
        foreach (CivilianAction Action in QueuedBehaviours)
        {
            MeshRenderer Renderer = Action.transform.GetComponent<MeshRenderer>();
            Renderer.enabled = !HideWaypoints;
            Action.transform.parent = null;
        }
        playerLastFramePosition = Player.transform.position;

        mediaIcon = GetComponentInChildren<BillboardScript>();
        if (mediaIcon == null) { Debug.LogError("BeCivilian does not have a billboard!"); }
        if (mediaPopUp == null) { Debug.LogError("BeCivilian does not have a PopUp!"); }
    }

    // Update is called once per frame
    void Update()
    {

        if (QueuedBehaviours == null || QueuedBehaviours.Length == 0) { return; }

        if (ActionIndex < 0) { ActionIndex = QueuedBehaviours.Length - 1; }
        if (ActionIndex >= QueuedBehaviours.Length) { ActionIndex = 0; }

        ActionFlag FlagsReturnedFromBehaviour = 0;
        CivilianAction Action = QueuedBehaviours[ActionIndex];

        switch (Action.ID)
        {
            case (CivAction.MoveTo):
                {
                    //Debug.Log("MOve To " + Action.transform.position);
                    FlagsReturnedFromBehaviour = MoveTo(Action.transform.position);
                    //phone.enabled = false;
                }
                break;
            case (CivAction.TakeSelfie):
                {
                    if (!iconShown)
                    {
                        mediaIcon.makeVisible("camera");
                        animator.SetTrigger("Action");
                        //phone.enabled = true;
                        iconShown = true;
                    }
                    FlagsReturnedFromBehaviour = TakeSelfie(Action.transform.position, Action.Parameter);
                }
                break;
            case (CivAction.UseTwitter):
                {
                    if (!iconShown)
                    {
                        mediaIcon.makeVisible("twitter");
                        animator.SetTrigger("Action");
                        //phone.enabled = true;
                        iconShown = true;
                    }
                    FlagsReturnedFromBehaviour = UseTwitter(Action.transform.position, Action.Parameter);
                }
                break;
            case (CivAction.UseTumblr):
                {
                    if (!iconShown)
                    {
                        mediaIcon.makeVisible("tumblr");
                        animator.SetTrigger("Action");
                        //phone.enabled = true;
                        iconShown = true;
                    }
                    FlagsReturnedFromBehaviour = UseTumblr(Action.transform.position, Action.Parameter);
                }
                break;
            default:
            case (CivAction.Wait):
                {
                    FlagsReturnedFromBehaviour = Wait(Action.Parameter);
                }
                break;
        }

        if (FlagsReturnedFromBehaviour == ActionFlag.ActionComplete)
        {
            initializeAction = true;
            ActionIndex++;
        }

        playerLastFramePosition = Player.transform.position;

        Vector3 Heading = transform.position - oldPosition;
        if (Heading.magnitude > .00001f)
        {
            Heading.Normalize();
            Quaternion direction = Quaternion.LookRotation(Heading);
            animator.SetFloat("Speed", Heading.magnitude);
            oldPosition = transform.position;
            transform.rotation = direction;
        }
        else
        {
            animator.SetFloat("Speed", 0);
        }
    }

    ActionFlag MoveTo(Vector3 TargetPosition)
    {
        if (initializeAction)
        {
            initializeAction = false;
        }

        /// POSSIBLE EDITOR ARGUMENTS
        float ArriveDistance = TravelSpeed * Time.deltaTime * 1.2f;

        Vector3 NewPosition = Vector3.Normalize(TargetPosition - transform.position)*TravelSpeed*Time.deltaTime;
        transform.position += NewPosition;

        float DistanceToTarget = Vector3.Distance(TargetPosition, transform.position);
        if ( DistanceToTarget < ArriveDistance)
        {
            return ActionFlag.ActionComplete;
        }
        return ActionFlag.None;
    }

    ActionFlag TakeSelfie(Vector3 TargetPosition, float Distance)
    {
        if (initializeAction)
        {
            initializeAction = false;
        }

        if (EndedSelfie)
        {
            EndedSelfie = false;
            TakingSelfie = false;
            iconShown = false;
            mediaIcon.makeVisible("camera");
            indicator.transform.position = transform.position + new Vector3(0, 0.01f, 0); ;
            indicator.transform.rotation = new Quaternion();
            // Set animation normal
            return ActionFlag.ActionComplete;
        }

        Vector3 TargetHeading = Vector3.Normalize(TargetPosition - transform.position);
        Quaternion SelfieDirection = Quaternion.LookRotation(TargetHeading);
        transform.rotation = SelfieDirection;
        float hazardDistance = Vector3.Distance(TargetPosition, transform.position);

        if (TakingSelfie)
        {
            // Trigger Selfie animation
            // Animation will call end selfie function

            // ======================================= TEMP while no animations
            if (elapsedSeconds > 1f)             //|
            {                                    //|
                elapsedSeconds = 0;              //|
                EndSelfie(TargetPosition);       //|
            }                                    //|
            elapsedSeconds += Time.deltaTime;    //|
            // ======================================= TEMP while no animations

            if (!HideWaypoints) { ShowDebugSelfieIndications(TargetPosition); }
            indicator.SetActive(true);
            indicator.transform.position = transform.position + (.5f * (TargetPosition - transform.position)) + new Vector3(0,0.01f,0);
            indicator.transform.rotation = SelfieDirection;
            indicatorRenderer.material.mainTexture = arcIndicator;
            float scale = hazardDistance * .1f;
            indicator.transform.localScale = new Vector3(scale, scale, scale);

        }
        else
        {
            TakingSelfie = true;
        }
        return 0;
    }

    public void EndSelfie(Vector3 TargetPosition)
    {
        
        // Check if the player is in the dangerous part of the hazard.
        Vector3 TargetHeading = Vector3.Normalize(TargetPosition - transform.position);
        Quaternion SelfieDirection = Quaternion.LookRotation(TargetHeading);
        Vector3 PlayerHeading = Vector3.Normalize(Player.transform.position - transform.position);
        Quaternion PlayerDirection = Quaternion.LookRotation(PlayerHeading);
        float Angle = Quaternion.Angle(SelfieDirection, PlayerDirection);

        if ((Angle < 30f) && (Vector3.Distance(transform.position,Player.transform.position) < Vector3.Magnitude(TargetPosition - transform.position)))
        {
            spawnAgent(Player, new Vector3(transform.position.x + 1, transform.position.y, transform.position.z + 1));
            caughtPlayer = true;
            Debug.Log("Player Caught in a selfie!");
        }
        EndedSelfie = true;
    }

    ActionFlag UseTwitter(Vector3 TargetPosition, float Duration)
    {
        if (initializeAction)
        {
            initializeAction = false;
        }

        float startTime = 1f;
        if (elapsedSeconds > Duration)         
        {                                
            elapsedSeconds = 0;
            caughtPlayer = false;
            mediaIcon.makeVisible("twitter");

            indicator.SetActive(false);
            iconShown = false;
            return ActionFlag.ActionComplete;
        }
        
        Color hazardColor = Color.blue;
        float hazardDistance = Vector3.Distance(transform.position, TargetPosition);
        if (elapsedSeconds > startTime)
        {
            var PlayerController = Player.GetComponent<UnityStandardAssets.Characters.FirstPerson.RigidbodyFirstPersonController>();
            float PlayerWalkSpeedDistance = PlayerController.movementSettings.ForwardSpeed * Time.deltaTime;
            float lastFramePlayerDistance = Vector3.Distance(Player.transform.position, playerLastFramePosition);

            // Check to see if the player is hit by hazards
            if (
                 ( Vector3.Distance(transform.position, Player.transform.position) < hazardDistance)
                  &&
                 ( lastFramePlayerDistance > PlayerWalkSpeedDistance)
               )
            {
                if (!caughtPlayer)
                {
                    spawnAgent(Player, new Vector3(transform.position.x + 1, transform.position.y, transform.position.z + 1));
                    Debug.Log("Player caught running next to twitter user!");
                    mediaPopUp.showNotification("twitter");
                    caughtPlayer = true;
                }
            }
            hazardColor = Color.red;
        }
        elapsedSeconds += Time.deltaTime;

        if (!HideWaypoints) { ShowDebugTwitterIndications(TargetPosition, hazardColor); }
        indicator.SetActive(true);
        indicatorRenderer.material.mainTexture = circleIndicator;
        float scale = hazardDistance * .2f;
        indicator.transform.localScale = new Vector3(scale, scale, scale);
        return 0;    
    }

    ActionFlag UseTumblr(Vector3 TargetPosition, float Duration)
    {
        if (initializeAction)
        {
            initializeAction = false;
        }

        float startTime = 1f;
        if (elapsedSeconds > Duration)
        {
            elapsedSeconds = 0;
            caughtPlayer = false;
            mediaIcon.makeVisible("tumblr");
            indicator.SetActive(false);
            iconShown = false;
            return ActionFlag.ActionComplete;
        }

        float hazardDistance = Vector3.Distance(transform.position, TargetPosition);
        Color hazardColor = Color.blue;
        if (elapsedSeconds > startTime)
        {
            // Check to see if the player is hit by hazards
            if (Vector3.Distance(transform.position, Player.transform.position) < hazardDistance)
            {
                if (!caughtPlayer)
                {
                    spawnAgent(Player, new Vector3(transform.position.x + 1, transform.position.y, transform.position.z + 1));
                    Debug.Log("Player caught next to tumblr user!");
                    mediaPopUp.showNotification("tumblr");
                    caughtPlayer = true;
                    
                }
            }
            hazardColor = Color.red;
        }
        elapsedSeconds += Time.deltaTime;

        if (!HideWaypoints) { ShowDebugTwitterIndications(TargetPosition, hazardColor); }
        indicator.SetActive(true);
        indicatorRenderer.material.mainTexture = circleIndicator;
        float scale = hazardDistance * .2f;
        indicator.transform.localScale = new Vector3(scale, scale, scale);
        //ShowTwitterIndications(TargetPosition);
        return 0;
    }

    ActionFlag Wait(float Seconds)
    {
        if (initializeAction)
        {
            initializeAction = false;
        }

        if (elapsedSeconds > Seconds)
        {
            elapsedSeconds = 0;
            return ActionFlag.ActionComplete;
        }

        elapsedSeconds += Time.deltaTime;
        return 0;
    }

    void ShowDebugSelfieIndications(Vector3 TargetPosition)
    {
        // Show Hazard Indications
        Vector3 TargetHeading = Vector3.Normalize(TargetPosition - transform.position);
        Quaternion SelfieDirection = Quaternion.LookRotation(TargetHeading);
        Vector3 LineEndpoint = transform.position + (SelfieDirection * (Vector3.forward * Vector3.Distance(transform.position, TargetPosition)));
        Debug.DrawLine(transform.position, LineEndpoint, Color.red, Time.deltaTime);

        float DangerHalfAngleDegrees = 30f;
        int DangerIndicationDivisions = 5;
        float DangerIncrementation = (DangerHalfAngleDegrees * 2f) / (DangerIndicationDivisions);
        int Lines = DangerIndicationDivisions + 1;

        Vector3 OriginDirection = SelfieDirection.eulerAngles;
        OriginDirection.y += 30f;
        Quaternion LoopOriginDirection = Quaternion.Euler(OriginDirection);

        for (int LineIndex = 0; LineIndex < Lines; LineIndex++)
        {
            Quaternion LineDirection = LoopOriginDirection;
            Vector3 LineDirectionEuler = LineDirection.eulerAngles;
            LineDirectionEuler.y -= (LineIndex * (DangerIncrementation));
            LineDirection = Quaternion.Euler(LineDirectionEuler);
            Vector3 IndicationEndpoint = transform.position + (LineDirection * (Vector3.forward * Vector3.Distance(transform.position, TargetPosition)));
            Debug.DrawLine(transform.position, IndicationEndpoint, Color.blue, Time.deltaTime);
        }
    }

    void ShowDebugTwitterIndications(Vector3 TargetPosition, Color lineColor)
    {
        // Show Hazard Indications
        Quaternion SelfieDirection = Quaternion.LookRotation(Vector3.forward);
        Vector3 LineEndpoint = transform.position + (SelfieDirection * (Vector3.forward * Vector3.Distance(transform.position, TargetPosition)));
        Debug.DrawLine(transform.position, LineEndpoint, lineColor, Time.deltaTime);

        float DangerHalfAngleDegrees = 180f;
        int DangerIndicationDivisions = 11;
        float DangerIncrementation = (DangerHalfAngleDegrees * 2f) / (DangerIndicationDivisions);
        int Lines = DangerIndicationDivisions + 1;

        Vector3 OriginDirection = SelfieDirection.eulerAngles;
        OriginDirection.y += DangerHalfAngleDegrees;
        Quaternion LoopOriginDirection = Quaternion.Euler(OriginDirection);

        for (int LineIndex = 0; LineIndex < Lines; LineIndex++)
        {
            Quaternion LineDirection = LoopOriginDirection;
            Vector3 LineDirectionEuler = LineDirection.eulerAngles;
            LineDirectionEuler.y -= (LineIndex * (DangerIncrementation));
            LineDirection = Quaternion.Euler(LineDirectionEuler);
            Vector3 IndicationEndpoint = transform.position + (LineDirection * (Vector3.forward * Vector3.Distance(transform.position, TargetPosition)));
            Debug.DrawLine(transform.position, IndicationEndpoint, lineColor, Time.deltaTime);
        }
    }

    void spawnAgent(GameObject player, Vector3 location)
    {
        GameObject newAgent = (GameObject)Instantiate(TSAAgentRunner, location, Quaternion.identity);
        Vector3 targetDirection = player.transform.position - newAgent.transform.position;
        float angleToPlayer = Vector3.Angle(targetDirection, newAgent.transform.forward);
        newAgent.transform.localEulerAngles = new Vector3(newAgent.transform.localEulerAngles.x, angleToPlayer, newAgent.transform.localEulerAngles.z);
        TSARunnerScript script = newAgent.GetComponent<TSARunnerScript>();
        script.player = player;

    }
}
