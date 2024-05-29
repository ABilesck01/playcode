using TMPro;

public class ConditionalActionView : ActionView
{
    public override void AddActionToView<T>(BaseController controller)
    {
        T action = new T();
        var button = Instantiate(BaseActionButtonTemplate, container);
        button.GetComponentInChildren<TextMeshProUGUI>().text = action.Name;
        button.onClick.AddListener(() =>
        {
            Destroy(currentActionView.gameObject);
            currentActionView = null;
            OnAddAction?.Invoke(controller, action);
        });
    }
}
