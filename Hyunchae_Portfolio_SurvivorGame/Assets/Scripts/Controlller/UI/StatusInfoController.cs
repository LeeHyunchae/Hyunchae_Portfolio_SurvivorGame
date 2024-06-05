using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusInfoController : MonoBehaviour
{
    [SerializeField] private Button firstStatusInfoButton;
    [SerializeField] private Button secondStatusInfoButton;
    [SerializeField] private Button weaponInventoryButton;
    [SerializeField] private Button itemInventoryButton;
    [SerializeField] private GameObject statusInfoObject;
    [SerializeField] private StatusElement[] statusElements;
    [SerializeField] private GameObject itemListObject;
    [SerializeField] private ItemButtonElement[] itemButtonElements;

    private ItemManager itemManager;
    private CharacterManager characterManager;

    private void Awake()
    {
        firstStatusInfoButton.onClick.AddListener(OnClickFirstStatusInfoButton);
        secondStatusInfoButton.onClick.AddListener(OnClickSecondStatusInfoButton);
        weaponInventoryButton.onClick.AddListener(OnClickWeaponInventoryButton);
        itemInventoryButton.onClick.AddListener(OnClickItemInventoryButton);

        itemManager = ItemManager.getInstance;
        characterManager = CharacterManager.getInstance;
    }

    private void OnClickFirstStatusInfoButton()
    {

    }
    private void OnClickSecondStatusInfoButton()
    {

    }
    private void OnClickWeaponInventoryButton()
    {

    }
    private void OnClickItemInventoryButton()
    {

    }
}
