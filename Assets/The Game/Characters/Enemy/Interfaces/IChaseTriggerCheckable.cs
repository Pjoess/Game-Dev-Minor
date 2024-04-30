public interface IChaseTriggerCheckable
{
    bool IsAggroed { get; set; }
    void SetAggroStatus(bool isAggroed);
}