
using System;

namespace TDDMicroExercises.TelemetrySystem
{
    public class TelemetryDiagnosticControls
    {
        private const string DiagnosticChannelConnectionString = "*111#";

        private readonly ITelemetryClient _telemetryClient;

        public const int ConnectAttemptCount = 3;

        public TelemetryDiagnosticControls()
        {
            _telemetryClient = new TelemetryClient();
        }

        public TelemetryDiagnosticControls(ITelemetryClient telemetryClient)
        {
            _telemetryClient = telemetryClient;
        }

        public string DiagnosticInfo { get; set; } = string.Empty;

        public void CheckTransmission()
        {
            DiagnosticInfo = string.Empty;

            _telemetryClient.Disconnect();

            int retryLeft = ConnectAttemptCount;
            while (_telemetryClient.OnlineStatus == false && retryLeft > 0)
            {
                _telemetryClient.Connect(DiagnosticChannelConnectionString);
                retryLeft -= 1;
            }

            if (_telemetryClient.OnlineStatus == false)
            {
                throw new Exception("Unable to connect.");
            }

            _telemetryClient.Send(TelemetryClient.DiagnosticMessage);
            DiagnosticInfo = _telemetryClient.Receive();
        }
    }
}
