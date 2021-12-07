using System;
using Moq;
using NUnit.Framework;

namespace TDDMicroExercises.TelemetrySystem.Tests
{
    [TestFixture]
    public class TelemetryDiagnosticControlsTest
    {
        private TelemetryDiagnosticControls _diagnosticControls;
        private Mock<ITelemetryClient> _telemetryClientMock;

        [SetUp]
        [Obsolete]
        public void Init()
        {
            _telemetryClientMock = new Mock<ITelemetryClient>();
            _diagnosticControls = new TelemetryDiagnosticControls(_telemetryClientMock.Object);
        }

        [Test]
        public void TelemetryDiagnosticControls_Should_Send_Diagnostic_Message_And_Receive_Status_Message_Response()
        {
            _telemetryClientMock.SetupGet(x => x.OnlineStatus)
                .Returns(true);

            var message = string.Empty;

            _telemetryClientMock.Setup(x => x.Send(It.IsAny<string>()))
                .Callback<string>((x) =>
                {
                    message = x;
                });

            _telemetryClientMock.Setup(x => x.Receive())
                .Returns("received text");

            _diagnosticControls.CheckTransmission();

            Assert.AreEqual("received text", _diagnosticControls.DiagnosticInfo);
            Assert.AreEqual(TelemetryClient.DiagnosticMessage, message);
        }

        [Test]
        public void TelemetryDiagnosticControls_Should_Disconnect_TelemetryClient()
        {
            _telemetryClientMock.SetupGet(x => x.OnlineStatus)
                .Returns(true);

            var countDisconnectCall = 0;

            _telemetryClientMock.Setup(x => x.Disconnect())
                .Callback(() =>
                {
                    countDisconnectCall++;
                });

            _diagnosticControls.CheckTransmission();

            Assert.AreEqual(1, countDisconnectCall);
        }

        [Test]
        public void TelemetryDiagnosticControls_Should_Connect_To_TelemetryClient()
        {
            var onlineStatus = false;
            var connString = string.Empty;

            _telemetryClientMock.SetupGet(x => x.OnlineStatus)
                .Returns(() => onlineStatus);

            _telemetryClientMock.Setup(x => x.Connect((It.IsAny<string>())))
                .Callback<string>((x) =>
                {
                    connString = x;
                    onlineStatus = true;
                });

            _diagnosticControls.CheckTransmission();

            Assert.AreEqual("*111#", connString);
        }

        [Test]
        public void TelemetryDiagnosticControls_Should_Try_To_TelemetryClient_Several_Times_And_Throw_Exception()
        {
            var countConnectCall = 0;
            var errorMessage = string.Empty;

            _telemetryClientMock.SetupGet(x => x.OnlineStatus)
                .Returns(() => false);

            _telemetryClientMock.Setup(x => x.Connect((It.IsAny<string>())))
                .Callback<string>((x) =>
                {
                    countConnectCall++;
                });

            try
            {
                _diagnosticControls.CheckTransmission();
            }
            catch (Exception e)
            {
                errorMessage = e.Message;
            }

            Assert.AreEqual(TelemetryDiagnosticControls.ConnectAttemptCount, countConnectCall);
            Assert.AreEqual("Unable to connect.", errorMessage);
        }
    }
}
