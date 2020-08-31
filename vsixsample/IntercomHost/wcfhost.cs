using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace vsixsample.IntercomHost
{

    [ServiceContract]
    public interface IService
    {
        [OperationContract]
        void HelloWorld();

    }
}
