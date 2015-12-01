using UnityEngine;
using System.Collections;

public enum CivAction { MoveTo, TakeSelfie, UseTwitter, UseTumblr, Wait };
public enum ActionFlag { None, ActionComplete};

public class BeCivilian : MonoBehaviour {

    private int ActionIndex;
    private CivilianAction[] QueuedBehaviours;

    public bool HideWaypoints = false;

	// Use this for initialization
	void Start () {
        QueuedBehaviours = GetComponentsInChildren<CivilianAction>();
        foreach (CivilianAction Action in QueuedBehaviours)
        {
            MeshRenderer Renderer = Action.transform.GetComponent<MeshRenderer>();
            Renderer.enabled = !HideWaypoints;
            Action.transform.parent = null;
        }
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
	}

    ActionFlag MoveTo(Vector3 TargetPosition)
    {
        float TravelSpeed = 2f;
        float ArriveDistance = TravelSpeed;// * (1f / 15f);
        Vector3 NewPosition = Vector3.Normalize(TargetPosition - transform.position)*TravelSpeed*Time.deltaTime;
        transform.position += NewPosition;

        float DistanceToTarget = Vector3.Distance(TargetPosition, NewPosition);
        Debug.Log(DistanceToTarget);
        if ( DistanceToTarget < ArriveDistance)
        {
            return ActionFlag.ActionComplete;
        }

        return ActionFlag.None;
    }

    ActionFlag TakeSelfie(Vector3 TargetPosition, float Distance)
    { return 0; }

    ActionFlag UseTwitter(Vector3 TargetPosition, float Radius)
    { return 0; }

    ActionFlag UseTumblr(Vector3 TargetPosition, float Radius)
    { return 0; }

    ActionFlag Wait(float Seconds)
    { return 0; }
}
