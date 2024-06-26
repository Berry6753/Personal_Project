using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.ComponentModel;

public class PlayerStatusView : MonoBehaviour
{
    [SerializeField] private Slider Slider_Hp;
    [SerializeField] private Slider Slider_EXP;
    [SerializeField] private TMP_Text TMP_Text_Level;

    private PlayerStatusViewModel _vm;

    private void OnEnable()
    {
        if (_vm != null) return;

        _vm = new PlayerStatusViewModel();
        _vm.PropertyChanged += OnPropertyChanged;
    }

    private void OnDisable()
    {
        if (_vm == null) return;

        _vm = null;
    }

    private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
    { 
        switch(e.PropertyName)
        {
            //case nameof(_vm.Name)
        }
    }
}
