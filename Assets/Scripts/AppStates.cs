using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class AppStates : MonoBehaviour
{
	[SerializeField]
    Animator titleAnimator;

    private struct Triggers
    {
        public const string ShowTitle = "ShowTitle";
        public const string HideTitle = "HideTitle";
    }


    public void ShowTitle()
	{
		titleAnimator.SetTrigger(Triggers.ShowTitle);
    }	

	public void HideTitle()
	{

	}

	public void ShowTutorial()
	{

	}

	public void ShowPlacementUI()
	{

	}
}
