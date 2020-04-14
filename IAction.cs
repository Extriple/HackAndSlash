using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    //Inteface odpowiedzialny za anulowanie jednej czynności albo walka albo poruszanie się.
    public interface IAction
    {
        void Cancel();
    }
}