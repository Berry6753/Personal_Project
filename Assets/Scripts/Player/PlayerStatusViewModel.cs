using ViewModel;
public class PlayerStatusViewModel : ViewModelBase
{
    private float _hp;
    private float _exp;
    private int _level;

    public float Hp
    { 
        get { return _hp; }
        set
        { 
            if(_hp ==value) return;

            _hp = value;
            OnPropertyChanged(nameof(Hp));
        }
    }
    public float EXP
    {
        get { return _exp; }
        set
        {
            if (_exp == value) return;

            _exp = value;
            OnPropertyChanged(nameof(EXP));
        }
    }
    public int Level
    {
        get { return _level; }
        set
        {
            if (_level == value) return;

            _level = value;
            OnPropertyChanged(nameof(Level));
        }
    }
}
