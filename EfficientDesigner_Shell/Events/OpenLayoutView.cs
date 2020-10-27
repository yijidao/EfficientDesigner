using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Events;
using EfficientDesigner_Service.Models;

namespace EfficientDesigner_Shell.Events
{
    class OpenLayoutView : PubSubEvent<Layout>
    {
    }
}
