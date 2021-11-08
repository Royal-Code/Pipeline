using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace RoyalCode.CommandAndQuery.Tests
{
    public class T03_BridgesTests
    {

        [Fact]
        public void T01_OneBridgeHandler_In()
        {

        }
    }

    public class OneBridgeInFirstRequest : IRequest
    {
        public OneBridgeInFirstRequest(int value)
        {
            Value = value;
        }

        public int Value { get; }
    }

    public class OneBridgeInSecondRequest : IRequest
    {
        public OneBridgeInSecondRequest(int value)
        {
            Value = value;
        }

        public int Value { get; }
    }
}
