namespace TDDMicroExercises.TelemetrySystem
{
    public interface ITelemetryClient
    {
        bool OnlineStatus { get; }

        void Connect(string telemetryServerConnectionString);

        void Disconnect();

        void Send(string message);

        string Receive();

    }
}