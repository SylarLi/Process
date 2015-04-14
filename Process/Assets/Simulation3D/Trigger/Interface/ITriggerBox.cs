public interface ITriggerBox
{
    /// <summary>
    /// 关联角色id
    /// </summary>
    long id { get; set; }

    void Trigger(ITriggerBox input);
}
