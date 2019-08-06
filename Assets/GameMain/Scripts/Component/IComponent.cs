using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Galaxy
{
    public interface IComponent
    {
		Avatar Owner
        {
            get;
            set;
        }

        void OnComponentReadyToStart();
        void OnComponentStart();
        void OnPreDestroy();
    }
}
