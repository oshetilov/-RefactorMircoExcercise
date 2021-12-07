using System;
using Moq;
using NUnit.Framework;

namespace TDDMicroExercises.TirePressureMonitoringSystem.Tests
{
    [TestFixture]
    public class AlarmTest
    {
        private Alarm _alarm;
        private Mock<ISensor> _sensorMock;

        [SetUp]
        [Obsolete]
        public void Init()
        {
            _sensorMock = new Mock<ISensor>();
            _alarm = new Alarm(_sensorMock.Object);
        }

        [Test]
        public void Alarm_Should_Be_Off_Initially()
        {
            Assert.IsFalse(_alarm.AlarmOn);
        }

        [Test]
        public void Alarm_Should_Be_Off_For_Expected_Pressure()
        {
            _sensorMock.Setup(x => x.PopNextPressurePsiValue())
                .Returns(20);

            _alarm.Check();

            Assert.IsFalse(_alarm.AlarmOn);
        }

        [Test]
        public void Alarm_Should_Be_On_For_Low_Pressure()
        {
            _sensorMock.Setup(x => x.PopNextPressurePsiValue())
                .Returns(1);

            _alarm.Check();

            Assert.IsTrue(_alarm.AlarmOn);
        }

        [Test]
        public void Alarm_Should_Be_On_For_High_Pressure()
        {
            _sensorMock.Setup(x => x.PopNextPressurePsiValue())
                .Returns(100);

            _alarm.Check();

            Assert.IsTrue(_alarm.AlarmOn);
        }
    }
}