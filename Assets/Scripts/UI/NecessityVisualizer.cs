using UnityEngine;
using UnityEngine.UI;

public class NecessityVisualizer
{
    private IEntity _entity;
    private Image _image;

    private Color _standardColor;
    private Color _criticalColor;

    private bool _showHunger;

    public NecessityVisualizer(IEntity necessities, Image image, Color criticalColor, bool showHunger)
    {
        _entity = necessities;
        _image = image;
        _standardColor = image.color;
        _criticalColor = criticalColor;
        _showHunger = showHunger;
    }

    public void Update()
    {
        Necessity visual = _showHunger ? _entity.Hunger : _entity.Thirst;
        
        _image.fillAmount = visual.Value / visual.MaxValue;

        if (visual.IsCritical())
            _image.color = _criticalColor;
        else
            _image.color = _standardColor;
    }
}
