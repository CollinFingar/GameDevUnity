﻿using UnityEngine;
using System.Collections;

public enum CivAction { MoveTo, TakeSelfie, UseTwitter, UseTumblr, Wait };
public enum ActionFlag { None, ActionComplete};

public class BeCivilian : MonoBehaviour {

    private int ActionIndex;
    private CivilianAction[] QueuedBehaviours;

    public bool HideWaypoints = false;
    public GameObject Player;

    // Behaviour Specific:
    // Wait
    float elapsedSeconds = 0f;
    // Selfie
    public bool EndedSelfie = false;
    public bool TakingSelfie = false;
    // Twitter
    private bool caughtPlayer = false;
    private Vector3 playerLastFramePosition = new Vector3();

	// Use this for initialization
	void Start ()
    {
        QueuedBehaviours = GetComponentsInChildren<CivilianAction>();
        foreach (CivilianAction Action in QueuedBehaviours)
        {
            MeshRenderer Renderer = Action.transform.GetComponent<MeshRenderer>();
            Renderer.enabled = !HideWaypoints;
            Action.transform.parent = null;
        }
        playerLastFramePosition = Player.transform.position;
    }
	
	// Update is called once per frame
	void Update () {

        if (QueuedBehaviours.Length == 0) { return; }

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
                } break;
            case (CivAction.TakeSelfie):
                {
                    FlagsReturnedFromBehaviour = TakeSelfie(Action.transform.position, Action.Parameter);
                } break;
            case (CivAction.UseTwitter):
                {
                    FlagsReturnedFromBehaviour = UseTwitter(Action.transform.position, Action.Parameter);
                } break;
            case (CivAction.UseTumblr):
                {
                    FlagsReturnedFromBehaviour = UseTumblr(Action.transform.position, Action.Parameter);
                } break;
            default:
            case (CivAction.Wait):
                {
                    FlagsReturnedFromBehaviour = Wait( Action.Parameter);
                } break;
        }

        if (FlagsReturnedFromBehaviour == ActionFlag.ActionComplete)
        {
            ActionIndex++;
        }

        playerLastFramePosition = Player.transform.position;
    }

    ActionFlag MoveTo(Vector3 TargetPosition)
    {
        /// POSSIBLE EDITOR ARGUMENTS
        float TravelSpeed = 2f;
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
        if (EndedSelfie)
        {
            EndedSelfie = false;
            TakingSelfie = false;

            // Set animation normal
            return ActionFlag.ActionComplete;
        }
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
            ShowSelfieIndications(TargetPosition);

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
            Debug.Log("Player Caught in a selfie!");
        }

        EndedSelfie = true;
    }

    ActionFlag UseTwitter(Vector3 TargetPosition, float Duration)
    {
        float startTime = 1f;
        if (elapsedSeconds > Duration)         
        {                                
            elapsedSeconds = 0;
            caughtPlayer = false;
            return ActionFlag.ActionComplete;
        }
        
        Color hazardColor = Color.blue;
        if (elapsedSeconds > startTime)
        {
            var PlayerController = Player.GetComponent<UnityStandardAssets.Characters.FirstPerson.RigidbodyFirstPersonController>();
            float PlayerWalkSpeedDistance = PlayerController.movementSettings.ForwardSpeed * Time.deltaTime;
            float lastFramePlayerDistance = Vector3.Distance(Player.transform.position, playerLastFramePosition);

            // Check to see if the player is hit by hazards
            float hazardDistance = Vector3.Distance(transform.position, TargetPosition);
            if (
                 ( Vector3.Distance(transform.position, Player.transform.position) < hazardDistance)
                  &&
                 ( lastFramePlayerDistance > PlayerWalkSpeedDistance)
               )
            {
                if (!caughtPlayer)
                {
                    Debug.Log("Player caught running next to twitter user!");
                    caughtPlayer = true;
                }
            }
            hazardColor = Color.red;
        }
        elapsedSeconds += Time.deltaTime;

        if (!HideWaypoints) { ShowDebugTwitterIndications(TargetPosition, hazardColor); }
        //ShowTwitterIndications(TargetPosition);
        return 0;    
    }

    ActionFlag UseTumblr(Vector3 TargetPosition, float Duration)
    {
        float startTime = 1f;
        if (elapsedSeconds > Duration)
        {
            elapsedSeconds = 0;
            caughtPlayer = false;
            return ActionFlag.ActionComplete;
        }

        Color hazardColor = Color.blue;
        if (elapsedSeconds > startTime)
        {
            // Check to see if the player is hit by hazards
            float hazardDistance = Vector3.Distance(transform.position, TargetPosition);
            if (Vector3.Distance(transform.position, Player.transform.position) < hazardDistance)
            {
                if (!caughtPlayer)
                {
                    Debug.Log("Player caught next to tumblr user!");
                    caughtPlayer = true;
                }
            }
            hazardColor = Color.red;
        }
        elapsedSeconds += Time.deltaTime;

        if (!HideWaypoints) { ShowDebugTwitterIndications(TargetPosition, hazardColor); }
        //ShowTwitterIndications(TargetPosition);
        return 0;
    }

    ActionFlag Wait(float Seconds)
    {
        if (elapsedSeconds > Seconds)
        {
            elapsedSeconds = 0;
            return ActionFlag.ActionComplete;
        }

        elapsedSeconds += Time.deltaTime;
        return 0;
    }

    void ShowSelfieIndications(Vector3 TargetPosition)
    {

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
}