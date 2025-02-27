using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(StateMachineInfo.AIBase))]
public class AIDetectionEditorScript : Editor
{
    private void OnSceneGUI()
    {
        StateMachineInfo.AIBase aiView = (StateMachineInfo.AIBase)target;

        Handles.color = Color.yellow;

        Handles.DrawWireArc(aiView.searchFunctionSettings.Eyes.transform.position, Vector3.up, Vector3.forward, 360, aiView.searchFunctionSettings.radius);

        //left half
        Vector3 viewAngle1 = AngleDirection(aiView.transform.eulerAngles.y, -aiView.searchFunctionSettings.angle / 2);

        
        //right half
        Vector3 viewAngle2 = AngleDirection(aiView.transform.eulerAngles.y, aiView.searchFunctionSettings.angle / 2);


        Handles.DrawLine(aiView.searchFunctionSettings.Eyes.transform.position, aiView.searchFunctionSettings.Eyes.transform.position + viewAngle1 * aiView.searchFunctionSettings.radius);
        Handles.DrawLine(aiView.searchFunctionSettings.Eyes.transform.position, aiView.searchFunctionSettings.Eyes.transform.position + viewAngle2 * aiView.searchFunctionSettings.radius);

    }

    /// <summary>
    /// Creates the the angle from a given direction
    /// returns the angle using the the center Y and the angle degrees
    /// </summary>
    /// 
    /// <param name="aEulerY"></param>
    /// <param name="aAngleDegrees"></param>
    /// <returns></returns>
    private Vector3 AngleDirection(float aEulerY, float aAngleDegrees)
    {
        aAngleDegrees += aEulerY;

        return new Vector3(Mathf.Sin(aAngleDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(aAngleDegrees * Mathf.Deg2Rad));
    }
}
