﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class Vibrations : MonoBehaviour {

	// Left vibration motor values used (shakes the controller)
	static float lightRumble = 0.25f;
	static float mediumRumble = 0.5f;
	static float bigRumble = 1.0f;

	// Right vibration motor values used (sends a sort of "interior" vibration in the controller)
	static float lightVibration = 0.25f;
	static float mediumVibration = 0.5f;
	static float bigVibration = 1.0f;

	// Durations used
	static float shortVibrationDuration = 0.1f;
	static float mediumVibrationDuration = 0.4f;
	static float longVibrationDuration = 1.0f;

	// Method called from other scripts to play vibrations
	// Also applies screen shakes
	public static float PlayVibration(string situationName)
	{
		float duration = 0f;
		switch (situationName)
		{
		case "Dash":
			ScreenShake.Instance.ApplyScreenShake (0.1f, 0.25f);
			GamePad.SetVibration(0, 0, Vibrations.lightVibration);
			duration = Vibrations.mediumVibrationDuration;
			return duration;
		case "HeavyDash":
			ScreenShake.Instance.ApplyScreenShake (0.1f, 0.5f);
			GamePad.SetVibration(0, 0, Vibrations.mediumVibration);
			duration = Vibrations.mediumVibrationDuration;
			return duration;
		case "FallingOnFloor":
			GamePad.SetVibration(0, 0, Vibrations.lightVibration);
			duration = Vibrations.shortVibrationDuration;
			return duration;
		case "TransitionToNextBoard":
			GamePad.SetVibration (0, 0, Vibrations.lightVibration);
			duration = Vibrations.longVibrationDuration;
			return duration;
		case "HeavyDashOnItem":
			ScreenShake.Instance.ApplyScreenShake (0.1f, 0.5f);
			GamePad.SetVibration (0, 0, Vibrations.mediumVibration);
			duration = Vibrations.shortVibrationDuration;
			return duration;
		case "DashFail":
			ScreenShake.Instance.ApplyScreenShake (0.1f, 0.25f);
			GamePad.SetVibration (0, Vibrations.lightRumble, 0);
			duration = Vibrations.shortVibrationDuration;
			return duration;
		case "Validate":
			GamePad.SetVibration (0, 0, Vibrations.lightVibration);
			duration = Vibrations.shortVibrationDuration;
			return duration;
		case "Death":
			ScreenShake.Instance.ApplyScreenShake (0.2f, 1f);
			GamePad.SetVibration (0, Vibrations.mediumRumble, 0);
			duration = Vibrations.mediumVibrationDuration;
			return duration;
		case "NewSkillGain":
			ScreenShake.Instance.ApplyScreenShake (0.1f, 1f);
			GamePad.SetVibration (0, Vibrations.lightRumble, Vibrations.mediumVibration);
			duration = Vibrations.mediumVibrationDuration;
			return duration;
		default:
			return 0f;
		}
	}
}
