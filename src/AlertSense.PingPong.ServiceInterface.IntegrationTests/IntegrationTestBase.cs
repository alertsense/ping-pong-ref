using NUnit.Framework;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlertSense.PingPong.ServiceInterface.IntegrationTests
{
    [TestFixture]
    public abstract class IntegrationTestBase
    {
        #region AppHost Configuration

        protected IntegrationAppHost AppHost { get; set; }
        protected const string AppHostAddress = "localhost";
        protected const string AppHostProtocol = "http";

        protected string AppHostUrl
        {
            get
            {
                return string.Format("{0}://{1}:{2}/", AppHostProtocol, AppHostAddress, Port);
            }
        }

        private string port;
        protected string Port
        {
            get
            {
                // this shenanigans is to auto generate the port for each test runner instance that is 
                //      being created, should hopefully help with collisions doing paralell test execution.
                if (string.IsNullOrEmpty(port))
                {
                    port = string.Format("5{0}", Process.GetCurrentProcess().Id);
                    if (port.Length > 5)
                        port = port.Substring(0, 5);
                    else
                        port = port.PadRight(5, '0');
                }

                return port;
            }
            set { port = value; }
        } 

        #endregion

        #region JsonServiceClient Configuration

        private JsonServiceClient client;

        public JsonServiceClient Client
        {
            get { 
                return client ?? (client = new JsonServiceClient(AppHostUrl)); 
            }
            set { client = value; }
        }
        
        #endregion

        [TestFixtureSetUp]
        protected void TestFixtureSetUp()
        {
            InitTestSession();
        }

        [SetUp]
        protected void SetUp()
        {
            InitTest();
            SetupAppHost();
        }

        [TearDown]
        protected void Cleanup()
        {
            AppHost.Stop();
            AppHost.Dispose();
        }

        private void SetupAppHost()
        {
            AppHost = new IntegrationAppHost();
            AppHost.Init();
            AppHost.Start(AppHostUrl);
        }

        public abstract void InitTestSession();
        public abstract void InitTest();
    }
}
