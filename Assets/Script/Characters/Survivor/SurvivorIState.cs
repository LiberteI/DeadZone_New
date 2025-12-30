using UnityEngine;

public interface SurvivorIState
{
    /*
        This script is the state transition interface.
    */
    void OnEnter();

    void OnUpdate();

    void HandleInput();

    void OnExit();
}
