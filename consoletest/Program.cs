using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;

namespace consoletest
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceProxy sp = new ServiceProxy();
            sp.InvokeHelloWorld();
        }

    }
    [ServiceContract]
    public interface IService
    {
        [OperationContract]
        void HelloWorld();


    }

    public class ServiceProxy : ClientBase<IService>
    {
        public ServiceProxy()
            : base(new ServiceEndpoint(ContractDescription.GetContract(typeof(IService)),
                new NetNamedPipeBinding(), new EndpointAddress("net.pipe://localhost/vsix/api")))
        {

        }
        public void InvokeHelloWorld()
        {
            Channel.HelloWorld();
        }
    }
}
