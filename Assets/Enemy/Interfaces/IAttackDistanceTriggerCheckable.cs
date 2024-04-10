public interface IAttackDistanceTriggerCheckable
{
    bool IsWithinStrikingDistance { get; set; }
    void SetStrikingDistanceBool(bool isWithinStrikingDistance);
}