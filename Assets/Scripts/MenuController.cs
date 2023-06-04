using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    [SerializeField] private Constant _constant;
    [SerializeField] private GameObject _menu;

    private bool _isOpenMenu = false;

    public void OnClickMenuButton()
    {
        if (_isOpenMenu)
        {
            _constant.UpdateConstant();
        }
        else
        {
            _constant.UpdateStartValue();
        }
        _menu.SetActive(!_isOpenMenu);
        _isOpenMenu = !_isOpenMenu;
    }

}