namespace Cards.Application.Configuration
{
    public record QueueSetting
    {
        public string Name { get; init; }
        public bool AutoAck { get; init; }
    }
}