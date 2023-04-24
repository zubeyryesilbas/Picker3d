using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupManager : SingletonComponent<PopupManager>
{   
    [SerializeField] private List<PopupModel> _popupModels = new List<PopupModel>();
    private Popup _activePopup;
    private Dictionary<PopupType , Popup> _popus = new Dictionary<PopupType, Popup>();
    protected override void Awake()
    {
        RegisterPopups();
    }

    private void RegisterPopups()
    {
        foreach(var popupModel in _popupModels)
        {
            _popus.Add(popupModel.PopupType , popupModel.Popup);
            popupModel.Popup.gameObject.SetActive(false);
        }

    }
    public void CloseActivePopup()
    {
        if(_activePopup != null)
        {
            _activePopup.gameObject.SetActive(false);
            _activePopup = null;
        }
    }
    public void OpenPopup(PopupType popupType)
    {
        if(_activePopup != null)
        {
            CloseActivePopup();
        } 
        _activePopup = _popus[popupType];
        _activePopup.gameObject.SetActive(true);
    }
    

    
}
public enum PopupType
{
    Level_Fail , Level_Complete,Progression
}
[System.Serializable]
public class PopupModel 
{
   public  PopupType PopupType;
   public  Popup Popup;

}
