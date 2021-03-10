using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
	public static event Action<int> onBuildButtonClicked;

	[SerializeField]
	private Text _warFundDisplay;

	[SerializeField]
	private Tower[] _towers;
	[SerializeField]
	private Button _buildGatlingGunButton;
	[SerializeField]
	private Button _buildMissileLauncherButton;



	private void OnEnable()
	{
		PlayerManager.onWarFundsChanged += WarFundsChanged;
	}


	void Start()
	{
		_warFundDisplay.text = PlayerManager.Instance.warFund.ToString();

		_buildGatlingGunButton.onClick.AddListener(() => OnBuildButtonClicked(0));
		_buildGatlingGunButton.GetComponentInChildren<Text>().text = "$" + _towers[0].buyFor;
		_buildMissileLauncherButton.onClick.AddListener(() => OnBuildButtonClicked(1));
		_buildMissileLauncherButton.GetComponentInChildren<Text>().text = "$" + _towers[1].buyFor;
	}


	void Update()
	{

	}


	private void OnDisable()
	{
		PlayerManager.onWarFundsChanged -= WarFundsChanged;
	}


	void OnBuildButtonClicked(int towerID)
	{
		onBuildButtonClicked?.Invoke(towerID);
	}


	void WarFundsChanged(int amount)
	{
		_warFundDisplay.text = amount.ToString();
	}
}