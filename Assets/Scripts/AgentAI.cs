using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Policies;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class AgentAI : Agent
{
  [SerializeField]
  Base myBase, otherBase;

  Health myBaseHealth, otherBaseHealth;
  int maxActions;
  Unit[] units;

  BehaviorParameters myBehaviorParams;

  public override void OnEpisodeBegin()
  {
    myBehaviorParams = GetComponent<BehaviorParameters>();
    maxActions = myBehaviorParams.BrainParameters.ActionSpec.BranchSizes[0];
    myBaseHealth = myBase.GetComponent<Health>();
    otherBaseHealth = otherBase.GetComponent<Health>();
    units = myBase.GetUnits();
  }

  public override void CollectObservations(VectorSensor sensor)
  {
    sensor.AddObservation(myBaseHealth.GetHealth() * 1.0f / myBaseHealth.GetMaxHealth());
    sensor.AddObservation(otherBaseHealth.GetHealth() * 1.0f / otherBaseHealth.GetMaxHealth());
    sensor.AddObservation(myBase.GetCurrentMoney());
  }

  public override void Heuristic(in ActionBuffers actionsOut)
  {
    base.Heuristic(actionsOut);
  }

  public override void OnActionReceived(ActionBuffers actions)
  {
    var acts = actions.DiscreteActions;
    if (acts[0] == 0)
    {
      return;
    }
    else if (acts[0] <= units.Length)
    {
      //Debug.Log($"{name} made action {acts[0]} out of {maxActions}");
      myBase.SpawnUnit(units[acts[0] - 1]);
    }
  }

  public override void WriteDiscreteActionMask(IDiscreteActionMask actionMask)
  {
    for (int i = 0; i < maxActions; i++)
    {
      if (i > units.Length)
      {
        actionMask.SetActionEnabled(0, i, false);
      }
      else if (i > 0)
      {
        actionMask.SetActionEnabled(0, i, myBase.GetCurrentMoney() >= units[i - 1].GetCost());
      }
    }
  }
}
