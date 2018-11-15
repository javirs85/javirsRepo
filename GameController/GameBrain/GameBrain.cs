using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using gameTools;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using GBCore.Connectivity;

namespace GBCore
{
    public static class GameBrain
    {
        public static void Init()
        {
            GameItems.Init();
        }
    }
}
